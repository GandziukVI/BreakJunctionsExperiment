using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;
using System.Windows;

namespace Hardware
{
    class COM_Device : IExperimentalDevice, IDisposable
    {
        #region COM Port settings

        private SerialPort _COM_Device;

        private string _comPort;
        private int _baud;
        private Parity _parity;
        private int _dataBits;
        private StopBits _stopBits;
        private string _returnToken;

        #endregion

        private void SetSerialPort(string comPort, int baud, Parity parity, int dataBits, StopBits stopBits, string returnToken)
        {
            this._comPort = comPort;
            this._baud = baud;
            this._parity = parity;
            this._dataBits = dataBits;
            this._stopBits = stopBits;
            this._returnToken = returnToken;

            this._COM_Device = new SerialPort(comPort, baud, parity, dataBits, stopBits);
            
            //COM Device general settings

            this._COM_Device.NewLine = returnToken;
            this._COM_Device.RtsEnable = true;
            this._COM_Device.DtrEnable = true;

            //Setting max possible timeouts for IO operations

            this._COM_Device.ReadTimeout = SerialPort.InfiniteTimeout;
            this._COM_Device.WriteTimeout = SerialPort.InfiniteTimeout;

            //Data received event handling
            _COM_Device.DataReceived += _COM_Device_DataReceived;

        }

        public virtual void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string data = sp.ReadExisting();
            //throw new NotImplementedException();
        }

        public COM_Device(string comPort = "COM1", int baud = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            this.SetSerialPort(comPort, baud, parity, dataBits, stopBits, returnToken);
            this.InitDevice();
        }

        ~COM_Device()
        {
            this.Dispose();
        }

        

        public virtual bool InitDevice()
        {
            try
            {
                if (!_COM_Device.IsOpen == true)
                    _COM_Device.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual bool SendCommandRequest(string RequestString)
        {
            try
            {
                var strBytes = Encoding.ASCII.GetBytes(RequestString+'\n');
                _COM_Device.Write(strBytes, 0, strBytes.Length);//.Write(RequestString);
                return true;
            }
            catch
            { return false; }
        }

        public virtual string ReceiveDeviceAnswer()
        {
            var COM_DeviceResponce = string.Empty;

            try { COM_DeviceResponce = _COM_Device.ReadLine(); }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                COM_DeviceResponce = string.Empty;
            }

            return COM_DeviceResponce;
        }

        public virtual void Dispose()
        {
            if (_COM_Device != null)
                if (_COM_Device.IsOpen == true)
                {
                    _COM_Device.Close();
                    _COM_Device.Dispose();
                }
        }
    }
}
