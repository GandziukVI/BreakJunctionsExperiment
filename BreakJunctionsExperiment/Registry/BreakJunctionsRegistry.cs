using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For registry
using Microsoft.Win32;

namespace BreakJunctions
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

        #region Functionality

        public void CreateApplicationRegistry()
        {
            var Software_RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            //var VHandziukSoftware_RegKey = Software_RegKey.CreateSubKey("V_Handziuk_Software");
            var BreakJunctionsExperiment_RegKey = Software_RegKey.CreateSubKey("BreakJunctionsExperiment");

            BreakJunctionsExperiment_RegKey.SetValue("RegistryCreated", true);

            BreakJunctionsExperiment_RegKey.Close();
            //VHandziukSoftware_RegKey.Close();
            Software_RegKey.Close();
        }

        #endregion
    }
}
