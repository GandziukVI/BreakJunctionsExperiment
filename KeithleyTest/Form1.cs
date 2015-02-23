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

            IExperimentalDevice d = new COM_Device("COM4", 57600);
            //var a = new Keithley_4200_SMU(ref d, SMUs.SMU2);
            //a.CurrentLimit = 0.001;
            //var b = a.MeasureResistance(0.01, 2, 0, SourceMode.Voltage);
            //var c = new ChannelDefinitionPage(ref d);
            //c.DisableChannel(SMUs.SMU2);

            var e = new E_755(ref d);
            e.SetServoControlMode(AxisIdentifier._1, ServoControlModes.ON_CurrentPos);
            e.MoveAbsolute(AxisIdentifier._1, 0);

            e.GetTargetPosition(AxisIdentifier._1);
            e.GetOnTargetStatus(AxisIdentifier._1);
            e.GetCurrentPosition(AxisIdentifier._1);

            e.ReadAutoPiezoGainCalibrationState(GainIDs.ChannelID_01);
            //e.SetServoControlMode(AxisIdentifier._1, ServoControlModes.ON_CurrentPos);
            //e.MoveAbsolute(AxisIdentifier._1, 2500);
        }
    }
}
