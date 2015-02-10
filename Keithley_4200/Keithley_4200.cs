using Keithley_4200.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200
{
    public class Keithley_4200
    {
        public void SetSystemMode(SystemModeCommands __SelectedMode)
        {
            AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(__SelectedMode));
        }
    }
}
