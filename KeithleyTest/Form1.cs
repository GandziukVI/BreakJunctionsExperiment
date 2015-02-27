using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Devices;
using Devices.SMU;
using Keithley_4200;
using Keithley_4200.Pages;
using E_755_PI_Controller;
using System.Threading;

namespace SMU.KeithleyTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var _TheDevice = new LAN_Device("169.254.222.49", 5050);
            _TheDevice.SendCommandRequest("beeper.enable = 1 ");
        }
    }
}
