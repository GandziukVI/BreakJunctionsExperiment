using System;
using System.Collections.Generic;
using System.Text;

namespace Devices.SMU
{
    public enum Channels { ChannelA, ChannelB, ChannelC, ChannelD, ChannelE, ChannelF, ChannelG, ChannelH }
    public enum Channel_Status { Channel_ON, Channel_OFF }
    public enum Sense { SENSE_LOCAL, SENSE_REMOTE }
    public enum SourceMode { Voltage = 1, Current = 3, Common = 3 }
    public enum MeasureMode { Voltage, Current, Resistance, Conductance, Power }
    public enum LimitMode { Voltage, Current }
}
