using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Hardware;
using Hardware.KEITHLEY_2602A;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BreakJunctions
{
    public class Keithley2602A_Settings
    {
        private bool _isBoardNumberSet = false;
        private bool isBoardNumberSet { get { return _isBoardNumberSet; } }
        private byte _BoardNumber;
        public byte BoardNumber { get { return _BoardNumber; } }
        public void SetBoardNumber(byte boardNumber)
        {
            _BoardNumber = boardNumber;
            _isBoardNumberSet = true;
        }

        private bool _isPrimaryAddressSet = false;
        private bool isPrimaryAddressSet { get { return _isPrimaryAddressSet; } }
        private byte _PrimaryAddress;
        public byte PrimaryAddress { get { return _PrimaryAddress; } }
        public void SetPrimaryAddress(byte primaryAddress)
        {
            _PrimaryAddress = primaryAddress;
            _isPrimaryAddressSet = true;
        }

        private bool _isSecondaryAddressSet = false;
        private bool isSecondaryAddressSet { get { return _isSecondaryAddressSet; } }
        private byte _SecondaryAddress;
        public byte SecondaryAddress { get { return _SecondaryAddress; } }
        public void SetSecondaryAddress(byte secondaryAddress)
        {
            _SecondaryAddress = secondaryAddress;
            _isSecondaryAddressSet = true;
        }

        private bool _isLimitModeSet = false;
        private bool isLimitModeSet { get { return _isLimitModeSet; } }
        private KEITHLEY_2601A_LimitMode _LimitMode;
        public KEITHLEY_2601A_LimitMode LimitMode { get { return _LimitMode; } }
        public void SetLimitMode(KEITHLEY_2601A_LimitMode limitMode)
        {
            _LimitMode = limitMode;
            _isLimitModeSet = true;
        }

        private bool _isLimitValueVoltageSet = false;
        private bool isLimitValueVoltageSet { get { return _isLimitValueVoltageSet; } }
        private double _LimitValueVoltage;
        public double LimitValueVoltage { get { return _LimitValueVoltage; } }
        public void SetLimitValueVoltage(double limitValueVoltage)
        {
            _LimitValueVoltage = limitValueVoltage;
            _isLimitValueVoltageSet = true;
        }

        private bool _isLimitValueCurrentSet = false;
        private bool isLimitValueCurrentSet { get { return _isLimitValueCurrentSet; } }
        private double _LimitValueCurrent;
        public double LimitValueCurrent { get { return _LimitValueCurrent; } }
        public void SetLimitValueCurrent(double limitValueCurrent)
        {
            _LimitValueCurrent = limitValueCurrent;
            _isLimitValueCurrentSet = true;
        }

        private bool _isSelectedChannelSet = false;
        private bool isSelectedChannelSet { get { return _isSelectedChannelSet; } }
        private KEITHLEY_2602A_Channels _SelectedChannel;
        public KEITHLEY_2602A_Channels SelectedChannel { get { return _SelectedChannel; } }
        public void SetSelectedChannel(KEITHLEY_2602A_Channels selectedChannel)
        {
            _SelectedChannel = selectedChannel;
            _isSelectedChannelSet = true;
        }

        public I_SMU SetDevice()
        {
            bool areAllSettingsSet = _isBoardNumberSet && _isPrimaryAddressSet && _isSecondaryAddressSet && _isLimitModeSet &&
                _isLimitValueVoltageSet && _isLimitValueCurrentSet && _isSelectedChannelSet;
            I_SMU _Device = null;

            if (areAllSettingsSet)
            {
                if((_SelectedChannel == KEITHLEY_2602A_Channels.ChannelA) && (_LimitMode == KEITHLEY_2601A_LimitMode.Voltage))
                {
                    var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_PrimaryAddress, _SecondaryAddress, _BoardNumber, KEITHLEY_2602A_Channels.ChannelA);
                    smu.SetSpeed(25.0, KEITHLEY_2602A_Channels.ChannelA);
                    _Device = smu;
                    _Device.SetVoltageLimit(_LimitValueVoltage);
                }
                else if ((_SelectedChannel == KEITHLEY_2602A_Channels.ChannelA) && (_LimitMode == KEITHLEY_2601A_LimitMode.Current))
                {
                    var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_PrimaryAddress, _SecondaryAddress, _BoardNumber, KEITHLEY_2602A_Channels.ChannelA);
                    smu.SetSpeed(25.0, KEITHLEY_2602A_Channels.ChannelA);
                    _Device = smu;
                    _Device.SetCurrentLimit(_LimitValueCurrent);
                }
                else if ((_SelectedChannel == KEITHLEY_2602A_Channels.ChannelB) && (_LimitMode == KEITHLEY_2601A_LimitMode.Voltage))
                {
                    var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_PrimaryAddress, _SecondaryAddress, _BoardNumber, KEITHLEY_2602A_Channels.ChannelB);
                    smu.SetSpeed(25.0, KEITHLEY_2602A_Channels.ChannelB);
                    _Device = smu;
                    _Device.SetVoltageLimit(_LimitValueVoltage);
                }
                else if ((_SelectedChannel == KEITHLEY_2602A_Channels.ChannelB) && (_LimitMode == KEITHLEY_2601A_LimitMode.Current))
                {
                    var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_PrimaryAddress, _SecondaryAddress, _BoardNumber, KEITHLEY_2602A_Channels.ChannelB);
                    smu.SetSpeed(25.0, KEITHLEY_2602A_Channels.ChannelB);
                    _Device = smu;
                    _Device.SetCurrentLimit(_LimitValueCurrent);
                }

                return _Device;
            }
            else
            {
                throw new Exception("Not all configurations set! Please set all the configurations and try again.");
            }
        }
    }

	/// <summary>
	/// Interaction logic for Keithley2602A_Settings.xaml
	/// </summary>
	public partial class Keithley2602A_Channel_Settings : Window
	{
        //The device

        private Keithley2602A_Settings _DeviceSettings;
        public Keithley2602A_Settings DeviceSettings
        {
            get { return _DeviceSettings; }
        }
        private I_SMU _Device;
        public I_SMU Device
        {
            get { return _Device; }
        }

		public Keithley2602A_Channel_Settings()
		{
			this.InitializeComponent();
            
            this.radioSourceMeasureModeVoltage.DataContext = ModelViewInteractions.IV_VoltageChangedModel;
            this.radioSourceMeasureModeCurrent.DataContext = ModelViewInteractions.IV_CurrentChangedModel;

		}

        private I_SMU GetSettings()
        {
            _DeviceSettings = new Keithley2602A_Settings();

            byte TempByteSettings;
            double TempDoubleSettings;

            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent;
            var culture = new CultureInfo("en-US");

            byte.TryParse(this.textBoxBoardNumber.Text, out TempByteSettings);
            _DeviceSettings.SetBoardNumber(TempByteSettings);
            byte.TryParse(this.textBoxPrimaryAddress.Text, out TempByteSettings);
            _DeviceSettings.SetPrimaryAddress(TempByteSettings);
            byte.TryParse(this.textBoxSecondaryAddress.Text, out TempByteSettings);
            _DeviceSettings.SetSecondaryAddress(TempByteSettings);

            //Device Source/Measure mode

            var IsVoltageMode = this.radioSourceMeasureModeVoltage.IsChecked;
            var IsCurrentMode = this.radioSourceMeasureModeCurrent.IsChecked;

            //Limit mode

            if (IsCurrentMode/*IsVoltageLimit*/ == true)
            {
                _DeviceSettings.SetLimitMode(KEITHLEY_2601A_LimitMode.Voltage);
            }
            else if (IsVoltageMode/*IsCurrentLimit*/ == true)
            {
                _DeviceSettings.SetLimitMode(KEITHLEY_2601A_LimitMode.Current);
            }

            //Limit value voltage

            var IsLimitVoltageRead = double.TryParse(this.textBoxVoltageLimit.Text, style, culture, out TempDoubleSettings);
            if (IsLimitVoltageRead)
            {
                var VoltageMultiplier = this.comboBoxVoltageLimitMultiplier.Text;
                _DeviceSettings.SetLimitValueVoltage(TempDoubleSettings * HandlingUserInput.GetMultiplier(VoltageMultiplier));
            }
            else throw new Exception("Voltage limit value is defined incorrectly!");

            //Limit value current

            var IsLimitCurrentRead = double.TryParse(this.textBoxCurrentLimit.Text, style, culture, out TempDoubleSettings);
            if (IsLimitCurrentRead)
            {
                var CurrentMultiplier = this.comboBoxCurrentLimitMultiplier.Text;
                _DeviceSettings.SetLimitValueCurrent(TempDoubleSettings * HandlingUserInput.GetMultiplier(CurrentMultiplier));
            }
            else throw new Exception("Current limit value is defined incorrectly!");

            //Working channel

            switch (this.comboBoxWorkingChannel.Text)
            {
                case "Channel A":
                    {
                        _DeviceSettings.SetSelectedChannel(KEITHLEY_2602A_Channels.ChannelA);
                    } break;
                case "Channel B":
                    {
                        _DeviceSettings.SetSelectedChannel(KEITHLEY_2602A_Channels.ChannelB);
                    } break;
                default:
                    break;
            }

            return _DeviceSettings.SetDevice();
        }

		private void on_cmdSaveSettingsClick(object sender, System.Windows.RoutedEventArgs e)
		{
            _Device = GetSettings();
            this.Close();
		}

        private void IntegerPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            HandlingUserInput.IntegerPastingHandler(ref sender, ref e);
        }

        private void FloatingPointPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            HandlingUserInput.FloatingPointPastingHandler(ref sender, ref e);
        }

        private void OnIntegerTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            HandlingUserInput.OnIntegerTextChanged(ref sender, ref e);
        }

        private void OnFloatingPointTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            HandlingUserInput.OnFloatingPointTextChanged(ref sender, ref e);
        }
	}
}