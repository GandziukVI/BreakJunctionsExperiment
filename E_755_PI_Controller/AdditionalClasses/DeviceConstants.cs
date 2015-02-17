using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_755_PI_Controller
{
    public enum GainIDs
    {
        ChannelID_01 = 0x07001500,
        ChannelID_02 = 0x07001502,
        ChannelID_03 = 0x07001600,
        ChannelID_04 = 0x07001602
    }

    public enum RangeLimits
    {
        RangeMinLimit = 0x07000000,
        RangeMaxLimit = 0x07000001
    }

    public enum AxisIdentifier
    {
        _1 = 1
    }

    public enum APG_State
    {
        NotSuccessfullyFinished = 0,
        SuccessfullyFinished = 1
    }

    public enum BaudRates
    {
        _110 = 110,
        _300 = 300,
        _600 = 600,
        _1200 = 1200,
        _2400 = 2400,
        _4800 = 4800,
        _9600 = 9600,
        _14400 = 14400,
        _19200 = 19200,
        _38400 = 38400,
        _57600 = 57600,
        _115200 = 115200
    }

    public enum ProtocolType
    {
        ReplyCommandProtocol = 1
    }

    public enum ProtocolOptions
    {
        NoTargetID_And_NoSenderID = 0,
        WithTargetID_Only = 1,
        WithSenderID_Only = 2,
        WithBothTargetID_And_SenderID = 3
    }

    public enum CommandLevels
    {
        /// <summary>
        /// All commands provided for "normal" users are available,
        /// read access to all parameters, no Password required.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Provides additional commands and write access to level- 1-parameters
        /// (commands and parameters from level 0 are included). The required Password is "advanced".
        /// </summary>
        Advanced = 1,
        /// <summary>
        /// Is provided for PI service personnel only. Users can not change to level 2.
        /// Contact your Physik Instrumente sales engineer or write info@pi.ws if there seem to be problems with level-2 parameters.
        /// </summary>
        Highest = 2
    }
}
