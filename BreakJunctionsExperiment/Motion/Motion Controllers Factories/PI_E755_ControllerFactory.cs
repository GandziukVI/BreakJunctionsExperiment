using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Motion
{
    class PI_E755_ControllerFactory : IMotionFactory
    {
        public PI_E755_MotionController _MotionController;

        public PI_E755_ControllerFactory(string comPort = "COM4", int baud = 57600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            _MotionController = new PI_E755_MotionController(comPort, baud, parity, dataBits, stopBits, returnToken);
        }

        public MotionController GetMotionController()
        {
            return _MotionController;
        }
    }
}
