using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Hardware.KEITHLEY_2602A;

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

        #region GPIB settings

        private byte _PrimaryAddress = 26;
        public byte PrimaryAddress
        {
            get { return _PrimaryAddress; }
            set 
            {
                _PrimaryAddress = value;
                OnPropertyChanged("PrimaryAddress");
            }
        }

        private byte _SecondaryAddress = 0;
        public byte SecondaryAddress
        {
            get { return _SecondaryAddress; }
            set 
            {
                _SecondaryAddress = value;
                OnPropertyChanged("SecondaryAddress");
            }
        }

        private byte _BoardNumber = 0;
        public byte BoardNumber
        {
            get { return _BoardNumber; }
            set 
            {
                _BoardNumber = value;
                OnPropertyChanged("BoardNumber");
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

        private KEITHLEY_2601A_LimitMode _LimitMode;
        public KEITHLEY_2601A_LimitMode LimitMode 
        {
            get 
            {
                if (_IsCurrentModeChecked == true)
                    _LimitMode = KEITHLEY_2601A_LimitMode.Voltage;
                else if (_IsVoltageModeChecked == true)
                    _LimitMode = KEITHLEY_2601A_LimitMode.Current;

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

        private double _LimitValueVoltage;
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

        private double _LimitValueCurrent;
        public double LimitValueCurrent 
        {
            get { return _LimitValueCurrent; }
            set { _LimitValueCurrent = value; }
        }

        private KEITHLEY_2602A_Channels _SelectedChannel;
        public KEITHLEY_2602A_Channels SelectedChannel 
        {
            get { return _SelectedChannel; }
            set { _SelectedChannel = value; }
        }

        #endregion
    }
}
