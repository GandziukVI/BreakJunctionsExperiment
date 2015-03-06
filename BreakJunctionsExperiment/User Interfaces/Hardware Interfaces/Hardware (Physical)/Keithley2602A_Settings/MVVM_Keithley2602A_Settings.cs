using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Devices.SMU;
using SMU.KEITHLEY_2602A;

namespace BreakJunctions
{
    /// <summary>
    /// Implementation MVVM for Keithley 2602A settings window
    /// </summary>
    public class MVVM_Keithley2602A_Settings : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region VisaDevice settings

        private string _VisaID = "TCPIP0::134.94.243.192::inst0::INSTR";
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
        public bool  IsCurrentModeCheched
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

        private double _LimitValueVoltage = 12.0;
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

        private double _LimitValueCurrent = 1.0;
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

        private string _SelectedChannelString = "Channel A";
        public string SelectedChannelString
        {
            get { return _SelectedChannelString; }
            set
            {
                _SelectedChannelString = value;
                OnPropertyChanged("SelectedChannelString");
            }
        }

        private Channels _SelectedChannel = Channels.ChannelA;
        public Channels SelectedChannel 
        {
            get 
            {
                if (_SelectedChannelString == "Channel A")
                    _SelectedChannel = Channels.ChannelA;
                else
                    _SelectedChannel = Channels.ChannelB;

                return _SelectedChannel; 
            }
            set
            {
                _SelectedChannel = value;
                OnPropertyChanged("SelectedChannel");
            }
        }

        private double _AccuracyCoefficient = 1.0;
        public double AccuracyCoefficient
        {
            get { return _AccuracyCoefficient; }
            set
            {
                _AccuracyCoefficient = value;
                OnPropertyChanged("AccuracyCoefficient");
            }
        }

        #endregion
    }
}
