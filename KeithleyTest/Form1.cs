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

namespace SMU.KeithleyTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            IExperimentalDevice d = new GPIB_Device(17, 0, 0);
            //var a = new Keithley_4200_SMU(ref d, SMUs.SMU2);
            //a.CurrentLimit = 0.001;
            //var b = a.MeasureResistance(0.01, 2, 0, SourceMode.Voltage);
            //var c = new ChannelDefinitionPage(ref d);
            //c.DisableChannel(SMUs.SMU2);

            var e = new E_755(ref d);
            e.AutoPiezoGainCalibration(GainIDs.ChannelID_02);
        }
    }
}
