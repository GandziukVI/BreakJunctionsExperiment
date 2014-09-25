using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aids.Graphics;
using BreakJunctions.Events;

namespace BreakJunctions.DataHandling
{
    #region Real time time trace single measurement file operations implementation

    /// <summary>
    /// Represents real time time trace single measurement data file
    /// </summary>
    public class RealTime_TimeTraceSingleMeasurement : IDisposable
    {

        #region RealTime_TimeTraceSingleMeasurement settings

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private ASCIIEncoding _asciiEncoding;

        #endregion

        #region Constructor / Destructor

        public RealTime_TimeTraceSingleMeasurement(string filename, double appliedVoltage, string sampleNumber)
        {
            _asciiEncoding = new ASCIIEncoding();
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived;
        }

        ~RealTime_TimeTraceSingleMeasurement()
        {
            this.Dispose();
        }

        #endregion

        #region RealTime_TimeTraceSingleMeasurement functionality

        /// <summary>
        /// Converts the List of PointD to the byte array
        /// </summary>
        /// <param name="Data">List of data points</param>
        /// <returns>Appropriate byte array</returns>
        private byte[] _GetDataBytes(List<PointD>[] Data)
        {
            string result = string.Empty;

            double PointsNumber = (new double[] { Data[0].Count, Data[1].Count, Data[2].Count, Data[3].Count }).Min();

            for (int i = 0; i < PointsNumber; i++)
                result += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", Data[0][i].X, Data[0][i].Y, Data[1][i].Y, Data[2][i].Y, Data[3][i].Y);

            return _asciiEncoding.GetBytes(result);
        }

        /// <summary>
        /// Writes arrived data to the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            byte[] result = _GetDataBytes(e.Data);

            using (FileStream WriteDataStream = File.Open(_FileName, FileMode.OpenOrCreate))
            {
                WriteDataStream.Seek(0, SeekOrigin.End);
                await WriteDataStream.WriteAsync(result, 0, result.Length);
            }
        }

        #endregion
        
        #region Correctly disposing the instance

        public void Dispose()
        {
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
        }

        #endregion
    }

    #endregion
}
