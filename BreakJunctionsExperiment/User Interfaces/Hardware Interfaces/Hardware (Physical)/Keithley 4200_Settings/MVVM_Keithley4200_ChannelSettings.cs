using Devices.SMU;
using Keithley_4200;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Keithley_4200;
using System.Collections.ObjectModel;

namespace BreakJunctions
{
    [ValueConversion(typeof(IntegrationTime), typeof(int))]
    public class Keithley4200_Accuracy_Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((IntegrationTime)value)
            {
                case IntegrationTime.Short:
                    return 0;
                case IntegrationTime.Medium:
                    return 1;
                case IntegrationTime.Long:
                    return 2;
                default:
                    return 1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch((int)value)
            {
                case 0:
                    return IntegrationTime.Short;
                case 1:
                    return IntegrationTime.Medium;
                case 2:
                    return IntegrationTime.Long;
                default:
                    return IntegrationTime.Medium;
            }
        }
    }    

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

        static Registry_Keithley4200 DeviceRegistry = BreakJunctionsRegistry.Instance.Reg_Keithley_4200;

        private bool _IsVoltageModeChecked = DeviceRegistry.IsVoltageModeChecked;
        public bool IsVoltageModeChecked
        {
            get { return _IsVoltageModeChecked; }
            set
            {
                _IsVoltageModeChecked = value;
                DeviceRegistry.IsVoltageModeChecked = value;
                OnPropertyChanged("IsVoltageModeChecked");
            }
        }

        private bool _IsCurrentModeChecked = DeviceRegistry.IsCurrentModeChecked;
        public bool IsCurrentModeCheched
        {
            get { return _IsCurrentModeChecked; }
            set
            {
                _IsCurrentModeChecked = value;
                DeviceRegistry.IsCurrentModeChecked = value;
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

        private double _LimitValueVoltage = DeviceRegistry.VoltageLimit;
        public double LimitValueVoltage
        {
            get
            {
                return _LimitValueVoltage * HandlingUserInput.GetMultiplier(_LimitValueVoltageMultiplier);
            }
            set
            {
                _LimitValueVoltage = value;
                DeviceRegistry.VoltageLimit = value;
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

        private double _LimitValueCurrent = DeviceRegistry.CurrentLimit;
        public double LimitValueCurrent
        {
            get
            {
                return _LimitValueCurrent * HandlingUserInput.GetMultiplier(_LimitValueCurrentMultiplier);
            }
            set
            {
                _LimitValueCurrent = value;
                DeviceRegistry.CurrentLimit = value;
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

        private IntegrationTime _CurrentIntegrationTime = DeviceRegistry.MeasurementSpeed;
        public IntegrationTime CurrentIntegrationTime 
        {
            get
            {
                _CurrentIntegrationTime = DeviceRegistry.MeasurementSpeed;
                return _CurrentIntegrationTime;
            }
            set 
            {
                _CurrentIntegrationTime = value;
                DeviceRegistry.MeasurementSpeed = value;
                OnPropertyChanged("CurrentIntegrationTime");
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _RangeAccuracyCollection;
        public ObservableCollection<Keithley4200_RangeAccuracySet> RangeAccuracyCollection
        {
            get 
            {
                if (_RangeAccuracyCollection == null)
                    _RangeAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();

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

        private IntegrationTime _NewAccuracy = IntegrationTime.Medium;
        public IntegrationTime NewAccuracy
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
