using Devices;
using Keithley_4200.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Pages
{
    public class MeasurementSetupPage
    {
        #region MeasurementSetupPage settings

        private IExperimentalDevice _TheDevice;

        private bool _IsMeasurementSetupPageSelected = false;
        public bool IsMeasurementSetupPageSelected { get { return _IsMeasurementSetupPageSelected; } }

        #endregion

        #region Constructor

        public MeasurementSetupPage(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;

            AllEventsHandler.Instance.SystemModeChanged += On_SystemModeChanged;
        }

        #endregion

        #region Functionality

        public void SetWaitTime(double __Time)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var Time = ImportantConstants.GetProperValueForSertainRange(__Time, 0.0, 100.0);

            var command = String.Format("WT {0}", Time.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void SetInterval(double __Interval)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var Interval = ImportantConstants.GetProperValueForSertainRange(__Interval, 0.01, 10.0);

            var command = String.Format("IN {0}", Interval.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void SelctNumberOfReadings(int __NumberOfReadings)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var NumberOfReadings = Convert.ToInt32(ImportantConstants.GetProperValueForSertainRange(__NumberOfReadings, 1.0, 4096.0));

            var command = String.Format("NR {0}", NumberOfReadings);
            _TheDevice.SendCommandRequest(command);
        }

        public void SelectDisplayMode(DisplayModes __Mode)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var command = String.Format("DM{0}", (int)__Mode);
            _TheDevice.SendCommandRequest(command);
        }

        public void ListMode(params string[] __ToBeListed)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var command = "LI ";
            var MaxArgLength = Convert.ToInt32(Math.Min(__ToBeListed.Length, 6));
            for (int i = 0; i < MaxArgLength - 1; i++)
            {
                var Name = (__ToBeListed[i].Length <= 6) ? __ToBeListed[i] : __ToBeListed[i].Substring(0, 6);
                command += String.Format("\'{0}\', ", Name);
            }
            command += String.Format("\'{0}\'", __ToBeListed[MaxArgLength - 1]);

            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphX_VoltageAxisForElectricalMeasurement(string __SMU_ChannelName, X_AxisScaleType __ScaleType, double __X_AxisMinValue, double __X_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var SMU_ChannelName = (__SMU_ChannelName.Length <= 6) ? __SMU_ChannelName : __SMU_ChannelName.Substring(0, 6);

            var X_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__X_AxisMinValue, -9999.0, 9999.0);
            var X_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__X_AxisMaxValue, -9999.0, 9999.0);

            var command = String.Format("XN \'{0}\', {1}, {2}, {3}", SMU_ChannelName, (int)__ScaleType, X_AxisMinValue.ToString(DataFormatting.NumberFormat), X_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphX_CurrentAxisForElectricalMeasurement(string __SMU_ChannelName, X_AxisScaleType __ScaleType, double __X_AxisMinValue, double __X_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var SMU_ChannelName = (__SMU_ChannelName.Length <= 6) ? __SMU_ChannelName : __SMU_ChannelName.Substring(0, 6);

            var X_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__X_AxisMinValue, -999.0, 999.0);
            var X_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__X_AxisMaxValue, -999.0, 999.0);

            var command = String.Format("XN \'{0}\', {1}, {2}, {3}", SMU_ChannelName, (int)__ScaleType, X_AxisMinValue.ToString(DataFormatting.NumberFormat), X_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphX_AxisForTimeDomain(double __X_AxisMinValue, double __X_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var X_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__X_AxisMinValue, 0.01, 9999.0);
            var X_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__X_AxisMaxValue, 0.01, 9999.0);

            var command = String.Format("XT {0}, {1}", X_AxisMinValue.ToString(DataFormatting.NumberFormat), X_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphYA_VoltageAxisForElectricalMeasurement(string __SMU_ChannelName, Y_AxisScaleType __ScaleType, double __Y_AxisMinValue, double __Y_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var SMU_ChannelName = (__SMU_ChannelName.Length <= 6) ? __SMU_ChannelName : __SMU_ChannelName.Substring(0, 6);

            var Y_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMinValue, -9999.0, 9999.0);
            var Y_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMaxValue, -9999.0, 9999.0);

            var command = String.Format("YA \'{0}\', {1}, {2}, {3}", SMU_ChannelName, (int)__ScaleType, Y_AxisMinValue.ToString(DataFormatting.NumberFormat), Y_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphYA_CurrentAxisForElectricalMeasurement(string __SMU_ChannelName, Y_AxisScaleType __ScaleType, double __Y_AxisMinValue, double __Y_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var SMU_ChannelName = (__SMU_ChannelName.Length <= 6) ? __SMU_ChannelName : __SMU_ChannelName.Substring(0, 6);

            var Y_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMinValue, -999.0, 999.0);
            var Y_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMaxValue, -999.0, 999.0);

            var command = String.Format("YA \'{0}\', {1}, {2}, {3}", SMU_ChannelName, (int)__ScaleType, Y_AxisMinValue.ToString(DataFormatting.NumberFormat), Y_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphYB_VoltageAxisForElectricalMeasurement(string __SMU_ChannelName, Y_AxisScaleType __ScaleType, double __Y_AxisMinValue, double __Y_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var SMU_ChannelName = (__SMU_ChannelName.Length <= 6) ? __SMU_ChannelName : __SMU_ChannelName.Substring(0, 6);

            var Y_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMinValue, -9999.0, 9999.0);
            var Y_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMaxValue, -9999.0, 9999.0);

            var command = String.Format("YB \'{0}\', {1}, {2}, {3}", SMU_ChannelName, (int)__ScaleType, Y_AxisMinValue.ToString(DataFormatting.NumberFormat), Y_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        public void ConfigureGraphYB_CurrentAxisForElectricalMeasurement(string __SMU_ChannelName, Y_AxisScaleType __ScaleType, double __Y_AxisMinValue, double __Y_AxisMaxValue)
        {
            if (!IsMeasurementSetupPageSelected)
                AllEventsHandler.Instance.On_SystemModeChanged(this, new SystemModeChanged_EventArgs(SystemModeCommands.MeasurementSetup));

            var SMU_ChannelName = (__SMU_ChannelName.Length <= 6) ? __SMU_ChannelName : __SMU_ChannelName.Substring(0, 6);

            var Y_AxisMinValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMinValue, -999.0, 999.0);
            var Y_AxisMaxValue = ImportantConstants.GetProperValueForSertainRange(__Y_AxisMaxValue, -999.0, 999.0);

            var command = String.Format("YB \'{0}\', {1}, {2}, {3}", SMU_ChannelName, (int)__ScaleType, Y_AxisMinValue.ToString(DataFormatting.NumberFormat), Y_AxisMaxValue.ToString(DataFormatting.NumberFormat));
            _TheDevice.SendCommandRequest(command);
        }

        #endregion

        #region SystemModeChanged

        private void On_SystemModeChanged(object sender, SystemModeChanged_EventArgs e)
        {
            if (e.SelectedMode == SystemModeCommands.MeasurementSetup)
            {
                _IsMeasurementSetupPageSelected = true;
                _TheDevice.SendCommandRequest("SM");
            }
            else
                _IsMeasurementSetupPageSelected = false;
        }

        #endregion
    }
}
