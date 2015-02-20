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

namespace BreakJunctions
{
	/// <summary>
	/// Interaction logic for MeasureDeviceConfiguration.xaml
	/// </summary>
	public partial class MeasureDeviceConfiguration : Window
	{
        private AvailableSources _SelectedSource;
        public AvailableSources SelectedSource
        {
            get { return _SelectedSource; }
        }

        //List of avaliable sources settings windows should be here

        private Keithley2602A_Channel_Settings _Keithley2602A_DeviceSettings;
        public Keithley2602A_Channel_Settings Keithley2602A_DeviceSettings
        {
            get { return _Keithley2602A_DeviceSettings; }
        }

        public MeasureDeviceConfiguration()
        {
            this.InitializeComponent();
        }

		private void on_cmdConfigureDeviceClick(object sender, System.Windows.RoutedEventArgs e)
		{
            switch (this.comboBoxSelectMeasureDevice.Text)
            {
                case "KEITHLEY 2602A":
                    {
                        _SelectedSource = AvailableSources.KEITHLEY_2602A;
                        _Keithley2602A_DeviceSettings = new Keithley2602A_Channel_Settings();
                        _Keithley2602A_DeviceSettings.Show();
                        this.Close();
                    } break;
                default:
                    break;
            }
		}
	}
}