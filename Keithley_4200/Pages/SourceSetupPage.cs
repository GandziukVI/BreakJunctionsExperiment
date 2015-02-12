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

        private bool _IsSourceSetupPageSelected = false;
        public bool IsSourceSetupPageSelected { get { return _IsSourceSetupPageSelected; } }

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

            var command = String.Format("VR{0}, {1}, {2}, {3}, {4}", (int)__SelectedSource, __StartValue.ToString(DataFormatting.NumberFormat), __StopValue.ToString(DataFormatting.NumberFormat), __StepValue.ToString(DataFormatting.NumberFormat), __CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureCurrentVAR1_Sweep(SMUs __SelectedSource, double __StartValue, double __StopValue, double __StepValue, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var command = String.Format("IR{0}, {1}, {2}, {3}, {4}", (int)__SelectedSource, __StartValue.ToString(DataFormatting.NumberFormat), __StopValue.ToString(DataFormatting.NumberFormat), __StepValue.ToString(DataFormatting.NumberFormat), __CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureVoltageVAR2_Sweep(double __StartValue, double __StepValue, int __NumberOfSteps, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var command = String.Format("VP {0}, {1}, {2}, {3}", __StartValue.ToString(DataFormatting.NumberFormat), __StepValue.ToString(DataFormatting.NumberFormat), __NumberOfSteps, __CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureCurrentVAR2_Sweep(double __StartValue, double __StepValue, int __NumberOfSteps, double __CompilanceValue)
        {
            if (!IsSourceSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.SourceSetup));

            var command = String.Format("VP {0}, {1}, {2}, {3}", __StartValue.ToString(DataFormatting.NumberFormat), __StepValue.ToString(DataFormatting.NumberFormat), __NumberOfSteps, __CompilanceValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
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
