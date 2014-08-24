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
using SMU.KEITHLEY_2602A;
using System.Text.RegularExpressions;
using System.Globalization;

namespace BreakJunctions
{
	/// <summary>
	/// Interaction logic for Keithley2602A_Settings.xaml
	/// </summary>
	public partial class Keithley2602A_Channel_Settings : Window
    {
        #region MVVM for Keithley2602ASettings

        private MVVM_Keithley2602A_Settings _DeviceSettings;
        public MVVM_Keithley2602A_Settings DeviceSettings
        {
            get { return _DeviceSettings; }
        }

        #endregion

        //The device

        private SMU.I_SMU _Device;
        public SMU.I_SMU Device
        {
            get { return _Device; }
        }

		public Keithley2602A_Channel_Settings()
		{
			this.InitializeComponent();

            #region Set MVVM

            _DeviceSettings = new MVVM_Keithley2602A_Settings();
            this.DataContext = _DeviceSettings;
            
            this.radioSourceMeasureModeVoltage.DataContext = ModelViewInteractions.IV_VoltageChangedModel;
            this.radioSourceMeasureModeCurrent.DataContext = ModelViewInteractions.IV_CurrentChangedModel;

            #endregion
        }

        private SMU.I_SMU SetDevice()
        {
            if ((_DeviceSettings.SelectedChannel == KEITHLEY_2602A_Channels.ChannelA) && (_DeviceSettings.LimitMode == KEITHLEY_2601A_LimitMode.Voltage))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, KEITHLEY_2602A_Channels.ChannelA);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, KEITHLEY_2602A_Channels.ChannelA);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == KEITHLEY_2602A_Channels.ChannelA) && (_DeviceSettings.LimitMode == KEITHLEY_2601A_LimitMode.Current))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, KEITHLEY_2602A_Channels.ChannelA);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, KEITHLEY_2602A_Channels.ChannelA);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);
            }
            else if ((_DeviceSettings.SelectedChannel == KEITHLEY_2602A_Channels.ChannelB) && (_DeviceSettings.LimitMode == KEITHLEY_2601A_LimitMode.Voltage))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, KEITHLEY_2602A_Channels.ChannelB);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, KEITHLEY_2602A_Channels.ChannelB);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == KEITHLEY_2602A_Channels.ChannelB) && (_DeviceSettings.LimitMode == KEITHLEY_2601A_LimitMode.Current))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, KEITHLEY_2602A_Channels.ChannelB);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, KEITHLEY_2602A_Channels.ChannelB);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);
            }

            return _Device;
        }

		private void on_cmdSaveSettingsClick(object sender, System.Windows.RoutedEventArgs e)
		{
            _Device = SetDevice();
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