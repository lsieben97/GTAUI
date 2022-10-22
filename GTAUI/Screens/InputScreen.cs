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

        public string Prompt { get; }
        public string Message { get; }
        public bool ShowHelpText { get; set; }

        public Action<string> InputEntered { get; set; }
        public Action InputCanceled { get; set; }


        public InputScreen(string prompt, string message, Action<string> inputEntered, Action inputCanceled)
        {
            Prompt = prompt;
            Message = message;
            InputEntered = inputEntered;
            InputCanceled = inputCanceled;
            NeedsGameControlsDisabled = true;
        }

        public InputScreen(string prompt, string message, Action<string> inputEntered) : this(prompt, message, inputEntered, null)
        {
        }

        public override void OnInitialize()
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

            editableText = new EditableText(new PointF(inputBoxOrigin + 2, 452), string.Empty, 0.4f, Color.White);
            AddChildComponent(editableText);
        }

        public override void Show()
        {
            base.Show();
            editableText.HasFocus = true;
        }

        public override void OnKeyDown(KeyEventArgs e)
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

        public override void Render()
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
