using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NationalInstruments.VisaNS;

namespace Devices
{
    public class VisaDevice : IExperimentalDevice
    {
        private MessageBasedSession mbSession;

        private string _ID;
        public string ID { get { return _ID; } }

        public VisaDevice(string __ID)
        {
            _ID = __ID;
            InitDevice();
        }

        public bool InitDevice()
        {
            try
            {
                mbSession = (MessageBasedSession)ResourceManager.GetLocalManager().Open(this.ID);
                mbSession.Timeout = 5000;
                return true;
            }
            catch { return false; }

        }

        public bool SendCommandRequest(string RequestString)
        {
            try
            {
                var _Request = RequestString.EndsWith("\n") ? RequestString : RequestString + "\n";
                mbSession.Write(Encoding.ASCII.GetBytes(_Request));
                return true;
            }
            catch { return false; }
        }

        public string ReceiveDeviceAnswer()
        {
            return mbSession.ReadString().TrimEnd("\r\n".ToCharArray());
        }

        public string RequestQuery(string Query)
        {
            var _Query = Query.EndsWith("\n") ? Encoding.ASCII.GetBytes(Query) : Encoding.ASCII.GetBytes(Query + "\n");
            return Encoding.ASCII.GetString(mbSession.Query(_Query)).TrimEnd("\r\n".ToCharArray());
        }
    }
}
