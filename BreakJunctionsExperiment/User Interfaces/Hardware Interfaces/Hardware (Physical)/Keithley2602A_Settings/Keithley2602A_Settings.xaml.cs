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
using System.Text.RegularExpressions;
using System.Globalization;
using Devices;
using System.Collections.ObjectModel;

using Keithley2602A;

//using SMU.KEITHLEY_2602A;
//using Keithley_2602A.DeviceConfiguration;

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

        private I_SMU _Device;
        public I_SMU Device
        {
            get { return _Device; }
        }

        public Keithley2602A_Channel_Settings()
        {
            #region Set MVVM

            _DeviceSettings = new MVVM_Keithley2602A_Settings();
            this.DataContext = _DeviceSettings;

            #endregion

            InitializeComponent();

            this.radioSourceMeasureModeVoltage.DataContext = ModelViewInteractions.IV_VoltageChangedModel;
            this.radioSourceMeasureModeCurrent.DataContext = ModelViewInteractions.IV_CurrentChangedModel;
        }

        private I_SMU SetDevice()
        {
            var _ExperimentalDevice = AvailableDevices.AddOrGetExistingDevice(_DeviceSettings.VisaID);

            if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                //KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);

                //var smu = KEITHLEY_2602A.Instance.ChannelA;
                var smu = Test.Device.ChannelA;

                //KEITHLEY_2602A.Instance.ChannelA.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;
                Test.Device.ChannelA.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                //smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);


                //BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_A_RangesAccuracyCollection = KEITHLEY_2602A.Instance.ChannelA.ChannelAccuracyParams.RangeAccuracySet;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_A_RangesAccuracyCollection = Test.Device.ChannelA.ChannelAccuracyParams.RangeAccuracySet;
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelA) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                //KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);

                //var smu = KEITHLEY_2602A.Instance.ChannelA;
                var smu = Test.Device.ChannelA;

                //KEITHLEY_2602A.Instance.ChannelA.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                Test.Device.ChannelA.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                //smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelA);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);

                //BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_A_RangesAccuracyCollection = KEITHLEY_2602A.Instance.ChannelA.ChannelAccuracyParams.RangeAccuracySet;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_A_RangesAccuracyCollection = Test.Device.ChannelA.ChannelAccuracyParams.RangeAccuracySet;
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Voltage))
            {
                //KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);

                //var smu = KEITHLEY_2602A.Instance.ChannelB;
                var smu = Test.Device.ChannelB;

                //KEITHLEY_2602A.Instance.ChannelB.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;
                Test.Device.ChannelB.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                //smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient);
                _Device = smu;
                _Device.SetVoltageLimit(_DeviceSettings.LimitValueVoltage);

                //BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_B_RangesAccuracyCollection = KEITHLEY_2602A.Instance.ChannelB.ChannelAccuracyParams.RangeAccuracySet;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_B_RangesAccuracyCollection = Test.Device.ChannelB.ChannelAccuracyParams.RangeAccuracySet;
            }
            else if ((_DeviceSettings.SelectedChannel == Channels.ChannelB) && (_DeviceSettings.LimitMode == LimitMode.Current))
            {
                //KEITHLEY_2602A.Instance.SetDevice(ref _ExperimentalDevice);

                //var smu = KEITHLEY_2602A.Instance.ChannelB;
                var smu = Test.Device.ChannelB;

                //KEITHLEY_2602A.Instance.ChannelB.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;
                Test.Device.ChannelB.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                //smu.SetSpeed(_DeviceSettings.AccuracyCoefficient, Channels.ChannelB);
                smu.SetSpeed(_DeviceSettings.AccuracyCoefficient);
                _Device = smu;
                _Device.SetCurrentLimit(_DeviceSettings.LimitValueCurrent);

                //BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_B_RangesAccuracyCollection = KEITHLEY_2602A.Instance.ChannelB.ChannelAccuracyParams.RangeAccuracySet;
                BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_B_RangesAccuracyCollection = Test.Device.ChannelB.ChannelAccuracyParams.RangeAccuracySet;
            }

            return _Device;
        }

        private void Menu_ItemDeleteClick(object sender, RoutedEventArgs e)
        {
            if (AccuracyListBox.SelectedIndex == -1)
                return;

            var selected = AccuracyListBox.SelectedItem as Keithley2602A_RangeAccuracySet;

            (AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>).RemoveAt(AccuracyListBox.SelectedIndex);
        }

        private bool IsOverlapped(Keithley2602A_RangeAccuracySet NewRangeAccuracyElement)
        {
            var RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

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
            var NewElement = new Keithley2602A_RangeAccuracySet(DeviceSettings.NewMinRangeLimit, DeviceSettings.NewMaxRangeLimit, DeviceSettings.NewAccuracy);
            var ElementCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

            if (!ElementCollection.Contains(NewElement) && !IsOverlapped(NewElement))
                ElementCollection.Add(NewElement);
        }

        private void on_cmdSaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _Device = SetDevice();
            this.Close();
        }

        private void on_WorkingChannelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = sender as ComboBox;
            switch (box.SelectedIndex)
            {
                case 0:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_A_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 1:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_2602A.Keithley2602A_Channel_B_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley2602A_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    }break;
            }
        }
    }
}