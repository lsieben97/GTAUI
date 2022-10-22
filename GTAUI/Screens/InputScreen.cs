using GTA.UI;
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
    /// </summary>
    public class InputScreen : UIComponent
    {
        private const int INPUT_BOX_WIDTH = 1000;
        private const int INPUT_BOX_HEIGHT = 50;

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
        /// <param name="inputCanceled">Action to execute when the user cancels the dialog.</param>
        public InputScreen(string prompt, string message, Action<string> inputEntered, Action inputCanceled)
        {
            Prompt = prompt;
            Message = message;
            InputEntered = inputEntered;
            InputCanceled = inputCanceled;
            NeedsGameControlsDisabled = true;
        }

        /// <summary>
        /// Create a new input dialog with the given parameters
        /// </summary>
        /// <param name="prompt">The prompt of the dialog.</param>
        /// <param name="message">The message of the dialog.</param>
        /// <param name="inputEntered">Action to execute when the user accepts the entered input.</param>
        public InputScreen(string prompt, string message, Action<string> inputEntered) : this(prompt, message, inputEntered, null)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            promptText = new ScaledText(new PointF(), string.Empty, 1.5f, GTA.UI.Font.Pricedown);
            promptText.Color = Color.FromArgb(240, 200, 80);

            descriptionText = new ScaledText(new PointF(), string.Empty, 0.4f, GTA.UI.Font.ChaletLondon);

            buttonHelpText = new ScaledText(new PointF(), "Press [Enter] to confirm input, [Escape] to cancel", 0.345f, GTA.UI.Font.ChaletLondon);
            buttonHelpText.Alignment = Alignment.Right;
            buttonHelpText.Position = new PointF(GTA.UI.Screen.Resolution.Width - 30, GTA.UI.Screen.Resolution.Height - (30 + buttonHelpText.LineHeight));

            backgroundRectangle = new ScaledRectangle(new PointF(0, 0), new SizeF(GTA.UI.Screen.Resolution.Width, GTA.UI.Screen.Resolution.Height));
            backgroundRectangle.Color = Color.Black;

            float inputBoxOrigin = GTA.UI.Screen.Resolution.Width / 2 - INPUT_BOX_WIDTH / 2;
            topBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin, 450), new SizeF(INPUT_BOX_WIDTH, 3));
            leftBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin, 450), new SizeF(3, INPUT_BOX_HEIGHT));
            bottomBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin, 450 + INPUT_BOX_HEIGHT), new SizeF(INPUT_BOX_WIDTH, 3));
            rightBorderRectangle = new ScaledRectangle(new PointF(inputBoxOrigin + INPUT_BOX_WIDTH, 450), new SizeF(3, INPUT_BOX_HEIGHT));

            topBorderRectangle.Color = Color.White;
            leftBorderRectangle.Color = Color.White;
            bottomBorderRectangle.Color = Color.White;
            rightBorderRectangle.Color = Color.White;

            promptText.Text = Prompt;
            descriptionText.Text = Message;
            descriptionText.Position = new PointF(GTA.UI.Screen.Resolution.Width / 2 - descriptionText.Width / 2, 400);
            promptText.Position = new PointF(GTA.UI.Screen.Resolution.Width / 2 - promptText.Width / 2, 300);

            editableText = new EditableText(new PointF(inputBoxOrigin + 2, 452));
            AddChildComponent(editableText);
        }

        public override void Show()
        {
            base.Show();
            editableText.HasFocus = true;
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
