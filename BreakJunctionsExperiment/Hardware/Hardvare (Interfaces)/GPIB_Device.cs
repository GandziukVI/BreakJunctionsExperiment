using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NationalInstruments.NI4882;
using System.Windows;

namespace Hardware
{
    public class GPIB_Device : IExperimentalDevice
    {
        private byte _PrimaryAddress;
        public byte PrimaryAddress
        {
            get { return _PrimaryAddress; }
            set { _PrimaryAddress = value; }
        }

        private byte _SecondaryAddress;
        public byte SecondaryAddress
        {
            get { return _SecondaryAddress; }
            set { _SecondaryAddress = value; }
        }

        private byte _BoardNumber;
        public byte BoardNumber
        {
            get { return _BoardNumber; }
            set { _BoardNumber = value; }
        }

        private Address _GPIB_Address;
        private Device _GPIB_Device;
        public Device GPIB_CurrentDevice { get { return _GPIB_Device; } }

        public GPIB_Device(byte primaryAddress, byte secondaryAddress, byte boardNumber)
        {
            this._PrimaryAddress = primaryAddress;
            this._SecondaryAddress = secondaryAddress;
            this._BoardNumber = boardNumber;

            InitDevice();
        }
        public virtual bool InitDevice()
        {
            try
            {
                _GPIB_Address = new Address(_PrimaryAddress, _SecondaryAddress);
                _GPIB_Device = new Device(_BoardNumber, _GPIB_Address);

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
        /// <returns>True, if the request was send to device</returns>
        public virtual bool SendCommandRequest(string RequestString)
        {
            try
            {
                _GPIB_Device.Write(RequestString);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Receives the answer of the denvice
        /// </summary>
        /// <returns>Returns the ansver, if succeed else returns empty string</returns>
        public virtual string ReceiveDeviceAnswer()
        {
            _GPIB_Device.IOTimeout = NationalInstruments.NI4882.TimeoutValue.None;
            var GPIB_Device_Responce = string.Empty;

            try { GPIB_Device_Responce = _GPIB_Device.ReadString(); }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                GPIB_Device_Responce = string.Empty;
            }

            return GPIB_Device_Responce;
        }
    }
}
