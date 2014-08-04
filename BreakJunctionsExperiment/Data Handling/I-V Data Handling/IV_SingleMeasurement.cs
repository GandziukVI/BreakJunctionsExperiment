using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using BreakJunctions.Events;

namespace BreakJunctions.DataHandling
{
    class IV_SingleMeasurement : IDisposable
    {
        private string _FileName;
        public string FileName { get { return _FileName; } }

        private FileStream _OutputSingleMeasureStream;
        private StreamWriter _OutputSingleMeasureStreamWriter;

        private StringBuilder _DataBuilder;
        private string _DataString;

        public IV_SingleMeasurement(string fileName)
        { 
            this._FileName = fileName;

            _OutputSingleMeasureStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            _OutputSingleMeasureStreamWriter = new StreamWriter(_OutputSingleMeasureStream);

            _OutputSingleMeasureStreamWriter.WriteLine("U\tI");
            _OutputSingleMeasureStreamWriter.WriteLine("V\tA");

            _OutputSingleMeasureStreamWriter.Close();
            _OutputSingleMeasureStream.Close();

            _DataBuilder = new StringBuilder();

            _DataString = "{0}\t{1}";

            AllEventsHandler.Instance.IV_PointReceived += OnIV_PointReceived;
        }

        ~IV_SingleMeasurement()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            AllEventsHandler.Instance.IV_PointReceived -= OnIV_PointReceived;

            _FileName = string.Empty;
            _DataString = string.Empty;
            _DataBuilder = null;
        }

        private void OnIV_PointReceived(object sender, IV_PointReceived_EventArgs e)
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
