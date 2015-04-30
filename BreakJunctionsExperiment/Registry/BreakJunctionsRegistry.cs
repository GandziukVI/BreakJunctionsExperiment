using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For registry
using Microsoft.Win32;
using Keithley_2602A.DeviceConfiguration;
using System.Collections.ObjectModel;
using System.Globalization;

namespace BreakJunctions
{
    public class Registry_Keithley2602A
    {
        private string _StringFormat = "E8";
        private NumberFormatInfo _NumberFormat = NumberFormatInfo.InvariantInfo;

        private string _VisaID = string.Empty;
        public string VisaID
        {
            get
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A"))
                {
                    _VisaID = (string)Keithley2602A_Settings.GetValue("VisaID", string.Empty);
                    return _VisaID;
                }
            }
            set
            {
                using(var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _VisaID = value;
                    Keithley2602A_Settings.SetValue("VisaID", _VisaID);
                }
            }
        }

        private double _Accuracy = 1.0;
        public double Accuracy
        {
            get
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A"))
                {
                    _Accuracy = double.Parse((string)Keithley2602A_Settings.GetValue("Accuracy"), NumberStyles.Float, _NumberFormat);
                    return _Accuracy;
                }
            }
            set
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _Accuracy = value;
                    Keithley2602A_Settings.SetValue("Accuracy", _Accuracy.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private bool _IsVoltageModeChecked = true;
        public bool IsVoltageModeChecked
        {
            get
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A"))
                {
                    _IsVoltageModeChecked = bool.Parse((string)Keithley2602A_Settings.GetValue("IsVoltageModeChecked"));
                }

                return _IsVoltageModeChecked;
            }
            set
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _IsVoltageModeChecked = value;
                    Keithley2602A_Settings.SetValue("IsVoltageModeChecked", _IsVoltageModeChecked, RegistryValueKind.String);
                }
            }
        }

        private bool _IsCurrentModeChecked = false;
        public bool IsCurrentModeChecked
        {
            get
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A"))
                {
                    _IsCurrentModeChecked = bool.Parse((string)Keithley2602A_Settings.GetValue("IsCurrentModeChecked"));
                }

                return _IsCurrentModeChecked;
            }
            set
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _IsCurrentModeChecked = value;
                    Keithley2602A_Settings.SetValue("IsCurrentModeChecked", _IsCurrentModeChecked, RegistryValueKind.String);
                }
            }
        }

        private double _CurrentLimit = 0.0001;
        public double CurrentLimit
        {
            get
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A"))
                {
                    _CurrentLimit = double.Parse((string)Keithley2602A_Settings.GetValue("CurrentLimit"), NumberStyles.Float, _NumberFormat);
                }

                return _CurrentLimit;
            }
            set
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _CurrentLimit = value;
                    Keithley2602A_Settings.SetValue("CurrentLimit", _CurrentLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private double _VoltageLimit = 1.0;
        public double VoltageLimit
        {
            get
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A"))
                {
                    _VoltageLimit = double.Parse((string)Keithley2602A_Settings.GetValue("VoltageLimit"), NumberStyles.Float, _NumberFormat);
                }

                return _VoltageLimit;
            }
            set
            {
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _VoltageLimit = value;
                    Keithley2602A_Settings.SetValue("VoltageLimit", _CurrentLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private ObservableCollection<RangeAccuracySet> _Keithley2602A_Channel_A_RangesAccuracyCollection = new ObservableCollection<RangeAccuracySet>();
        public ObservableCollection<RangeAccuracySet> Keithley2602A_Channel_A_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley2602A_Channel_A_RangesAccuracyCollection.Count > 0)
                    _Keithley2602A_Channel_A_RangesAccuracyCollection.Clear();

                using (var Keithley2602A_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A\RangeSettings\Channel_A"))
                {
                    var RangeNames = Keithley2602A_Ranges.GetSubKeyNames();

                    foreach(var name in RangeNames)
                    {
                        using(var range = Keithley2602A_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = double.Parse((string)range.GetValue("Accuracy"), NumberStyles.Float, _NumberFormat);

                            _Keithley2602A_Channel_A_RangesAccuracyCollection.Add(new RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }
                
                return _Keithley2602A_Channel_A_RangesAccuracyCollection;
            }
            set
            {
                _Keithley2602A_Channel_A_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley2602A_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A\RangeSettings\Channel_A", true))
                {
                    var PreviousData = Keithley2602A_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley2602A_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley2602A_Channel_A_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley2602A_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley2602A_Channel_A_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley2602A_Channel_A_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", _Keithley2602A_Channel_A_RangesAccuracyCollection[i].Accuracy.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<RangeAccuracySet> _Keithley2602A_Channel_B_RangesAccuracyCollection = new ObservableCollection<RangeAccuracySet>();
        public ObservableCollection<RangeAccuracySet> Keithley2602A_Channel_B_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley2602A_Channel_B_RangesAccuracyCollection.Count > 0)
                    _Keithley2602A_Channel_B_RangesAccuracyCollection.Clear();

                using (var Keithley2602A_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A\RangeSettings\Channel_B", true))
                {
                    var RangeNames = Keithley2602A_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley2602A_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = double.Parse((string)range.GetValue("Accuracy"), NumberStyles.Float, _NumberFormat);

                            _Keithley2602A_Channel_B_RangesAccuracyCollection.Add(new RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley2602A_Channel_B_RangesAccuracyCollection;
            }
            set
            {
                _Keithley2602A_Channel_B_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley2602A_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A\RangeSettings\Channel_B", true))
                {
                    var PreviousData = Keithley2602A_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley2602A_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley2602A_Channel_B_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley2602A_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley2602A_Channel_A_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley2602A_Channel_A_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", _Keithley2602A_Channel_A_RangesAccuracyCollection[i].Accuracy.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                        }
                    }
                }
            }
        }
    }

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

        #region BreakJunctionsRegistry settings



        #endregion

        #region Functionality

        private Registry_Keithley2602A _Reg_Keithley_2602A;
        public Registry_Keithley2602A Reg_Keithley_2602A 
        {
            get
            {
                if (_Reg_Keithley_2602A == null)
                    _Reg_Keithley_2602A = new Registry_Keithley2602A();

                return _Reg_Keithley_2602A;
            }
        }

        public void CreateApplicationRegistry()
        {
            var Software_RegKey = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);

            var IsRegistryAlreadyCreated = Software_RegKey.OpenSubKey(@"V.Handziuk_Software\BreakJunctionsExperiment");

            if (IsRegistryAlreadyCreated == null)
            {
                var VHandziukSoftware_RegKey = Software_RegKey.CreateSubKey("V.Handziuk_Software");
                var BreakJunctionsExperiment_RegKey = VHandziukSoftware_RegKey.CreateSubKey("BreakJunctionsExperiment");

                var BreakJunctionsExperiment_NotificationSystem = BreakJunctionsExperiment_RegKey.CreateSubKey("NotificationSystem");
                var BreakJunctionsExperiment_NotificationSystem_User_EMails = BreakJunctionsExperiment_NotificationSystem.CreateSubKey("UserEMails");
                BreakJunctionsExperiment_NotificationSystem.SetValue("BreakJunctionsExperiment_Mail", "break_junctions_experiment@hotmail.com");
                BreakJunctionsExperiment_NotificationSystem.SetValue("BreakJunctionsExperiment_MailPassword", "BreakJunctions");

                var BreakJunctionsExperiment_SoftwareSettings = BreakJunctionsExperiment_RegKey.CreateSubKey("SoftwareSettings");

                var IV_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("IV_MeasurementSettings");
                var TimeTrace_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("TimeTrace_MeasurementSettings");
                var RealTimeTimeTrace_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("RealTimeTimeTrace_MeasurementSettings");
                var Noise_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("Noise_MeasurementSettings");

                var BreakJunctionsExperiment_HardwareSettings = BreakJunctionsExperiment_RegKey.CreateSubKey("HardwareSettings");

                var Keithley2602A_Settings = BreakJunctionsExperiment_HardwareSettings.CreateSubKey("Keithley_2602A");

                Keithley2602A_Settings.SetValue("VisaID", "GPIB0::26::INSTR", RegistryValueKind.String);
                Keithley2602A_Settings.SetValue("Accuracy", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                Keithley2602A_Settings.SetValue("IsVoltageModeChecked", (true).ToString(), RegistryValueKind.String);
                Keithley2602A_Settings.SetValue("IsCurrentModeChecked", (false).ToString(), RegistryValueKind.String);
                Keithley2602A_Settings.SetValue("VoltageLimit", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                Keithley2602A_Settings.SetValue("CurrentLimit", (0.0001).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);

                var Keithley2602A_Ranges = Keithley2602A_Settings.CreateSubKey("RangeSettings");
                var Keithley2602A_Ranges_Channnel_A = Keithley2602A_Ranges.CreateSubKey("Channel_A");
                var Keithley2602A_Ranges_Channnel_B = Keithley2602A_Ranges.CreateSubKey("Channel_B");
                
                var Keithley4200_Settings = BreakJunctionsExperiment_HardwareSettings.CreateSubKey("Keithley_4200");
                var Keithley4200_Ranges = Keithley4200_Settings.CreateSubKey("RangeSettings");
                var Keithley4200_Ranges_Channel_01 = Keithley4200_Ranges.CreateSubKey("Channel_01");
                var Keithley4200_Ranges_Channel_02 = Keithley4200_Ranges.CreateSubKey("Channel_02");
                var Keithley4200_Ranges_Channel_03 = Keithley4200_Ranges.CreateSubKey("Channel_03");
                var Keithley4200_Ranges_Channel_04 = Keithley4200_Ranges.CreateSubKey("Channel_04");
                var Keithley4200_Ranges_Channel_05 = Keithley4200_Ranges.CreateSubKey("Channel_05");
                var Keithley4200_Ranges_Channel_06 = Keithley4200_Ranges.CreateSubKey("Channel_06");
                var Keithley4200_Ranges_Channel_07 = Keithley4200_Ranges.CreateSubKey("Channel_07");
                var Keithley4200_Ranges_Channel_08 = Keithley4200_Ranges.CreateSubKey("Channel_08");

                IV_MeasurementSettings.Close();
                TimeTrace_MeasurementSettings.Close();
                RealTimeTimeTrace_MeasurementSettings.Close();
                Noise_MeasurementSettings.Close();

                BreakJunctionsExperiment_SoftwareSettings.Close();

                Keithley2602A_Ranges_Channnel_A.Close();
                Keithley2602A_Ranges_Channnel_B.Close();
                Keithley2602A_Ranges.Close();
                Keithley2602A_Settings.Close();

                Keithley4200_Ranges_Channel_01.Close();
                Keithley4200_Ranges_Channel_02.Close();
                Keithley4200_Ranges_Channel_03.Close();
                Keithley4200_Ranges_Channel_04.Close();
                Keithley4200_Ranges_Channel_05.Close();
                Keithley4200_Ranges_Channel_06.Close();
                Keithley4200_Ranges_Channel_07.Close();
                Keithley4200_Ranges_Channel_08.Close();
                Keithley4200_Ranges.Close();
                Keithley4200_Settings.Close();

                BreakJunctionsExperiment_HardwareSettings.Close();

                BreakJunctionsExperiment_NotificationSystem_User_EMails.Close();
                BreakJunctionsExperiment_NotificationSystem.Close();

                BreakJunctionsExperiment_RegKey.Close();

                VHandziukSoftware_RegKey.Close();
                Software_RegKey.Close();
            }
        }

        #endregion
    }
}
