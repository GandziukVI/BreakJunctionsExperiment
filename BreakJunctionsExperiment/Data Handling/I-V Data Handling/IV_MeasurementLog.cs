using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BreakJunctions.DataHandling
{
    class IV_MeasurementLog
    {
        private string _LogFileName;

        private FileStream _OutputLogStream;
        private StreamWriter _OutputLogStreamWriter;

        private StringBuilder _LogBuilder;
        private string _SingleLog;

        public IV_MeasurementLog(string logFileName)
        {
            _LogFileName = logFileName;
            _SingleLog = "{0}\t{1}\t{2}\t{3}";

            if (!File.Exists(logFileName))
            {
                File.WriteAllText(logFileName, "File name\tSource mode\tMicrometric bolt position\tComment\n");
            }
        }

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
    }
}
