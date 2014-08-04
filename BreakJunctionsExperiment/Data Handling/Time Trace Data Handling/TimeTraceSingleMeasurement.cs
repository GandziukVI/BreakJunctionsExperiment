using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Hardware;
using BreakJunctions.Events;

namespace BreakJunctions.DataHandling
{
    class TimeTraceSingleMeasurement : IDisposable
    {
        private string _FileName;
        public string FileName { get { return _FileName; } }

        private FileStream _OutputSingleMeasureStream;
        private StreamWriter _OutputSingleMeasureStreamWriter;

        private StringBuilder _DataBuilder;
        private string _DataString;

        private string _Header;
        private string _Subheader;

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

            AllEventsHandler.Instance.TimetracePointReceived += OnTimeTracePointReceived;
        }

        ~TimeTraceSingleMeasurement()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            AllEventsHandler.Instance.TimetracePointReceived -= OnTimeTracePointReceived;

            _FileName = string.Empty;
            _DataString = string.Empty;
            _DataBuilder = null;
        }

        private void OnTimeTracePointReceived(object sender, TimeTracePointReceived_EventArgs e)
        {
            _OutputSingleMeasureStream = new FileStream(_FileName, FileMode.Append, FileAccess.Write);
            _OutputSingleMeasureStreamWriter = new StreamWriter(_OutputSingleMeasureStream);

            _DataBuilder = new StringBuilder();
            _DataBuilder.AppendFormat(_DataString, e.X, e.Y);

            _OutputSingleMeasureStreamWriter.WriteLine(_DataBuilder.ToString());

            _OutputSingleMeasureStreamWriter.Close();
            _OutputSingleMeasureStream.Close();
        }
    }
}
