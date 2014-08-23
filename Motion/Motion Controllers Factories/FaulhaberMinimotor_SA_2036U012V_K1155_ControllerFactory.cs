using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motion
{
    class FaulhaberMinimotor_SA_2036U012V_K1155_ControllerFactory : IMotionFactory
    {
        public MotionController GetMotionController()
        {
            return new FaulhaberMinimotor_SA_2036U012V_K1155_MotionController();
        }
    }
}
