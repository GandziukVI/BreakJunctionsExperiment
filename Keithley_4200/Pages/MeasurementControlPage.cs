using Devices;
using Keithley_4200.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Pages
{
    public class MeasurementControlPage
    {
        #region MeasurementControlPage settings

        IExperimentalDevice _TheDevice;

        private static bool _IsMeasurementControlPageSelected = false;
        public static bool IsMeasurementControlPageSelected { get { return _IsMeasurementControlPageSelected; } }

        #endregion

        #region Constructor

        public MeasurementControlPage(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;

            AllEventsHandler.Instance.SystemModeChanged += On_SystemModeChanged;
        }

        #endregion

        #region Functionality

        private void ControlMeasurements(ControlMeasurement __ControlMeasurement)
        {
            if (!IsMeasurementControlPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementControl));

            var command = String.Format("ME{0}", (int)__ControlMeasurement);
            _TheDevice.SendCommandRequest(command);
        }

        #endregion

        #region SystemModeChanged

        private void On_SystemModeChanged(object sender, SystemModeChanged_EventArgs e)
        {
            if (e.SelectedMode == SystemModeCommands.MeasurementControl)
            {
                _IsMeasurementControlPageSelected = true;
                _TheDevice.SendCommandRequest("MD");
            }
            else
                _IsMeasurementControlPageSelected = false;
        }

        #endregion
    }
}
