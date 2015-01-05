﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.SMU
{
    public enum Channels { ChannelA, ChannelB, ChannelC, ChannelD }
    public enum Channel_Status { Channel_ON, Channel_OFF }
    public enum Sense { SENSE_LOCAL, SENSE_REMOTE }
    public enum SourceMode { Voltage, Current }
    public enum MeasureMode { Voltage, Current, Resistance, Power }
    public enum LimitMode { Voltage, Current }
}