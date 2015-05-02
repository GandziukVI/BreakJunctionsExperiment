using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.AdditionalClasses
{
    public class Keithley4200_RangeAccuracySet : IEquatable<Keithley4200_RangeAccuracySet>
    {
        public double MinRangeLimit { get; set; }
        public double MaxRangeLimit { get; set; }
        public IntegrationTime Accuracy { get; set; }

        public Keithley4200_RangeAccuracySet(double _MinRangeLimit, double _MaxRangeLimit, IntegrationTime _Accuracy)
        {
            MinRangeLimit = _MinRangeLimit;
            MaxRangeLimit = _MaxRangeLimit;
            Accuracy = _Accuracy;
        }

        public bool Equals(Keithley4200_RangeAccuracySet Other)
        {
            return (MinRangeLimit == Other.MinRangeLimit) && (MaxRangeLimit == Other.MaxRangeLimit) && (Accuracy == Other.Accuracy);
        }
    }
}
