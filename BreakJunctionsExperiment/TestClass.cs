using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hardware;

namespace BreakJunctions
{
    class TestClass
    {
        public static void StartTest()
        {
            var motor = new FAULHABER_MINIMOTOR_SA("COM4");

            motor.InitDevice();
            motor.EnableDevice();
            motor.LoadAbsolutePosition(0);
            motor.NotifyPosition();
            motor.InitiateMotion();
        }
    }
}
