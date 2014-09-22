using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BreakJunctions
{
    public class RealTimeTimeTraceMeasurementSettingsDataModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region RealTimeTimeTraceMeasurement model-view intreactions

        #region Measured values in channels

        private double _Channel_001_Value = 0.0;
        /// <summary>
        /// Represents measured value in the first channel
        /// </summary>
        public double Channel_001_Value
        {
            get { return _Channel_001_Value; }
            set
            {
                _Channel_001_Value = value;
                OnPropertyChanged("Channel_001_Value");
            }
        }

        private double _Channel_002_Value = 0.0;
        /// <summary>
        /// Represents measured value in the second channel
        /// </summary>
        public double Channel_002_Value
        {
            get { return _Channel_002_Value; }
            set
            {
                _Channel_002_Value = value;
                OnPropertyChanged("Channel_002_Value");
            }
        }

        private double _Channel_003_Value = 0.0;
        /// <summary>
        /// Represents measured value in the third channel
        /// </summary>
        public double Channel_003_Value
        {
            get { return _Channel_003_Value; }
            set
            {
                _Channel_003_Value = value;
                OnPropertyChanged("Channel_003_Value");
            }
        }

        private double _Channel_004_Value = 0.0;
        /// <summary>
        /// Represents measured value in the fourth channel
        /// </summary>
        public double Channel_004_Value
        {
            get { return _Channel_004_Value; }
            set
            {
                _Channel_004_Value = value;
                OnPropertyChanged("Channel_004_Value");
            }
        }

        #endregion

        #region Use channels

        private bool _IsChannel_01_Checked = true;
        public bool IsChannel_01_Checked
        {
            get { return _IsChannel_01_Checked; }
            set
            {
                _IsChannel_01_Checked = value;
                OnPropertyChanged("IsChannel_01_Checked");
            }
        }

        private bool _IsChannel_02_Checked = true;
        public bool IsChannel_02_Checked
        {
            get { return _IsChannel_02_Checked; }
            set
            {
                _IsChannel_02_Checked = value;
                OnPropertyChanged("IsChannel_02_Checked");
            }
        }

        private bool _IsChannel_03_Checked = true;
        public bool IsChannel_03_Checked
        {
            get { return _IsChannel_03_Checked; }
            set
            {
                _IsChannel_03_Checked = value;
                OnPropertyChanged("IsChannel_03_Checked");
            }
        }

        private bool _IsChannel_04_Checked = true;
        public bool IsChannel_04_Checked
        {
            get { return _IsChannel_04_Checked; }
            set
            {
                _IsChannel_04_Checked = value;
                OnPropertyChanged("IsChannel_04_Checked");
            }
        }

        #endregion

        #region Amplification coefficient

        private double _AmplificationCoefficient = 1000000.0;
        public double AmplificationCoefficient
        {
            get { return _AmplificationCoefficient; }
            set
            {
                _AmplificationCoefficient = value;
                OnPropertyChanged("AmplificationCoefficient");
            }
        }

        #endregion

        #region Measured resistance

        private double _Resistance_1st_Sample;
        public double Resistance_1st_Sample
        {
            get { return _Resistance_1st_Sample; }
            set
            {
                _Resistance_1st_Sample = value;
                OnPropertyChanged("Resistance_1st_Sample");
            }
        }

        private double _Resistance_2nd_Sample;
        public double Resistance_2nd_Sample
        {
            get { return _Resistance_2nd_Sample; }
            set
            {
                _Resistance_2nd_Sample = value;
                OnPropertyChanged("Resistance_2nd_Sample");
            }
        }

        #endregion

        #endregion
    }
}
