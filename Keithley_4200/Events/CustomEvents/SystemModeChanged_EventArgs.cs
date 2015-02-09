using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Events
{
    class SystemModeChanged_EventArgs : EventArgs
    {
        public SystemModeCommands SelectedMode { get; set; }

        public SystemModeChanged_EventArgs(SystemModeCommands __SelectedMode)
            : base()
        {
            SelectedMode = __SelectedMode;
        }
    }
}
