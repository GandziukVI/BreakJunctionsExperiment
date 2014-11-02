using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Motion
{
    public interface IMotionFactory
    {
        MotionController GetMotionController();
    }
}
