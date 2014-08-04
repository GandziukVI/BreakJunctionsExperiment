using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NationalInstruments;
using NationalInstruments.NI4882;
using System.Windows;

namespace Hardware
{
    class GPIB_HP_35670A : IExperimentalDevice
    {
        private byte _PrimaryAddress = 27;
        public byte PrimaryAddress
        {
            get { return _PrimaryAddress; }
            set { _PrimaryAddress = value; }
        }

        private byte _SecondaryAddress = 0;
        public byte SecondaryAddress
        {
            get { return _SecondaryAddress; }
            set { _SecondaryAddress = value; }
        }

        private byte _BoardNumber = 0;
        public byte BoardNumber
        {
            get { return _BoardNumber; }
            set { _BoardNumber = value; }
        }

        private Address GPIB_HP_35670A_Address;
        private Device GPIB_HP_35670A_Device;

        public GPIB_HP_35670A(byte primaryAddress, byte secondaryAddress, byte boardNumber)
        {
            this._PrimaryAddress = primaryAddress;
            this._SecondaryAddress = secondaryAddress;
            this._BoardNumber = boardNumber;

            InitDevice();
        }

        public bool InitDevice()
        {
            try
            {
                GPIB_HP_35670A_Address = new Address(_PrimaryAddress, _SecondaryAddress);
                GPIB_HP_35670A_Device = new Device(_BoardNumber, GPIB_HP_35670A_Address);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sends command request to the device
        /// </summary>
        /// <param name="RequestString">Command, to be sent to the device</param>
        /// <returns></returns>
        public bool SendCommandRequest(string RequestString)
        {
            try
            {
                GPIB_HP_35670A_Device.Write(RequestString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Receives the answer of the denvice
        /// </summary>
        /// <returns>Returns the ansver, if succeed else returns empty string</returns>
        public string ReceiveDeviceAnswer()
        {
            GPIB_HP_35670A_Device.IOTimeout = NationalInstruments.NI4882.TimeoutValue.None;
            var GPIB_HP_35670A_2602A_Responce = string.Empty;

            try
            {
                GPIB_HP_35670A_2602A_Responce = GPIB_HP_35670A_Device.ReadString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                GPIB_HP_35670A_2602A_Responce = string.Empty;
            }

            return GPIB_HP_35670A_2602A_Responce;
        }
    }
}
