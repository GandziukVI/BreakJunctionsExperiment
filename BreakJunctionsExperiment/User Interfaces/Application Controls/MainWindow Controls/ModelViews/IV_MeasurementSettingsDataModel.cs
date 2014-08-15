using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BreakJunctions
{
    public class IV_MeasurementSettingsDataModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region I-V measurement model-view interactions

        //Source mode settings
        private bool _IsIV_MeasurementVoltageModeChecked = true;
        public bool IsIV_MeasurementVoltageModeChecked
        {
            get { return _IsIV_MeasurementVoltageModeChecked; }
            set
            {
                _IsIV_MeasurementVoltageModeChecked = value;
                OnPropertyChanged("IsIV_MeasurementVoltageModeChecked");
            }
        }
        private bool _IsIV_MeasurementCurrentModeChecked = false;
        public bool IsIV_MeasurementCurrentModeChecked
        {
            get { return _IsIV_MeasurementCurrentModeChecked; }
            set
            {
                _IsIV_MeasurementCurrentModeChecked = value;
                OnPropertyChanged("IsIV_MeasurementCurrentModeChecked");
            }
        }

        //Measurement value settings
        private string _IV_MeasurementStartValueMultiplier = "None";
        public string IV_MeasurementStartValueMultiplier
        {
            get { return _IV_MeasurementStartValueMultiplier; }
            set
            {
                _IV_MeasurementStartValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementStartValueMultiplier");
            }
        }
        private double _IV_MeasurementStartValue = 0.0;
        public double IV_MeasurementStartValue
        {
            get
            {
                return _IV_MeasurementStartValue * HandlingUserInput.GetMultiplier(_IV_MeasurementStartValueMultiplier);
            }
            set
            {
                _IV_MeasurementStartValue = value;
                OnPropertyChanged("IV_MeasurementStartValue");
            }
        }

        private string _IV_MeasurementEndValueMultiplier = "None";
        public string IV_MeasurementEndValueMultiplier
        {
            get { return _IV_MeasurementEndValueMultiplier; }
            set
            {
                _IV_MeasurementEndValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementEndValueMultiplier");
            }
        }
        private double _IV_MeasurementEndValue = 1.0;
        public double IV_MeasurementEndValue
        {
            get
            {
                return _IV_MeasurementEndValue * HandlingUserInput.GetMultiplier(_IV_MeasurementEndValueMultiplier);
            }
            set
            {
                _IV_MeasurementEndValue = value;
                OnPropertyChanged("IV_MeasurementEndValue");
            }
        }

        private string _IV_MeasurementStepValueMultiplier = "None";
        public string IV_MeasurementStepValueMultiplier
        {
            get { return _IV_MeasurementStepValueMultiplier; }
            set
            {
                _IV_MeasurementStepValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementStepValueMultiplier");
            }
        }
        private double _IV_MeasurementStep = 0.01;
        public double IV_MeasurementStep
        {
            get
            {
                return _IV_MeasurementStep * HandlingUserInput.GetMultiplier(_IV_MeasurementStepValueMultiplier);
            }
            set
            {
                _IV_MeasurementStep = value;
                OnPropertyChanged("IV_MeasurementStep");
            }
        }

        //Measurement parameters
        private int _IV_MeasurementNumberOfAverages = 2;
        public int IV_MeasurementNumberOfAverages
        {
            get { return _IV_MeasurementNumberOfAverages; }
            set
            {
                _IV_MeasurementNumberOfAverages = value;
                OnPropertyChanged("IV_MeasurementNumberOfAverages");
            }
        }

        private string _IV_MeasurementTimeDelayValueMultiplier = "None";
        public string IV_MeasurementTimeDelayValueMultiplier
        {
            get { return _IV_MeasurementTimeDelayValueMultiplier; }
            set
            {
                _IV_MeasurementTimeDelayValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementTimeDelayValueMultiplier");
            }
        }
        private double _IV_MeasurementTimeDelay = 0.005;
        public double IV_MeasurementTimeDelay
        {
            get
            {
                return _IV_MeasurementTimeDelay * HandlingUserInput.GetMultiplier(_IV_MeasurementTimeDelayValueMultiplier);
            }
            set
            {
                _IV_MeasurementTimeDelay = value;
                OnPropertyChanged("IV_MeasurementTimeDelay");
            }
        }

        //Saving data
        private string _IV_MeasurementDataFileName = "IV_Measurement.dat";
        public string IV_MeasurementDataFileName
        {
            get { return _IV_MeasurementDataFileName; }
            set
            {
                _IV_MeasurementDataFileName = value;
                OnPropertyChanged("IV_MeasurementDataFileName");
            }
        }

        private string _IV_MeasurementDataComment = "";
        public string IV_MeasurementDataComment
        {
            get { return _IV_MeasurementDataComment; }
            set
            {
                _IV_MeasurementDataComment = value;
                OnPropertyChanged("IV_MeasurementDataComment");
            }
        }

        #endregion
    }
}
