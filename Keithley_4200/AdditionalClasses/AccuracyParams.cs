using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200
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

    public class AccuracyParams
    {
        public ObservableCollection<Keithley4200_RangeAccuracySet> RangeAccuracySet { get; set; }

        public void Add_New_RangeAccuracy_Value(double[] _Range, IntegrationTime _Accuracy)
        {
            RangeAccuracySet.Add(new Keithley4200_RangeAccuracySet(_Range[0], _Range[1], _Accuracy));
        }

        public void Remove_RangeAccuracy_Value(double[] _Range, IntegrationTime _Accuracy)
        {
            RangeAccuracySet.Remove(new Keithley4200_RangeAccuracySet(_Range[0], _Range[1], _Accuracy));
        }
    }
}
