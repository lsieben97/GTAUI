using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAUI.UIResources
{
    /// <summary>
    /// Represents a class that can provide a ui resource from a file path or embedded resource stream.
    /// </summary>
    public abstract class UIResourceProvider
    {
        /// <summary>
        /// The folder where this resource provider can extract resources.
        /// Guaranteed to be not null when called from <see cref="GetEmbeddedResource(string, Stream, UIResourceManifestEntry)"/> or <see cref="GetFileResource(string, UIResourceManifestEntry)"/>.
        /// </summary>
        protected internal string ResourceExtractionFolder { get; internal set; }

        /// <summary>
        /// Get a ui resource from the given stream.
        /// </summary>
        /// <param name="uiResourceStream">The stream to get data from.</param>
        /// <param name="manifestEntry">The manifest entry for the ui resource. Can be null if there is no manifest entry for the ui resource.</param>
        /// <returns>An object representing a ui resource.</returns>
        public abstract object GetEmbeddedResource(string resourceName, Stream uiResourceStream, UIResourceManifestEntry manifestEntry);

        /// <summary>
        /// Get a ui resource from the given path on disk. The file is guaranteed to exist.
        /// </summary>
        /// <param name="path">The path to get the ui resource from.</param>
        /// <param name="manifestEntry">The manifest entry for the ui resource. Can be null if there is no manifest entry for the ui resource.</param>
        /// <returns>An object representing a ui resource.</returns>
        public abstract object GetFileResource(string resourceName, UIResourceManifestEntry manifestEntry);

        /// <summary>
        /// Reset any internal data related to the last loaded resource.
        /// </summary>
        public virtual void Reset() { }

        /// <summary>
        /// Extracts the resource to the default extraction folder and returns the full path to the extracted resource.
        /// </summary>
        /// <param name="resourceStream">The stream to get the data from.</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <returns></returns>
        protected string ExtractResource(Stream resourceStream, string resourceName)
        {
            string cleanPath = ReplaceInvalidChars(resourceName);
            string extractedPath = Path.Combine(ResourceExtractionFolder, cleanPath);

            using (FileStream extractedResourceFile = File.Create(extractedPath))
            {
                resourceStream.CopyTo(extractedResourceFile);
                resourceStream.Flush();
            }

            return extractedPath;
        }

        private string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
