﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For registry
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using Keithley_4200;
using BreakJunctions.NotificationSystem;

//using Keithley_2602A.DeviceConfiguration;
using KeithleyInstruments;

namespace BreakJunctions
{
    public class Registry_Keithley4200
    {
        private string _StringFormat = "E8";
        private NumberFormatInfo _NumberFormat = NumberFormatInfo.InvariantInfo;

        private string _VisaID = string.Empty;
        public string VisaID
        {
            get
            {
                using (var Keithley4200A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200"))
                {
                    _VisaID = (string)Keithley4200A_Settings.GetValue("VisaID", string.Empty);
                    return _VisaID;
                }
            }
            set
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200", true))
                {
                    _VisaID = value;
                    Keithley4200_Settings.SetValue("VisaID", _VisaID, RegistryValueKind.String);
                }
            }
        }

        private IntegrationTime _MeasurementSpeed = IntegrationTime.Medium;
        public IntegrationTime MeasurementSpeed
        {
            get
            {
                using (var Keithley4200A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200"))
                {
                    _MeasurementSpeed = (IntegrationTime)int.Parse((string)Keithley4200A_Settings.GetValue("IntegrationTime"));
                    return _MeasurementSpeed;
                }
            }
            set
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200", true))
                {
                    _MeasurementSpeed = value;
                    Keithley4200_Settings.SetValue("IntegrationTime", ((int)value).ToString(), RegistryValueKind.String);
                }
            }
        }

        private bool _IsVoltageModeChecked = true;
        public bool IsVoltageModeChecked
        {
            get
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200"))
                {
                    _IsVoltageModeChecked = bool.Parse((string)Keithley4200_Settings.GetValue("IsVoltageModeChecked"));
                }

                return _IsVoltageModeChecked;
            }
            set
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200", true))
                {
                    _IsVoltageModeChecked = value;
                    Keithley4200_Settings.SetValue("IsVoltageModeChecked", _IsVoltageModeChecked, RegistryValueKind.String);
                }
            }
        }

        private bool _IsCurrentModeChecked = false;
        public bool IsCurrentModeChecked
        {
            get
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200"))
                {
                    _IsCurrentModeChecked = bool.Parse((string)Keithley4200_Settings.GetValue("IsCurrentModeChecked"));
                }

                return _IsCurrentModeChecked;
            }
            set
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200", true))
                {
                    _IsCurrentModeChecked = value;
                    Keithley4200_Settings.SetValue("IsCurrentModeChecked", _IsCurrentModeChecked, RegistryValueKind.String);
                }
            }
        }

        private double _CurrentLimit = 0.0001;
        public double CurrentLimit
        {
            get
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200"))
                {
                    _CurrentLimit = double.Parse((string)Keithley4200_Settings.GetValue("CurrentLimit"), NumberStyles.Float, _NumberFormat);
                }

                return _CurrentLimit;
            }
            set
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200", true))
                {
                    _CurrentLimit = value;
                    Keithley4200_Settings.SetValue("CurrentLimit", _CurrentLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private double _VoltageLimit = 1.0;
        public double VoltageLimit
        {
            get
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200"))
                {
                    _VoltageLimit = double.Parse((string)Keithley4200_Settings.GetValue("VoltageLimit"), NumberStyles.Float, _NumberFormat);
                }

                return _VoltageLimit;
            }
            set
            {
                using (var Keithley4200_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200", true))
                {
                    _VoltageLimit = value;
                    Keithley4200_Settings.SetValue("VoltageLimit", _CurrentLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_01_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_01_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_01_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_01_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_01"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_01_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_01_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_01_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_01", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_01_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_01_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_01_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_01_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_02_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_02_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_02_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_02_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_02"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_02_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_02_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_02_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_02", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_02_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_02_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_02_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_02_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_03_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_03_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_03_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_03_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_03"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_03_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_03_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_03_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_03", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_03_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_03_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_03_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_03_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_04_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_04_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_04_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_04_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_04"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_04_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_04_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_04_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_04", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_04_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_04_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_04_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_04_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_05_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_05_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_05_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_05_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_05"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_05_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_05_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_05_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_05", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_05_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_05_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_05_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_05_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_06_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_06_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_06_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_06_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_06"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_06_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_06_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_06_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_06", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_06_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_06_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_06_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_06_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_07_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_07_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_07_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_07_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_07"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_07_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_07_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_07_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_07", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_07_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_07_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_07_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_07_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }

        private ObservableCollection<Keithley4200_RangeAccuracySet> _Keithley4200_Channel_08_RangesAccuracyCollection = new ObservableCollection<Keithley4200_RangeAccuracySet>();
        public ObservableCollection<Keithley4200_RangeAccuracySet> Keithley4200_Channel_08_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley4200_Channel_08_RangesAccuracyCollection.Count > 0)
                    _Keithley4200_Channel_08_RangesAccuracyCollection.Clear();

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_08"))
                {
                    var RangeNames = Keithley4200_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley4200_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = (IntegrationTime)int.Parse((string)range.GetValue("Accuracy"));

                            _Keithley4200_Channel_08_RangesAccuracyCollection.Add(new Keithley4200_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
                        }
                    }
                }

                return _Keithley4200_Channel_08_RangesAccuracyCollection;
            }
            set
            {
                _Keithley4200_Channel_08_RangesAccuracyCollection = value;
                var name = "Range_{0}{1}{2}";

                using (var Keithley4200_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_4200\RangeSettings\Channel_08", true))
                {
                    var PreviousData = Keithley4200_Ranges.GetSubKeyNames();
                    foreach (var item in PreviousData)
                        Keithley4200_Ranges.DeleteSubKeyTree(item);

                    for (int i = 0; i < _Keithley4200_Channel_08_RangesAccuracyCollection.Count; i++)
                    {
                        using (var range = Keithley4200_Ranges.CreateSubKey(string.Format(name, (i / 100) % 10, (i / 10) % 10, i % 10)))
                        {
                            range.SetValue("MinRangeLimit", _Keithley4200_Channel_08_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley4200_Channel_08_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", ((int)_Keithley4200_Channel_08_RangesAccuracyCollection[i].Accuracy).ToString(), RegistryValueKind.String);
                        }
                    }
                }
            }
        }
    }

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
                using (var Keithley2602A_Settings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A", true))
                {
                    _VisaID = value;
                    Keithley2602A_Settings.SetValue("VisaID", _VisaID, RegistryValueKind.String);
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

        private ObservableCollection<Keithley2602A_RangeAccuracySet> _Keithley2602A_Channel_A_RangesAccuracyCollection = new ObservableCollection<Keithley2602A_RangeAccuracySet>();
        public ObservableCollection<Keithley2602A_RangeAccuracySet> Keithley2602A_Channel_A_RangesAccuracyCollection
        {
            get
            {
                if (_Keithley2602A_Channel_A_RangesAccuracyCollection.Count > 0)
                    _Keithley2602A_Channel_A_RangesAccuracyCollection.Clear();

                using (var Keithley2602A_Ranges = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\HardwareSettings\Keithley_2602A\RangeSettings\Channel_A"))
                {
                    var RangeNames = Keithley2602A_Ranges.GetSubKeyNames();

                    foreach (var name in RangeNames)
                    {
                        using (var range = Keithley2602A_Ranges.OpenSubKey(name))
                        {
                            var MinRangeLimit = double.Parse((string)range.GetValue("MinRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var MaxRangeLimit = double.Parse((string)range.GetValue("MaxRangeLimit"), NumberStyles.Float, _NumberFormat);
                            var Accuracy = double.Parse((string)range.GetValue("Accuracy"), NumberStyles.Float, _NumberFormat);

                            _Keithley2602A_Channel_A_RangesAccuracyCollection.Add(new Keithley2602A_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
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

        private ObservableCollection<Keithley2602A_RangeAccuracySet> _Keithley2602A_Channel_B_RangesAccuracyCollection = new ObservableCollection<Keithley2602A_RangeAccuracySet>();
        public ObservableCollection<Keithley2602A_RangeAccuracySet> Keithley2602A_Channel_B_RangesAccuracyCollection
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

                            _Keithley2602A_Channel_B_RangesAccuracyCollection.Add(new Keithley2602A_RangeAccuracySet(MinRangeLimit, MaxRangeLimit, Accuracy));
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
                            range.SetValue("MinRangeLimit", _Keithley2602A_Channel_B_RangesAccuracyCollection[i].MinRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("MaxRangeLimit", _Keithley2602A_Channel_B_RangesAccuracyCollection[i].MaxRangeLimit.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                            range.SetValue("Accuracy", _Keithley2602A_Channel_B_RangesAccuracyCollection[i].Accuracy.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                        }
                    }
                }
            }
        }
    }

    public class Registry_IV_MeasurementSettings
    {
        private string _StringFormat = "E8";
        private NumberFormatInfo _NumberFormat = NumberFormatInfo.InvariantInfo;

        private bool _IsVoltageModeChecked = true;
        public bool IsVoltageModeChecked
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings"))
                {
                    _IsVoltageModeChecked = bool.Parse((string)MeasurementSettings.GetValue("IsVoltageModeChecked"));
                }

                return _IsVoltageModeChecked;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings", true))
                {
                    _IsVoltageModeChecked = value;
                    MeasurementSettings.SetValue("IsVoltageModeChecked", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private bool _IsCurrentModeChecked = false;
        public bool IsCurrentModeChecked
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings"))
                {
                    _IsCurrentModeChecked = bool.Parse((string)MeasurementSettings.GetValue("IsCurrentModeChecked"));
                }

                return _IsCurrentModeChecked;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings", true))
                {
                    _IsCurrentModeChecked = value;
                    MeasurementSettings.SetValue("IsCurrentModeChecked", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        #region 1-st Channel Measurement value settings

        //Start value settings
        private double _IV_MeasurementStartValueChannel_01 = 0.0;
        public double IV_MeasurementStartValueChannel_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _IV_MeasurementStartValueChannel_01 = double.Parse((string)MeasurementSettings.GetValue("StartValue"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _IV_MeasurementStartValueChannel_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _IV_MeasurementStartValueChannel_01 = value;
                    MeasurementSettings.SetValue("StartValue", value.ToString(_StringFormat, _NumberFormat));
                }
            }
        }

        //End value settings
        private double _IV_MeasurementEndValueChannel_01 = 1.0;
        public double IV_MeasurementEndValueChannel_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _IV_MeasurementEndValueChannel_01 = double.Parse((string)MeasurementSettings.GetValue("EndValue"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _IV_MeasurementEndValueChannel_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _IV_MeasurementEndValueChannel_01 = value;
                    MeasurementSettings.SetValue("EndValue", value.ToString(_StringFormat, _NumberFormat));
                }
            }
        }

        //Step value settings
        private double _IV_MeasurementStepChannel_01 = 0.01;
        public double IV_MeasurementStepChannel_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _IV_MeasurementStepChannel_01 = double.Parse((string)MeasurementSettings.GetValue("Step"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _IV_MeasurementStepChannel_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _IV_MeasurementStepChannel_01 = value;
                    MeasurementSettings.SetValue("Step", value.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private string _DataFileName_CH_01;
        public string DataFileName_CH_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _DataFileName_CH_01 = (string)MeasurementSettings.GetValue("DataFileName");
                }

                return _DataFileName_CH_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _DataFileName_CH_01 = value;
                    MeasurementSettings.SetValue("DataFileName", value, RegistryValueKind.String);
                }
            }
        }

        #endregion

        #region 2-nd Channel Measurement value settings

        //Start value settings
        private double _IV_MeasurementStartValueChannel_02 = 0.0;
        public double IV_MeasurementStartValueChannel_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _IV_MeasurementStartValueChannel_02 = double.Parse((string)MeasurementSettings.GetValue("StartValue"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _IV_MeasurementStartValueChannel_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _IV_MeasurementStartValueChannel_02 = value;
                    MeasurementSettings.SetValue("StartValue", value.ToString(_StringFormat, _NumberFormat));
                }
            }
        }

        //End value settings
        private double _IV_MeasurementEndValueChannel_02 = 1.0;
        public double IV_MeasurementEndValueChannel_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _IV_MeasurementEndValueChannel_02 = double.Parse((string)MeasurementSettings.GetValue("EndValue"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _IV_MeasurementEndValueChannel_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _IV_MeasurementEndValueChannel_02 = value;
                    MeasurementSettings.SetValue("EndValue", value.ToString(_StringFormat, _NumberFormat));
                }
            }
        }

        //Step value settings
        private double _IV_MeasurementStepChannel_02 = 0.01;
        public double IV_MeasurementStepChannel_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _IV_MeasurementStepChannel_02 = double.Parse((string)MeasurementSettings.GetValue("Step"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _IV_MeasurementStepChannel_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _IV_MeasurementStepChannel_02 = value;
                    MeasurementSettings.SetValue("Step", value.ToString(_StringFormat, _NumberFormat));
                }
            }
        }

        private string _DataFileName_CH_02;
        public string DataFileName_CH_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _DataFileName_CH_02 = (string)MeasurementSettings.GetValue("DataFileName");
                }

                return _DataFileName_CH_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _DataFileName_CH_02 = value;
                    MeasurementSettings.SetValue("DataFileName", value, RegistryValueKind.String);
                }
            }
        }

        #endregion

        #region Measurement Parameters

        private int _NumberOfAverages = 2;
        public int NumberOfAverages
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\MeasurementParameters"))
                {
                    _NumberOfAverages = int.Parse((string)MeasurementSettings.GetValue("NumberOfAverages"));
                }

                return _NumberOfAverages;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\MeasurementParameters", true))
                {
                    _NumberOfAverages = value;
                    MeasurementSettings.SetValue("NumberOfAverages", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private double _TimeDelay;
        public double TimeDelay
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\MeasurementParameters"))
                {
                    _TimeDelay = double.Parse((string)MeasurementSettings.GetValue("TimeDelay"), NumberStyles.Float, _NumberFormat);
                }

                return _TimeDelay;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\MeasurementParameters", true))
                {
                    _TimeDelay = value;
                    MeasurementSettings.SetValue("TimeDelay", value.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private string _Comments;
        public string Comments
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\MeasurementParameters"))
                {
                    _Comments = (string)MeasurementSettings.GetValue("Comments");
                }

                return _Comments;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\IV_MeasurementSettings\MeasurementParameters", true))
                {
                    _Comments = value;
                    MeasurementSettings.SetValue("Comments", value, RegistryValueKind.String);
                }
            }
        }


        #endregion
    }

    public class Registry_TimeTrace_MeasurementSettings
    {
        private string _StringFormat = "E8";
        private NumberFormatInfo _NumberFormat = NumberFormatInfo.InvariantInfo;

        #region 1-st Channel Parameters

        private bool _IsVoltageModeChecked_CH_01 = true;
        public bool IsVoltageModeChecked_CH_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _IsVoltageModeChecked_CH_01 = bool.Parse((string)MeasurementSettings.GetValue("IsVoltageModeChecked"));
                }

                return _IsVoltageModeChecked_CH_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _IsVoltageModeChecked_CH_01 = value;
                    MeasurementSettings.SetValue("IsVoltageModeChecked", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private bool _IsCurrentModeChecked_CH_01 = false;
        public bool IsCurrentModeChecked_CH_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _IsCurrentModeChecked_CH_01 = bool.Parse((string)MeasurementSettings.GetValue("IsCurrentModeChecked"));
                }

                return _IsCurrentModeChecked_CH_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _IsCurrentModeChecked_CH_01 = value;
                    MeasurementSettings.SetValue("IsCurrentModeChecked", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private double _ValueThroughTheStructure_CH_01;
        public double ValueThroughTheStructure_CH_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _ValueThroughTheStructure_CH_01 = double.Parse((string)MeasurementSettings.GetValue("ValueThroughTheStructure"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _ValueThroughTheStructure_CH_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _ValueThroughTheStructure_CH_01 = value;
                    MeasurementSettings.SetValue("ValueThroughTheStructure", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private string _DataFileName_CH_01;
        public string DataFileName_CH_01
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams"))
                {
                    _DataFileName_CH_01 = (string)MeasurementSettings.GetValue("DataFileName");
                }

                return _DataFileName_CH_01;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_01_ChannelParams", true))
                {
                    _DataFileName_CH_01 = value;
                    MeasurementSettings.SetValue("DataFileName", value, RegistryValueKind.String);
                }
            }
        }


        #endregion

        #region 2-nd Channel Parameters

        private bool _IsVoltageModeChecked_CH_02 = true;
        public bool IsVoltageModeChecked_CH_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _IsVoltageModeChecked_CH_02 = bool.Parse((string)MeasurementSettings.GetValue("IsVoltageModeChecked"));
                }

                return _IsVoltageModeChecked_CH_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _IsVoltageModeChecked_CH_02 = value;
                    MeasurementSettings.SetValue("IsVoltageModeChecked", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private bool _IsCurrentModeChecked_CH_02 = false;
        public bool IsCurrentModeChecked_CH_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _IsCurrentModeChecked_CH_02 = bool.Parse((string)MeasurementSettings.GetValue("IsCurrentModeChecked"));
                }

                return _IsCurrentModeChecked_CH_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _IsCurrentModeChecked_CH_02 = value;
                    MeasurementSettings.SetValue("IsCurrentModeChecked", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private double _ValueThroughTheStructure_CH_02;
        public double ValueThroughTheStructure_CH_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _ValueThroughTheStructure_CH_02 = double.Parse((string)MeasurementSettings.GetValue("ValueThroughTheStructure"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _ValueThroughTheStructure_CH_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _ValueThroughTheStructure_CH_02 = value;
                    MeasurementSettings.SetValue("ValueThroughTheStructure", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private string _DataFileName_CH_02;
        public string DataFileName_CH_02
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams"))
                {
                    _DataFileName_CH_02 = (string)MeasurementSettings.GetValue("DataFileName");
                }

                return _DataFileName_CH_02;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\CH_02_ChannelParams", true))
                {
                    _DataFileName_CH_02 = value;
                    MeasurementSettings.SetValue("DataFileName", value, RegistryValueKind.String);
                }
            }
        }

        #endregion

        #region Measurement Parameters

        private int _NumberOfAverages = 2;
        public int NumberOfAverages
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\MeasurementParameters"))
                {
                    _NumberOfAverages = int.Parse((string)MeasurementSettings.GetValue("NumberOfAverages"));
                }

                return _NumberOfAverages;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\MeasurementParameters", true))
                {
                    _NumberOfAverages = value;
                    MeasurementSettings.SetValue("NumberOfAverages", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private double _TimeDelay;
        public double TimeDelay
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\MeasurementParameters"))
                {
                    _TimeDelay = double.Parse((string)MeasurementSettings.GetValue("TimeDelay"), NumberStyles.Float, _NumberFormat);
                }

                return _TimeDelay;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\MeasurementParameters", true))
                {
                    _TimeDelay = value;
                    MeasurementSettings.SetValue("TimeDelay", value.ToString(_StringFormat, _NumberFormat), RegistryValueKind.String);
                }
            }
        }

        private string _Comments;
        public string Comments
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\MeasurementParameters"))
                {
                    _Comments = (string)MeasurementSettings.GetValue("Comments");
                }

                return _Comments;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\TimeTrace_MeasurementSettings\MeasurementParameters", true))
                {
                    _Comments = value;
                    MeasurementSettings.SetValue("Comments", value, RegistryValueKind.String);
                }
            }
        }


        #endregion
    }

    public class Registry_MotionSettings
    {
        #region Motion Parameters

        private int _PointsPerMilimeter;
        public int PointsPerMilimeter
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters"))
                {
                    _PointsPerMilimeter = int.Parse((string)MeasurementSettings.GetValue("PointsPerMilimeter"));
                }

                return _PointsPerMilimeter;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters", true))
                {
                    _PointsPerMilimeter = value;
                    MeasurementSettings.SetValue("PointsPerMilimeter", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private double _CurrentPosition;
        public double CurrentPosition
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters"))
                {
                    _CurrentPosition = double.Parse((string)MeasurementSettings.GetValue("CurrentPosition"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _CurrentPosition;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters", true))
                {
                    _CurrentPosition = value;
                    MeasurementSettings.SetValue("CurrentPosition", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private double _SpeedGoingUp;
        public double SpeedGoingUp
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\MotionSpeed"))
                {
                    _SpeedGoingUp = double.Parse((string)MeasurementSettings.GetValue("Up"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _SpeedGoingUp;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\MotionSpeed", true))
                {
                    _SpeedGoingUp = value;
                    MeasurementSettings.SetValue("Up", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private double _SpeedGoingDown;
        public double SpeedGoingDown
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\MotionSpeed"))
                {
                    _SpeedGoingDown = double.Parse((string)MeasurementSettings.GetValue("Down"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _SpeedGoingDown;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\MotionSpeed", true))
                {
                    _SpeedGoingDown = value;
                    MeasurementSettings.SetValue("Down", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private double _StartPosition;
        public double StartPosition
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\Distance"))
                {
                    _StartPosition = double.Parse((string)MeasurementSettings.GetValue("StartPosition"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _StartPosition;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\Distance", true))
                {
                    _StartPosition = value;
                    MeasurementSettings.SetValue("StartPosition", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private double _FinalDestination;
        public double FinalDestination
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\Distance"))
                {
                    _FinalDestination = double.Parse((string)MeasurementSettings.GetValue("FinalDestination"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _FinalDestination;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\Distance", true))
                {
                    _FinalDestination = value;
                    MeasurementSettings.SetValue("FinalDestination", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private int _numberCycles;
        public int NumberCycles
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\DistanceRepetitive"))
                {
                    _numberCycles = int.Parse((string)MeasurementSettings.GetValue("NumberCycles"));
                }

                return _numberCycles;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\DistanceRepetitive", true))
                {
                    _numberCycles = value;
                    MeasurementSettings.SetValue("NumberCycles", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private TimeSpan _MeasureTime;
        public TimeSpan MeasureTime
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\Time"))
                {
                    _MeasureTime = TimeSpan.Parse((string)MeasurementSettings.GetValue("MeasureTime"));
                }

                return _MeasureTime;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\Time", true))
                {
                    _MeasureTime = value;
                    MeasurementSettings.SetValue("MeasureTime", value.ToString(), RegistryValueKind.String);
                }
            }
        }

        private double _ResistanceValue;
        public double ResistanceValue
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\FixedR"))
                {
                    _ResistanceValue = double.Parse((string)MeasurementSettings.GetValue("ResistanceValue"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _ResistanceValue;
            }
            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\FixedR", true))
                {
                    _ResistanceValue = value;
                    MeasurementSettings.SetValue("ResistanceValue", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private double _currentResistanceValue;
        public double CurrentResistanceValue
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\FixedR"))
                {
                    _currentResistanceValue = double.Parse((string)MeasurementSettings.GetValue("CurrentResistanceValue"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _currentResistanceValue;
            }

            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\FixedR", true))
                {
                    _currentResistanceValue = value;
                    MeasurementSettings.SetValue("CurrentResistanceValue", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        private double _allowableDeviation;
        public double AllowableDeviation
        {
            get
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\FixedR"))
                {
                    _allowableDeviation = double.Parse((string)MeasurementSettings.GetValue("AllowableDeviation"), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                }

                return _allowableDeviation;
            }

            set
            {
                using (var MeasurementSettings = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\SoftwareSettings\MotionParameters\FixedR", true))
                {
                    _allowableDeviation = value;
                    MeasurementSettings.SetValue("AllowableDeviation", value.ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                }
            }
        }

        #endregion
    }

    public class Registry_NotificationSystem
    {
        private string _E_Mail_Address;
        public string E_Mail_Address
        {
            get
            {
                using (var NotificationSystem = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\NotificationSystem"))
                {
                    _E_Mail_Address = (string)NotificationSystem.GetValue("BreakJunctionsExperiment_Mail");
                    return _E_Mail_Address;
                }
            }
            set
            {
                using (var NotificationSystem = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\NotificationSystem", true))
                {
                    _E_Mail_Address = value;
                    NotificationSystem.SetValue("BreakJunctionsExperiment_Mail", value, RegistryValueKind.String);
                }
            }
        }

        private string _E_Mail_Password;
        public string E_Mail_Password
        {
            get
            {
                using (var NotificationSystem = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\NotificationSystem"))
                {
                    _E_Mail_Password = (string)NotificationSystem.GetValue("BreakJunctionsExperiment_MailPassword");
                    return _E_Mail_Password;
                }
            }
            set
            {
                using (var NotificationSystem = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\NotificationSystem", true))
                {
                    _E_Mail_Password = value;
                    NotificationSystem.SetValue("BreakJunctionsExperiment_MailPassword", value, RegistryValueKind.String);
                }
            }
        }

        private List<E_Mail_Info> _User_E_Mails = new List<E_Mail_Info>();
        public List<E_Mail_Info> User_E_Mails
        {
            get
            {
                if (_User_E_Mails.Count > 0)
                    _User_E_Mails.Clear();

                using (var NotificationSystem = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\NotificationSystem\UserEMails"))
                {
                    foreach (var MailInfo in NotificationSystem.GetSubKeyNames())
                        using (var info = NotificationSystem.OpenSubKey(MailInfo))
                        {
                            var FirstName = (string)info.GetValue("FirstName");
                            var LastName = (string)info.GetValue("LastName");
                            var Address = (string)info.GetValue("E_Mail_Address");

                            _User_E_Mails.Add(new E_Mail_Info(FirstName, LastName, Address));
                        }
                }

                return _User_E_Mails;
            }
            set
            {
                _User_E_Mails = value;

                using (var NotificationSystem = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment\NotificationSystem\UserEMails", true))
                {
                    foreach (var MailInfo in NotificationSystem.GetSubKeyNames())
                        NotificationSystem.DeleteSubKeyTree(MailInfo);

                    foreach (var MailInfo in value)
                    {
                        using (var info = NotificationSystem.CreateSubKey(string.Format("{0}_{1}", MailInfo.Name, MailInfo.Surname)))
                        {
                            info.SetValue("FirstName", MailInfo.Name, RegistryValueKind.String);
                            info.SetValue("Lastname", MailInfo.Surname, RegistryValueKind.String);
                            info.SetValue("E_Mail_Address", MailInfo.E_Mail_Address, RegistryValueKind.String);
                        }
                    }
                }
            }
        }
    }

    public class BreakJunctionsRegistry
    {
        #region Singleton pattern implementation

        private static readonly object _singletonCreationLock = new object();

        private static BreakJunctionsRegistry _Instance;
        public static BreakJunctionsRegistry Instance
        {
            get
            {
                if (_Instance == null)
                    lock (_singletonCreationLock)
                    {
                        if (_Instance == null)
                            _Instance = new BreakJunctionsRegistry();
                    }

                return _Instance;
            }
        }

        private BreakJunctionsRegistry() { }

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

        private Registry_Keithley4200 _Reg_Keithley_4200;
        public Registry_Keithley4200 Reg_Keithley_4200
        {
            get
            {
                if (_Reg_Keithley_4200 == null)
                    _Reg_Keithley_4200 = new Registry_Keithley4200();

                return _Reg_Keithley_4200;
            }
        }

        private Registry_IV_MeasurementSettings _Reg_IV_MeasurementSettings;
        public Registry_IV_MeasurementSettings Reg_IV_MeasurementSettings
        {
            get
            {
                if (_Reg_IV_MeasurementSettings == null)
                    _Reg_IV_MeasurementSettings = new Registry_IV_MeasurementSettings();

                return _Reg_IV_MeasurementSettings;
            }
        }

        private Registry_TimeTrace_MeasurementSettings _Reg_TimeTrace_MeasurementSettings;
        public Registry_TimeTrace_MeasurementSettings Reg_TimeTrace_MeasurementSettings
        {
            get
            {
                if (_Reg_TimeTrace_MeasurementSettings == null)
                    _Reg_TimeTrace_MeasurementSettings = new Registry_TimeTrace_MeasurementSettings();

                return _Reg_TimeTrace_MeasurementSettings;
            }
        }

        private Registry_MotionSettings _Reg_MotionSettings;
        public Registry_MotionSettings Reg_MotionSettings
        {
            get
            {
                if (_Reg_MotionSettings == null)
                    _Reg_MotionSettings = new Registry_MotionSettings();

                return _Reg_MotionSettings;
            }
        }

        private Registry_NotificationSystem _Reg_NotificationSystem;
        public Registry_NotificationSystem Reg_NotificationSystem
        {
            get
            {
                if (_Reg_NotificationSystem == null)
                    _Reg_NotificationSystem = new Registry_NotificationSystem();

                return _Reg_NotificationSystem;
            }
        }

        public void CreateApplicationRegistry()
        {
            using (var IsRegistryAlreadyCreated = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment", true))
            {
                if (IsRegistryAlreadyCreated == null)
                {
                    using (var BreakJunctionsExperiment_RegKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\V.Handziuk_Software\BreakJunctionsExperiment"))
                    {
                        #region Notification System

                        using (var BreakJunctionsExperiment_NotificationSystem = BreakJunctionsExperiment_RegKey.CreateSubKey("NotificationSystem"))
                        {
                            BreakJunctionsExperiment_NotificationSystem.SetValue("BreakJunctionsExperiment_Mail", "break_junctions_experiment@hotmail.com", RegistryValueKind.String);
                            BreakJunctionsExperiment_NotificationSystem.SetValue("BreakJunctionsExperiment_MailPassword", "BreakJunctions", RegistryValueKind.String);

                            using (var BreakJunctionsExperiment_NotificationSystem_User_EMails = BreakJunctionsExperiment_NotificationSystem.CreateSubKey("UserEMails")) { }
                        }

                        #endregion

                        #region Software Settings

                        using (var BreakJunctionsExperiment_SoftwareSettings = BreakJunctionsExperiment_RegKey.CreateSubKey("SoftwareSettings"))
                        {
                            #region I-V Measurement Settings

                            using (var IV_MeasurementSettings_Reg = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("IV_MeasurementSettings"))
                            {
                                IV_MeasurementSettings_Reg.SetValue("IsVoltageModeChecked", true.ToString(), RegistryValueKind.String);
                                IV_MeasurementSettings_Reg.SetValue("IsCurrentModeChecked", false.ToString(), RegistryValueKind.String);

                                using (var FirstChannel_Params = IV_MeasurementSettings_Reg.CreateSubKey("CH_01_ChannelParams"))
                                {
                                    FirstChannel_Params.SetValue("StartValue", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    FirstChannel_Params.SetValue("EndValue", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    FirstChannel_Params.SetValue("Step", (0.001).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    FirstChannel_Params.SetValue("DataFileName", "IV_CH_01.dat", RegistryValueKind.String);
                                }

                                using (var SecondChannel_Params = IV_MeasurementSettings_Reg.CreateSubKey("CH_02_ChannelParams"))
                                {
                                    SecondChannel_Params.SetValue("StartValue", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    SecondChannel_Params.SetValue("EndValue", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    SecondChannel_Params.SetValue("Step", (0.001).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    SecondChannel_Params.SetValue("DataFileName", "IV_CH_02.dat", RegistryValueKind.String);
                                }

                                using (var MeasurementParams = IV_MeasurementSettings_Reg.CreateSubKey("MeasurementParameters"))
                                {
                                    MeasurementParams.SetValue("NumberOfAverages", (2).ToString(), RegistryValueKind.String);
                                    MeasurementParams.SetValue("TimeDelay", (0.0005).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    MeasurementParams.SetValue("Comments", string.Empty, RegistryValueKind.String);
                                }
                            }

                            #endregion

                            #region Time Trace Measurement Settings

                            using (var TimeTrace_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("TimeTrace_MeasurementSettings"))
                            {
                                using (var FirstChannel_Params = TimeTrace_MeasurementSettings.CreateSubKey("CH_01_ChannelParams"))
                                {
                                    FirstChannel_Params.SetValue("IsVoltageModeChecked", true.ToString(), RegistryValueKind.String);
                                    FirstChannel_Params.SetValue("IsCurrentModeChecked", false.ToString(), RegistryValueKind.String);
                                    FirstChannel_Params.SetValue("ValueThroughTheStructure", (0.02).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    FirstChannel_Params.SetValue("DataFileName", "TimeTrace_CH_01.dat", RegistryValueKind.String);
                                }

                                using (var SecondChannel_Params = TimeTrace_MeasurementSettings.CreateSubKey("CH_02_ChannelParams"))
                                {
                                    SecondChannel_Params.SetValue("IsVoltageModeChecked", true.ToString(), RegistryValueKind.String);
                                    SecondChannel_Params.SetValue("IsCurrentModeChecked", false.ToString(), RegistryValueKind.String);
                                    SecondChannel_Params.SetValue("ValueThroughTheStructure", (0.02).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    SecondChannel_Params.SetValue("DataFileName", "TimeTrace_CH_02.dat", RegistryValueKind.String);
                                }

                                using (var MeasurementParams = TimeTrace_MeasurementSettings.CreateSubKey("MeasurementParameters"))
                                {
                                    MeasurementParams.SetValue("NumberOfAverages", (1).ToString(), RegistryValueKind.String);
                                    MeasurementParams.SetValue("TimeDelay", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    MeasurementParams.SetValue("Comments", string.Empty, RegistryValueKind.String);
                                }
                            }

                            #endregion

                            #region Motion Settings

                            using (var MotionParams = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("MotionParameters"))
                            {
                                MotionParams.SetValue("PointsPerMilimeter", (2000).ToString(), RegistryValueKind.String);
                                MotionParams.SetValue("CurrentPosition", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);

                                using (var MotionSpeed = MotionParams.CreateSubKey("MotionSpeed"))
                                {
                                    MotionSpeed.SetValue("Up", (4.8).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    MotionSpeed.SetValue("Down", (4.8).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                }

                                using (var DistanceParams = MotionParams.CreateSubKey("Distance"))
                                {
                                    DistanceParams.SetValue("StartPosition", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    DistanceParams.SetValue("FinalDestination", (0.005).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                }

                                using (var DistanceRepetitiveParams = MotionParams.CreateSubKey("DistanceRepetitive"))
                                {
                                    DistanceRepetitiveParams.SetValue("NumberCycles", (10).ToString(), RegistryValueKind.String);
                                }

                                using (var TimeParams = MotionParams.CreateSubKey("Time"))
                                {
                                    TimeParams.SetValue("MeasureTime", (new TimeSpan(0, 0, 0)).ToString(), RegistryValueKind.String);
                                }

                                using (var FixedR = MotionParams.CreateSubKey("FixedR"))
                                {
                                    FixedR.SetValue("ResistanceValue", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    FixedR.SetValue("CurrentResistanceValue", (0.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    FixedR.SetValue("AllowableDeviation", (20.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                }
                            }

                            #endregion

                            #region Real Time Time Trace Measurement Settings

                            using (var RealTimeTimeTrace_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("RealTimeTimeTrace_MeasurementSettings"))
                            {

                            }

                            #endregion

                            #region Noise Measurement Settings

                            using (var Noise_MeasurementSettings = BreakJunctionsExperiment_SoftwareSettings.CreateSubKey("Noise_MeasurementSettings"))
                            {

                            }

                            #endregion
                        }

                        #endregion

                        #region Hardware Settings

                        using (var BreakJunctionsExperiment_HardwareSettings = BreakJunctionsExperiment_RegKey.CreateSubKey("HardwareSettings"))
                        {
                            using (var Keithley2602A_Settings = BreakJunctionsExperiment_HardwareSettings.CreateSubKey("Keithley_2602A"))
                            {
                                Keithley2602A_Settings.SetValue("VisaID", "GPIB0::26::INSTR", RegistryValueKind.String);
                                Keithley2602A_Settings.SetValue("Accuracy", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                Keithley2602A_Settings.SetValue("IsVoltageModeChecked", (true).ToString(), RegistryValueKind.String);
                                Keithley2602A_Settings.SetValue("IsCurrentModeChecked", (false).ToString(), RegistryValueKind.String);
                                Keithley2602A_Settings.SetValue("VoltageLimit", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                Keithley2602A_Settings.SetValue("CurrentLimit", (0.0001).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);

                                using (var Keithley2602A_Ranges = Keithley2602A_Settings.CreateSubKey("RangeSettings"))
                                {
                                    var Keithley2602A_Ranges_Channnel_A = Keithley2602A_Ranges.CreateSubKey("Channel_A");
                                    var Keithley2602A_Ranges_Channnel_B = Keithley2602A_Ranges.CreateSubKey("Channel_B");
                                }
                            }

                            using (var Keithley4200_Settings = BreakJunctionsExperiment_HardwareSettings.CreateSubKey("Keithley_4200"))
                            {
                                using (var Keithley4200_Ranges = Keithley4200_Settings.CreateSubKey("RangeSettings"))
                                {
                                    Keithley4200_Settings.SetValue("VisaID", "GPIB0::17::INSTR", RegistryValueKind.String);
                                    Keithley4200_Settings.SetValue("IntegrationTime", ((int)IntegrationTime.Medium).ToString(), RegistryValueKind.String);
                                    Keithley4200_Settings.SetValue("IsVoltageModeChecked", (true).ToString(), RegistryValueKind.String);
                                    Keithley4200_Settings.SetValue("IsCurrentModeChecked", (false).ToString(), RegistryValueKind.String);
                                    Keithley4200_Settings.SetValue("VoltageLimit", (1.0).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);
                                    Keithley4200_Settings.SetValue("CurrentLimit", (0.0105).ToString("E8", NumberFormatInfo.InvariantInfo), RegistryValueKind.String);

                                    var Keithley4200_Ranges_Channel_01 = Keithley4200_Ranges.CreateSubKey("Channel_01");
                                    var Keithley4200_Ranges_Channel_02 = Keithley4200_Ranges.CreateSubKey("Channel_02");
                                    var Keithley4200_Ranges_Channel_03 = Keithley4200_Ranges.CreateSubKey("Channel_03");
                                    var Keithley4200_Ranges_Channel_04 = Keithley4200_Ranges.CreateSubKey("Channel_04");
                                    var Keithley4200_Ranges_Channel_05 = Keithley4200_Ranges.CreateSubKey("Channel_05");
                                    var Keithley4200_Ranges_Channel_06 = Keithley4200_Ranges.CreateSubKey("Channel_06");
                                    var Keithley4200_Ranges_Channel_07 = Keithley4200_Ranges.CreateSubKey("Channel_07");
                                    var Keithley4200_Ranges_Channel_08 = Keithley4200_Ranges.CreateSubKey("Channel_08");
                                }
                            }
                        }

                        #endregion
                    }
                }
            }
        }

        #endregion
    }
}
