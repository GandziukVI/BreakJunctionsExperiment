using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Events
{
    class AllEventsHandler
    {
        #region Singleton pattern implementation

        private static AllEventsHandler _Instance;
        public static AllEventsHandler Instance
        {
            get
            {
                if (_Instance == null) _Instance = new AllEventsHandler();
                return _Instance;
            }
        }

        #endregion

        #region SystemModeChanged

        private readonly object SystemModeChanged_EventLock = new object();
        private EventHandler<SystemModeChanged_EventArgs> _SystemModeChanged;
        public event EventHandler<SystemModeChanged_EventArgs> SystemModeChanged
        {
            add
            {
                lock (SystemModeChanged_EventLock)
                    if (_SystemModeChanged == null || !_SystemModeChanged.GetInvocationList().Contains(value))
                        _SystemModeChanged += value;
            }
            remove
            {
                lock (SystemModeChanged_EventLock)
                    _SystemModeChanged -= value;
            }
        }

        public void On_SystemModeChanged(object sender, SystemModeChanged_EventArgs e)
        {
            EventHandler<SystemModeChanged_EventArgs> handler;

            lock (SystemModeChanged_EventLock)
                handler = _SystemModeChanged;

            if (handler != null)
                handler(sender, e);
        }

        #endregion
    }
}
