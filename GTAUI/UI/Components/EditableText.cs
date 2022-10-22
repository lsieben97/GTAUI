using LemonUI.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTAUI.UI.Components
{
    /// <summary>
    /// A single line of text that show a blinking cursor at the insertion point.
    /// Takes keyboard input as long as <see cref="UIComponent.HasFocus"/> is <c>true</c>.
    /// </summary>
    public class EditableText : UIComponent
    {
        private ScaledText textElement;
        private int blinkDelay = 45;
        private bool showCursor = true;
        private int currentCursorIndex = 0;
        private string displayText = string.Empty;

        /// <summary>
        /// The text that can be edited.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// The size of the text being rendered.
        /// </summary>
        public float TextSize { get; }

        /// <summary>
        /// The color of the text being rendered.
        /// </summary>
        public Color Color { get; }

        /// <summary>
        /// The font of the text being rendered.
        /// </summary>
        public GTA.UI.Font Font { get; }


        /// <summary>
        /// Create a new EditableText with the given parameters.
        /// </summary>
        /// <param name="position">The position to render the text.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="textSize">The size of the text to render.</param>
        /// <param name="color">The color of the text to render.</param>
        /// <param name="font">The font of the text to render.</param>
        public EditableText(PointF position, string text, float textSize, Color color, GTA.UI.Font font)
        {
            Position = position;
            Text = text;
            TextSize = textSize;
            Color = color;
            Font = font;
        }

        /// <summary>
        /// Create a new EditableText with the given parameters.
        /// </summary>
        /// <param name="position">The position to render the text.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="textSize">The size of the text to render.</param>
        /// <param name="color">The color of the text to render.</param>
        public EditableText(PointF position, string text, float textSize, Color color) : this(position, text, textSize, color, GTA.UI.Font.ChaletLondon)
        {
        }
        /// <summary>
        /// Create a new EditableText with the given parameters.
        /// </summary>
        /// <param name="position">The position to render the text.</param>
        /// <param name="text">The text to edit.</param>
        /// <param name="textSize">The size of the text to render.</param>
        public EditableText(PointF position, string text, float textSize) : this(position, text, textSize, Color.White, GTA.UI.Font.ChaletLondon)
        {
        }

        /// <summary>
        /// Create a new EditableText with the given parameters.
        /// </summary>
        /// <param name="position">The position to render the text.</param>
        /// <param name="text">The text to edit.</param>
        public EditableText(PointF position, string text) : this(position, text, 0.4f, Color.White, GTA.UI.Font.ChaletLondon)
        {
        }

        /// <summary>
        /// Create a new EditableText with the given parameters.
        /// </summary>
        /// <param name="position">The position to render the text.</param>
        public EditableText(PointF position) : this(position, string.Empty, 0.4f, Color.White, GTA.UI.Font.ChaletLondon)
        {
        }

        /// <summary>
        /// Set the text to be edited.
        /// </summary>
        /// <param name="text">The new text.</param>
        public void SetText(string text)
        {
            Text = text;
            if (currentCursorIndex > Text.Length)
            {
                currentCursorIndex = Text.Length;
            }
            UpdateDisplayText();
        }

        protected override void OnInitialize()
        {
            textElement = new ScaledText(Position, Text);
            textElement.Color = Color;
            textElement.Scale = TextSize;
            currentCursorIndex = Text.Length;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (HasFocus == false)
            {
                return;
            }

            UIController.Log($"Receiving key down event for EditableText {e.KeyCode}");
            UIController.Log(Environment.StackTrace);
            if (e.KeyCode == Keys.Back)
            {
                if (currentCursorIndex - 1>= 0)
                {
                    Text = Text.Remove(currentCursorIndex - 1, 1);
                    currentCursorIndex--;
                    showCursor = false;
                    blinkDelay = 0;
                }
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (currentCursorIndex != Text.Length)
                {
                    Text = Text.Substring(0, Text.Length - 1);
                    showCursor = false;
                    blinkDelay = 0;
                }
            }

            if (e.KeyCode.RepresentsPrintableChar())
            {
                string newChar = e.KeyCode.GetString().ToLower();
                if (e.Shift == true)
                {
                    newChar = newChar.ToUpper();
                }

                AddCharacter(newChar);
                showCursor = false;
                blinkDelay = 0;
            }

            if (e.KeyCode == Keys.Right && currentCursorIndex != Text.Length)
            {
                currentCursorIndex++;
                showCursor = false;
                blinkDelay = 0;
            }

            if (e.KeyCode == Keys.Left && currentCursorIndex != 0)
            {
                currentCursorIndex--;
                showCursor = false;
                blinkDelay = 0;
            }

            UpdateDisplayText();
        }

        private void AddCharacter(string character)
        {
            if (currentCursorIndex == Text.Length)
            {
                Text += character;
            }
            else
            {
                Text = Text.Insert(currentCursorIndex, character);
            }

            currentCursorIndex++;
            
        }

        private void UpdateDisplayText()
        {
            if (showCursor)
            {
                if (currentCursorIndex == Text.Length)
                {
                    displayText = Text + "|";
                }
                else
                {
                    displayText = new string(Text.ToArray());
                    //displayText = displayText.Remove(currentCursorIndex, 1);
                    displayText = displayText.Insert(currentCursorIndex, "|");
                }
            }
            else
            {
                if (currentCursorIndex == Text.Length)
                {
                    displayText = Text;
                }
                else
                {
                    displayText = new string(Text.ToArray());
                    //displayText = displayText.Remove(currentCursorIndex, 1);
                    displayText = displayText.Insert(currentCursorIndex, " ");
                }
            }
        }

        protected override void Render()
        {
            textElement.Draw();
        }

        protected override void Update()
        {
            if (blinkDelay != 0)
            {
                blinkDelay--;
                return;
            }

            showCursor = !showCursor;
            blinkDelay = 45;

            textElement.Text = displayText;

            UpdateDisplayText();
            textElement.Text = displayText;
        }
    }
}
