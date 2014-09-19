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

        private double _RealTimeTimeTraceMeasurementAppliedVoltage = 0.02;
        public double RealTimeTimeTraceMeasurementAppliedVoltage
        {
            get { return _RealTimeTimeTraceMeasurementAppliedVoltage; }
            set
            {
                _RealTimeTimeTraceMeasurementAppliedVoltage = value;
                OnPropertyChanged("RealTimeTimeTraceMeasurementAppliedVoltage");
            }
        }

        #endregion
    }
}
