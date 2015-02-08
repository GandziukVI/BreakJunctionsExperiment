using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devices;

namespace Keithley_4200.Pages
{
    class UserModeCommands
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
        }

        #endregion

        #region UserModeCommands functionality



        #endregion
    }
}
