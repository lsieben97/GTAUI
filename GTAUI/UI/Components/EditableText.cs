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
    public class EditableText : UIComponent
    {
        private ScaledText textElement;
        private int blinkDelay = 45;
        private bool showCursor = true;
        private int currentCursorIndex = 0;
        private float currentCursorX = 0;
        private string displayText = string.Empty;

        public string Text { get; set; }
        public float TextSize { get; }
        public Color Color { get; }

        public EditableText(PointF position, string text, float textSize, Color color)
        {
            Position = position;
            Text = text;
            TextSize = textSize;
            Color = color;
        }
        public EditableText(PointF position, string text, float textSize) : this(position, text, textSize, Color.White)
        {
        }

        public EditableText(PointF position, string text) : this(position, text, 0.4f, Color.White)
        {
        }


        public EditableText(PointF position) : this(position, string.Empty, 0.4f, Color.White)
        {
        }

        public override void OnInitialize()
        {
            textElement = new ScaledText(Position, Text);
            textElement.Color = Color;
            textElement.Scale = TextSize;
            currentCursorIndex = Text.Length;
        }

        public override void OnKeyDown(KeyEventArgs e)
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

        public override void Render()
        {
            textElement.Draw();
        }

        public override void Update()
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
