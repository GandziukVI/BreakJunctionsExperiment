using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_2602A.DeviceConfiguration
{
    public class AccuracyParams
    {
        private Dictionary<double[], double> _RangeAccuracySet;
        public Dictionary<double[], double> RangeAccuracySet { get { return _RangeAccuracySet; } }

        private static AccuracyParams _Instance;
        public static AccuracyParams Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AccuracyParams();

                return _Instance;
            }
        }

        private AccuracyParams()
        {
            _RangeAccuracySet = new Dictionary<double[], double>();
        }

        public void Add_New_RangeAccuracy_Value(double[] _Range, double _Accuracy)
        {
            _RangeAccuracySet.Add(_Range, _Accuracy);
        }

        public void Remove_RangeAccuracy_Value(double[] _Range)
        {
            _RangeAccuracySet.Remove(_Range);
        }
    }
}
