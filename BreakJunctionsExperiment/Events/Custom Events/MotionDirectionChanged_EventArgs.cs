using BreakJunctions.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Events
{
    public class MotionDirectionChanged_EventArgs : EventArgs
    {
        public MotionDirection NewDirection { get; set; }

        public MotionDirectionChanged_EventArgs(MotionDirection __NewDirection)
            : base() 
        {
            NewDirection = __NewDirection;
        }
    }
}
