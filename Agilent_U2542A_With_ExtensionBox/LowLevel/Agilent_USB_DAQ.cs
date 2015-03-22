using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using NationalInstruments.VisaNS;
using Ivi.Visa.Interop;

using Agilent_U2542A_With_ExtensionBox.Interfaces;
using Agilent_U2542A_With_ExtensionBox;

namespace Agilent_U2542A_With_ExtensionBox.Classes
{
    public class Agilent_USB_DAQ : SCPI_Device
    {
        //private MessageBasedSession mbSession;
        private ResourceManager _rMgr;
        private FormattedIO488 _src;

        private string _Id;
        private bool _Alive;
        private bool _Busy;

        private static Agilent_USB_DAQ _Instance;
        public static Agilent_USB_DAQ Instance
        {
            get
            {
                if (_Instance == null) _Instance = new Agilent_USB_DAQ();
                return _Instance;

            }
        }

        private Agilent_USB_DAQ()
        {
            _rMgr = new ResourceManager();
            _src = new FormattedIO488();

            _Id = "USB0::0x0957::0x1718::TW52524501::INSTR";
            //_Id = "USB0::0x0957::0x1718::TW54334510::0::INSTR";
            _Alive = false;
            _Busy = false;
        }

        private void _SetBusy()
        {
            _Busy = true;
        }

        private void _SetNotBusy()
        {
            _Busy = false;
        }

        public bool isBusy()
        {
            return _Busy;
        }

        public void SetId(string Id)
        {
            _Id = Id;
        }

        public bool Open()
        {
            this._SetBusy();
            try
            {
                //mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(this._Id);
                this._src.IO = (IMessage)this._rMgr.Open(this._Id);
                _Alive = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            this._SetNotBusy();
            return true;
        }

        public bool isAlive()
        {
            return _Alive;
        }

        public bool Open(string Id)
        {
            this.SetId(Id);
            return this.Open();

        }

        public bool WriteString(string WhatToWrite)
        {
            if (this.isBusy()) { throw new Exception("Device is busy"); }
            this._SetBusy();
            try { CheckValue.assertTrue(_Alive, "No Device Opened"); }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this._SetNotBusy();
                return false;
            }
            try
            {
                //mbSession.Write(WhatToWrite);
                _src.WriteString(WhatToWrite);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this._SetNotBusy();
                return false;
            }
            this._SetNotBusy();
            return true;
        }

        public string QueryString(string Query)
        {
            //return mbSession.Query(Query).TrimEnd('\n');
            _src.WriteString(Query);
            return this.ReadString();
        }

        public string ReadString()
        {
            if (this.isBusy()) { throw new Exception("Device is busy"); }
            this._SetBusy();
            try { CheckValue.assertTrue(_Alive, "No Device Opened"); }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                _SetNotBusy();
                return null;
            }
            try
            {
                //string result = mbSession.ReadString().TrimEnd('\n');
                string result = _src.ReadString().TrimEnd('\n');
                this._SetNotBusy();
                return result;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                this._SetNotBusy();
                return null;
            }
        }

        public void Close()
        {
            //if (mbSession != null)
            //    mbSession.Dispose();
            _src.IO.Close();
        }
    }
}
