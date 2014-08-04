using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BreakJunctions
{
    public static class HandlingUserInput
    {
        private static bool IsIntegerText(string text)
        {
            Regex regex = new Regex(@"^-?\d*");

            return regex.Match(text).Length == text.Length;
        }
        private static bool IsFloatingPointText(string text)
        {
            Regex regex = new Regex(@"-?\d*\.?\d*");

            return regex.Match(text).Length == text.Length;
        }
        public static void IntegerPastingHandler(ref object sender, ref DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!HandlingUserInput.IsIntegerText(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
        public static void FloatingPointPastingHandler(ref object sender, ref DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!HandlingUserInput.IsFloatingPointText(text)) e.CancelCommand();
            }
            else e.CancelCommand();
        }
        public static void OnIntegerTextChanged(ref object sender, ref TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Int32 selectionStart = textBox.SelectionStart;
            Int32 selectionLength = textBox.SelectionLength;

            string newText = string.Empty;

            if (textBox.Text.Length >= 1)
                newText = textBox.Text.Remove(textBox.Text.Length - 1);

            if (!IsIntegerText(textBox.Text))
                textBox.Text = newText;

            textBox.SelectionStart = selectionStart <= textBox.Text.Length ?
                selectionStart : textBox.Text.Length;
        }
        public static void OnFloatingPointTextChanged(ref object sender, ref TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Int32 selectionStart = textBox.SelectionStart;
            Int32 selectionLength = textBox.SelectionLength;

            string newText = string.Empty;

            if (textBox.Text.Length >= 1)
                newText = textBox.Text.Remove(textBox.Text.Length - 1);

            if (!IsFloatingPointText(textBox.Text))
                textBox.Text = newText;

            textBox.SelectionStart = selectionStart <= textBox.Text.Length ?
                selectionStart : textBox.Text.Length; 
        }
        public static double GetMultiplier(string Multiplier)
        {
            var ValueMultiplier = 1.0;

            switch (Multiplier)
            {
                case "None":
                    {
                        ValueMultiplier = 1.0;
                        return ValueMultiplier;
                    }
                case "Deci":
                    {
                        ValueMultiplier = 0.1;
                        return ValueMultiplier;
                    }
                case "Senti":
                    {
                        ValueMultiplier = 0.01;
                        return ValueMultiplier;
                    }
                case "Mili":
                    {
                        ValueMultiplier = 0.001;
                        return ValueMultiplier;
                    }
                case "Micro":
                    {
                        ValueMultiplier = 0.000001;
                        return ValueMultiplier;
                    }
                case "Nano":
                    {
                        ValueMultiplier = 0.000000001;
                        return ValueMultiplier;
                    }
                case "Pico":
                    {
                        ValueMultiplier = 0.000000000001;
                        return ValueMultiplier;
                    }
                default:
                    break;
            }

            return ValueMultiplier;
        }
    }
}
