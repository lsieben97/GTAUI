using GTA;
using GTAUI.Styling;
using LemonUI;
using LemonUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI
{
    /// <summary>
    /// This class controlls all UI displayed via GTAUI.
    /// It's <see cref="OnTick"/>, <see cref="OnKeyDown(KeyEventArgs)"/> and <see cref="OnKeyUp(KeyEventArgs)"/> methods should be called when the parent <see cref="Script"/> object receives the same events.
    /// To Actually use this class, Create an instance of any class inheriting from <see cref="UIComponent"/> and call <see cref="UIComponent.Register"/>.
    /// Note: Some objects also require to call the <see cref="UIComponent.Show"/> method.
    /// </summary>
    public class UIController
    {
        internal static UIController instance { get; set; }

        /// <summary>
        /// Get the current screensize.
        /// </summary>
        public Size ScreenSize { get => GTA.UI.Screen.Resolution; }

        private bool isInitialized = false;
        private List<UIComponent> components;
        private Dictionary<Assembly, string[]> assemblyResources;
        private bool isIterating = false;
        private List<UIComponent> componentsToAdd;
        private List<UIComponent> componentsToRemove;

        private bool gameControlWasDisabledLastFrame = false;
        private bool disableGameControl = false;
        private int gameControlDisabledDelayCounter = 0;
        private bool showCursor = false;
        private float previousMouseX = 0;
        private float previousMouseY = 0;
        private float previousMouseAccept = 0;
        private float previousMouseCancel = 0;

        /// <summary>
        /// Get the current instance of UIController.
        /// </summary>
        /// <returns>The current instance of UIController.</returns>
        /// <exception cref="Exception">When no instance has been created.</exception>
        public UIController GetInstance()
        {
            if (instance == null)
            {
                throw new Exception("Unable to get an instance of UIController as no instance exists!");
            }

            return instance;
        }

        /// <summary>
        /// Create a new instance of UIController.
        /// You can only create one instance of this class.
        /// </summary>
        /// <exception cref="Exception">When an instance already exists.</exception>
        public UIController()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new Exception("Cannot create multiple instances of UIController");
            }
        }

        /// <summary>
        /// Initialize the UIController.
        /// </summary>
        public void Initialize()
        {
            if (File.Exists("GTAUI.log"))
            {
                File.Delete("GTAUI.log");
            }

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            components = new List<UIComponent>();
            assemblyResources = new Dictionary<Assembly, string[]>();
            componentsToAdd = new List<UIComponent>();
            componentsToRemove = new List<UIComponent>();

            RegisterAssemblyResources(Assembly.GetExecutingAssembly());

            UIStyle.GetInstance().RegisterStylingProperties("GTAUI.resources.builtinStyleProperties.json");
            UIStyle.GetInstance().DumpStyleProperties();
            Log($"float value: {UIStyle.GetInstance().GetStyleProperty<float>("floatTest")}");

            UIStyle.GetInstance().ApplyStyle("GTAUI.resources.lightStyle.json");
            UIStyle.GetInstance().DumpStyleProperties();

            isInitialized = true;
        }

        /// <summary>
        /// Method to be called on every tick.
        /// </summary>
        public void OnTick()
        {
            if (Game.IsLoading == true || isInitialized == false)
            {
                return;
            }

            if (isIterating == false)
            {
                foreach (UIComponent component in componentsToRemove)
                {
                    components.Remove(component);
                }
                componentsToRemove.Clear();

                foreach (UIComponent component in componentsToAdd)
                {
                    components.Add(component);
                }
                componentsToAdd.Clear();
            }

            HandleGameControl();

            if (components.Any(c => c.NeedsVisibleMouseCursor && c.Visible))
            {
                GTA.Native.Function.Call(GTA.Native.Hash._SET_MOUSE_CURSOR_ACTIVE_THIS_FRAME);
            }

            float cursorX = Game.GetDisabledControlValueNormalized(GTA.Control.CursorX);
            float cursorY = Game.GetDisabledControlValueNormalized(GTA.Control.CursorY);
            float cursorAccept = Game.GetDisabledControlValueNormalized(GTA.Control.CursorAccept);
            float cursorCancel = Game.GetDisabledControlValueNormalized(GTA.Control.CursorCancel);
            float cursorScrollDown = Game.GetDisabledControlValueNormalized(GTA.Control.CursorScrollDown);
            float cursorScrollUp = Game.GetDisabledControlValueNormalized(GTA.Control.CursorScrollUp);

            float actualX = ScreenSize.Width * cursorX;
            float actualY = ScreenSize.Height * cursorY;

            MouseButtons actualMouseButtons = MouseButtons.None;

            if (cursorAccept == 1 && cursorCancel != 1)
            {
                actualMouseButtons = MouseButtons.Left;
            }

            if (cursorCancel == 1 && cursorAccept != 1)
            {
                actualMouseButtons = MouseButtons.Right;
            }

            if (cursorAccept == 1 && cursorCancel == 1)
            {
                actualMouseButtons = MouseButtons.Left | MouseButtons.Right;
            }

            bool shouldFireMouseMove = false;
            bool shouldFireMouseButtons = false;
            bool shouldFireMouseScroll = false;

            if (actualX != previousMouseX || actualY != previousMouseY)
            {
                shouldFireMouseMove = true;
            }

            if (cursorAccept != previousMouseAccept || cursorCancel != previousMouseCancel)
            {
                shouldFireMouseButtons = true;
            }

            if (cursorScrollUp > 0 || cursorScrollDown > 0)
            {
                shouldFireMouseScroll = true;
            }

            isIterating = true;
            foreach (UIComponent component in components)
            {
                if (component.IsInitialized == false)
                {
                    component.FireInitialize();
                    component.IsInitialized = true;
                }

                component.FireUpdate();

                if (component.Visible || component.AlwaysNeedsInput)
                {
                    if (shouldFireMouseMove)
                    {
                        component.FireMouseMove(new PointF(actualX, actualY));
                    }

                    if (shouldFireMouseButtons)
                    {
                        component.FireMouseButtonDown(actualMouseButtons);
                    }

                    if (shouldFireMouseScroll)
                    {
                        if (cursorScrollDown > 0)
                        {
                            component.FireMouseScroll(ScrollDirection.Down);
                        }
                        else
                        {
                            component.FireMouseScroll(ScrollDirection.Up);
                        }
                    }
                }

                if (component.Visible)
                {
                    component.FireRender();
                }
            }

            isIterating = false;

            previousMouseX = actualX;
            previousMouseY = actualY;
            previousMouseAccept = cursorAccept;
            previousMouseCancel = cursorCancel;
        }

        private void HandleGameControl()
        {

            if (gameControlDisabledDelayCounter > 0)
            {
                gameControlDisabledDelayCounter--;
                Game.DisableAllControlsThisFrame();
                return;
            }

            if (gameControlWasDisabledLastFrame == true)
            {
                gameControlDisabledDelayCounter = 20;
                gameControlWasDisabledLastFrame = false;
            }

            if (disableGameControl || gameControlDisabledDelayCounter > 0)
            {
                Game.DisableAllControlsThisFrame();
            }

            if (components.Any(c => c.NeedsGameControlsDisabled && c.Visible) && disableGameControl == false)
            {
                DisableGameControl();

            }
            else if (components.Any(c => c.NeedsGameControlsDisabled && c.Visible) == false && disableGameControl == true)
            {
                EnableGameControl();
            }
        }

        /// <summary>
        /// Method to be called when a key is pushed down.
        /// </summary>
        /// <param name="e">The event arguments</param>
        public void OnKeyDown(KeyEventArgs e)
        {
            isIterating = true;
            foreach (UIComponent component in components)
            {
                if (component.Visible || component.AlwaysNeedsInput)
                {
                    component.FireKeyDown(e);
                }

            }
            isIterating = false;
        }

        /// <summary>
        /// Method to be called when a key is lifted up.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        public void OnKeyUp(KeyEventArgs e)
        {
            isIterating = true;
            foreach (UIComponent component in components)
            {
                if (component.Visible || component.AlwaysNeedsInput)
                {
                    component.FireKeyUp(e);
                }
            }
            isIterating = false;
        }

        /// <summary>
        /// Add a component to the component list handeled by the UIController.
        /// Calling this method is no guarantee that the given component will be added. If the coltroller is currently iterating over all components, the given component will be added next frame (game tick).
        /// </summary>
        /// <param name="component">The component to add</param>
        public void AddComponent(UIComponent component)
        {
            if (isIterating)
            {
                componentsToAdd.Add(component);
            }
            else
            {
                components.Add(component);
            }
        }

        /// <summary>
        /// Remove a component from the component list handeled by the UIController.
        /// Calling this method is no guarantee that the given component will be removed. If the coltroller is currently iterating over all components, the given component will be removed next frame (game tick).
        /// </summary>
        /// <param name="component">The component to add</param>
        public void RemoveComponent(UIComponent component)
        {
            if (isIterating)
            {
                componentsToRemove.Add(component);
            }
            else
            {
                components.Remove(component);
            }
        }

        /// <summary>
        /// Register all embeded resources the given assembly has inside.
        /// This allows you to refer to the resources inside for menu and style definitions.
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterAssemblyResources(Assembly assembly)
        {
            if (assemblyResources.ContainsKey(assembly))
            {
                return;
            }
            string[] resources = assembly.GetManifestResourceNames();
            Log($"Registering the following resources from {assembly}:\n {string.Join("\n", resources)}");
            assemblyResources.Add(assembly, resources);
        }

        /// <summary>
        /// Get a resource fron any of the assemblies registered with <see cref="RegisterAssemblyResources(Assembly)"/> or from the fle system if no embedded resource can be found.
        /// </summary>
        /// <param name="path">The name if it's an embeddable resource or path if it's from the file system.</param>
        /// <returns>The text contents of the resource or null if the resource could not be found.</returns>
        public string GetUIResource(string path)
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

                        using (StreamReader reader = new StreamReader(resourceStream))
                        {
                            return reader.ReadToEnd();
                        }

                    }
                    catch (Exception ex)
                    {
                        Log($"Error while getting UI resource '{path}': {ex}");
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
                
                return File.ReadAllText(path);
            }
            catch(Exception ex)
            {
                Log($"Error while getting UI resource fromn file '{path}': {ex}");
                return null;
            }
        }

        private void DisableGameControl()
        {
            disableGameControl = true;
            gameControlWasDisabledLastFrame = true;
            gameControlDisabledDelayCounter = 0;
        }

        private void EnableGameControl()
        {
            disableGameControl = false;
            gameControlWasDisabledLastFrame = true;
            gameControlDisabledDelayCounter = 0;
        }

        internal static void Log(string message)
        {
            File.AppendAllText("GTAUI.log", DateTime.Now + " : " + message + Environment.NewLine);
        }
    }
}
