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

        private static int _FileCounter = 0;

        private ASCIIEncoding _asciiEncoding;

        private int _PointsNumber = 0;
        private double _TimeShift = 0.0;

        private FileStream _WriteDataStream;

        #endregion

        #region Constructor / Destructor

        public RealTime_TimeTraceSingleMeasurement(string filename, double appliedVoltage, string sampleNumber)
        {
            _FileName = filename;
            _asciiEncoding = new ASCIIEncoding();

            _WriteDataStream = File.Open(_FileName, FileMode.OpenOrCreate);

            AllEventsHandler.Instance.RealTime_TimeTrace_ResetTimeShift += OnRealTime_TimeTrace_ResetTimeShift;
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived;
        }

        ~RealTime_TimeTraceSingleMeasurement()
        {
            this.Dispose();
        }

        #endregion

        #region RealTime_TimeTraceSingleMeasurement functionality

        public void AttachPointDataReceiveEvent()
        {
            AllEventsHandler.Instance.RealTime_TimeTrace_ResetTimeShift += OnRealTime_TimeTrace_ResetTimeShift;
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived;
        }

        public void DetachPointReceiveEvent()
        {
            AllEventsHandler.Instance.RealTime_TimeTrace_ResetTimeShift -= OnRealTime_TimeTrace_ResetTimeShift;
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
        }

        /// <summary>
        /// Converts the List of PointD to the byte array
        /// </summary>
        /// <param name="Data">List of data points</param>
        /// <returns>Appropriate byte array</returns>
        private byte[] _GetDataBytes(List<PointD>[] Data)
        {
            string result = string.Empty;

            for (int i = 0; i < this._PointsNumber; i++)
                result += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", Data[0][i].X + _TimeShift, Data[0][i].Y, Data[1][i].Y, Data[2][i].Y, Data[3][i].Y);

            return _asciiEncoding.GetBytes(result);
        }

        /// <summary>
        /// Resets the time shift to zero for
        /// the new mwasurement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRealTime_TimeTrace_ResetTimeShift(object sender, RealTime_TimeTrace_ResetTimeShift_EventArgs e)
        {
            this._TimeShift = 0.0;
        }

        /// <summary>
        /// Writes arrived data to the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    this._PointsNumber = (new int[] { e.Data[0].Count, e.Data[1].Count, e.Data[2].Count, e.Data[3].Count }).Min();

                    byte[] result = _GetDataBytes(e.Data);
                    this._TimeShift += e.Data[0].Last().X;
                    this._WriteDataStream.Write(result, 0, result.Length);
                }
            }
            catch { }
        }

        #endregion
        
        #region Correctly disposing the instance

        public void Dispose()
        {
            _WriteDataStream.Close();
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
        }

        #endregion
    }

    #endregion
}
