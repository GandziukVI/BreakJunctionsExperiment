using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace BreakJunctions
{
    public class IV_MeasurementSettingsDataModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region I-V measurement model-view interactions

        private static Registry_IV_MeasurementSettings MeasurementSettings = BreakJunctionsRegistry.Instance.Reg_IV_MeasurementSettings;

        #region General settings

        //Source mode settings
        private bool _IsIV_MeasurementVoltageModeChecked = MeasurementSettings.IsVoltageModeChecked;
        public bool IsIV_MeasurementVoltageModeChecked
        {
            get { return _IsIV_MeasurementVoltageModeChecked; }
            set
            {
                _IsIV_MeasurementVoltageModeChecked = value;
                MeasurementSettings.IsVoltageModeChecked = value;
                OnPropertyChanged("IsIV_MeasurementVoltageModeChecked");
            }
        }
        private bool _IsIV_MeasurementCurrentModeChecked = MeasurementSettings.IsCurrentModeChecked;
        public bool IsIV_MeasurementCurrentModeChecked
        {
            get { return _IsIV_MeasurementCurrentModeChecked; }
            set
            {
                _IsIV_MeasurementCurrentModeChecked = value;
                MeasurementSettings.IsCurrentModeChecked = value;
                OnPropertyChanged("IsIV_MeasurementCurrentModeChecked");
            }
        }

        #endregion

        #region 1-st Channel Measurement value settings

        //Start value settings
        private MultiplierRanges _IV_MeasurementStartValueMultiplierChannel_01 = MultiplierRanges.None;
        public MultiplierRanges IV_MeasurementStartValueMultiplierChannel_01
        {
            get { return _IV_MeasurementStartValueMultiplierChannel_01; }
            set
            {
                _IV_MeasurementStartValueMultiplierChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStartValueMultiplierChannel_01");
            }
        }
        private double _IV_MeasurementStartValueChannel_01 = MeasurementSettings.IV_MeasurementStartValueChannel_01;
        public double IV_MeasurementStartValueChannel_01
        {
            get { return _IV_MeasurementStartValueChannel_01; }
            set
            {
                _IV_MeasurementStartValueChannel_01 = value;
                MeasurementSettings.IV_MeasurementStartValueChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStartValueChannel_01");
            }
        }
        public double IV_MeasurementStartValueWithMultiplierChannel_01
        {
            get { return _IV_MeasurementStartValueChannel_01 * _IV_MeasurementStartValueMultiplierChannel_01.AsDouble(); }
        }

        //End value settings
        private MultiplierRanges _IV_MeasurementEndValueMultiplierChannel_01 = MultiplierRanges.None;
        public MultiplierRanges IV_MeasurementEndValueMultiplierChannel_01
        {
            get { return _IV_MeasurementEndValueMultiplierChannel_01; }
            set
            {
                _IV_MeasurementEndValueMultiplierChannel_01 = value;
                OnPropertyChanged("IV_MeasurementEndValueMultiplierChannel_01");
            }
        }
        private double _IV_MeasurementEndValueChannel_01 = MeasurementSettings.IV_MeasurementEndValueChannel_01;
        public double IV_MeasurementEndValueChannel_01
        {
            get { return _IV_MeasurementEndValueChannel_01; }
            set
            {
                _IV_MeasurementEndValueChannel_01 = value;
                MeasurementSettings.IV_MeasurementEndValueChannel_01 = value;
                OnPropertyChanged("IV_MeasurementEndValueChannel_01");
            }
        }
        public double IV_MeasurementEndValueWithMultiplierChannel_01
        {
            get { return _IV_MeasurementEndValueChannel_01 * _IV_MeasurementEndValueMultiplierChannel_01.AsDouble(); }
        }

        //Step value settings
        private MultiplierRanges _IV_MeasurementStepValueMultiplierChannel_01 = MultiplierRanges.None;
        public MultiplierRanges IV_MeasurementStepValueMultiplierChannel_01
        {
            get { return _IV_MeasurementStepValueMultiplierChannel_01; }
            set
            {
                _IV_MeasurementStepValueMultiplierChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStepValueMultiplierChannel_01");
            }
        }
        private double _IV_MeasurementStepChannel_01 = MeasurementSettings.IV_MeasurementStepChannel_01;
        public double IV_MeasurementStepChannel_01
        {
            get { return _IV_MeasurementStepChannel_01; }
            set
            {
                _IV_MeasurementStepChannel_01 = value;
                MeasurementSettings.IV_MeasurementStepChannel_01 = value;
                OnPropertyChanged("IV_MeasurementStepChannel_01");
            }
        }
        public double IV_MeasurementStepWithMultiplierChannel_01
        {
            get { return _IV_MeasurementStepChannel_01 * _IV_MeasurementStepValueMultiplierChannel_01.AsDouble(); }
        }

        //Saving data
        private string _IV_MeasurementDataFileNameChannel_01 = MeasurementSettings.DataFileName_CH_01;
        public string IV_MeasurementDataFileNameChannel_01
        {
            get { return _IV_MeasurementDataFileNameChannel_01; }
            set
            {
                _IV_MeasurementDataFileNameChannel_01 = value;
                MeasurementSettings.DataFileName_CH_01 = value;
                OnPropertyChanged("IV_MeasurementDataFileNameChannel_01");
            }
        }

        #endregion

        #region 2-nd Channel Measurement value settings

        //Start value settings
        private MultiplierRanges _IV_MeasurementStartValueMultiplierChannel_02 = MultiplierRanges.None;
        public MultiplierRanges IV_MeasurementStartValueMultiplierChannel_02
        {
            get { return _IV_MeasurementStartValueMultiplierChannel_02; }
            set
            {
                _IV_MeasurementStartValueMultiplierChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStartValueMultiplierChannel_02");
            }
        }
        private double _IV_MeasurementStartValueChannel_02 = MeasurementSettings.IV_MeasurementStartValueChannel_02;
        public double IV_MeasurementStartValueChannel_02
        {
            get { return _IV_MeasurementStartValueChannel_02; }
            set
            {
                _IV_MeasurementStartValueChannel_02 = value;
                MeasurementSettings.IV_MeasurementStartValueChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStartValueChannel_02");
            }
        }
        public double IV_MeasurementStartValueWithMultiplierChannel_02
        {
            get { return _IV_MeasurementStartValueChannel_02 * _IV_MeasurementStartValueMultiplierChannel_02.AsDouble(); }
        }

        //End value settings
        private MultiplierRanges _IV_MeasurementEndValueMultiplierChannel_02 = MultiplierRanges.None;
        public MultiplierRanges IV_MeasurementEndValueMultiplierChannel_02
        {
            get { return _IV_MeasurementEndValueMultiplierChannel_02; }
            set
            {
                _IV_MeasurementEndValueMultiplierChannel_02 = value;
                OnPropertyChanged("IV_MeasurementEndValueMultiplierChannel_02");
            }
        }
        private double _IV_MeasurementEndValueChannel_02 = MeasurementSettings.IV_MeasurementEndValueChannel_02;
        public double IV_MeasurementEndValueChannel_02
        {
            get { return _IV_MeasurementEndValueChannel_02; }
            set
            {
                _IV_MeasurementEndValueChannel_02 = value;
                MeasurementSettings.IV_MeasurementEndValueChannel_02 = value;
                OnPropertyChanged("IV_MeasurementEndValueChannel_02");
            }
        }
        public double IV_MeasurementEndValueWithMultiplierChannel_02
        {
            get { return _IV_MeasurementEndValueChannel_02 * _IV_MeasurementEndValueMultiplierChannel_02.AsDouble(); }
        }

        //Step value multiplier
        private MultiplierRanges _IV_MeasurementStepValueMultiplierChannel_02 = MultiplierRanges.None;
        public MultiplierRanges IV_MeasurementStepValueMultiplierChannel_02
        {
            get { return _IV_MeasurementStepValueMultiplierChannel_02; }
            set
            {
                _IV_MeasurementStepValueMultiplierChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStepValueMultiplierChannel_02");
            }
        }
        private double _IV_MeasurementStepChannel_02 = MeasurementSettings.IV_MeasurementStepChannel_02;
        public double IV_MeasurementStepChannel_02
        {
            get { return _IV_MeasurementStepChannel_02; }
            set
            {
                _IV_MeasurementStepChannel_02 = value;
                MeasurementSettings.IV_MeasurementStepChannel_02 = value;
                OnPropertyChanged("IV_MeasurementStepChannel_02");
            }
        }
        public double IV_MeasurementStepWithMultiplierChannel_02
        {
            get { return _IV_MeasurementStepChannel_02 * _IV_MeasurementStepValueMultiplierChannel_02.AsDouble(); }
        }

        //Saving data
        private string _IV_MeasurementDataFileNameChannel_02 = MeasurementSettings.DataFileName_CH_02;
        public string IV_MeasurementDataFileNameChannel_02
        {
            get { return _IV_MeasurementDataFileNameChannel_02; }
            set
            {
                _IV_MeasurementDataFileNameChannel_02 = value;
                MeasurementSettings.DataFileName_CH_02 = value;
                OnPropertyChanged("IV_MeasurementDataFileNameChannel_02");
            }
        }

        #endregion

        #region Measurement parameters

        private int _IV_MeasurementNumberOfAverages = MeasurementSettings.NumberOfAverages;
        public int IV_MeasurementNumberOfAverages
        {
            get { return _IV_MeasurementNumberOfAverages; }
            set
            {
                _IV_MeasurementNumberOfAverages = value;
                MeasurementSettings.NumberOfAverages = value;
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
        private double _IV_MeasurementTimeDelay = MeasurementSettings.TimeDelay;
        public double IV_MeasurementTimeDelay
        {
            get
            {
                return _IV_MeasurementTimeDelay * HandlingUserInput.GetMultiplier(_IV_MeasurementTimeDelayValueMultiplier);
            }
            set
            {
                _IV_MeasurementTimeDelay = value;
                MeasurementSettings.TimeDelay = value;
                OnPropertyChanged("IV_MeasurementTimeDelay");
            }
        }

        private string _IV_MeasurementDataComment = MeasurementSettings.Comments;
        public string IV_MeasurementDataComment
        {
            get { return _IV_MeasurementDataComment; }
            set
            {
                _IV_MeasurementDataComment = value;
                MeasurementSettings.Comments = value;
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
