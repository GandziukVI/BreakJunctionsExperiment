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
        #region Constructor / destructor

        public AgilentUSB_Device(string ID)
        {
            this._Id = ID;
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
                _rMgr = new ResourceManager();
                _src = new FormattedIO488();
                _Alive = false;
                _IsBusy = false;

                _SetBusy();

                _src.IO = (IMessage)_rMgr.Open(_Id);
                //_src.IO.Timeout = int.MaxValue;
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
                _src.FlushWrite();
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

        public virtual string RequestQuery(string Query)
        {
            SendCommandRequest(Query);
            return ReceiveDeviceAnswer();
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
