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
using System.Collections.ObjectModel;
using Keithley_2602A.DeviceConfiguration;

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
            var _ExperimentalDevice = AvailableDevices.AddOrGetExistingDevice(_DeviceSettings.VisaID);

            if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);
                
                var smu = KEITHLEY_2602A.Instance.ChannelA;

                KEITHLEY_2602A.Instance.ChannelA.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>;

                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
                
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);

                var smu = KEITHLEY_2602A.Instance.ChannelA;

                KEITHLEY_2602A.Instance.ChannelA.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>;

                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);
                
                var smu = KEITHLEY_2602A.Instance.ChannelB;

                KEITHLEY_2602A.Instance.ChannelB.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>;

                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);
                
                var smu = KEITHLEY_2602A.Instance.ChannelB;

                KEITHLEY_2602A.Instance.ChannelB.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>;

                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);
            }

            return _Device;
        }

        private void Menu_ItemDeleteClick(object sender, RoutedEventArgs e)
        {
            if (AccuracyListBox.SelectedIndex == -1)
                return;

            var selected = AccuracyListBox.SelectedItem as RangeAccuracySet;

            (AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>).RemoveAt(AccuracyListBox.SelectedIndex);
        }

        private bool IsOverlapped(RangeAccuracySet NewRangeAccuracyElement)
        {
            var RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>;
            
            var Overlapped = false;
            
            foreach (var element in RangesAccuracyCollection)
                Overlapped = Overlapped ||
                    ((NewRangeAccuracyElement.MinRangeLimit >= element.MinRangeLimit && NewRangeAccuracyElement.MaxRangeLimit <= element.MaxRangeLimit) ||
                    (NewRangeAccuracyElement.MinRangeLimit <= element.MinRangeLimit && NewRangeAccuracyElement.MaxRangeLimit >= element.MaxRangeLimit) ||
                    (NewRangeAccuracyElement.MinRangeLimit <= element.MinRangeLimit && NewRangeAccuracyElement.MaxRangeLimit <= element.MaxRangeLimit && NewRangeAccuracyElement.MaxRangeLimit >= element.MinRangeLimit) ||
                    (NewRangeAccuracyElement.MinRangeLimit >= element.MinRangeLimit && NewRangeAccuracyElement.MinRangeLimit <= element.MaxRangeLimit && NewRangeAccuracyElement.MaxRangeLimit >= element.MaxRangeLimit));

            return Overlapped;
        }

        private void on_cmd_AddNewRangeClick(object sender, RoutedEventArgs e)
        {
            var NewElement = new RangeAccuracySet(DeviceSettings.NewMinRangeLimit, DeviceSettings.NewMaxRangeLimit, DeviceSettings.NewAccuracy);
            var ElementCollection = AccuracyListBox.ItemsSource as ObservableCollection<RangeAccuracySet>;

            if (!ElementCollection.Contains(NewElement) && !IsOverlapped(NewElement))
                ElementCollection.Add(NewElement);
        }

        private void on_cmdSaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _Device = SetDevice();
            this.Close();
        }
    }
}