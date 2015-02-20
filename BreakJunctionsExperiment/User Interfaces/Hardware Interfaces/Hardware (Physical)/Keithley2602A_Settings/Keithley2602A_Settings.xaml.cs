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

using Devices.SMU;
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

        private MVVM_Keithley2602A_Settings _DeviceSettings = null;
        public MVVM_Keithley2602A_Settings DeviceSettings
        {
            get { return _DeviceSettings; }
        }

        #endregion

        //The device

        private I_SMU _Device;
        public I_SMU Device
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

        private I_SMU SetDevice()
        {
            if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, Channels.ChannelA);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, Channels.ChannelA);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, Channels.ChannelB);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                var smu = new GPIB_KEITHLEY_2602A_CHANNEL(_DeviceSettings.PrimaryAddress, _DeviceSettings.SecondaryAddress, _DeviceSettings.BoardNumber, Channels.ChannelB);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
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
	}
}