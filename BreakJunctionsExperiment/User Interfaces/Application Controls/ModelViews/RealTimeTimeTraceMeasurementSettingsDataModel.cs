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

        #region Samples measurement enable

        private bool _IsSample_01_MeasurementEnabled = true;
        public bool IsSample_01_MeasurementEnabled
        {
            get { return _IsSample_01_MeasurementEnabled; }
            set
            {
                IsChannel_01_Checked = value;
                IsChannel_02_Checked = value;

                _IsSample_01_MeasurementEnabled = value;
                OnPropertyChanged("IsSample_01_MeasurementEnabled");
            }
        }

        private bool _IsSample_02_MeasurementEnabled = true;
        public bool IsSample_02_MeasurementEnabled
        {
            get { return _IsSample_02_MeasurementEnabled; }
            set
            {
                IsChannel_03_Checked = value;
                IsChannel_04_Checked = value;

                _IsSample_02_MeasurementEnabled = value;
                OnPropertyChanged("IsSample_02_MeasurementEnabled");
            }
        }

        #endregion

        #region Start / Stop Quick sample check

        private bool _IsStartStopQuickSampleCheckChecked = false;
        public bool IsStartStopQuickSampleCheckChecked
        {
            get { return _IsStartStopQuickSampleCheckChecked; }
            set
            {
                _IsStartStopQuickSampleCheckChecked = value;
                OnPropertyChanged("IsStartStopQuickSampleCheckChecked");

                if (_IsStartStopQuickSampleCheckChecked == false)
                    cmdStartStopQuickSampleCheckTextContent = "Start Quick Sample Check";
                else
                    cmdStartStopQuickSampleCheckTextContent = "Stop Quick Sample Check";
            }
        }

        private bool _IsStartStopQuickSampleCheckEnabled = true;
        public bool IsStartStopQuickSampleCheckEnabled
        {
            get { return _IsStartStopQuickSampleCheckEnabled; }
            set
            {
                _IsStartStopQuickSampleCheckEnabled = value;
                OnPropertyChanged("IsStartStopQuickSampleCheckEnabled");
            }
        }

        private string _cmdStartStopQuickSampleCheckTextContent = "Start Quick Sample Check";
        public string cmdStartStopQuickSampleCheckTextContent
        {
            get { return _cmdStartStopQuickSampleCheckTextContent; }
            set
            {
                _cmdStartStopQuickSampleCheckTextContent = value;
                OnPropertyChanged("cmdStartStopQuickSampleCheckTextContent");
            }
        }

        #endregion

        #region SaveFile TextBox text

        private string _SaveFileName = string.Empty;
        public string SaveFileName
        {
            get { return _SaveFileName; }
            set
            {
                _SaveFileName = value;
                OnPropertyChanged("SaveFileName");
            }
        }

        #endregion

        #region Start / Stop motor

        private bool _IsStartStopEnabled = false;
        public bool IsStartStopEnabled
        {
            get { return _IsStartStopEnabled; }
            set
            {
                _IsStartStopEnabled = value;
                IsStartStopQuickSampleCheckEnabled = !value;

                OnPropertyChanged("IsStartStopEnabled");
            }
        }

        private string _cmdStartStopTextContent = "Stop Motor";
        public string cmdStartStopTextContent
        {
            get { return _cmdStartStopTextContent; }
            set
            {
                _cmdStartStopTextContent = value;
                OnPropertyChanged("cmdStartStopTextContent");
            }
        }

        private bool _IsMotorStopped = false;
        public bool IsMotorStopped
        {
            get { return _IsMotorStopped; }
            set
            {
                _IsMotorStopped = value;
                OnPropertyChanged("MotorStopped");

                if (_IsMotorStopped == true)
                    cmdStartStopTextContent = "Start Motor";
                else
                    cmdStartStopTextContent = "Stop Motor";
            }
        }

        #endregion

        #region Start / Stop Measurement enabled

        private bool _IsStartMeasurementButtonEnabled = true;
        public bool IsStartMeasurementButtonEnabled
        {
            get { return _IsStartMeasurementButtonEnabled; }
            set
            {
                _IsStartMeasurementButtonEnabled = value;
                OnPropertyChanged("IsStartMeasurementButtonEnabled");
            }
        }

        private bool _IsStopMeasurementButtonEnabled = false;
        public bool IsStopMeasurementButtonEnabled
        {
            get { return _IsStopMeasurementButtonEnabled; }
            set
            {
                _IsStopMeasurementButtonEnabled = value;
                OnPropertyChanged("IsStopMeasurementButtonEnabled");
            }
        }

        #endregion

        #region Amplification coefficient

        private double _AmplificationCoefficient = 10000.0;
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
