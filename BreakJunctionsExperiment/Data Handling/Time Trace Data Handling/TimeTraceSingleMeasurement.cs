using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Hardware;
using BreakJunctions.Events;

namespace BreakJunctions.DataHandling
{
    #region Time trace single measurement file operations implementation

    /// <summary>
    /// Represents time trace single measurement data file
    /// </summary>
    class TimeTraceSingleMeasurement : IDisposable
    {
        #region Single measurement file paremeters

        private string _FileName;
        public string FileName { get { return _FileName; } }

        private FileStream _OutputSingleMeasureStream;
        private StreamWriter _OutputSingleMeasureStreamWriter;

        private StringBuilder _DataBuilder;
        private string _DataString;

        private string _Header;
        private string _Subheader;

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// Creates new IV_SingleMeasurement instance
        /// with specified file name and source mode
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="sourceMode">Source mode</param>
        public TimeTraceSingleMeasurement(string fileName, SourceMode sourceMode)
        { 
            this._FileName = fileName;

            _OutputSingleMeasureStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            _OutputSingleMeasureStreamWriter = new StreamWriter(_OutputSingleMeasureStream);

            switch (sourceMode)
            {
                case SourceMode.Voltage:
                    {
                        _Header = "Distance\tI";
                        _Subheader = "m\tA";
                    } break;
                case SourceMode.Current:
                    {
                        _Header = "Distance\tU";
                        _Subheader = "m\tV";
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

            AllEventsHandler.Instance.TimetracePointReceivedChannel_01 += OnTimeTracePointReceived;
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
            AllEventsHandler.Instance.TimetracePointReceivedChannel_01 -= OnTimeTracePointReceived;

            _FileName = string.Empty;
            _DataString = string.Empty;
            _DataBuilder = null;
        }

        #endregion

        #region Functionality implementation

        private void OnTimeTracePointReceived(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
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
