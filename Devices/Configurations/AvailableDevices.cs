using System;
using System.Collections.Generic;
using System.Text;

using KeithleyInstruments;

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

        public enum KnownDevices { Keithley2602A, Keithley4200, Agilent2542A }

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

        //public static DeviceInfo AddOrGetExistingInstrument(string _VisaID, KnownDevices _Device)
        //{
        //    DeviceInfo result;
        //    if (DeviceCollection.ContainsKey(_VisaID))
        //    {
        //        DeviceCollection.TryGetValue(_VisaID, out result);
        //        return result;
        //    }
        //    else
        //    {
        //        switch (_Device)
        //        {
        //            case KnownDevices.Keithley2602A:
        //                {
        //                    //var instrument = new Keithley2602A
        //                } break;
        //            case KnownDevices.Keithley4200:
        //                break;
        //            case KnownDevices.Agilent2542A:
        //                break;
        //            default:
        //                return null;
        //        }
        //    }
        //}
    }
}
