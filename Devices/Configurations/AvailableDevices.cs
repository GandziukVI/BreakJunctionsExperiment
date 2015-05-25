using System;
using System.Collections.Generic;
using System.Text;

namespace Devices
{
    public enum KnownDevices { Keithley2602A, Keithley4200, Agilent2542A }

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

        public class DeviceInfo
        {
            public KnownDevices DeviceType { get; set; }
            public object TheDevice { get; set; }

            public DeviceInfo(KnownDevices Type, object Device)
            {
                DeviceType = Type;
                TheDevice = Device;
            }
        }

        private static Dictionary<string, DeviceInfo> _DeviceCollection;
        public static Dictionary<string, DeviceInfo> DeviceCollection
        {
            get
            {
                if (_DeviceCollection == null)
                    _DeviceCollection = new Dictionary<string, DeviceInfo>();

                return _DeviceCollection;
            }
        }

        public static DeviceInfo GetExistingInstrument(string _VisaID)
        {
            DeviceInfo result;
            if (DeviceCollection.ContainsKey(_VisaID))
            {
                DeviceCollection.TryGetValue(_VisaID, out result);
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
