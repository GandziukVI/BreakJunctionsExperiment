using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devices;

using Ivi.Visa.Interop;
using System.Threading;

namespace Agilent_U2542A
{
    public class AgilentUSB_Device : IExperimentalDevice, IDisposable
    {
        #region Singleton pattern implementation

        private static AgilentUSB_Device _Instance;
        public static AgilentUSB_Device Instance
        {
            get 
            {
                if (_Instance == null)
                    _Instance = new AgilentUSB_Device();

                return _Instance; 
            }
        }

        #endregion

        #region Constructor / destructor

        public AgilentUSB_Device() 
        {
            _Id = "USB0::0x0957::0x1718::TW52524501::INSTR";
            _rMgr = new ResourceManager();
            _src = new FormattedIO488();
            _Alive = false;
            _IsBusy = false;

            this.InitDevice();
        }

        ~AgilentUSB_Device()
        {
            this.Dispose();
        }

        #endregion

        #region Internal implementation variables and functions

        private ResourceManager _rMgr;
        private FormattedIO488 _src;

        private string _Id;
        public string Id
        {
            get { return _Id; }
            set 
            {
                _Id = value;
                this.Dispose();
                this.InitDevice();
            }
        }

        private bool _Alive;
        public bool IsAlive
        {
            get { return _Alive; }
        }

        private bool _IsBusy;
        public bool IsBusy
        {
            get { return _IsBusy; }
        }

        private int _TimeDelay = 25;
        public int TimeDelay
        {
            get { return _TimeDelay; }
            set { _TimeDelay = value; }
        }

        private void _SetBusy()
        {
            _IsBusy = true;
        }

        private void _SetNotBusy()
        {
            _IsBusy = false;
        }

        #endregion

        #region IExperimentalDevice implementation

        public virtual bool InitDevice()
        {
            try
            {
                _SetBusy();

                _src.IO = (IMessage)_rMgr.Open(this._Id);
                _Alive = true;

                _SetNotBusy();

                return true;
            }
            catch { return false; }
        }

        public virtual bool SendCommandRequest(string RequestString)
        {
            if (IsBusy) 
            {
                throw new Exception("Device is busy"); 
            }
            _SetBusy();
            
            try 
            {
                CheckValue.assertTrue(_Alive, "No Device Opened"); 
            }
            catch 
            {
                _SetNotBusy();
                return false; 
            }
            
            try
            {
                _src.WriteString(RequestString);
                //Thread.Sleep(_TimeDelay);
            }
            catch 
            {
                _SetNotBusy();
                return false; 
            }
            
            _SetNotBusy();
            
            return true;
        }

        public virtual string ReceiveDeviceAnswer()
        {
            if (IsBusy) 
                throw new Exception("Device is busy"); 
            
            _SetBusy();
            
            try  { CheckValue.assertTrue(_Alive, "No Device Opened"); }
            catch 
            {
                _SetNotBusy();
                return null; 
            }
            
            try
            {
                string result = _src.ReadString();
                _SetNotBusy();
                return result;
            }
            catch
            {
                _SetNotBusy();
                return null;
            }
        }

        public string RequestQuery(string Query)
        {
            SendCommandRequest(Query);
            return ReceiveDeviceAnswer().TrimEnd('\n');
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if(_src != null)
                _src.IO.Close();
        }

        #endregion
    }
}
