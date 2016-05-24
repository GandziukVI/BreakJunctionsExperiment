using Devices;
using Devices.SMU;
using Keithley_4200;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BreakJunctions
{
    /// <summary>
    /// Interaction logic for Keithley4200_Channel_Settings.xaml
    /// </summary>
    public partial class Keithley4200_Channel_Settings : Window
    {
        #region MVVM for Keithley4200_Settings

        private MVVM_Keithley4200_ChannelSettings _DeviceSettings = null;
        public MVVM_Keithley4200_ChannelSettings DeviceSettings
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

        public Keithley4200_Channel_Settings()
        {
            #region Set MVVM

            _DeviceSettings = new MVVM_Keithley4200_ChannelSettings();
            this.DataContext = _DeviceSettings;

            #endregion

            InitializeComponent();

            this.radioSourceMeasureModeVoltage.DataContext = ModelViewInteractions.IV_VoltageChangedModel;
            this.radioSourceMeasureModeCurrent.DataContext = ModelViewInteractions.IV_CurrentChangedModel;
        }

        private I_SMU SetDevice()
        {
            var TheDevice = AvailableDevices.AddOrGetExistingDevice(_DeviceSettings.VisaID);
            var smu = new Keithley_4200_SMU(ref TheDevice, DeviceSettings.SelectedSMU);

            smu.VoltageLimit = DeviceSettings.LimitValueVoltage;
            smu.CurrentLimit = DeviceSettings.LimitValueCurrent;
            smu.ChannelAccuracyParams.RangeAccuracySet = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
            smu.SetIntegrationTime(DeviceSettings.CurrentIntegrationTime);

            return smu;
        }

        private bool IsOverlapped(Keithley4200_RangeAccuracySet NewRangeAccuracyElement)
        {
            var RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

            var Overlapped = false;

            foreach (var element in RangesAccuracyCollection)
                Overlapped = Overlapped ||
                    ((NewRangeAccuracyElement.MinRangeLimit >= element.MinRangeLimit && NewRangeAccuracyElement.MaxRangeLimit <= element.MaxRangeLimit) ||
                    (NewRangeAccuracyElement.MinRangeLimit <= element.MinRangeLimit && NewRangeAccuracyElement.MaxRangeLimit >= element.MaxRangeLimit) ||
                    (NewRangeAccuracyElement.MinRangeLimit <= element.MinRangeLimit && NewRangeAccuracyElement.MaxRangeLimit <= element.MaxRangeLimit && NewRangeAccuracyElement.MaxRangeLimit >= element.MinRangeLimit) ||
                    (NewRangeAccuracyElement.MinRangeLimit >= element.MinRangeLimit && NewRangeAccuracyElement.MinRangeLimit <= element.MaxRangeLimit && NewRangeAccuracyElement.MaxRangeLimit >= element.MaxRangeLimit));

            return Overlapped;
        }

        private void Menu_ItemDeleteClick(object sender, RoutedEventArgs e)
        {
            if (AccuracyListBox.SelectedIndex == -1)
                return;

            var selected = AccuracyListBox.SelectedItem as Keithley4200_RangeAccuracySet;

            (AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>).RemoveAt(AccuracyListBox.SelectedIndex);
        }

        private void on_cmd_AddNewRangeClick(object sender, RoutedEventArgs e)
        {
            var NewElement = new Keithley4200_RangeAccuracySet(DeviceSettings.NewMinRangeLimit, DeviceSettings.NewMaxRangeLimit, DeviceSettings.NewAccuracy);
            var ElementCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

            if (!ElementCollection.Contains(NewElement) && !IsOverlapped(NewElement))
                ElementCollection.Add(NewElement);
        }

        private void on_cmdSaveSettingsClick(object sender, RoutedEventArgs e)
        {
            switch (comboBoxWorkingChannel.SelectedIndex)
            {
                case 0:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_01_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 1:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_02_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 2:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_03_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 3:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_04_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 4:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_05_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 5:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_06_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 6:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_07_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
                case 7:
                    BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_08_RangesAccuracyCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;
                    break;
            }

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
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_01_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

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
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_02_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 2:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_03_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 3:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_04_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 4:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_05_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 5:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_06_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 6:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_07_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
                case 7:
                    {
                        var collection = BreakJunctionsRegistry.Instance.Reg_Keithley_4200.Keithley4200_Channel_08_RangesAccuracyCollection;
                        var itemsCollection = AccuracyListBox.ItemsSource as ObservableCollection<Keithley4200_RangeAccuracySet>;

                        if (collection != null)
                        {
                            if (itemsCollection.Count > 0)
                                itemsCollection.Clear();

                            foreach (var item in collection)
                                itemsCollection.Add(item);
                        }
                    } break;
            }
        }
    }
}
