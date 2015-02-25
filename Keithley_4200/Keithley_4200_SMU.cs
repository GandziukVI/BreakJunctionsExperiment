using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Devices;
using Devices.SMU;
using Keithley_4200.Events;
using Keithley_4200.Pages;

namespace Keithley_4200
{
    public class Keithley_4200_SMU : I_SMU
    {
        #region Keithley_4200_SMU settings

        public double VoltageLimit { get; set; }
        public double CurrentLimit { get; set; }

        private bool _IsEnabled = false;
        public bool IsEnabled { get { return _IsEnabled; } }

        private IExperimentalDevice _TheDevice;

        private ChannelDefinitionPage _ChannelDefinition;
        private CommonCommands _CommonCommands;
        private SourceSetupPage _SourceSetupPage;
        private MeasurementSetupPage _MeasurementSetupPage;
        private MeasurementControlPage _MeasurementControlPage;
        private UserModeCommands _UserModeCommands;

        private SMUs _SelectedSMU;

        #endregion

        #region Constructor

        public Keithley_4200_SMU(ref IExperimentalDevice __TheDevice, SMUs __SelectedSMU)
        {
            _SelectedSMU = __SelectedSMU;
            _TheDevice = __TheDevice;

            _ChannelDefinition = new ChannelDefinitionPage(ref _TheDevice);
            _CommonCommands = new CommonCommands(ref _TheDevice);
            _SourceSetupPage = new SourceSetupPage(ref _TheDevice);
            _MeasurementSetupPage = new MeasurementSetupPage(ref _TheDevice);
            _MeasurementControlPage = new MeasurementControlPage(ref _TheDevice);
            _UserModeCommands = new UserModeCommands(ref _TheDevice);
        }

        #endregion

        public bool InitDevice()
        {
            _IsEnabled = _TheDevice.InitDevice();
            return _IsEnabled;
        }

        public void SwitchON()
        {

        }

        public void SwitchOFF()
        {
            _ChannelDefinition.DisableChannel(_SelectedSMU);
        }

        public bool SetVoltageLimit(double Value)
        {
            VoltageLimit = Value;
            return true;
        }

        public bool SetCurrentLimit(double Value)
        {
            CurrentLimit = Value;
            return true;
        }

        public bool SetSourceVoltage(double Value)
        {
            try
            {
                _UserModeCommands.SMU_SetDirectVoltage(_SelectedSMU, Value, CurrentLimit);
                return true;
            }
            catch { return false; }
        }

        public bool SetSourceCurrent(double Value)
        {
            try
            {
                _UserModeCommands.SMU_SetDirectCurrent(_SelectedSMU, Value, VoltageLimit);
                return true;
            }
            catch { return false; }
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            try
            {
                switch (_SelectedSMU)
                {
                    case SMUs.SMU1:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU1).Data;
                    case SMUs.SMU2:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU2).Data;
                    case SMUs.SMU3:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU3).Data;
                    case SMUs.SMU4:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU4).Data;
                    case SMUs.SMU5:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU5).Data;
                    case SMUs.SMU6:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU6).Data;
                    case SMUs.SMU7:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU7).Data;
                    case SMUs.SMU8:
                        return _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU8).Data;
                    default:
                        return double.NaN;
                }
            }
            catch { return double.NaN; }
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            try
            {
                switch (_SelectedSMU)
                {
                    case SMUs.SMU1:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU1).Data;
                    case SMUs.SMU2:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU2).Data;
                    case SMUs.SMU3:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU3).Data;
                    case SMUs.SMU4:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU4).Data;
                    case SMUs.SMU5:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU5).Data;
                    case SMUs.SMU6:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU6).Data;
                    case SMUs.SMU7:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU7).Data;
                    case SMUs.SMU8:
                        return _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU8).Data;
                    default:
                        return double.NaN;
                }
            }
            catch { return double.NaN; }
        }

        public double MeasureResistance(double valueThroughTheStrusture, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            switch (sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        //_UserModeCommands.SMU_SetDirectVoltage(_SelectedSMU, valueThroughTheStrusture, CurrentLimit);
                        var MeasuredCurrent = 0.0;
                        switch (_SelectedSMU)
                        {
                            case SMUs.SMU1:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU1).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU2:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU2).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU3:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU3).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU4:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU4).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU5:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU5).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU6:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU6).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU7:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU7).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU8:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU8).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture / MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            default:
                                return double.NaN;
                        }
                    }
                case SourceMode.Current:
                    {
                        //_UserModeCommands.SMU_SetDirectCurrent(_SelectedSMU, valueThroughTheStrusture, VoltageLimit);
                        var MeasuredVoltage = 0.0;
                        switch (_SelectedSMU)
                        {
                            case SMUs.SMU1:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU1).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU2:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU2).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU3:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU3).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU4:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU4).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU5:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU5).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU6:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU6).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU7:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU7).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU8:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU8).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage / valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            default:
                                return double.NaN;
                        }
                    }
                default:
                    return double.NaN;
            }
        }

        public double MeasurePower(double valueThroughTheStrusture, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            switch (sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        //_UserModeCommands.SMU_SetDirectVoltage(_SelectedSMU, valueThroughTheStrusture, CurrentLimit);
                        var MeasuredCurrent = 0.0;
                        switch (_SelectedSMU)
                        {
                            case SMUs.SMU1:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU1).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU2:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU2).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU3:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU3).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU4:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU4).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU5:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU5).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU6:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU6).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU7:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU7).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU8:
                                {
                                    MeasuredCurrent = _UserModeCommands.TriggerCurrent(TriggerCurrent.SMU8).Data;
                                    if (MeasuredCurrent != double.NaN)
                                        return valueThroughTheStrusture * MeasuredCurrent;
                                    else
                                        return double.NaN;
                                }
                            default:
                                return double.NaN;
                        }
                    }
                case SourceMode.Current:
                    {
                        //_UserModeCommands.SMU_SetDirectCurrent(_SelectedSMU, valueThroughTheStrusture, VoltageLimit);
                        var MeasuredVoltage = 0.0;
                        switch (_SelectedSMU)
                        {
                            case SMUs.SMU1:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU1).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU2:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU2).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU3:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU3).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU4:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU4).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU5:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU5).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU6:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU6).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU7:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU7).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            case SMUs.SMU8:
                                {
                                    MeasuredVoltage = _UserModeCommands.TriggerVoltage(TriggerVoltage.SMU8).Data;
                                    if (MeasuredVoltage != double.NaN)
                                        return MeasuredVoltage * valueThroughTheStrusture;
                                    else
                                        return double.NaN;
                                }
                            default:
                                return double.NaN;
                        }
                    }
                default:
                    return double.NaN;
            }
        }

        public void SetIntegrationTime(IntegrationTime __IntegrationTime)
        {
            _CommonCommands.SetIntegrationTime(__IntegrationTime);
        }
    }
}
