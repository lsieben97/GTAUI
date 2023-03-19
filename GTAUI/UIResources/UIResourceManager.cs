using GTAUI.UIResources.Providers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.UIResources
{
    internal sealed class UIResourceManager
    {
        private const string RESOURCE_MANIFEST_NAME = "manifest.json";

        private Dictionary<Assembly, string[]> assemblyResources;
        private string extractedUIResourcesFolder;

        private readonly Dictionary<string, UIResourceManifestEntry> manifestEntries;
        private readonly Dictionary<string, UIResourceProvider> resourceProviders;
        private readonly UIResourceProvider defaultResourceProvider;

        internal UIResourceManager()
        {
            assemblyResources = new Dictionary<Assembly, string[]>();
            manifestEntries = new Dictionary<string, UIResourceManifestEntry>();
            resourceProviders = new Dictionary<string, UIResourceProvider>();
            defaultResourceProvider = new DefaultUIResourceProvider();

            extractedUIResourcesFolder = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "_GTAUIExtractedUIResources");
            Directory.CreateDirectory(extractedUIResourcesFolder);

            RegisterUIResourceProvider("Sprite", new SpriteResourceProvider());
        }

        internal void RegisterAssemblyResources(Assembly assembly)
        {
            if (assemblyResources.ContainsKey(assembly))
            {
                return;
            }

            string[] resources = assembly.GetManifestResourceNames();
            UIController.Log($"Registering the following resources from {assembly}:\n{string.Join("\n", resources)}");
            assemblyResources.Add(assembly, resources);

            string manifestResource = resources.ToList().Find(r => r.EndsWith(RESOURCE_MANIFEST_NAME));
            if (manifestResource == null)
            {
                UIController.Log($"No resource manifest file found for assembly {assembly}");
                return;
            }

            LoadManifest(manifestResource, assembly);
        }

        private void LoadManifest(string resourceName, Assembly assembly)
        {
            try
            {
                Stream manifestStream = assembly.GetManifestResourceStream(resourceName);
                if (manifestStream == null)
                {
                    return;
                }
                    int newEntries = 0;

                using (StreamReader reader = new StreamReader(manifestStream))
                {
                    string manifestJson = reader.ReadToEnd();
                    JArray manifestArray = JArray.Parse(manifestJson);
                    foreach (JObject manifestObject in manifestArray.Children<JObject>())
                    {
                        if (manifestObject.Properties().Any(p => p.Name == "Name") == false)
                        {
                            continue;
                        }

                        if (manifestObject.Properties().Any(p => p.Name == "Type") == false)
                        {
                            continue;
                        }
                        string name = manifestObject.Property("Name").Value.ToString();
                        UIController.Log($"Manifest entry name: {name}");
                        string type = manifestObject.Property("Type").Value.ToString();
                        UIController.Log($"Manifest entry type: {type}");
                        manifestEntries.Add(name,new UIResourceManifestEntry(name, type, manifestObject));
                        newEntries++;
                    }
                }
                UIController.Log($"Loaded {newEntries} manifest entries for manifest resource {resourceName}");
            }
            catch (Exception ex)
            {
                UIController.Log($"Warning: Unable to load manifest resource '{resourceName}' from assembly {assembly}:\n{ex}");
            }
        }

        internal T GetUIResource<T>(string uiResourceName) where T: class
        {
            object resource = GetUIResource(uiResourceName);
            if (resource.GetType() != typeof(T))
            {
                UIController.Log($"Error: ui resource '{uiResourceName}' is a {resource.GetType()} but {typeof(T)} was expected.");
                return null;
            }

            return resource as T;
        }


        private object GetUIResource(string path)
        {
            foreach (KeyValuePair<Assembly, string[]> assembly in assemblyResources)
            {
                if (assembly.Value.Contains(path))
                {
                    try
                    {
                        Stream resourceStream = assembly.Key.GetManifestResourceStream(path);
                        if (resourceStream == null)
                        {
                            return null;
                        }

                        UIResourceProvider resourceProvider = defaultResourceProvider;

                        manifestEntries.TryGetValue(path, out UIResourceManifestEntry manifestEntry);

                        if (manifestEntry != null)
                        {
                            resourceProviders.TryGetValue(manifestEntry.Type, out UIResourceProvider specializedResourceProvider);

                            if (specializedResourceProvider != null)
                            {
                                resourceProvider = specializedResourceProvider;
                            }
                        }
                        resourceProvider.Reset();

                        return resourceProvider.GetEmbeddedResource(path, resourceStream, manifestEntry);
                    }
                    catch (Exception ex)
                    {
                        UIController.Log($"Error while getting UI resource '{path}': {ex}");
                        return null;
                    }
                }
            }

            try
            {
                if (File.Exists(path) == false)
                {
                    return null;
                }

                UIResourceProvider resourceProvider = defaultResourceProvider;

                manifestEntries.TryGetValue(path, out UIResourceManifestEntry manifestEntry);

                if (manifestEntry != null)
                {
                    resourceProviders.TryGetValue(manifestEntry.Type, out UIResourceProvider specializedResourceProvider);

                    if (specializedResourceProvider != null)
                    {
                        resourceProvider = specializedResourceProvider;
                    }
                }

                resourceProvider.Reset();

                return resourceProvider.GetFileResource(path, manifestEntry);
            }
            catch (Exception ex)
            {
                UIController.Log($"Error while getting UI resource from file '{path}': {ex}");
                return null;
            }
        }

        internal void RegisterUIResourceProvider(string uiResourceType,  UIResourceProvider resourceProvider)
        {
            if (resourceProviders.ContainsKey(uiResourceType))
            {
                return;
            }

            resourceProvider.ResourceExtractionFolder = extractedUIResourcesFolder;
            resourceProviders.Add(uiResourceType, resourceProvider);
        }
    }
}
