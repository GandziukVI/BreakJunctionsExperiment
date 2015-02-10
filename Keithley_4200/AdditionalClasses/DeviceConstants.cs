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

    public enum SourceMode
    {
        Voltage,
        Current
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
    }
}
