using GTA.UI;
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
    public class AlertScreen : UIComponent
    {
        private ScaledRectangle backgroundRectangle;
        private ScaledText promptText;
        private ScaledText buttonHelpText;
        private ScaledText descriptionText;

        public string Prompt { get; private set; }
        public string Message { get; private set; }
        public Action Acccepted { get; set; }
        public Action Canceled { get; set; }
        public bool ShowHelpText { get; set; } = true;

        public AlertScreen(string prompt, string message, Action acccepted, Action canceled)
        {
            Prompt = prompt;
            Message = message;
            Acccepted = acccepted;
            Canceled = canceled;

            AlwaysOnTop = true;
            NeedsGameControlsDisabled = true;
        }

        public AlertScreen(string prompt, string message, Action acccepted) : this(prompt, message, acccepted, null)
        {
        }

        public AlertScreen(string prompt, string message) : this(prompt, message, null, null)
        {
        }

        public override void OnInitialize()
        {
            promptText = new ScaledText(new PointF(), Prompt, 1.5f, GTA.UI.Font.Pricedown);
            promptText.Color = Color.FromArgb(240, 200, 80);

            descriptionText = new ScaledText(new PointF(), Message, 0.4f, GTA.UI.Font.ChaletLondon);

            buttonHelpText = new ScaledText(new PointF(), "Press [Enter] to accept, [Escape] to cancel", 0.345f, GTA.UI.Font.ChaletLondon);
            buttonHelpText.Alignment = Alignment.Right;
            buttonHelpText.Position = new PointF(UIController.instance.ScreenSize.Width - 30, UIController.instance.ScreenSize.Height - (30 + buttonHelpText.LineHeight));

            backgroundRectangle = new ScaledRectangle(new PointF(0, 0), new SizeF(UIController.instance.ScreenSize.Width, UIController.instance.ScreenSize.Height));
            backgroundRectangle.Color = Color.Black;



            descriptionText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - descriptionText.Width / 2, 400);
            promptText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - promptText.Width / 2, 300);
        }

        public void SetPrompt(string prompt)
        {
            Prompt = prompt;
            promptText.Text = prompt;
            promptText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - promptText.Width / 2, 300);
        }

        public void SetMessage(string message)
        {
            Message = message;
            descriptionText.Text = message;
            descriptionText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - descriptionText.Width / 2, 400);
        }

        public override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Acccepted?.Invoke();
                Dispose();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Canceled?.Invoke();
                Dispose();
            }
        }

        public override void Render()
        {
            descriptionText.Draw();
            backgroundRectangle.Draw();
            promptText.Draw();

            if (ShowHelpText == true)
            {
                buttonHelpText.Draw();
            }
        }
    }
}
