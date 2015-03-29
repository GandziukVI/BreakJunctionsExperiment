using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devices;
using System.IO.Ports;

namespace FaulhaberMinimotors
{
    /// <summary>
    /// Device functionality for Faulhaber minimotor SA U012V K1155
    /// </summary>
    public class FaulhaberMinimotor_SA_2036U012V_K1155 : COM_Device
    {
        #region Configuration settings

        private readonly int _IncPerRevolution = 3000;
        /// <summary>
        /// Value to make motor 1 revolution.
        /// Is multiplied by GearFactor to get the real value.
        /// </summary>
        public int IncPerRevolution
        {
            get { return _IncPerRevolution; }
        }

        private int _GearFactor = 1526;
        /// <summary>
        /// Gear factor value
        /// </summary>
        public int GearFactor
        {
            get { return _GearFactor; }
            set { _GearFactor = value; }
        }

        /// <summary>
        /// Gets ValuePerRevolution value.
        /// For correct work, specify IncPerRevolution and TransferFactor values!
        /// </summary>
        public int ValuePerRevolution
        {
            get { return _IncPerRevolution * _GearFactor; }
        }

        private int _SpeedRPM = 15000;
        public int SpeedRPM
        {
            get { return _SpeedRPM; }
            set
            {
                if (value <= 150)
                    _SpeedRPM = 150;
                else if (value >= 15000)
                    _SpeedRPM = 15000;
                else
                    _SpeedRPM = value;

                SendCommandRequest(String.Format("SP{0}", _SpeedRPM));
            }
        }

        #endregion

        #region Constructor / Destructor

        public FaulhaberMinimotor_SA_2036U012V_K1155(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
            : base(comPort, baud, parity, dataBits, stopBits, returnToken) { }

        ~FaulhaberMinimotor_SA_2036U012V_K1155()
        {
            this.Dispose();
        }

        #endregion

        #region Motor controlling functions

        /// <summary>
        /// Enables the motor
        /// </summary>
        public void EnableDevice()
        {
            SendCommandRequest("EN");
        }

        /// <summary>
        /// Disables the motor
        /// </summary>
        public void DisableDevice()
        {
            SendCommandRequest("DI");
        }

        /// <summary>
        /// Initiates the motion
        /// </summary>
        public void InitiateMotion()
        {
            SendCommandRequest("M");
        }

        /// <summary>
        /// Loads absolute position
        /// </summary>
        /// <param name="Value"></param>
        public void LoadAbsolutePosition(int Value)
        {
            const int MaxValue = 1800000000;

            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;

            SendCommandRequest(String.Format("LA{0}", Value));
        }

        /// <summary>
        /// Loads relative position
        /// </summary>
        /// <param name="Value"></param>
        public void LoadRelativePosition(int Value)
        {
            const int MaxValue = 2140000000;

            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;

            SendCommandRequest(String.Format("LR{0}", Value));
        }

        /// <summary>
        /// Notifies position
        /// </summary>
        public void NotifyPosition()
        {
            SendCommandRequest("NP");
        }

        /// <summary>
        /// Notofoes specified position
        /// </summary>
        /// <param name="Value">Value to be notified</param>
        public void NotifyPosition(int Value)
        {
            const int MaxValue = 1800000000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;

            SendCommandRequest(String.Format("NP{0}", Value));
        }

        /// <summary>
        /// Disabling position notification
        /// </summary>
        public void NotifyPositionOff()
        {
            SendCommandRequest("NPOFF");
        }

        /// <summary>
        /// Sets velosity. Can be used only in velosit mode
        /// </summary>
        /// <param name="Value">Velosity</param>
        public void SelectVelocityMode(int Value)
        {
            int MaxValue = 30000;

            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;

            SendCommandRequest(String.Format("V{0}", Value));
        }

        /// <summary>
        /// Notifies specified velosity
        /// </summary>
        /// <param name="Value"></param>
        public void NotifyVelocity(int Value)
        {
            int MaxValue = 30000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;

            SendCommandRequest(String.Format("NV{0}", Value));
        }

        /// <summary>
        /// Disables velosity notification
        /// </summary>
        public void NotifyVelocityOff()
        {
            SendCommandRequest("NVOFF");
        }

        public int GetPosition()
        {
            var responce = RequestQuery("POS").TrimEnd("\r\n".ToCharArray());
            int result;
            int.TryParse(responce, out result);
            return result;
        }

        public void SetOutputVoltage()
        {
            throw new NotImplementedException();
        }

        public void GoHomingSequence()
        {
            throw new NotImplementedException();
        }

        public void FindHallIndex()
        {
            throw new NotImplementedException();
        }

        public void GoHallIndex()
        {
            throw new NotImplementedException();
        }

        public void GoEncoderIndex()
        {
            throw new NotImplementedException();
        }

        public void DefineHomePosition()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
