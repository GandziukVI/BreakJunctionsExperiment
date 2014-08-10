using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using BreakJunctions.Events;

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

        private FileStream _OutputSingleMeasureStream;
        private StreamWriter _OutputSingleMeasureStreamWriter;

        private StringBuilder _DataBuilder;
        private string _DataString;

        #endregion

        #region Constructor / Destructor

        /// <summary>
        /// Creates new IV_SingleMeasurement instance
        /// with specified file name
        /// </summary>
        /// <param name="fileName">File name</param>
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
            AllEventsHandler.Instance.IV_PointReceived -= OnIV_PointReceived;

            _FileName = string.Empty;
            _DataString = string.Empty;
            _DataBuilder = null;
        }

        #endregion

        #region Functionality implementation

        /// <summary>
        /// Writes the data point, which arrives from event to file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        #endregion
    }

    #endregion
}
