using Devices.SMU;
using Keithley_4200;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions
{
    public class MVVM_Keithley4200_ChannelSettings : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Visa device settings

        private string _VisaID = "GPIB0::17::INSTR";
        public string VisaID
        {
            get { return _VisaID; }
            set
            {
                _VisaID = value;
                OnPropertyChanged("VisaID");
            }
        }

        #endregion

        #region Device Source / Measurement settings

        private bool _IsVoltageModeChecked = true;
        public bool IsVoltageModeChecked
        {
            get { return _IsVoltageModeChecked; }
            set
            {
                _IsVoltageModeChecked = value;
                OnPropertyChanged("IsVoltageModeChecked");
            }
        }

        private bool _IsCurrentModeChecked = false;
        public bool IsCurrentModeCheched
        {
            get { return _IsCurrentModeChecked; }
            set
            {
                _IsCurrentModeChecked = value;
                OnPropertyChanged("IsCurrentModeCheched");
            }
        }

        private LimitMode _LimitMode;
        public LimitMode LimitMode
        {
            get
            {
                if (_IsCurrentModeChecked == true)
                    _LimitMode = LimitMode.Voltage;
                else if (_IsVoltageModeChecked == true)
                    _LimitMode = LimitMode.Current;

                return _LimitMode;
            }
            set
            {
                _LimitMode = value;
                OnPropertyChanged("LimitMode");
            }
        }

        private string _LimitValueVoltageMultiplier = "None";
        public string LimitValueVoltageMultiplier
        {
            get { return _LimitValueVoltageMultiplier; }
            set
            {
                _LimitValueVoltageMultiplier = value;
                OnPropertyChanged("LimitValueVoltageMultiplier");
            }
        }

        private double _LimitValueVoltage = 20.0;
        public double LimitValueVoltage
        {
            get
            {
                return _LimitValueVoltage * HandlingUserInput.GetMultiplier(_LimitValueVoltageMultiplier);
            }
            set
            {
                _LimitValueVoltage = value;
                OnPropertyChanged("LimitValueVoltage");
            }
        }

        private string _LimitValueCurrentMultiplier = "None";
        public string LimitValueCurrentMultiplier
        {
            get { return _LimitValueCurrentMultiplier; }
            set
            {
                _LimitValueCurrentMultiplier = value;
                OnPropertyChanged("LimitValueCurrentMultiplier");
            }
        }

        private double _LimitValueCurrent = 0.0105;
        public double LimitValueCurrent
        {
            get
            {
                return _LimitValueCurrent * HandlingUserInput.GetMultiplier(_LimitValueCurrentMultiplier);
            }
            set
            {
                _LimitValueCurrent = value;
                OnPropertyChanged("LimitValueCurrent");
            }
        }

        private string _SelectedChannelString = "SMU 1";
        public string SelectedChannelString
        {
            get { return _SelectedChannelString; }
            set
            {
                _SelectedChannelString = value;
                OnPropertyChanged("SelectedChannelString");
            }
        }

        private SMUs _SelectedSMU = SMUs.SMU1;
        public SMUs SelectedSMU
        {
            get
            {
                if (_SelectedChannelString == "SMU 1")
                    _SelectedSMU = SMUs.SMU1;
                else if (_SelectedChannelString == "SMU 2")
                    _SelectedSMU = SMUs.SMU2;
                else if (_SelectedChannelString == "SMU 3")
                    _SelectedSMU = SMUs.SMU3;
                else if (_SelectedChannelString == "SMU 4")
                    _SelectedSMU = SMUs.SMU4;
                else if (_SelectedChannelString == "SMU 5")
                    _SelectedSMU = SMUs.SMU5;
                else if (_SelectedChannelString == "SMU 6")
                    _SelectedSMU = SMUs.SMU6;
                else if (_SelectedChannelString == "SMU 7")
                    _SelectedSMU = SMUs.SMU7;
                else if (_SelectedChannelString == "SMU 8")
                    _SelectedSMU = SMUs.SMU8;

                return _SelectedSMU;
            }
            set
            {
                _SelectedSMU = value;
                OnPropertyChanged("SelectedChannel");
            }
        }

        private string _TheIntegrationTime = "Medium";
        public string TheIntegrationTime
        {
            get { return _TheIntegrationTime; }
            set
            {
                _TheIntegrationTime = value;
                OnPropertyChanged("TheIntegrationTime");
            }
        }

        public IntegrationTime CurrentIntegrationTime
        {
            get
            {
                if (_TheIntegrationTime == "Short")
                    return IntegrationTime.Short;
                if (_TheIntegrationTime == "Medium")
                    return IntegrationTime.Medium;
                else if (_TheIntegrationTime == "Long")
                    return IntegrationTime.Long;
                else
                    return IntegrationTime.Medium;
            }
        }

        #endregion
    }
}
