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
    /// A full screen progress dialog with customizable prompt and message.
    /// Call <see cref="UIComponent.Show"/> to display the alert.
    /// </summary>
    public class ProgressScreen : UIComponent
    {
        private const int PROGRESS_WIDTH = 1500;
        private const int PROGRESS_HEIGHT = 12;

        private ScaledRectangle backgroundRectangle;
        private ScaledText promptText;
        private ScaledText descriptionText;
        private ScaledRectangle progressRectangle;

        /// <summary>
        /// The maximum amount of progress can be set with <see cref="SetProgress(int)"/>. Minimum is always 0.
        /// </summary>
        public int Maximum { get; private set; } = 100;

        /// <summary>
        /// The current amount of progress.
        /// </summary>
        public int CurrentProgress { get; private set; }

        /// <summary>
        /// The prompt of the dialog.
        /// </summary>
        public string Prompt { get; private set; }

        /// <summary>
        /// The Message of the dialog.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Create a new progress screen with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt of the dialog.</param>
        /// <param name="message">The Message of the dialog.</param>
        /// <param name="maximum">The maximum amount of progress can be set with <see cref="SetProgress(int)"/>. Minimum is always 0.</param>
        public ProgressScreen(string prompt, string message, int maximum)
        {
            Prompt = prompt;
            Message = message;
            Maximum = maximum;
        }

        /// <summary>
        /// Create a new progress screen with the given parameters and a <see cref="Maximum"/> of 100.
        /// </summary>
        /// <param name="prompt">The prompt of the dialog.</param>
        /// <param name="message">The Message of the dialog.</param>
        public ProgressScreen(string prompt, string message) : this(prompt, message, 100)
        {
        }

        protected override void OnInitialize()
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

        /// <summary>
        /// Set the progress of the dialog.
        /// </summary>
        /// <param name="progress">The new amount of progress</param>
        public void SetProgress(int progress)
        {
            if (progress > Maximum)
            {
                progress = Maximum;
            }
            
            if (progress < 0)
            {
                progress = 0;
            }

            CurrentProgress = progress;
            progressRectangle.Size = new SizeF(PROGRESS_WIDTH / Maximum * progress, PROGRESS_HEIGHT);
        }

        /// <summary>
        /// Set the maximum amount of progress. Must be larger than 0.
        /// </summary>
        /// <param name="maximum">The new maximum amount of progress.</param>
        public void SetMaximum(int maximum)
        {
            if (maximum < 1)
            {
                maximum = 1;
            }
            Maximum = maximum;
            progressRectangle.Size = new SizeF(PROGRESS_WIDTH / Maximum * CurrentProgress, PROGRESS_HEIGHT);
        }

        /// <summary>
        /// Set the prompt text. Can also be called while the dialog is displaying.
        /// </summary>
        /// <param name="prompt">The new prompt text.</param>
        public void SetPrompt(string prompt)
        {
            Prompt = prompt;
            promptText.Text = prompt;
            promptText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - promptText.Width / 2, 300);
        }

        /// <summary>
        /// Set the message text. Can also be called while the dialog is displaying.
        /// </summary>
        /// <param name="message">The new prompt text.</param>
        public void SetMessage(string message)
        {
            Message = message;
            descriptionText.Text = message;
            descriptionText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - descriptionText.Width / 2, 400);
        }

        protected override void Render()
        {
            descriptionText.Draw();
            backgroundRectangle.Draw();
            promptText.Draw();
            progressRectangle.Draw();
        }
    }
}
