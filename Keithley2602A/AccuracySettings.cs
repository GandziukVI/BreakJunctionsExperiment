using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Keithley2602A
{
    public class Keithley2602A_RangeAccuracySet : IEquatable<Keithley2602A_RangeAccuracySet>
    {
        public double MinRangeLimit { get; set; }
        public double MaxRangeLimit { get; set; }
        public double Accuracy { get; set; }

        public Keithley2602A_RangeAccuracySet(double _MinRangeLimit, double _MaxRangeLimit, double _Accuracy)
        {
            MinRangeLimit = _MinRangeLimit;
            MaxRangeLimit = _MaxRangeLimit;
            Accuracy = _Accuracy;
        }

        public bool Equals(Keithley2602A_RangeAccuracySet Other)
        {
            return (MinRangeLimit == Other.MinRangeLimit) && (MaxRangeLimit == Other.MaxRangeLimit) && (Accuracy == Other.Accuracy);
        }
    }

    public class AccuracyParams
    {
        public ObservableCollection<Keithley2602A_RangeAccuracySet> RangeAccuracySet { get; set; }

        public void Add_New_RangeAccuracy_Value(double[] _Range, double _Accuracy)
        {
            RangeAccuracySet.Add(new Keithley2602A_RangeAccuracySet(_Range[0], _Range[1], _Accuracy));
        }

        public void Remove_RangeAccuracy_Value(double[] _Range, double _Accuracy)
        {
            RangeAccuracySet.Remove(new Keithley2602A_RangeAccuracySet(_Range[0], _Range[1], _Accuracy));
        }
    }
}
