using GTAUI.Styling;
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
    /// A call to <see cref="UIComponent.Register"/> is required in order to show the progress screen multiple times.
    /// </summary>
    public class ProgressScreen : UIComponent
    {

        private readonly UIStyle uiStyle = UIStyle.GetInstance();
        private ScaledRectangle backgroundRectangle;
        private ScaledText promptText;
        private ScaledText descriptionText;
        private ScaledRectangle progressRectangle;
        private Point progressRectangleSize;
        private int progressRectangleYPosition;

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
        /// Create a new progress screen with the given parameters and a <see cref="Maximum"/> of 100.
        /// </summary>
        /// <param name="prompt">The prompt of the dialog.</param>
        /// <param name="message">The Message of the dialog.</param>
        public ProgressScreen(string prompt, string message) : this(prompt, message, 100) { }

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

            NeedsGameControlsDisabled = true;
            Register();
        }

        protected override void OnInitialize()
        {
            float promptFontSize = uiStyle.GetStyleProperty<float>("gtaui.progressScreen.promptFontSize");
            GTA.UI.Font promptFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.progressScreen.promptFont");
            Color promptColor = uiStyle.GetStyleProperty<Color>("gtaui.progressScreen.promptColor");
            int promptYPosition = uiStyle.GetStyleProperty<int>("gtaui.progressScreen.promptYPosition");

            float descriptionFontSize = uiStyle.GetStyleProperty<float>("gtaui.progressScreen.descriptionFontSize");
            GTA.UI.Font descriptionFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.progressScreen.descriptionFont");
            Color descriptionColor = uiStyle.GetStyleProperty<Color>("gtaui.progressScreen.descriptionColor");
            int descriptionYPosition = uiStyle.GetStyleProperty<int>("gtaui.progressScreen.descriptionYPosition");

            progressRectangleSize = uiStyle.GetStyleProperty<Point>("gtaui.progressScreen.progressRectangleSize");
            progressRectangleYPosition = uiStyle.GetStyleProperty<int>("gtaui.progressScreen.progressRectangleYposition");

            promptText = new ScaledText(new PointF(), Prompt, promptFontSize, promptFont);
            promptText.Color = promptColor;
            promptText.Position = new PointF(UIController.GetInstance().ScreenSize.Width / 2 - promptText.Width / 2, promptYPosition);

            descriptionText = new ScaledText(new PointF(), Message, descriptionFontSize, descriptionFont);
            descriptionText.Color = descriptionColor;
            descriptionText.Position = new PointF(UIController.GetInstance().ScreenSize.Width / 2 - descriptionText.Width / 2, descriptionYPosition);

            backgroundRectangle = new ScaledRectangle(new PointF(0, 0), new SizeF(UIController.GetInstance().ScreenSize.Width, UIController.GetInstance().ScreenSize.Height));
            backgroundRectangle.Color = uiStyle.GetStyleProperty<Color>("gtaui.progressScreen.backgroundColor");

            progressRectangle = new ScaledRectangle(new PointF(), new SizeF());
            progressRectangle.Color = uiStyle.GetStyleProperty<Color>("gtaui.progressScreen.progressRectangleColor");
            progressRectangle.Position = new PointF(GTA.UI.Screen.Resolution.Width / 2 - progressRectangleSize.X / 2, progressRectangleYPosition);


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
            if (IsInitialized)
            {
                progressRectangle.Size = new SizeF(progressRectangleSize.X / Maximum * progress, progressRectangleSize.Y);
            }
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

            if (IsInitialized)
            {
                progressRectangle.Size = new SizeF(progressRectangleSize.X / Maximum * CurrentProgress, progressRectangleSize.Y);
            }
        }

        /// <summary>
        /// Set the prompt text. Can also be called while the dialog is displaying.
        /// </summary>
        /// <param name="prompt">The new prompt text.</param>
        public void SetPrompt(string prompt)
        {
            Prompt = prompt;
            if (IsInitialized)
            {
                promptText.Text = prompt;
                promptText.Position = new PointF(UIController.GetInstance().ScreenSize.Width / 2 - promptText.Width / 2, 300);
            }
        }

        /// <summary>
        /// Set the message text. Can also be called while the dialog is displaying.
        /// </summary>
        /// <param name="message">The new prompt text.</param>
        public void SetMessage(string message)
        {
            Message = message;
            if (IsInitialized)
            {
                descriptionText.Text = message;
                descriptionText.Position = new PointF(UIController.GetInstance().ScreenSize.Width / 2 - descriptionText.Width / 2, 400);
            }
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
