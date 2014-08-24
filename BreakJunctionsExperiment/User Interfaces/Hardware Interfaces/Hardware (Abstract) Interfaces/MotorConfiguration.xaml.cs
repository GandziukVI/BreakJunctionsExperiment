using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for MotorConfiguration.xaml
    /// </summary>
    public partial class MotorConfiguration : Window
    {
        //List of avaliable motor settings should be here

        public MotorConfiguration()
        {
            InitializeComponent();
        }

        public void on_cmdConfigureDeviceClick(object sender, RoutedEventArgs e)
        {
            switch (this.comboBoxSelectMotorDevice.Text)
            {
                case "Faulhaber minimotor SA 2036U012V K1155":
                    {

                    }break;
                default:
                    break;
            }
        }
    }
}
