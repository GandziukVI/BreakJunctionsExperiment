using KeithleyInstruments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeasureResistance
{
    public class MVVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        private double _FirstChannelResistance;
        public double FirstChannelResistance 
        {
            get { return _FirstChannelResistance; }
            set
            {
                _FirstChannelResistance = value;
                OnPropertyChanged("FirstChannelResistance");
            }
        }

        private double _SecondChannelResistance;
        public double SecondChannelResistance
        {
            get { return _SecondChannelResistance; }
            set
            {
                _SecondChannelResistance = value;
                OnPropertyChanged("SecondChannelResistance");
            }
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Keithley2602A _device;
        MVVM _mvvm;
        bool isInProcess = false;

        public MainWindow()
        {
            _device = new Keithley2602A("GPIB0::26::INSTR");

            _mvvm = new MVVM();
            this.DataContext = _mvvm;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isInProcess = true;

            var measuringThread = new Thread(new ThreadStart(DataCapture));
            measuringThread.Start();
        }

        private void DataCapture()
        {
            _device.ChannelA.SwitchON();
            _device.ChannelB.SwitchON();

            _device.ChannelA.SetSpeed(1.0);
            _device.ChannelB.SetSpeed(1.0);

            while (isInProcess)
            {
                _mvvm.FirstChannelResistance = _device.ChannelA.MeasureResistance(0.02, 2, 0.0, Devices.SMU.SourceMode.Voltage);
                _mvvm.SecondChannelResistance = _device.ChannelB.MeasureResistance(0.02, 2, 0.0, Devices.SMU.SourceMode.Voltage);
            }

            _device.ChannelA.SwitchOFF();
            _device.ChannelB.SwitchOFF();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            isInProcess = false;
        }
    }
}
