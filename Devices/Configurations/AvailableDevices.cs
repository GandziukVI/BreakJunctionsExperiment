using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    public static class AvailableDevices
    {
        private static Dictionary<string, IExperimentalDevice> _Collection;
        public static Dictionary<string, IExperimentalDevice> Collection
        {
            get
            {
                if (_Collection == null)
                    _Collection = new Dictionary<string, IExperimentalDevice>();

                return _Collection;
            }
        }

        public static IExperimentalDevice AddOrGetExistingDevice(string _VisaID)
        {
            IExperimentalDevice result;
            if (Collection.ContainsKey(_VisaID))
            {
                Collection.TryGetValue(_VisaID, out result);
                return result;
            }
            else
            {
                var NewDevice = new VisaDevice(_VisaID) as IExperimentalDevice;
                Collection.Add(_VisaID, NewDevice);
                return NewDevice;
            }
        }
    }
}
