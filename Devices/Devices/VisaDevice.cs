using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using NationalInstruments.VisaNS;
using Ivi.Visa.Interop;

namespace Devices
{
    public class VisaDevice : IExperimentalDevice
    {
        //private MessageBasedSession mbSession;
        private FormattedIO488 mbSession;
        private ResourceManager _rMgr;

        private string _ID;
        public string ID { get { return _ID; } }

        public VisaDevice(string __ID)
        {
            _ID = __ID;
            mbSession = new FormattedIO488();
            _rMgr = new ResourceManager();
            InitDevice();
        }

        public bool InitDevice()
        {
            try
            {
                //mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(this.ID);
                //mbSession.Timeout = 5000;
                mbSession.IO = (IMessage)_rMgr.Open(_ID);
                return true;
            }
            catch { return false; }

        }

        public bool SendCommandRequest(string RequestString)
        {
            try
            {
                var _Request = RequestString.EndsWith("\n") ? 
                    Encoding.ASCII.GetBytes(RequestString) : 
                    Encoding.ASCII.GetBytes(RequestString + "\n");
                //mbSession.Write(Encoding.ASCII.GetBytes(_Request));

                mbSession.IO.LockRsrc();

                mbSession.IO.Write(ref _Request, _Request.Length);

                mbSession.IO.UnlockRsrc();

                return true;
            }
            catch { return false; }
        }

        public string ReceiveDeviceAnswer()
        {
            var result = String.Empty;

            mbSession.IO.LockRsrc();

            result = mbSession.ReadString();

            mbSession.IO.UnlockRsrc();

            return result.TrimEnd("\r\n".ToCharArray());//mbSession.ReadString().TrimEnd("\r\n".ToCharArray());
        }

        public string RequestQuery(string Query)
        {
            //var _Query = Query.EndsWith("\n") ? Encoding.ASCII.GetBytes(Query) : Encoding.ASCII.GetBytes(Query + "\n");
            //return Encoding.ASCII.GetString(mbSession.Query(_Query)).TrimEnd("\r\n".ToCharArray());
            SendCommandRequest(Query);
            return ReceiveDeviceAnswer();
        }
    }
}
