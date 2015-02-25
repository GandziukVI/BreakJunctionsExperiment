using Devices;
using Keithley_4200.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Pages
{
    public class SourceSetupPage
    {
        #region SourceSetupPage settings

        private IExperimentalDevice _TheDevice;

        private static bool _IsSourceSetupPageSelected = false;
        public static bool IsSourceSetupPageSelected { get { return _IsSourceSetupPageSelected; } }

        #endregion

        #region Constructor

        public SourceSetupPage(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;

            AllEventsHandler.Instance.SystemModeChanged += On_SystemModeChanged;
        }

        #endregion

        #region Functionality

        public void ConfigureVoltageVAR1_Sweep(SMUs __SelectedSource, double __StartValue, double __StopValue, double __StepValue, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var StartValue = ImportantConstants.GetProperValueForSertainRange(__StartValue, -210.0, 210.0);
            var StepValue = ImportantConstants.GetProperValueForSertainRange(__StepValue, -210.0, 210.0);
            var StopValue = ImportantConstants.GetProperValueForSertainRange(__StopValue, -210.0, 210.0);
            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, -210.0, 210.0);

            var command = String.Format("VR{0}, {1}, {2}, {3}, {4}", (int)__SelectedSource, StartValue.ToString(DataFormatting.NumberFormat), StopValue.ToString(DataFormatting.NumberFormat), StepValue.ToString(DataFormatting.NumberFormat), CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureCurrentVAR1_Sweep(SMUs __SelectedSource, double __StartValue, double __StopValue, double __StepValue, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var StartValue = ImportantConstants.GetProperValueForSertainRange(__StartValue, -0.105, 0.105);
            var StepValue = ImportantConstants.GetProperValueForSertainRange(__StepValue, -0.105, 0.105);
            var StopValue = ImportantConstants.GetProperValueForSertainRange(__StopValue, -0.105, 0.105);
            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, -0.105, 0.105);

            var command = String.Format("IR{0}, {1}, {2}, {3}, {4}", (int)__SelectedSource, StartValue.ToString(DataFormatting.NumberFormat), StopValue.ToString(DataFormatting.NumberFormat), StepValue.ToString(DataFormatting.NumberFormat), CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureVoltageVAR2_Sweep(double __StartValue, double __StepValue, int __NumberOfSteps, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var StartValue = ImportantConstants.GetProperValueForSertainRange(__StartValue, -210.0, 210.0);
            var StepValue = ImportantConstants.GetProperValueForSertainRange(__StepValue, -210.0, 210.0);
            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, -210.0, 210.0);
            var NumberOfSteps = Convert.ToInt16(ImportantConstants.GetProperValueForSertainRange(__NumberOfSteps, 1.0, 32.0));

            var command = String.Format("VP {0}, {1}, {2}, {3}", StartValue.ToString(DataFormatting.NumberFormat), StepValue.ToString(DataFormatting.NumberFormat), NumberOfSteps, CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureCurrentVAR2_Sweep(double __StartValue, double __StepValue, int __NumberOfSteps, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var StartValue = ImportantConstants.GetProperValueForSertainRange(__StartValue, -0.105, 0.105);
            var StepValue = ImportantConstants.GetProperValueForSertainRange(__StepValue, -0.105, 0.105);
            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, -0.105, 0.105);
            var NumberOfSteps = Convert.ToInt16(ImportantConstants.GetProperValueForSertainRange(__NumberOfSteps, 1.0, 32.0));

            var command = String.Format("IP {0}, {1}, {2}, {3}", StartValue.ToString(DataFormatting.NumberFormat), StepValue.ToString(DataFormatting.NumberFormat), NumberOfSteps, CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void SetAutoStandby(SMUs __SelectedSMU, bool __Eanbled)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var command = String.Format("ST {0}, {1}", (int)__SelectedSMU, Convert.ToInt16(__Eanbled));
            _TheDevice.SendCommandRequest(command);
        }

        public void SetVAR1_StrichRatio(double __Ratio, SMUs __SelectedSMU)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var Ratio = ImportantConstants.GetProperValueForSertainRange(__Ratio, -10.0, 10.0);

            var command = String.Format("RT {0}[,{1}]", Ratio.ToString(DataFormatting.NumberFormat), (int)__SelectedSMU);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetVAR1_StrichOffset(double __Offset, SMUs __SelectedSMU)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var Offset = ImportantConstants.GetProperValueForSertainRange(__Offset, -210.0, 210.0);

            var command = String.Format("FS {0}[,{1}]", Offset.ToString(DataFormatting.NumberFormat), (int)__SelectedSMU);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetVoltageListSweep(SMUs __SelectedSMU, MasterOrSlaveMode __Mode, double __CompilanceValue, params double[] __ListValues)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.CurrentRange);
            var ListValues = new double[__ListValues.Length];

            for (int i = 0; i < ListValues.Length; i++)
                ListValues[i] = ImportantConstants.GetProperValueForSertainRange(__ListValues[i], Ranges.VoltageRange);

            var command = String.Format("VL{0}, {1}, {2}, ", (int)__SelectedSMU,(int)__Mode, CompilanceValue.ToString(DataFormatting.NumberFormat));
            for (int i = 0; i < ListValues.Length - 1; i++)
                command += String.Format("{0}, ", ListValues[i].ToString(DataFormatting.NumberFormat));

            command += ListValues[ListValues.Length - 1].ToString(DataFormatting.NumberFormat);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetVoltageListSweep(SMUs __SelectedSMU, MasterOrSlaveMode __Mode, double __CompilanceValue, List<double> __ListValues)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.CurrentRange);
            var ListValues = new double[__ListValues.Count];

            for (int i = 0; i < ListValues.Length; i++)
                ListValues[i] = ImportantConstants.GetProperValueForSertainRange(__ListValues[i], Ranges.VoltageRange);

            var command = String.Format("VL{0}, {1}, {2}, ", (int)__SelectedSMU, (int)__Mode, CompilanceValue.ToString(DataFormatting.NumberFormat));
            for (int i = 0; i < ListValues.Length - 1; i++)
                command += String.Format("{0}, ", ListValues[i].ToString(DataFormatting.NumberFormat));

            command += ListValues[ListValues.Length - 1].ToString(DataFormatting.NumberFormat);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetCurrentListSweep(SMUs __SelectedSMU, MasterOrSlaveMode __Mode, double __CompilanceValue, params double[] __ListValues)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.VoltageRange);
            var ListValues = new double[__ListValues.Length];

            for (int i = 0; i < ListValues.Length; i++)
                ListValues[i] = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.CurrentRange);

            var command = String.Format("IL{0}, {1}, {2}, ", (int)__SelectedSMU, (int)__Mode, CompilanceValue.ToString(DataFormatting.NumberFormat));
            for (int i = 0; i < ListValues.Length - 1; i++)
                command += String.Format("{0}, ", ListValues[i].ToString(DataFormatting.NumberFormat));

            command += ListValues[ListValues.Length - 1].ToString(DataFormatting.NumberFormat);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetCurrentListSweep(SMUs __SelectedSMU, MasterOrSlaveMode __Mode, double __CompilanceValue, List<double> __ListValues)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.VoltageRange);
            var ListValues = new double[__ListValues.Count];

            for (int i = 0; i < ListValues.Length; i++)
                ListValues[i] = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.CurrentRange);

            var command = String.Format("IL{0}, {1}, {2}, ", (int)__SelectedSMU, (int)__Mode, CompilanceValue.ToString(DataFormatting.NumberFormat));
            for (int i = 0; i < ListValues.Length - 1; i++)
                command += String.Format("{0}, ", ListValues[i].ToString(DataFormatting.NumberFormat));

            command += ListValues[ListValues.Length - 1].ToString(DataFormatting.NumberFormat);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetSMU_ConstantVoltage(SMUs __SelectedSMU, double __OutputValue, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var OutputValue = ImportantConstants.GetProperValueForSertainRange(__OutputValue, Ranges.VoltageRange);
            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.CurrentRange);

            var command = String.Format("VC{0}, {1}, {2}", (int)__SelectedSMU, OutputValue.ToString(DataFormatting.NumberFormat), CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void SetSMU_ConstantCurrent(SMUs __SelectedSMU, double __OutputValue, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var OutputValue = ImportantConstants.GetProperValueForSertainRange(__OutputValue, Ranges.CurrentRange);
            var CompilanceValue = ImportantConstants.GetProperValueForSertainRange(__CompilanceValue, Ranges.VoltageRange);

            var command = String.Format("IC{0}, {1}, {2}", (int)__SelectedSMU, OutputValue.ToString(DataFormatting.NumberFormat), CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void SetVS_ConstantVoltage(SMUs __SelectedVS, double __OutputValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var OutputValue = ImportantConstants.GetProperValueForSertainRange(__OutputValue, Ranges.VoltageRange);

            var command = String.Format("SC{0}, {1}", (int)__SelectedVS, OutputValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void SetSweepHoldTime(double __HoldTime)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var HoldTime = ImportantConstants.GetProperValueForSertainRange(__HoldTime, 0.0, 655.3);

            var command = String.Format("HT {0}", HoldTime.ToString(DataFormatting.NumberFormat));
        }

        public void SetSweepDelayTime(double __DelayTime)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var DelayTime = ImportantConstants.GetProperValueForSertainRange(__DelayTime, 0.0, 6.553);

            var command = String.Format("DT {0}", DelayTime.ToString(DataFormatting.NumberFormat));
        }

        #endregion

        #region SystemModeChanged

        private void On_SystemModeChanged(object sender, SystemModeChanged_EventArgs e)
        {
            if (e.SelectedMode == SystemModeCommands.SourceSetup)
            {
                _IsSourceSetupPageSelected = true;
                _TheDevice.SendCommandRequest("SS");
            }
            else
                _IsSourceSetupPageSelected = false;
        }

        #endregion
    }
}
