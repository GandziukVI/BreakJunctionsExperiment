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
            //var motor = new FAULHABER_MINIMOTOR_SA("COM5");

            //motor.SendCommandRequest("ENCRES8");
            //motor.SendCommandRequest("MOTTYP3");
            //motor.SendCommandRequest("STW1");
            //motor.SendCommandRequest("STN1000");
            //motor.SendCommandRequest("POR6");
            //motor.SendCommandRequest("I45");
            //motor.SendCommandRequest("PP10");
            //motor.SendCommandRequest("PD14");
            //motor.SendCommandRequest("CL50");

            //motor.sendcommandrequest("lr4500000");
            //motor.sendcommandrequest("m");

            //motor.InitiateMotion();

            var d = new COM_Device("COM5");

            d.InitDevice();

            d.SendCommandRequest("DI");
            d.SendCommandRequest("EN");
            d.SendCommandRequest("LR4500000");
            d.SendCommandRequest("M");
        }
    }
}
