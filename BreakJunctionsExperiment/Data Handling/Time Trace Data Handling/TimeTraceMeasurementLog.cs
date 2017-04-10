using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BreakJunctions.DataHandling
{
    #region Time trace measurement log file writting implementation

    /// <summary>
    /// Represents time trace measurement log file
    /// </summary>
    class TimeTraceMeasurementLog
    {
        #region Measurement log file implementation

        private string _LogFileName;

        private FileStream _OutputLogStream;
        private StreamWriter _OutputLogStreamWriter;

        private StringBuilder _LogBuilder;
        private string _SingleLog;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates new TimeTraceMeasurementLog instance
        /// with specified file name
        /// </summary>
        /// <param name="logFileName">File name</param>
        public TimeTraceMeasurementLog(string logFileName)
        {
            _LogFileName = logFileName;
            _SingleLog = "{0}\t{1}\t{2}\t{3}";
            
            if (!File.Exists(logFileName))
            {
                File.WriteAllText(logFileName, "File name\tSource mode\tValue through the structure\tMolecule type\tComment\n");
            }
        }

        #endregion

        #region Functionality implmentation

        public void AddNewTimeTraceMeasurementLog(string fileName, string sourceMode, double valueThroughTheStructure, string comment)
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
            _LogBuilder.AppendFormat(_SingleLog, fileName, sourceMode, valueThroughTheStructure, comment);

            _OutputLogStreamWriter.WriteLine(_LogBuilder.ToString());

            _OutputLogStreamWriter.Close();
        }

        #endregion
    }

    #endregion
}
