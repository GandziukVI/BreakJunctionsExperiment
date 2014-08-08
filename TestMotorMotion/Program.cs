using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Hardware;

namespace TestMotorMotion
{
    class Program
    {
        static void Main(string[] args)
        {
            var motor = new FAULHABER_MINIMOTOR_SA("COM3");
            motor.LoadRelativePosition(50000000);
            motor.InitiateMotion();
        }
    }
}
