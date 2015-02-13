using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200
{
    public enum SystemModeCommands
    {
        ChannelDefinition,
        SourceSetup,
        MeasurementSetup,
        MeasurementControl,
        UserMode
    }

    public enum VoltageSourceRanges
    {
        Autorange = 0,
        _20V = 1,
        _200V = 2,
        _200V_Enhanced = 3
    }

    public enum CurrentSourceRanges
    {
        Autorange = 0,
        _1_nA = 1,
        _10_nA = 2,
        _100_nA = 3,
        _1_uA = 4,
        _10_uA = 5,
        _100_uA = 6,
        _1_mA = 7,
        _10_mA = 8,
        _100_mA = 9,
        _1_A = 10,
        _1_pA = 11,
        _10_pA = 11,
        _100_pA = 12
    }

    public enum SMUs
    {
        SMU1 = 1,
        SMU2 = 2,
        SMU3 = 3,
        SMU4 = 4,
        SMU5 = 5,
        SMU6 = 6,
        SMU7 = 7,
        SMU8 = 8
    }

    public enum VoltageSources
    {
        VS1 = 1,
        VS2 = 2,
        VS3 = 3,
        VS4 = 4,
        VS5 = 5,
        VS6 = 6,
        VS7 = 7,
        VS8 = 8
    }

    public enum TriggerVoltage
    {
        SMU1 = 1,
        SMU2 = 2,
        SMU3 = 3,
        SMU4 = 4,
        VM1 = 5,
        VM2 = 6,
        SMU5 = 7,
        SMU6 = 8,
        SMU7 = 9,
        SMU8 = 10,
        VM3 = 11,
        VM4 = 12,
        VM5 = 13,
        VM6 = 14,
        VM7 = 15,
        VM8 = 16
    }

    public enum TriggerCurrent
    {
        SMU1 = 1,
        SMU2 = 2,
        SMU3 = 3,
        SMU4 = 4,
        SMU5 = 5,
        SMU6 = 6,
        SMU7 = 7,
        SMU8 = 8
    }

    public enum IntegrationTime
    {
        Short = 1,
        Medium = 2,
        Long = 3
    }

    public enum DataReady
    {
        DisableServiceRequestForDataReady = 0,
        EnableServiceRequestForDataReady = 1
    }

    public enum ExitOnCompilance
    {
        OFF = 0,
        ON = 1
    }

    public enum DeviceMode
    {
        _4145_Mode = 0,
        _4200_Mode = 1
    }

    public enum SessionMode
    {
        ThisSessionOnly = 0,
        ThisAndSubsequentSessions = 1
    }

    public enum SourceFunction
    {
        VAR1 = 1,
        VAR2 = 2,
        Constant = 3,
        VAR1_Extended = 4
    }

    public enum SweepMode
    {
        LinearSweep = 1,
        Log10_Sweep = 2,
        Log25_Sweep = 3,
        Log50_Sweep = 4
    }

    public enum MasterOrSlaveMode
    {
        SlaveMode = 0,
        MasterMode = 1
    }

    public enum Ranges
    {
        VoltageRange,
        CurrentRange
    }

    public struct ReturnData
    {
        /// <summary>
        /// The status of the data
        /// </summary>
        public string X;
        /// <summary>
        /// The measure channel
        /// </summary>
        public string Y;
        /// <summary>
        /// The measure mode
        /// </summary>
        public string Z;
        /// <summary>
        /// Thr data reading
        /// </summary>
        public double Data;

        public ReturnData(string __X, string __Y, string __Z, double __Data)
        {
            X = __X;
            Y = __Y;
            Z = __Z;
            Data = __Data;
        }

        public ReturnData(string DeviceResponce)
        {
            var index = DeviceResponce.LastIndexOf(' ');
            var data = DeviceResponce.Remove(index).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            X = data[0];
            Y = data[1];
            Z = data[2];

            var _Data = 0.0;
            var IsReadingSuccess = double.TryParse(data[3], DataFormatting.NumberStyle, DataFormatting.NumberFormat, out _Data);

            if (IsReadingSuccess)
                Data = _Data;
            else
                Data = double.NaN;
        }
    }

    public static class ImportantConstants
    {
        public static double[] _VoltageSourceRanges = { 20.0, 200.0, 200.0 };
        public static double[] _CurrentSourceRanges = { 0.000000001, 0.00000001, 0.0000001, 0.000001, 0.00001, 0.0001, 0.001, 0.01, 0.1, 1.0, 0.000000000001, 0.000000000001, 0.0000000001 };

        public static VoltageSourceRanges GetProperVoltageRange(double __Value)
        {
            var ProperRangeValue = _VoltageSourceRanges.Select(p => new { Value = p, Difference = p - Math.Abs(__Value) })
                .Where(p => p.Difference >= 0)
                .OrderBy(p => p.Difference)
                .First().Value;

            return (VoltageSourceRanges)(Array.IndexOf(_VoltageSourceRanges, ProperRangeValue) + 1);
        }

        public static CurrentSourceRanges GetProperCurrentRange(double __Value)
        {
            var ProperRangeValue = _CurrentSourceRanges.Select(p => new { Value = p, Difference = p - Math.Abs(__Value) })
                .Where(p => p.Difference >= 0)
                .OrderBy(p => p.Difference)
                .First().Value;

            return (CurrentSourceRanges)(Array.IndexOf(_VoltageSourceRanges, ProperRangeValue) + 1);
        }

        public static double GetProperValueForSertainRange(double __Value, double __Min, double __Max)
        {
            var ProperValue = 0.0;

            if (__Value < __Min)
                ProperValue = __Min;
            else if (__Value > __Max)
                ProperValue = __Max;
            else
                ProperValue = __Value;

            return ProperValue;
        }

        public static double GetProperValueForSertainRange(double __Value, Ranges __CompilanceType)
        {
            var __Min = (__CompilanceType == Ranges.VoltageRange) ? -210.0 : -0.105;
            var __Max = (__CompilanceType == Ranges.VoltageRange) ? 210.0 : 0.105;

            var ProperValue = 0.0;

            if (__Value < __Min)
                ProperValue = __Min;
            else if (__Value > __Max)
                ProperValue = __Max;
            else
                ProperValue = __Value;

            return ProperValue;
        }
    }
}
