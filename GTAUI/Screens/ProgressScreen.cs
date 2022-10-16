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
    public class ProgressScreen : UIComponent
    {
        private const int PROGRESS_WIDTH = 1500;
        private const int PROGRESS_HEIGHT = 12;

        private ScaledRectangle backgroundRectangle;
        private ScaledText promptText;
        private ScaledText descriptionText;
        private ScaledRectangle progressRectangle;

        public int Maximum { get; set; } = 100;
        public int CurrentProgress { get; private set; }
        public string Prompt { get; private set; }
        public string Message { get; private set; }

        public ProgressScreen(string prompt, string message, int maximum)
        {
            Prompt = prompt;
            Message = message;
            Maximum = maximum;
        }

        public ProgressScreen(string prompt, string message) : this(prompt, message, 100)
        {
        }

        public override void OnInitialize()
        {
            promptText = new ScaledText(new PointF(), Prompt, 1.5f, GTA.UI.Font.Pricedown);
            promptText.Color = Color.FromArgb(240, 200, 80);

            descriptionText = new ScaledText(new PointF(), Message, 0.4f, GTA.UI.Font.ChaletLondon);

            backgroundRectangle = new ScaledRectangle(new PointF(0, 0), new SizeF(UIController.instance.ScreenSize.Width, UIController.instance.ScreenSize.Height));
            backgroundRectangle.Color = Color.Black;

            progressRectangle = new ScaledRectangle(new PointF(), new SizeF());

            descriptionText.Position = new PointF(GTA.UI.Screen.Resolution.Width / 2 - descriptionText.Width / 2, 400);
            promptText.Position = new PointF(GTA.UI.Screen.Resolution.Width / 2 - promptText.Width / 2, 300);
            progressRectangle.Position = new PointF(GTA.UI.Screen.Resolution.Width / 2 - PROGRESS_WIDTH / 2, 480);
        }

        public void SetProgress(int progress)
        {
            if (progress > Maximum)
            {
                progress = Maximum;
            }

            CurrentProgress = progress;
            progressRectangle.Size = new SizeF(PROGRESS_WIDTH / Maximum * progress, PROGRESS_HEIGHT);
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

        public override void Render()
        {
            descriptionText.Draw();
            backgroundRectangle.Draw();
            promptText.Draw();
            progressRectangle.Draw();
        }
    }
}
