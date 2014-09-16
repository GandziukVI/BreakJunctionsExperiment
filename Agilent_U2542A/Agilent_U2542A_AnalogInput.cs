using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A
{
    public class Agilent_U2542A_AnalogInput : AgilentUSB_Device
    {
        #region Constructor

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

        public bool CheckAcquisitionStatus()
        {
            string status = RequestQuery("WAV:STAT?");

            if (status == "OVER\n")
                throw new Exception("Device buffer overload");
            if (status == "DATA\n") 
                return true;

            return false;
        }

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

        public override string RequestQuery(string Query)
        {
            try
            {
                string result = base.RequestQuery(Query);
                return result.Substring(0, result.Length - 1);
            }
            catch { return null; }
        }

        public string AcquireRawADC_Data()
        {
            return RequestQuery("WAV:DATA?");
        }

        #endregion

        #endregion
    }
}
