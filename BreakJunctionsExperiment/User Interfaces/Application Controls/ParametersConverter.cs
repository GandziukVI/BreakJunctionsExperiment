using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BreakJunctions
{
    public enum MultiplierRanges
    {
        Pico,
        Nano,
        Micro,
        Mili,
        Santi,
        Decy,
        None
    }

    [ValueConversion(typeof(string), typeof(MultiplierRanges))]
    public class MultiplierConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var mult = (MultiplierRanges)value;

            switch (mult)
            {
                case MultiplierRanges.Pico:
                    return "Pico";
                case MultiplierRanges.Nano:
                    return "Nano";
                case MultiplierRanges.Micro:
                    return "Micro";
                case MultiplierRanges.Mili:
                    return "Mili";
                case MultiplierRanges.Santi:
                    return "Santi";
                case MultiplierRanges.Decy:
                    return "Decy";
                case MultiplierRanges.None:
                    return "None";
                default:
                    return "None";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = value as string;

            switch (text)
            {
                case "Pico":
                    return MultiplierRanges.Pico;
                case "Nano":
                    return MultiplierRanges.Nano;
                case "Micro":
                    return MultiplierRanges.Micro;
                case "Mili":
                    return MultiplierRanges.Mili;
                case "Santi":
                    return MultiplierRanges.Santi;
                case "Decy":
                    return MultiplierRanges.Decy;
                case "None":
                    return MultiplierRanges.None;
                default:
                    return MultiplierRanges.None;
            }
        }
    }

    public static class MultiplierRangesExtension
    {
        public static double AsDouble(this MultiplierRanges Range)
        {
            switch (Range)
            {
                case MultiplierRanges.Pico:
                    return 0.000000000001;
                case MultiplierRanges.Nano:
                    return 0.000000001;
                case MultiplierRanges.Micro:
                    return 0.000000001;
                case MultiplierRanges.Mili:
                    return 0.001;
                case MultiplierRanges.Santi:
                    return 0.01;
                case MultiplierRanges.Decy:
                    return 0.1;
                case MultiplierRanges.None:
                    return 1.0;
                default:
                    return 1.0;
            }
        }
    }
}
