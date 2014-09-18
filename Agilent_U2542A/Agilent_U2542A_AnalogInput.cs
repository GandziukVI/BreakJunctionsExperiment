using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A
{
    public class Agilent_U2542A_AnalogInput : AgilentUSB_Device
    {
        #region Constructor

        /// <summary>
        /// Creates the instance of Agilent_U2542A_AnalogInput class
        /// for managing analog input of the device
        /// </summary>
        /// <param name="ID"></param>
        public Agilent_U2542A_AnalogInput(string ID = "USB0::0x0957::0x1718::TW52524501::INSTR")
            : base(ID)
        {
            var _InitSuccess = false;

            if (!this.IsAlive)
                _InitSuccess = this.InitDevice();

            if (!_InitSuccess) throw new Exception("Device Not Connected");
        }

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

            if (status == "OVER\n")
                throw new Exception("Device buffer overload");
            if (status == "DATA\n") 
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

            if (status == "NO\n") 
                return false;
            if (status == "YES\n") 
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
        public override string RequestQuery(string Query)
        {
            try
            {
                string result = base.RequestQuery(Query);
                return result.Substring(0, result.Length - 1);
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
