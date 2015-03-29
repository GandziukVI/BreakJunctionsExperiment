using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BreakJunctions
{
    [ValueConversion(typeof(double), typeof(string))]
    public class MultiplierConversion : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var ValueMultiplier = (double)value;
                var Multiplier = "None";

                if (ValueMultiplier == 1.0)
                {
                    Multiplier = "None";
                    return Multiplier;
                }
                else if (ValueMultiplier == 0.1)
                {
                    Multiplier = "Deci";
                    return Multiplier;
                }
                else if (ValueMultiplier == 0.01)
                {
                    Multiplier = "Senti";
                    return Multiplier;
                }
                else if (ValueMultiplier == 0.001)
                {
                    Multiplier = "Mili";
                    return Multiplier;
                }
                else if (ValueMultiplier == 0.000001)
                {
                    Multiplier = "Micro";
                    return Multiplier;
                }
                else if (ValueMultiplier == 0.000000001)
                {
                    Multiplier = "Nano";
                    return Multiplier;
                }
                else if (ValueMultiplier == 0.000000000001)
                {
                    Multiplier = "Pico";
                    return Multiplier;
                }
                else
                    return Multiplier;
            }
            catch { return "None"; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var Multiplier = (string)value;
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
