using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Motion
{
    class FaulhaberMinimotor_SA_2036U012V_K1155_ControllerFactory : IMotionFactory
    {
        private FaulhaberMinimotor_SA_2036U012V_K1155_MotionController _MotionController;

        public FaulhaberMinimotor_SA_2036U012V_K1155_ControllerFactory(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            _MotionController = new FaulhaberMinimotor_SA_2036U012V_K1155_MotionController(comPort, baud, parity, dataBits, stopBits, returnToken);
        }
        public MotionController GetMotionController()
        {
            return _MotionController;
        }
    }
}
