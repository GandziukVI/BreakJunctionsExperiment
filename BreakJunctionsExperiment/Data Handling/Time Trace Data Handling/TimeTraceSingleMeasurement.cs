using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using BreakJunctions.Events;
using BreakJunctions.Plotting;

using Devices.SMU;
using System.Threading.Tasks;
using System.Globalization;

namespace BreakJunctions.DataHandling
{
    #region Time trace single measurement file operations implementation

    /// <summary>
    /// Represents time trace single measurement data file
    /// </summary>
    public class TimeTraceSingleMeasurement : IDisposable
    {
        #region Single measurement paremeters

        private string _FileName;
        public string FileName { get { return _FileName; } }

        private FileStream _OutputSingleMeasureStream;

        private NumberFormatInfo _NumberFormatInfo;

        private string _DataString;

        private ChannelsToInvestigate _Channel;

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// Creates new IV_SingleMeasurement instance
        /// with specified file name and source mode
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="sourceMode">Source mode</param>
        public TimeTraceSingleMeasurement(string fileName, SourceMode sourceMode, ChannelsToInvestigate Channel)
        {
            _FileName = fileName;
            _Channel = Channel;

            using (_OutputSingleMeasureStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var _Header = Encoding.ASCII.GetBytes("Distance\tR\r\n");
                var _Subheader = Encoding.ASCII.GetBytes("m\tOhm\r\n");

                _OutputSingleMeasureStream.Write(_Header, 0, _Header.Length);
                _OutputSingleMeasureStream.Write(_Subheader, 0, _Subheader.Length);
            }

            _NumberFormatInfo = NumberFormatInfo.InvariantInfo;

            _DataString = "{0}\t{1}\r\n";

            switch (_Channel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 += OnTimeTracePointReceivedChannel_01;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 += OnTimeTracePointReceivedChannel_02;
                    } break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Correctly destroying the instance
        /// </summary>
        ~TimeTraceSingleMeasurement()
        {
            this.Dispose();
        }

        #endregion

        #region Correctly disposing the instance

        /// <summary>
        /// Correctly destroy the instance
        /// </summary>
        public void Dispose()
        {
            switch (_Channel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 -= OnTimeTracePointReceivedChannel_01;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 -= OnTimeTracePointReceivedChannel_02;
                    } break;
                default:
                    break;
            }

            _FileName = string.Empty;
            _DataString = string.Empty;
        }

        #endregion

        #region Functionality implementation

        private async Task WriteData(byte[] __ToWrite)
        {
            using (_OutputSingleMeasureStream = new FileStream(_FileName, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await _OutputSingleMeasureStream.WriteAsync(__ToWrite, 0, __ToWrite.Length);
            };
        }

        private async void OnTimeTracePointReceivedChannel_01(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
        {
            try
            {
                var toWrite = Encoding.ASCII.GetBytes(String.Format(_DataString, e.X.ToString(_NumberFormatInfo), e.Y.ToString(_NumberFormatInfo)));
                await WriteData(toWrite);
            }
            catch { }
        }

        private async void OnTimeTracePointReceivedChannel_02(object sender, TimeTracePointReceivedChannel_02_EventArgs e)
        {
            try
            {
                var toWrite = Encoding.ASCII.GetBytes(String.Format(_DataString, e.X.ToString(_NumberFormatInfo), e.Y.ToString(_NumberFormatInfo)));
                await WriteData(toWrite);
            }
            catch { }
        }

        #endregion
    }

    #endregion
}
