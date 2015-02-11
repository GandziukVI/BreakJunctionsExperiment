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
using Keithley_4200;
using Keithley_4200.Pages;

namespace SMU.KeithleyTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            IExperimentalDevice d = new GPIB_Device(0, 0, 0);
            var a = new UserModeCommands(ref d);
            a.SMU_SetDirectVoltage(SMUs.SMU1, -158.56789, 0.001);
        }
    }
}
