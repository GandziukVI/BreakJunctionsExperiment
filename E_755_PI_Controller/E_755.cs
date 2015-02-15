using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_755_PI_Controller
{
    public class E_755
    {
        #region E_755 settings

        private IExperimentalDevice _TheDevice;

        #endregion

        #region Constructor

        public E_755(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
        }

        #endregion

        #region Functionality

        public string RequestMotionStatus()
        {
            return _TheDevice.RequestQuery("#5");
        }

        public string GetReadyStatus()
        {
            return _TheDevice.RequestQuery("#7");
        }

        public void StopAllAxes()
        {
            _TheDevice.SendCommandRequest("#24");
        }

        public string GetID()
        {
            return _TheDevice.RequestQuery("*IDN?");
        }

        public void AutoPiezoGainCalibration()
        {

        }

        #endregion
    }
}
