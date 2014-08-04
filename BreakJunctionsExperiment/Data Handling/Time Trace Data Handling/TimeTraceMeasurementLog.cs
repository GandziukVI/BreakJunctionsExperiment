using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BreakJunctions.DataHandling
{
    class TimeTraceMeasurementLog
    {
        private string _LogFileName;

        private FileStream _OutputLogStream;
        private StreamWriter _OutputLogStreamWriter;

        private StringBuilder _LogBuilder;
        private string _SingleLog;

        public TimeTraceMeasurementLog(string logFileName)
        {
            _LogFileName = logFileName;
            _SingleLog = "{0}\t{1}\t{2}\t{3}";
            
            if (!File.Exists(logFileName))
            {
                File.WriteAllText(logFileName, "File name\tSource mode\tValue through the structure\tMolecule type\tComment\n");
            }
        }

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
    }
}
