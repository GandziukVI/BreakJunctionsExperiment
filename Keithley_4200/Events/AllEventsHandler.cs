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



        #endregion
    }
}
