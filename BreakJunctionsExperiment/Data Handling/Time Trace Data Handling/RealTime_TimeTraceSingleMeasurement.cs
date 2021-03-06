﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using BreakJunctions.Events;
using System.Globalization;

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

        private int _PointsNumber = 0;
        private double _TimeShift = 0.0;

        private NumberFormatInfo _NumberFormatInfo;

        private FileStream _WriteDataStream;
        private FileStream _WriteMotionDataStream;

        #endregion

        #region Constructor / Destructor

        public RealTime_TimeTraceSingleMeasurement(string __FileName, double __AppliedVoltage, string __SampleNumber)
        {
            _FileName = __FileName;

            _NumberFormatInfo = NumberFormatInfo.InvariantInfo;

            AllEventsHandler.Instance.RealTime_TimeTrace_ResetTimeShift += OnRealTime_TimeTrace_ResetTimeShift;
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived;
            AllEventsHandler.Instance.Motion_RealTime += OnMotion_RealTime_DataArrived;
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
        private byte[] _GetDataBytes(List<Point>[] Data)
        {
            string result = string.Empty;

            for (int i = 0; i < this._PointsNumber; i++)
                result += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", (Data[0][i].X + _TimeShift).ToString(_NumberFormatInfo), Data[0][i].Y.ToString(_NumberFormatInfo), Data[1][i].Y.ToString(_NumberFormatInfo), Data[2][i].Y.ToString(_NumberFormatInfo), Data[3][i].Y.ToString(_NumberFormatInfo));

            return Encoding.ASCII.GetBytes(result);
        }

        /// <summary>
        /// Converts the List of PointD to the byte array
        /// </summary>
        /// <param name="_Data">List of data points</param>
        /// <returns>Appropriate byte array</returns>
        private byte[] _GetDataBytes(LinkedList<Point>[] Data)
        {
            var _Data = new Point[4][]
            {
                Data[0].ToArray(),
                Data[1].ToArray(),
                Data[2].ToArray(),
                Data[3].ToArray()
            };

            string result = string.Empty;

            for (int i = 0; i < this._PointsNumber; i++)
                result += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", (_Data[0][i].X + _TimeShift).ToString(_NumberFormatInfo), _Data[0][i].Y.ToString(_NumberFormatInfo), _Data[1][i].Y.ToString(_NumberFormatInfo), _Data[2][i].Y.ToString(_NumberFormatInfo), _Data[3][i].Y.ToString(_NumberFormatInfo));

            return Encoding.ASCII.GetBytes(result);
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
        private async void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            try
            {
                if (e.Data != null)
                {
                    this._PointsNumber = (new int[] { e.Data[0].Count, e.Data[1].Count, e.Data[2].Count, e.Data[3].Count }).Min();

                    byte[] DataToWrite = _GetDataBytes(e.Data);
                    this._TimeShift += e.Data[0].Last().X;

                    await WriteAcquisitionDataBytesAsync(_FileName, DataToWrite);
                }
            }
            catch { }
        }

        private async void OnMotion_RealTime_DataArrived(object sender, Motion_RealTime_EventArgs e)
        {
            try
            {
                var ToWrite = Encoding.ASCII.GetBytes(String.Format("{0}\t{1}\r\n", e.Time.ToString(_NumberFormatInfo), e.Position.ToString(_NumberFormatInfo)));
                var MotionDataFileName = _FileName.Insert(_FileName.LastIndexOf('.'), "_MotionData");

                await WriteMotionDataBytesAsync(MotionDataFileName, ToWrite);
            }
            catch { }
        }

        private async Task WriteAcquisitionDataBytesAsync(string __FilePath, byte[] __ToWrite)
        {
            using (_WriteDataStream = new FileStream(__FilePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await _WriteDataStream.WriteAsync(__ToWrite, 0, __ToWrite.Length);
            };
        }

        private async Task WriteMotionDataBytesAsync(string __FilePath, byte[] __ToWrite)
        {
            using (_WriteMotionDataStream = new FileStream(__FilePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await _WriteMotionDataStream.WriteAsync(__ToWrite, 0, __ToWrite.Length);
            };
        }

        #endregion

        #region Correctly disposing the instance

        public void Dispose()
        {
            AllEventsHandler.Instance.RealTime_TimeTrace_ResetTimeShift -= OnRealTime_TimeTrace_ResetTimeShift;
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
            AllEventsHandler.Instance.Motion_RealTime -= OnMotion_RealTime_DataArrived;
        }

        #endregion
    }

    #endregion
}
