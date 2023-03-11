using GTA.UI;
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
    /// A full screen alert dialog with customizable prompt and message.
    /// Call <see cref="UIComponent.Show"/> to display the alert.
    /// A call to <see cref="UIComponent.Register"/> is required in order to show the alert multiple times.
    /// </summary>
    public class AlertScreen : UIComponent
    {
        private UIStyle uiStyle = UIStyle.GetInstance();
        private ScaledRectangle backgroundRectangle;
        private ScaledText promptText;
        private ScaledText buttonHelpText;
        private ScaledText descriptionText;

        /// <summary>
        /// The prompt of the alert.
        /// </summary>
        public string Prompt { get; private set; }

        /// <summary>
        /// The message of the alert.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Action to execute if the alert was accepted.
        /// </summary>
        public Action Accepted { get; set; }

        /// <summary>
        /// Action to execute if the alert was canceled.
        /// </summary>
        public Action Canceled { get; set; }

        /// <summary>
        /// <c>true</c> when a help text indicating what keys the user can press to accept and cancel the alert must be shown when the alert is visible.
        /// </summary>
        public bool ShowHelpText { get; set; } = true;

        /// <summary>
        /// Create a new Alert screen with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt of the alert.</param>
        /// <param name="message">The message of the alert.</param>
        public AlertScreen(string prompt, string message) : this(prompt, message, null)
        {
        }

        /// <summary>
        /// Create a new Alert screen with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt of the alert.</param>
        /// <param name="message">The message of the alert.</param>
        /// <param name="acccepted">Action to execute if the alert was accepted.</param>
        public AlertScreen(string prompt, string message, Action accepted) : this(prompt, message, accepted, null)
        {
        }

        /// <summary>
        /// Create a new Alert screen with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt of the alert.</param>
        /// <param name="message">The message of the alert.</param>
        /// <param name="acccepted">Action to execute if the alert was accepted.</param>
        /// <param name="canceled">Action to execute if the alert was canceled.</param>
        public AlertScreen(string prompt, string message, Action acccepted, Action canceled)
        {
            Prompt = prompt;
            Message = message;
            Accepted = acccepted;
            Canceled = canceled;

            NeedsGameControlsDisabled = true;

            Register();
        }

        protected override void OnInitialize()
        {
            float promptFontSize = uiStyle.GetStyleProperty<float>("gtaui.alertScreen.promptFontSize");
            GTA.UI.Font promptFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.alertScreen.promptFont");
            Color promptColor = uiStyle.GetStyleProperty<Color>("gtaui.alertScreen.promptColor");
            int promptYPosition = uiStyle.GetStyleProperty<int>("gtaui.alertScreen.promptYPosition");

            float descriptionFontSize = uiStyle.GetStyleProperty<float>("gtaui.alertScreen.descriptionFontSize");
            GTA.UI.Font descriptionFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.alertScreen.descriptionFont");
            Color descriptionColor = uiStyle.GetStyleProperty<Color>("gtaui.alertScreen.descriptionColor");
            int descriptionYPosition = uiStyle.GetStyleProperty<int>("gtaui.alertScreen.descriptionYPosition");

            float helpTextFontSize = uiStyle.GetStyleProperty<float>("gtaui.alertScreen.helpTextFontSize");
            GTA.UI.Font helpTextFont = uiStyle.GetStyleProperty<GTA.UI.Font>("gtaui.alertScreen.helpTextFont");
            Color helpTextColor = uiStyle.GetStyleProperty<Color>("gtaui.alertScreen.helpTextColor");
            Point helpTextOffset = uiStyle.GetStyleProperty<Point>("gtaui.alertScreen.helpTextOffset");
            string helptTextText = uiStyle.GetStyleProperty<string>("gtaui.alertScreen.helpTextText");

            promptText = new ScaledText(new PointF(), Prompt, promptFontSize, promptFont);
            promptText.Color = promptColor;
            promptText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - promptText.Width / 2, promptYPosition);

            descriptionText = new ScaledText(new PointF(), Message, descriptionFontSize, descriptionFont);
            descriptionText.Color = descriptionColor;
            descriptionText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - descriptionText.Width / 2, descriptionYPosition);

            buttonHelpText = new ScaledText(new PointF(), helptTextText, helpTextFontSize, helpTextFont);
            buttonHelpText.Alignment = Alignment.Right;
            buttonHelpText.Color = helpTextColor;
            buttonHelpText.Position = new PointF(UIController.instance.ScreenSize.Width - helpTextOffset.X, UIController.instance.ScreenSize.Height - (helpTextOffset.Y + buttonHelpText.LineHeight));

            backgroundRectangle = new ScaledRectangle(new PointF(0, 0), new SizeF(UIController.instance.ScreenSize.Width, UIController.instance.ScreenSize.Height));
            backgroundRectangle.Color = uiStyle.GetStyleProperty<Color>("gtaui.alertScreen.backgroundColor");
        }

        /// <summary>
        /// Set the prompt text. Can also be called while the alert is displaying.
        /// </summary>
        /// <param name="prompt">The new prompt text.</param>
        public void SetPrompt(string prompt)
        {
            Prompt = prompt;
            promptText.Text = prompt;
            promptText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - promptText.Width / 2, 300);
        }

        /// <summary>
        /// Set the message text. Can also be called while the alert is displaying.
        /// </summary>
        /// <param name="message">The new prompt text.</param>
        public void SetMessage(string message)
        {
            Message = message;
            descriptionText.Text = message;
            descriptionText.Position = new PointF(UIController.instance.ScreenSize.Width / 2 - descriptionText.Width / 2, 400);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Accepted?.Invoke();
                Dispose();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Canceled?.Invoke();
                Dispose();
            }
        }

        protected override void Render()
        {
            backgroundRectangle.Draw();
            descriptionText.Draw();
            promptText.Draw();

            if (ShowHelpText == true)
            {
                buttonHelpText.Draw();
            }
        }
    }
}
