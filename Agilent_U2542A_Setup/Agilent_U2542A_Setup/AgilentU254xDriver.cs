using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ivi.Driver.Interop;
using Agilent.AgilentU254x.Interop;

namespace Agilent_U2542A_Setup
{
    public class AgilentU254xDriver : IDisposable
    {
        private AgilentU254x _Driver;
        public AgilentU254x Driver
        {
            get { return _Driver; }
        }

        private bool _Connected = false;

        private static AgilentU254xDriver _Instance;
        public static AgilentU254xDriver Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AgilentU254xDriver();

                return _Instance;
            }
        }

        private AgilentU254xDriver()
        {
            _Driver = new AgilentU254x();
            Connect();
        }

        private void Connect()
        {
            if (_Connected)
            {
                try
                {
                    _Driver.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
                _Connected = false;
            }
            else
            {
                string strOption;
                _Driver = new AgilentU254x();

                strOption = "Simulate=false, Cache=false, QueryInstrStatus=true";

                try
                {
                    _Driver.Initialize("USB0::2391::5912::TW52524501::INSTR", false, true, strOption);
                }
                catch (System.Exception err)
                {
                    Console.WriteLine(err.Message);
                    return;
                }
            }
        }

        public void Dispose()
        {
            _Driver.Close();
        }
    }
}
