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
    class Keithley_4200_SMU : I_SMU
    {
        #region Keithley_4200_SMU settings

        private double _VoltageLimit;
        public double VoltageLimit
        {
            get { return _VoltageLimit; }
            set
            {
                _VoltageLimit = value;
                SetVoltageLimit(value);
            }
        }

        private IExperimentalDevice _TheDevice;
        private ChannelDefinitionPage _ChannelDefinition;
        private UserModeCommands _UserModeCommands;

        private SMUs _SelectedSMU;

        #endregion

        #region Constructor

        public Keithley_4200_SMU(ref IExperimentalDevice __TheDevice, SMUs __SelectedSMU)
        {
            _TheDevice = __TheDevice;
            _UserModeCommands = new UserModeCommands(ref _TheDevice);
        }

        #endregion

        public bool InitDevice()
        {
            var InitSuccess = _TheDevice.InitDevice();
            AllEventsHandler.Instance.On_SystemModeChanged(new object(), new SystemModeChanged_EventArgs(SystemModeCommands.UserMode));
            return InitSuccess;
        }

        public void SwitchON()
        {
            
        }

        public void SwitchOFF()
        {
            throw new NotImplementedException();
        }

        public bool SetVoltageLimit(double Value)
        {
            throw new NotImplementedException();
        }

        public bool SetCurrentLimit(double Value)
        {
            throw new NotImplementedException();
        }

        public bool SetSourceVoltage(double Value)
        {
            throw new NotImplementedException();
        }

        public bool SetSourceCurrent(double Value)
        {
            throw new NotImplementedException();
        }

        public double MeasureVoltage(int NumberOfAverages, double TimeDelay)
        {
            throw new NotImplementedException();
        }

        public double MeasureCurrent(int NumberOfAverages, double TimeDelay)
        {
            throw new NotImplementedException();
        }

        public double MeasureResistance(double valueThroughTheStrusture, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            throw new NotImplementedException();
        }

        public double MeasurePower(double valueThroughTheStrusture, int NumberOfAverages, double TimeDelay, SourceMode sourceMode)
        {
            throw new NotImplementedException();
        }
    }
}
