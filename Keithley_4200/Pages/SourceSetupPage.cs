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

        private void Configure

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
