using GTA;
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
    public class UIController
    {
        internal static UIController instance { get; set; }

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
        private ScaledText TEMP_debugText;
        private ScaledRectangle TEMP_debugRectangle;
        private float previousMouseX = 0;
        private float previousMouseY = 0;
        private float previousMouseAccept = 0;
        private float previousMouseCancel = 0;

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

        public void Initialize()
        {
            components = new List<UIComponent>();
            assemblyResources = new Dictionary<Assembly, string[]>();
            componentsToAdd = new List<UIComponent>();
            componentsToRemove = new List<UIComponent>();

            isInitialized = true;
            TEMP_debugText = new ScaledText(new PointF(50, 50), "");
            TEMP_debugText.Color = Color.Red;
            TEMP_debugText.Scale = 0.5f;

            TEMP_debugRectangle = new ScaledRectangle(new PointF(), new SizeF(10, 10));
            TEMP_debugRectangle.Color = Color.Green;
        }

        public void OnTick()
        {
            if (Game.IsLoading == true || isInitialized == false)
            {
                return;
            }

            if (isIterating == false)
            {
                foreach(UIComponent component in componentsToRemove)
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
            foreach (UIComponent component in components.Where(c => c.AlwaysOnTop == false))
            {
                if (component.IsInitialized == false)
                {
                    component.FireInitialize();
                    component.IsInitialized = true;
                }

                component.FireUpdate();


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

                if (component.Visible)
                {
                    component.FireRender();
                }
            }

            foreach (UIComponent component in components.Where(c => c.AlwaysOnTop == true))
            {
                if (component.IsInitialized == false)
                {
                    component.FireInitialize();
                    component.IsInitialized = true;
                }

                component.FireUpdate();


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
            if (components.Any(c => c.NeedsGameControlsDisabled && c.Visible) && disableGameControl == false)
            {
                DisableGameControl();

            }
            else if (components.Any(c => c.NeedsGameControlsDisabled && c.Visible) == false && disableGameControl == true)
            {
                EnableGameControl();
            }

            if (gameControlDisabledDelayCounter > 0)
            {
                gameControlDisabledDelayCounter--;
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
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            isIterating = true;
            foreach (UIComponent component in components)
            {
                component.FireKeyDown(e);
            }
            isIterating = false;
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            isIterating = true;
            foreach (UIComponent component in components)
            {
                component.FireKeyUp(e);
            }
            isIterating = false;

            if (e.KeyCode == Keys.Pause)
            {
                if (disableGameControl)
                {
                    disableGameControl = false;
                    GTA.UI.Notification.Show("Enable game control");
                }
                else
                {
                    disableGameControl = true;
                    GTA.UI.Notification.Show("Disable game control");
                }
            }
            else if (e.KeyCode == Keys.NumPad0)
            {
                if (showCursor)
                {
                    showCursor = false;
                    GTA.UI.Notification.Show("Enable cursor");
                }
                else
                {
                    showCursor = true;
                    GTA.UI.Notification.Show("Disable cursor");
                }
            }
        }

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

        private void DisableGameControl()
        {
            disableGameControl = true;
            gameControlWasDisabledLastFrame = true;
            gameControlDisabledDelayCounter = 0;
        }

        private void EnableGameControl()
        {
            disableGameControl = false;
        }

        internal static void Log(string message)
        {
            File.AppendAllText("GTAUI.log", DateTime.Now + " : " + message + Environment.NewLine);
        }
    }
}
