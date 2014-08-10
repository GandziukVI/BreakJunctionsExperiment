using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Hardware;

namespace TestMotorMotion
{
    class Program
    {
        static void Main(string[] args)
        {
            var motor = new FAULHABER_MINIMOTOR_SA("COM4");

            motor.InitDevice();
            motor.EnableDevice();
            motor.LoadAbsolutePosition(0);
            motor.NotifyPosition();
            motor.InitiateMotion();

            Console.ReadKey();
        }
    }
}
