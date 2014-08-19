using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Hardware;
using BreakJunctions.Events;
using BreakJunctions.Plotting;

namespace BreakJunctions.DataHandling
{
    #region Time trace single measurement file operations implementation

    /// <summary>
    /// Represents time trace single measurement data file
    /// </summary>
    class TimeTraceSingleMeasurement : IDisposable
    {
        #region Single measurement paremeters

        private string _FileName;
        public string FileName { get { return _FileName; } }

        private FileStream _OutputSingleMeasureStream;
        private StreamWriter _OutputSingleMeasureStreamWriter;

        private StringBuilder _DataBuilder;
        private string _DataString;

        private string _Header;
        private string _Subheader;

        private Channels _Channel;

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// Creates new IV_SingleMeasurement instance
        /// with specified file name and source mode
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="sourceMode">Source mode</param>
        public TimeTraceSingleMeasurement(string fileName, SourceMode sourceMode, Channels Channel)
        { 
            this._FileName = fileName;
            this._Channel = Channel;

            _OutputSingleMeasureStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            _OutputSingleMeasureStreamWriter = new StreamWriter(_OutputSingleMeasureStream);

            switch (sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        _Header = "Distance\tR";
                        _Subheader = "m\tOhm";
                    } break;
                case SourceMode.Current:
                    {
                        _Header = "Distance\tR";
                        _Subheader = "m\tOhm";
                    } break;
                default:
                    break;
            }

            _OutputSingleMeasureStreamWriter.WriteLine(_Header);
            _OutputSingleMeasureStreamWriter.WriteLine(_Subheader);

            _OutputSingleMeasureStreamWriter.Close();
            _OutputSingleMeasureStream.Close();

            _DataBuilder = new StringBuilder();

            _DataString = "{0}\t{1}";
            
            switch (_Channel)
            {
                case Channels.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 += OnTimeTracePointReceivedChannel_01;
                    } break;
                case Channels.Channel_02:
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

        #region Destroying instance

        /// <summary>
        /// Correctly destroy the instance
        /// </summary>
        public void Dispose()
        {
            switch (_Channel)
            {
                case Channels.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 -= OnTimeTracePointReceivedChannel_01;
                    } break;
                case Channels.Channel_02:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 -= OnTimeTracePointReceivedChannel_02;
                    } break;
                default:
                    break;
            }

            _FileName = string.Empty;
            _DataString = string.Empty;
            _DataBuilder = null;
        }

        #endregion

        #region Functionality implementation

        private void OnTimeTracePointReceivedChannel_01(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
        {
            _OutputSingleMeasureStream = new FileStream(_FileName, FileMode.Append, FileAccess.Write);
            _OutputSingleMeasureStreamWriter = new StreamWriter(_OutputSingleMeasureStream);

            _DataBuilder = new StringBuilder();
            _DataBuilder.AppendFormat(_DataString, e.X, e.Y);

            _OutputSingleMeasureStreamWriter.WriteLine(_DataBuilder.ToString());

            _OutputSingleMeasureStreamWriter.Close();
            _OutputSingleMeasureStream.Close();
        }

        private void OnTimeTracePointReceivedChannel_02(object sender, TimeTracePointReceivedChannel_02_EventArgs e)
        {
            _OutputSingleMeasureStream = new FileStream(_FileName, FileMode.Append, FileAccess.Write);
            _OutputSingleMeasureStreamWriter = new StreamWriter(_OutputSingleMeasureStream);

            _DataBuilder = new StringBuilder();
            _DataBuilder.AppendFormat(_DataString, e.X, e.Y);

            _OutputSingleMeasureStreamWriter.WriteLine(_DataBuilder.ToString());

            _OutputSingleMeasureStreamWriter.Close();
            _OutputSingleMeasureStream.Close();
        }

        #endregion
    }

    #endregion
}
