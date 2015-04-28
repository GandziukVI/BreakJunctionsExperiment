using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_2602A.DeviceConfiguration
{
    public class RangeAccuracySet
    {
        public double MinRangeLimit { get; set; }
        public double MaxRangeLimit { get; set; }
        public double Accuracy { get; set; }

        public RangeAccuracySet(double _MinRangeLimit, double _MaxRangeLimit, double _Accuracy)
        {
            MinRangeLimit = _MinRangeLimit;
            MaxRangeLimit = _MaxRangeLimit;
            Accuracy = _Accuracy;
        }
    }

    public class AccuracyParams
    {
        private ObservableCollection<RangeAccuracySet> _RangeAccuracySet;
        public ObservableCollection<RangeAccuracySet> RangeAccuracySet { get { return _RangeAccuracySet; } }

        public AccuracyParams()
        {
            _RangeAccuracySet = new ObservableCollection<RangeAccuracySet>();
        }

        public void Add_New_RangeAccuracy_Value(double[] _Range, double _Accuracy)
        {
            _RangeAccuracySet.Add(new RangeAccuracySet(_Range[0], _Range[1], _Accuracy));
        }

        public void Remove_RangeAccuracy_Value(double[] _Range, double _Accuracy)
        {
            _RangeAccuracySet.Remove(new RangeAccuracySet(_Range[0], _Range[1], _Accuracy));
        }
    }
}
