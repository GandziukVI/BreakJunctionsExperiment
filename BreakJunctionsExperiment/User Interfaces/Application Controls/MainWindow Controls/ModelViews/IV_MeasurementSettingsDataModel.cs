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

        #region General settings

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

        #endregion

        #region 1-st Channel Measurement value settings

        //Start value settings
        private double _IV_MeasurementStartValueMultiplierChannel_01 = 1.0;
        public double IV_MeasurementStartValueMultiplierChannel_01
        {
            get { return _IV_MeasurementStartValueMultiplierChannel_01; }
            set
            {
                _IV_MeasurementStartValueMultiplierChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStartValueMultiplierChannel_01");
            }
        }
        private double _IV_MeasurementStartValueChannel_01 = 0.0;
        public double IV_MeasurementStartValueChannel_01
        {
            get { return _IV_MeasurementStartValueChannel_01; }
            set
            {
                _IV_MeasurementStartValueChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStartValueChannel_01");
            }
        }
        public double IV_MeasurementStartValueWithMultiplierChannel_01
        {
            get { return _IV_MeasurementStartValueChannel_01 * _IV_MeasurementStartValueMultiplierChannel_01; }
        }

        //End value settings
        private string _IV_MeasurementEndValueMultiplierChannel_01 = "None";
        public string IV_MeasurementEndValueMultiplierChannel_01
        {
            get { return _IV_MeasurementEndValueMultiplierChannel_01; }
            set
            {
                _IV_MeasurementEndValueMultiplierChannel_01 = value;
                OnPropertyChanged("IV_MeasurementEndValueMultiplierChannel_01");
            }
        }
        private double _IV_MeasurementEndValueChannel_01 = 1.0;
        public double IV_MeasurementEndValueChannel_01
        {
            get { return _IV_MeasurementEndValueChannel_01; }
            set
            {
                _IV_MeasurementEndValueChannel_01 = value;
                OnPropertyChanged("IV_MeasurementEndValueChannel_01");
            }
        }
        public double IV_MeasurementEndValueWithMultiplierChannel_01
        {
            get { return _IV_MeasurementEndValueChannel_01 * HandlingUserInput.GetMultiplier(_IV_MeasurementEndValueMultiplierChannel_01); }
        }

        //Step value settings
        private string _IV_MeasurementStepValueMultiplierChannel_01 = "None";
        public string IV_MeasurementStepValueMultiplierChannel_01
        {
            get { return _IV_MeasurementStepValueMultiplierChannel_01; }
            set
            {
                _IV_MeasurementStepValueMultiplierChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStepValueMultiplierChannel_01");
            }
        }
        private double _IV_MeasurementStepChannel_01 = 0.01;
        public double IV_MeasurementStepChannel_01
        {
            get { return _IV_MeasurementStepChannel_01; }
            set
            {
                _IV_MeasurementStepChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStepChannel_01");
            }
        }
        public double IV_MeasurementStepWithMultiplierChannel_01
        {
            get { return _IV_MeasurementStepChannel_01 * HandlingUserInput.GetMultiplier(_IV_MeasurementStepValueMultiplierChannel_01); }
        }

        //Saving data
        private string _IV_MeasurementDataFileNameChannel_01 = "IV_CH_01.dat";
        public string IV_MeasurementDataFileNameChannel_01
        {
            get { return _IV_MeasurementDataFileNameChannel_01; }
            set
            {
                _IV_MeasurementDataFileNameChannel_01 = value;
                OnPropertyChanged("IV_MeasurementDataFileNameChannel_01");
            }
        }

        #endregion

        #region 2-nd Channel Measurement value settings

        //Start value settings
        private double _IV_MeasurementStartValueMultiplierChannel_02 = 1.0;
        public double IV_MeasurementStartValueMultiplierChannel_02
        {
            get { return _IV_MeasurementStartValueMultiplierChannel_02; }
            set
            {
                _IV_MeasurementStartValueMultiplierChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStartValueMultiplierChannel_02");
            }
        }
        private double _IV_MeasurementStartValueChannel_02 = 0.0;
        public double IV_MeasurementStartValueChannel_02
        {
            get { return _IV_MeasurementStartValueChannel_02; }
            set
            {
                _IV_MeasurementStartValueChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStartValueChannel_02");
            }
        }
        public double IV_MeasurementStartValueWithMultiplierChannel_02
        {
            get { return _IV_MeasurementStartValueChannel_02 * _IV_MeasurementStartValueMultiplierChannel_02; }
        }

        //End value settings
        private string _IV_MeasurementEndValueMultiplierChannel_02 = "None";
        public string IV_MeasurementEndValueMultiplierChannel_02
        {
            get { return _IV_MeasurementEndValueMultiplierChannel_02; }
            set
            {
                _IV_MeasurementEndValueMultiplierChannel_02 = value;
                OnPropertyChanged("IV_MeasurementEndValueMultiplierChannel_02");
            }
        }
        private double _IV_MeasurementEndValueChannel_02 = 1.0;
        public double IV_MeasurementEndValueChannel_02
        {
            get { return _IV_MeasurementEndValueChannel_02; }
            set
            {
                _IV_MeasurementEndValueChannel_02 = value;
                OnPropertyChanged("IV_MeasurementEndValueChannel_02");
            }
        }
        public double IV_MeasurementEndValueWithMultiplierChannel_02
        {
            get { return _IV_MeasurementEndValueChannel_02 * HandlingUserInput.GetMultiplier(_IV_MeasurementEndValueMultiplierChannel_02); }
        }

        //Step value multiplier
        private string _IV_MeasurementStepValueMultiplierChannel_02 = "None";
        public string IV_MeasurementStepValueMultiplierChannel_02
        {
            get { return _IV_MeasurementStepValueMultiplierChannel_02; }
            set
            {
                _IV_MeasurementStepValueMultiplierChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStepValueMultiplierChannel_02");
            }
        }
        private double _IV_MeasurementStepChannel_02 = 0.01;
        public double IV_MeasurementStepChannel_02
        {
            get { return _IV_MeasurementStepChannel_02; }
            set
            {
                _IV_MeasurementStepChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStepChannel_02");
            }
        }
        public double IV_MeasurementStepWithMultiplierChannel_02
        {
            get { return _IV_MeasurementStepChannel_02 * HandlingUserInput.GetMultiplier(_IV_MeasurementStepValueMultiplierChannel_02); }
        }

        //Saving data
        private string _IV_MeasurementDataFileNameChannel_02 = "IV_CH_02.dat";
        public string IV_MeasurementDataFileNameChannel_02
        {
            get { return _IV_MeasurementDataFileNameChannel_02; }
            set
            {
                _IV_MeasurementDataFileNameChannel_02 = value;
                OnPropertyChanged("IV_MeasurementDataFileNameChannel_02");
            }
        }

        #endregion

        #region Measurement parameters

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

        private double _IV_MeasurementMicrometricBoltPosition = 0.0;
        public double IV_MeasurementMicrometricBoltPosition
        {
            get { return _IV_MeasurementMicrometricBoltPosition; }
            set
            {
                _IV_MeasurementMicrometricBoltPosition = value;
                OnPropertyChanged("IV_MeasurementMicrometricBoltPosition");
            }
        }

        #endregion

        #endregion
    }
}
