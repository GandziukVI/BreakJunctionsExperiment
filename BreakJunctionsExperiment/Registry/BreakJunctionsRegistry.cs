using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For registry
using Microsoft.Win32;

namespace BreakJunctions.Registry
{
    public class BreakJunctionsRegistry
    {
        #region Singleton pattern implementation

        private static BreakJunctionsRegistry _Instance;
        public static BreakJunctionsRegistry Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BreakJunctionsRegistry();

                return _Instance;
            }
        }

        private BreakJunctionsRegistry() { }

        #endregion


    }
}
