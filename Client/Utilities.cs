using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Utilities
    {
        public static void SetTextBoxPlaceholder(System.Windows.Forms.TextBox textBox, string placeholder)
        {
            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == placeholder)
                {
                    textBox.Text = ""; // Clear the placeholder text
                }
            };

            // Event handler for when the TextBox loses focus
            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = placeholder; // Restore the placeholder text
                }
            };
        }
    }
}
