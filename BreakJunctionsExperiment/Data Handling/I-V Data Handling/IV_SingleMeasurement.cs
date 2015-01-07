using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using BreakJunctions.Events;
using BreakJunctions.Plotting;
using System.Globalization;
using System.Threading.Tasks;

namespace BreakJunctions.DataHandling
{
    #region I-V single measurement file operations implementation

    /// <summary>
    /// Represents I-V single measurement data file
    /// </summary>
    class IV_SingleMeasurement : IDisposable
    {

        #region Single measurement file parameters

        private string _FileName;
        /// <summary>
        /// Gets specified data file name
        /// </summary>
        public string FileName { get { return _FileName; } }

        private FileStream _OutputSingleMeasureStreamWriter;

        private string _DataString;

        private ChannelsToInvestigate _Channel;

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// Creates new IV_SingleMeasurement instance
        /// with specified file name
        /// </summary>
        /// <param name="fileName">File name</param>
        public IV_SingleMeasurement(string fileName, ChannelsToInvestigate Channel)
        {
            this._FileName = fileName;

            using (_OutputSingleMeasureStreamWriter = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                var FirstRow = Encoding.ASCII.GetBytes("U\tI\r\n");
                var SecondRow = Encoding.ASCII.GetBytes("V\tA\r\n");

                _OutputSingleMeasureStreamWriter.Write(FirstRow, 0, FirstRow.Length);
                _OutputSingleMeasureStreamWriter.Write(SecondRow, 0, SecondRow.Length);
            }

            _DataString = "{0}\t{1}\r\n";

            _Channel = Channel;

            switch (_Channel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_01 += OnIV_PointReceivedChannel_01;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_02 += OnIV_PointReceivedChannel_02;
                    } break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Correctly destroying the instance
        /// </summary>
        ~IV_SingleMeasurement()
        {
            this.Dispose();
        }

        #endregion

        #region Destroying instance

        /// <summary>
        /// Correctly destroy the instance
        /// </summary>
        public void Dispose()
        {
            switch (_Channel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_01 -= OnIV_PointReceivedChannel_01;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_02 -= OnIV_PointReceivedChannel_02;
                    } break;
                default:
                    break;
            }

            _FileName = string.Empty;
            _DataString = string.Empty;
        }

        #endregion

        #region Functionality implementation

        private async Task WriteChannelData(byte[] __ToWrite)
        {
            using (_OutputSingleMeasureStreamWriter = new FileStream(_FileName, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                await _OutputSingleMeasureStreamWriter.WriteAsync(__ToWrite, 0, __ToWrite.Length);
            };
        }

        /// <summary>
        /// Writes the data point, which arrives from event to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnIV_PointReceivedChannel_01(object sender, IV_PointReceivedChannel_01_EventArgs e)
        {
            try
            {
                var toWrite = Encoding.ASCII.GetBytes(String.Format(_DataString, e.X.ToString(NumberFormatInfo.InvariantInfo), e.Y.ToString(NumberFormatInfo.InvariantInfo)));
                await WriteChannelData(toWrite);
            }
            catch { }
        }

        private async void OnIV_PointReceivedChannel_02(object sender, IV_PointReceivedChannel_02_EventArgs e)
        {
            try
            {
                var toWrite = Encoding.ASCII.GetBytes(String.Format(_DataString, e.X.ToString(NumberFormatInfo.InvariantInfo), e.Y.ToString(NumberFormatInfo.InvariantInfo)));
                await WriteChannelData(toWrite);
            }
            catch { }
        }

        #endregion
    }

    #endregion
}
