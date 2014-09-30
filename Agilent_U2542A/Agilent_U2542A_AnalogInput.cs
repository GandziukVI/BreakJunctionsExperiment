using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A
{
    public class Agilent_U2542A_AnalogInput
    {
        #region Agilent_U2542A_AnalogInput settings

        private AgilentUSB_Device _TheDevice = AgilentUSB_Device.Instance;

        #endregion

        #region Agilent U2542A functionality

        #region Checking Aquistion status

        /// <summary>
        /// Checks the status of the device.
        /// </summary>
        /// <returns>
        /// true, if data for reading is avaliable and false otherwise.
        /// If overload appears, the exception is thrown
        /// </returns>
        public bool CheckAcquisitionStatus()
        {
            string status = RequestQuery("WAV:STAT?");

            if (status == "OVER")
                throw new Exception("Device buffer overload");
            if (status == "DATA") 
                return true;

            return false;
        }

        /// <summary>
        /// Checks the status of the device. in single shot mode
        /// </summary>
        /// <returns>
        /// true, if the data for reading is avaliable and false otherwise
        /// </returns>
        public bool CheckSingleShotAcquisitionStatus()
        {
            string status = RequestQuery("WAV:COMP?");

            if (status == "NO") 
                return false;
            if (status == "YES") 
                return true;

            return false;
        }

        #endregion

        #region Read / Write operations

        /// <summary>
        /// Sends command to the device and receives answer
        /// </summary>
        /// <param name="Query">Query to be sent to the device</param>
        /// <returns>Result of quer request</returns>
        public string RequestQuery(string Query)
        {
            try
            {
                return _TheDevice.RequestQuery(Query).TrimEnd('\n');
            }
            catch { return null; }
        }

        /// <summary>
        /// Requests raw ADC data
        /// </summary>
        /// <returns></returns>
        public string AcquireRawADC_Data()
        {
            return RequestQuery("WAV:DATA?");
        }

        #endregion

        #endregion
    }
}
