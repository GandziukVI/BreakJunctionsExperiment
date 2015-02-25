using Devices;
using Devices.SMU;
using Keithley_4200.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Pages
{
    public class ChannelDefinitionPage
    {
        #region ChannelDefinitionPage settings

        private IExperimentalDevice _TheDevice;

        private static bool _IsChannelDefinitionPageSelected = false;
        public static bool IsChannelDefinitionPageSelected { get { return _IsChannelDefinitionPageSelected; } }

        #endregion

        #region Constructor

        public ChannelDefinitionPage(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;

            AllEventsHandler.Instance.SystemModeChanged += On_SystemModeChanged;
        }

        #endregion

        #region Functionality

        public void DisableChannel(SMUs __SelectedSMU)
        {
            if (!IsChannelDefinitionPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.ChannelDefinition));

            var command = String.Format("CH{0}", (int)__SelectedSMU);
            _TheDevice.SendCommandRequest(command);
        }

        public void DefineSMU_Channel(SMUs __SelectedSMU, string __VoltageName, string __CurrentName, SourceMode __SourceMode, SourceFunction __SourceFunction)
        {
            if (!IsChannelDefinitionPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.ChannelDefinition));

            var VoltageName = (__VoltageName.Length <= 6) ? __VoltageName : __VoltageName.Substring(0, 6);
            var CurrentName = (__CurrentName.Length <= 6) ? __CurrentName : __CurrentName.Substring(0, 6);

            var command = String.Format("CH{0}, \'{1}\', \'{2}\', {3}, {4}", (int)__SelectedSMU, VoltageName, CurrentName, (int)__SourceMode, (int)__SourceFunction);
            _TheDevice.SendCommandRequest(command);
        }

        public void DefineVS_Channel(SMUs __SelectedSMU, string __VoltageSourceName, SourceFunction __SourceFunction)
        {
            if (!IsChannelDefinitionPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.ChannelDefinition));

            var VoltageSourceName = (__VoltageSourceName.Length <= 6) ? __VoltageSourceName : __VoltageSourceName.Substring(0, 6);

            var command = String.Format("VS{0}, \'{1}\', {2}", (int)__SelectedSMU, VoltageSourceName, (int)__SourceFunction);
            _TheDevice.SendCommandRequest(command);
        }

        public void DefineVM_Channel(SMUs __SelectedChannel, string __VoltmeterName)
        {
            if (!IsChannelDefinitionPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.ChannelDefinition));

            var VoltmeterName = (__VoltmeterName.Length <= 6) ? __VoltmeterName : __VoltmeterName.Substring(0, 6);

            var command = String.Format("VM{0}, \'{1}\'", (int)__SelectedChannel, VoltmeterName);
            _TheDevice.SendCommandRequest(command);
        }

        #endregion

        #region SystemModeChanged

        private void On_SystemModeChanged(object sender, SystemModeChanged_EventArgs e)
        {
            if (e.SelectedMode == SystemModeCommands.ChannelDefinition)
            {
                _IsChannelDefinitionPageSelected = true;
                _TheDevice.SendCommandRequest("DE");
            }
            else
                _IsChannelDefinitionPageSelected = false;
        }

        #endregion
    }
}
