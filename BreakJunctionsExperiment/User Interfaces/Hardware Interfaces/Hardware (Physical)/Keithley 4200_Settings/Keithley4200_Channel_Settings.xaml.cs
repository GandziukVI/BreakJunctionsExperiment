using Devices;
using Devices.SMU;
using Keithley_4200;
using System;
using System.Collections.Generic;
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
            InitializeComponent();

            #region Set MVVM

            _DeviceSettings = new MVVM_Keithley4200_ChannelSettings();
            this.DataContext = _DeviceSettings;

            this.radioSourceMeasureModeVoltage.DataContext = ModelViewInteractions.IV_VoltageChangedModel;
            this.radioSourceMeasureModeCurrent.DataContext = ModelViewInteractions.IV_CurrentChangedModel;

            #endregion
        }

        private I_SMU SetDevice()
        {
            var TheDevice = AvailableDevices.AddOrGetExistingDevice(_DeviceSettings.VisaID);
            var smu = new Keithley_4200_SMU(ref TheDevice, DeviceSettings.SelectedSMU);

            smu.VoltageLimit = DeviceSettings.LimitValueVoltage;
            smu.CurrentLimit = DeviceSettings.LimitValueCurrent;
            smu.SetIntegrationTime(DeviceSettings.CurrentIntegrationTime);

            return smu;
        }

        private void on_cmdSaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _Device = SetDevice();
            this.Close();
        }
    }
}
