using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BreakJunctions.DataHandling
{
    #region I-V measurement log file writting implementation

    /// <summary>
    /// Represents I-V measurement log file
    /// </summary>
    class IV_MeasurementLog
    {
        #region Measurement log file parameters

        private string _LogFileName;

        private FileStream _OutputLogStream;
        private StreamWriter _OutputLogStreamWriter;

        private StringBuilder _LogBuilder;
        private string _SingleLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates new IV_MeasurementLog instance with
        /// specified FileName
        /// </summary>
        /// <param name="logFileName">FileName</param>
        public IV_MeasurementLog(string logFileName)
        {
            _LogFileName = logFileName;
            _SingleLog = "{0}\t{1}\t{2}\t{3}";

            if (!File.Exists(logFileName))
            {
                File.WriteAllText(logFileName, "File name\tSource mode\tMicrometric bolt position\tComment\n");
            }
        }

        #endregion

        #region Functionality implementation

        /// <summary>
        /// Creates new file with time trace data
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="sourceMode">Source mode</param>
        /// <param name="micrometricBoltPosition">Micrometric bolt position</param>
        /// <param name="comment">Comment</param>
        public void AddNewIV_MeasurementLog(string fileName, string sourceMode, double micrometricBoltPosition, string comment)
        {
            if (File.Exists(_LogFileName))
            {
                _OutputLogStream = new FileStream(_LogFileName, FileMode.Append, FileAccess.Write);
                _OutputLogStreamWriter = new StreamWriter(_OutputLogStream);
            }
            else
            {
                _OutputLogStream = new FileStream(_LogFileName, FileMode.Create, FileAccess.Write);
                _OutputLogStreamWriter = new StreamWriter(_OutputLogStream);
            }

            _LogBuilder = new StringBuilder();
            _LogBuilder.AppendFormat(_SingleLog, fileName, sourceMode, micrometricBoltPosition, comment);

            var a = _LogBuilder.ToString();

            _OutputLogStreamWriter.WriteLine(_LogBuilder.ToString());

            _OutputLogStreamWriter.Close();
        }

        #endregion
    }

    #endregion
}
