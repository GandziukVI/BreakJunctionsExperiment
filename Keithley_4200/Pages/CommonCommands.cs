using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Pages
{
    public class CommonCommands
    {
        #region CommonCommands settings

        private IExperimentalDevice _TheDevice;
        private IntegrationTime _currentIntegrationTime;

        #endregion

        #region Constructor

        public CommonCommands(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
        }

        #endregion

        #region Functionality

        public void SetIntegrationTime(IntegrationTime __IntegrationTime)
        {
            if (__IntegrationTime != _currentIntegrationTime)
            {
                var command = String.Format("IT{0}", (int)__IntegrationTime);
                _TheDevice.SendCommandRequest(command);
            }
            _currentIntegrationTime = __IntegrationTime;
        }

        public void SetIntegrationTime(double __DelayFactor, double __FilterFactor, double __AD_CIT)
        {
            var delay = (0.0 <= __DelayFactor && __DelayFactor <= 10.0) ? __DelayFactor : 1.0;
            var filter = (0.0 <= __FilterFactor && __FilterFactor <= 10.0) ? __FilterFactor : 1.0;
            var AD_CIT = (0.01 <= __AD_CIT && __AD_CIT <= 10.0) ? __AD_CIT : 1.0;

            var command = String.Format("IT4,{0},{1},{2}", delay.ToString(DataFormatting.NumberFormat), filter.ToString(DataFormatting.NumberFormat), AD_CIT.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public string RetrieveInstruumentID()
        {
            return _TheDevice.RequestQuery("ID");
        }

        public void ControlDataReadyRequest(DataReady __DataReadyControl)
        {
            var command = String.Format("DR{0}", (int)__DataReadyControl);
            _TheDevice.SendCommandRequest(command);
        }

        public void ClearDataBuffer()
        {
            _TheDevice.SendCommandRequest("BC");
        }

        public void SetGlobalMeasurementResolution(uint __NumberOfDigits)
        {
            uint NumDigits;

            if (__NumberOfDigits < 3)
                NumDigits = 3;
            else if (__NumberOfDigits > 7)
                NumDigits = 7;
            else
                NumDigits = __NumberOfDigits;

            var command = String.Format("RS {0}", NumDigits);

            _TheDevice.SendCommandRequest(command);
        }

        public void SetSMU_CurrentSpecifiedRange(SMUs __SelectedSMU, double __Range, double __Compilance)
        {
            double compilance = 0.0;
            if (__Compilance < 0.1 * __Range)
                compilance = 0.1 * __Range;
            else if (__Compilance > __Range)
                compilance = __Range;
            else
                compilance = __Compilance;

            if ((Array.IndexOf(ImportantConstants._VoltageSourceRanges, __Range) > 0) || (Array.IndexOf(ImportantConstants._CurrentSourceRanges, __Range) > 0))
                _TheDevice.SendCommandRequest(String.Format("RI {0},{1},{2}", (int)__SelectedSMU, __Range.ToString(DataFormatting.NumberFormat), compilance.ToString(DataFormatting.NumberFormat)));
        }

        public void SetLowestCurrentMeasurementRange(SMUs __SelectedSMU, double __Range)
        {
            if (Array.IndexOf(ImportantConstants._CurrentSourceRanges, __Range) > 0)
                _TheDevice.SendCommandRequest(String.Format("RG {0},{1}", (int)__SelectedSMU, __Range.ToString(DataFormatting.NumberFormat)));
        }

        public void PerformAutocalibration(SMUs __SelectedSMU)
        {
            var command = String.Format("AC {0}", (int)__SelectedSMU);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetExitOnCompilance(ExitOnCompilance __ExitOnCompilanceMode)
        {
            var command = String.Format("EC {0}", (int)__ExitOnCompilanceMode);
            _TheDevice.SendCommandRequest(command);
        }

        public void SwitchBetween_4145_and_4200_Modes(DeviceMode __DeviceMode, SessionMode __SessionMode)
        {
            var command = String.Format("EM {0},{1}", (int)__DeviceMode, (int)__SessionMode);
            _TheDevice.SendCommandRequest(command);
        }

        #endregion
    }
}
