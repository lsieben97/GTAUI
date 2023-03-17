using GTA.UI;
using GTAUI.Styling;
using GTAUI.UI.Components;
using LemonUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI.Screens
{
    /// <summary>
    /// Full screen input dialog with a customizable prompt and message.
    /// Call <see cref="UIComponent.Show"/> to display the dialog.
    /// A call to <see cref="UIComponent.Register"/> is required in order to show the input screen multiple times.
    /// </summary>
    public class InputScreen : UIComponent
    {
        private readonly UIStyle uiStyle = UIStyle.GetInstance();

        private ScaledRectangle backgroundRectangle;
        private ScaledRectangle topBorderRectangle;
        private ScaledRectangle leftBorderRectangle;
        private ScaledRectangle rightBorderRectangle;
        private ScaledRectangle bottomBorderRectangle;
        private ScaledText promptText;
        private ScaledText buttonHelpText;
        private ScaledText descriptionText;

        private EditableText editableText;

        /// <summary>
        /// The prompt of the dialog.
        /// </summary>
        public string Prompt { get; }

        /// <summary>
        /// The message of the dialog.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// <c>true</c> when a help text indicating what keys the user can press to accept and cancel the dialog must be shown when the dialog is visible.
        /// </summary>
        public bool ShowHelpText { get; set; }

        /// <summary>
        /// Action to execute when the user accepts the entered input.
        /// </summary>
        public Action<string> InputEntered { get; set; }

        /// <summary>
        /// Action to execute when the user cancels the dialog.
        /// </summary>
        public Action InputCanceled { get; set; }

        /// <summary>
        /// Create a new input dialog with the given parameters
        /// </summary>
        /// <param name="prompt">The prompt of the dialog.</param>
        /// <param name="message">The message of the dialog.</param>
        /// <param name="inputEntered">Action to execute when the user accepts the entered input.</param>
        public InputScreen(string prompt, string message, Action<string> inputEntered) : this(prompt, message, inputEntered, null) { }

        /// <summary>
        /// Create a new input dialog with the given parameters
        /// </summary>
        /// <param name="prompt">The prompt of the dialog.</param>
        /// <param name="message">The message of the dialog.</param>
        /// <param name="inputEntered">Action to execute when the user accepts the entered input.</param>
        /// <param name="inputCanceled">Action to execute when the user cancels the dialog.</param>
        public InputScreen(string prompt, string message, Action<string> inputEntered, Action inputCanceled)
        {
            Prompt = prompt;
            Message = message;
            InputEntered = inputEntered;
            InputCanceled = inputCanceled;

            NeedsGameControlsDisabled = true;
            NeedsStartTimeout = true;
            Register();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            float promptFontSize = uiStyle.GetStyleProperty<float>("gtaui.inputScreen.promptFontSize");
            GTA.UI.Font promptFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.inputScreen.promptFont");
            Color promptColor = uiStyle.GetStyleProperty<Color>("gtaui.inputScreen.promptColor");
            int promptYPosition = uiStyle.GetStyleProperty<int>("gtaui.inputScreen.promptYPosition");

            float descriptionFontSize = uiStyle.GetStyleProperty<float>("gtaui.inputScreen.descriptionFontSize");
            GTA.UI.Font descriptionFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.inputScreen.descriptionFont");
            Color descriptionColor = uiStyle.GetStyleProperty<Color>("gtaui.inputScreen.descriptionColor");
            int descriptionYPosition = uiStyle.GetStyleProperty<int>("gtaui.inputScreen.descriptionYPosition");

            float helpTextFontSize = uiStyle.GetStyleProperty<float>("gtaui.inputScreen.helpTextFontSize");
            GTA.UI.Font helpTextFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.inputScreen.helpTextFont");
            Color helpTextColor = uiStyle.GetStyleProperty<Color>("gtaui.inputScreen.helpTextColor");
            Point helpTextOffset = uiStyle.GetStyleProperty<Point>("gtaui.inputScreen.helpTextOffset");
            string helpTextText = uiStyle.GetStyleProperty<string>("gtaui.inputScreen.helpTextText");

            Color inputBoxColor = uiStyle.GetStyleProperty<Color>("gtaui.inputScreen.inputBoxColor");
            int inputBoxYOffset = uiStyle.GetStyleProperty<int>("gtaui.inputScreen.inputBoxYOffset");
            int inputBoxBorderThickness = uiStyle.GetStyleProperty<int>("gtaui.inputScreen.inputBoxBorderThickness");
            Point inputBoxSize = uiStyle.GetStyleProperty<Point>("gtaui.inputScreen.inputBoxSize");

            float inputTextFontSize = uiStyle.GetStyleProperty<float>("gtaui.inputScreen.inputTextFontSize");
            GTA.UI.Font inputTextFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.inputScreen.inputTextFont");
            Color inputTextColor = uiStyle.GetStyleProperty<Color>("gtaui.inputScreen.inputTextColor");

            promptText = new ScaledText(new PointF(), Prompt, promptFontSize, promptFont);
            promptText.Color = promptColor;
            promptText.Position = new PointF(UIController.GetInstance().ScreenSize.Width / 2 - promptText.Width / 2, promptYPosition);

            descriptionText = new ScaledText(new PointF(), Message, descriptionFontSize, descriptionFont);
            descriptionText.Color = descriptionColor;
            descriptionText.Position = new PointF(UIController.GetInstance().ScreenSize.Width / 2 - descriptionText.Width / 2, descriptionYPosition);

            buttonHelpText = new ScaledText(new PointF(), helpTextText, helpTextFontSize, helpTextFont);
            buttonHelpText.Alignment = Alignment.Right;
            buttonHelpText.Color = helpTextColor;
            buttonHelpText.Position = new PointF(UIController.GetInstance().ScreenSize.Width - helpTextOffset.X, UIController.GetInstance().ScreenSize.Height - (helpTextOffset.Y + buttonHelpText.LineHeight));

            backgroundRectangle = new ScaledRectangle(new PointF(0, 0), new SizeF(UIController.GetInstance().ScreenSize.Width, UIController.GetInstance().ScreenSize.Height));
            backgroundRectangle.Color = uiStyle.GetStyleProperty<Color>("gtaui.inputScreen.backgroundColor");

            float inputBoxOrigin = GTA.UI.Screen.Resolution.Width / 2 - inputBoxSize.X / 2;
            topBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin, inputBoxYOffset), new SizeF(inputBoxSize.X, inputBoxBorderThickness));
            leftBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin, inputBoxYOffset), new SizeF(inputBoxBorderThickness, inputBoxSize.Y));
            bottomBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin, inputBoxYOffset + inputBoxSize.Y), new SizeF(inputBoxSize.X, inputBoxBorderThickness));
            rightBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin + inputBoxSize.X, inputBoxYOffset), new SizeF(inputBoxBorderThickness, inputBoxSize.Y));

            topBorderRectangle.Color = inputBoxColor;
            leftBorderRectangle.Color = inputBoxColor;
            bottomBorderRectangle.Color = inputBoxColor;
            rightBorderRectangle.Color = inputBoxColor;

            editableText = new EditableText(new PointF(inputBoxOrigin + inputBoxBorderThickness - 1, inputBoxYOffset + inputBoxBorderThickness - 1), string.Empty, inputTextFontSize, inputTextColor, inputTextFont);
            AddChildComponent(editableText);

            if (Visible)
            {
                editableText.HasFocus = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                editableText.HasFocus = false;
                Dispose();
                InputCanceled?.Invoke();
            }

            if (e.KeyCode == Keys.Enter)
            {
                editableText.HasFocus = false;
                Dispose();
                InputEntered?.Invoke(editableText.Text);
            }
        }

        protected override void Render()
        {
            descriptionText.Draw();
            backgroundRectangle.Draw();
            promptText.Draw();

            topBorderRectangle.Draw();
            leftBorderRectangle.Draw();
            bottomBorderRectangle.Draw();
            rightBorderRectangle.Draw();

            if (ShowHelpText == true)
            {
                buttonHelpText.Draw();
            }
        }
    }
}
