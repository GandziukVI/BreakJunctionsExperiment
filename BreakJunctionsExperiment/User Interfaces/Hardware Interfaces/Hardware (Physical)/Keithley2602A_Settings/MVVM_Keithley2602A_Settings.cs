﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Devices.SMU;
using System.Collections.ObjectModel;

//using SMU.KEITHLEY_2602A;
//using Keithley_2602A.DeviceConfiguration;

using KeithleyInstruments;

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

        private string _VisaID = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.VisaID;
        public string VisaID
        {
            get { return _VisaID; }
            set
            {
                _VisaID = value;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.VisaID = value;
                OnPropertyChanged("VisaID");
            }
        }

        #endregion

        #region Device Source / Measurement settings

        private bool _IsVoltageModeChecked = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.IsVoltageModeChecked;
        public bool IsVoltageModeChecked
        {
            get { return _IsVoltageModeChecked; }
            set
            {
                _IsVoltageModeChecked = value;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.IsVoltageModeChecked = value;
                OnPropertyChanged("IsVoltageModeChecked");
            }
        }

        private bool _IsCurrentModeChecked = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.IsCurrentModeChecked;
        public bool IsCurrentModeCheched
        {
            get { return _IsCurrentModeChecked; }
            set
            {
                _IsCurrentModeChecked = value;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.IsCurrentModeChecked = value;
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

        private double _LimitValueVoltage = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.VoltageLimit;
        public double LimitValueVoltage
        {
            get
            {
                return _LimitValueVoltage * HandlingUserInput.GetMultiplier(_LimitValueVoltageMultiplier);
            }
            set
            {
                _LimitValueVoltage = value;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.VoltageLimit = value;
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

        private double _LimitValueCurrent = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.CurrentLimit;
        public double LimitValueCurrent
        {
            get
            {
                return _LimitValueCurrent * HandlingUserInput.GetMultiplier(_LimitValueCurrentMultiplier);
            }
            set
            {
                _LimitValueCurrent = value;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.CurrentLimit = value;
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

        private double _AccuracyCoefficient = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Accuracy;
        public double AccuracyCoefficient
        {
            get { return _AccuracyCoefficient; }
            set
            {
                _AccuracyCoefficient = value;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Accuracy = value;
                OnPropertyChanged("AccuracyCoefficient");
            }
        }

        private ObservableCollection<Keithley2602A_RangeAccuracySet> _RangeAccuracyCollection;
        public ObservableCollection<Keithley2602A_RangeAccuracySet> RangeAccuracyCollection 
        {
            get
            {
                if (_RangeAccuracyCollection == null)
                    _RangeAccuracyCollection = new ObservableCollection<Keithley2602A_RangeAccuracySet>();

                return _RangeAccuracyCollection; 
            }
        }

        private double _NewMinRangeLimit = 0.0;
        public double NewMinRangeLimit 
        {
            get { return _NewMinRangeLimit; }
            set
            {
                _NewMinRangeLimit = value;
                OnPropertyChanged("NewMinRangeLimit");
            }
        }

        private double _NewMaxRangeLimit = 0.0;
        public double NewMaxRangeLimit
        {
            get { return _NewMaxRangeLimit; }
            set
            {
                _NewMaxRangeLimit = value;
                OnPropertyChanged("NewMaxRangeLimit");
            }
        }

        private double _NewAccuracy = 1.0;
        public double NewAccuracy
        {
            get { return _NewAccuracy; }
            set
            {
                _NewAccuracy = value;
                OnPropertyChanged("NewAccuracy");
            }
        }

        #endregion
    }
}
