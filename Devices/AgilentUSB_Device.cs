using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Devices;

using Ivi.Visa.Interop;

namespace Agilent_U2542A
{
    public class AgilentUSB_Device : IExperimentalDevice, IDisposable
    {
        #region Constructor / destructor

        public AgilentUSB_Device(string ID)
        {
            this._Id = ID;
            //this.InitDevice();
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

        private bool _Busy;
        public bool IsBusy
        {
            get { return _Busy; }
        }

        private void _SetBusy()
        {
            _Busy = true;
        }

        private void _SetNotBusy()
        {
            _Busy = false;
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
                _Busy = false;

                _SetBusy();

                _src.IO = (IMessage)_rMgr.Open(_Id);
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
