using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hardware
{
    class GPIB_Device : IExperimentalDevice
    {
        public bool InitDevice()
        {
            throw new NotImplementedException();
        }

        public bool SendCommandRequest(string RequestString)
        {
            throw new NotImplementedException();
        }

        public string ReceiveDeviceAnswer()
        {
            throw new NotImplementedException();
        }
    }
}
