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
using Devices;

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

        KEITHLEY_2602A _TheDevice;

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
            var _ExperimentalDevice = AvailableDevices.AddOrGetExistingDevice(_DeviceSettings.VisaID);

            if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                _TheDevice.SetDevice(ref _ExperimentalDevice);
                var smu = _TheDevice.ChannelA;
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {

                _TheDevice.SetDevice(ref _ExperimentalDevice);
                var smu = _TheDevice.ChannelA;
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                _TheDevice.SetDevice(ref _ExperimentalDevice);
                var smu = _TheDevice.ChannelB;
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                _TheDevice.SetDevice(ref _ExperimentalDevice);
                var smu = _TheDevice.ChannelB;                
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