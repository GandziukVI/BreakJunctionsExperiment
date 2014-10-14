using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Agilent_U2542A_Setup
{
    public static class ImportantConstants
    {
        public static NumberFormatInfo NumberFormat()
        {
            NumberFormatInfo a = new NumberFormatInfo();
            a.NumberDecimalSeparator = ".";
            a.NumberGroupSeparator = "";
            return a;
        }
        
        public static double[] Ranges = new double[] { 1.25, 2.5, 5, 10 };
        public static int[] CutOffFrequencies = new int[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };
        public static int[] FilterGains = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        public static int[] ProgrammableAmplifierGains = new int[] { 1, 10, 100 };
        public static double Rload;
        public static double K_Ampl_first_Channel;
    }
}
