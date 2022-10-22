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
    /// <summary>
    /// A full screen alert dialog with customizable prompt and message.
    /// Call <see cref="UIComponent.Show"/> to display the alert.
    /// </summary>
    public class AlertScreen : UIComponent
    {
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
        public Action Acccepted { get; set; }

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
        /// <param name="acccepted">Action to execute if the alert was accepted.</param>
        /// <param name="canceled">Action to execute if the alert was canceled.</param>
        public AlertScreen(string prompt, string message, Action acccepted, Action canceled)
        {
            Prompt = prompt;
            Message = message;
            Acccepted = acccepted;
            Canceled = canceled;

            NeedsGameControlsDisabled = true;
        }

        /// <summary>
        /// Create a new Alert screen with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt of the alert.</param>
        /// <param name="message">The message of the alert.</param>
        /// <param name="acccepted">Action to execute if the alert was accepted.</param>
        public AlertScreen(string prompt, string message, Action acccepted) : this(prompt, message, acccepted, null)
        {
        }

        /// <summary>
        /// Create a new Alert screen with the given parameters.
        /// </summary>
        /// <param name="prompt">The prompt of the alert.</param>
        /// <param name="message">The message of the alert.</param>
        public AlertScreen(string prompt, string message) : this(prompt, message, null, null)
        {
        }

        protected override void OnInitialize()
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
                Acccepted?.Invoke();
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
