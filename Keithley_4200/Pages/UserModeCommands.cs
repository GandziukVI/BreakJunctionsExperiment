using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devices;
using Keithley_4200.Events;

namespace Keithley_4200.Pages
{
    public class UserModeCommands
    {
        #region UserModeCommands settings

        private IExperimentalDevice _TheDevice;

        private bool _IsUserModeSelected = false;
        public bool IsUserModeSelected { get { return _IsUserModeSelected; } }

        #endregion

        #region Constructor / Destructor

        public UserModeCommands(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
            _TheDevice.InitDevice();

            AllEventsHandler.Instance.SystemModeChanged += On_SystemModeChanged;
        }

        #endregion

        #region UserModeCommands functionality

        /// <summary>
        /// Sets direct voltage for channel, configured as SMU
        /// </summary>
        /// <param name="__SMU_ChannelNumber">Number of the channed, configured as SMU</param>
        /// <param name="__OutputValue">Value of voltage to output</param>
        /// <param name="__CompilanceValue">Limitation for current</param>
        public void SMU_SetDirectVoltage(SMUs __SMU_ChannelNumber, double __OutputValue, double __CompilanceValue)
        {
            var range = ImportantConstants.GetProperVoltageRange(__OutputValue);
            var command = String.Format("DV{0}, {1}, {2}, {3}", (int)__SMU_ChannelNumber, (int)range, __OutputValue.ToString("0.####", DataFormatting.NumberFormat), __CompilanceValue.ToString("0.####", DataFormatting.NumberFormat));

            _TheDevice.SendCommandRequest(command);
        }

        /// <summary>
        /// Sets direct voltage for channel, configured as SMU
        /// </summary>
        /// <param name="__SMU_ChannelNumber">Number of the channed, configured as SMU</param>
        /// <param name="__Range">Appropriate range</param>
        /// <param name="__OutputValue">Value of voltage to output</param>
        /// <param name="__CompilanceValue">Limitation for current</param>
        public void SMU_SetDirectVoltage(SMUs __SMU_ChannelNumber, VoltageSourceRanges __Range, double __OutputValue, double __CompilanceValue)
        {
            var command = String.Format("DV{0}, {1}, {2}, {3}", (int)__SMU_ChannelNumber, (int)__Range, __OutputValue.ToString("0.####", DataFormatting.NumberFormat), __CompilanceValue.ToString("0.####", DataFormatting.NumberFormat));

            _TheDevice.SendCommandRequest(command);
        }

        /// <summary>
        /// Sets direct current for channel, configured as SMU
        /// </summary>
        /// <param name="__SMU_ChannelNumber">Number of the channed, configured as SMU</param>
        /// <param name="__OutputValue">Value of current to output</param>
        /// <param name="__CompilanceValue">Limitation for voltage</param>
        public void SMU_SetDirectCurrent(SMUs __SMU_ChannelNumber, double __OutputValue, double __CompilanceValue)
        {
            var range = ImportantConstants.GetProperCurrentRange(__OutputValue);
            var command = String.Format("DI{0}, {1}, {2}, {3}", (int)__SMU_ChannelNumber, (int)range, __OutputValue.ToString("0.####", DataFormatting.NumberFormat), __CompilanceValue.ToString("0.####", DataFormatting.NumberFormat));

            _TheDevice.SendCommandRequest(command);
        }

        /// <summary>
        /// Sets direct current for channel, configured as SMU
        /// </summary>
        /// <param name="__SMU_ChannelNumber">Number of the channed, configured as SMU</param>
        /// <param name="__Range">Appropriate range</param>
        /// <param name="__OutputValue">Value of current to output</param>
        /// <param name="__CompilanceValue">Limitation for voltage</param>
        public void SMU_SetDirectCurrent(SMUs __SMU_ChannelNumber, VoltageSourceRanges __Range, double __OutputValue, double __CompilanceValue)
        {
            var command = String.Format("DI{0}, {1}, {2}, {3}", (int)__SMU_ChannelNumber, (int)__Range, __OutputValue.ToString("0.####", DataFormatting.NumberFormat), __CompilanceValue.ToString("0.####", DataFormatting.NumberFormat));

            _TheDevice.SendCommandRequest(command);
        }

        public void VS_SetVoltage(SMUs __SMU_ChannelNumber, double __Value)
        {
            var command = String.Format("DS{0}, {1}", (int)__SMU_ChannelNumber, __Value.ToString("0.####", DataFormatting.NumberFormat));

            _TheDevice.SendCommandRequest(command);
        }

        #region SystemModeChanged

        private void On_SystemModeChanged(object sender, SystemModeChanged_EventArgs e)
        {
            if (e.SelectedMode == SystemModeCommands.UserMode)
            {
                _IsUserModeSelected = true;
                _TheDevice.SendCommandRequest("US");
            }
            else
                _IsUserModeSelected = false;
        }

        #endregion

        #endregion
    }
}
