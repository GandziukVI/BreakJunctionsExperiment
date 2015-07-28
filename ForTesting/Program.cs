using Agilent_U2542A_With_ExtensionBox.Classes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ForTesting
{
    class Program
    {
        public static bool MeasurementInProcess = true;
        public static ConcurrentQueue<string> _StringDataQueue = new ConcurrentQueue<string>();
        public static Thread _DataTransformingAndSendingThread;
        public static AI_Channels _Channels = AI_Channels.Instance;
        public static DataStringConverter _DataConverter = new DataStringConverter();
        public static int ACQ_Rate;
        public static int _PointsNumber;
        public static double _TimeShift = 0.0;

        private static byte[] _GetDataBytes(List<Point>[] Data)
        {
            string result = string.Empty;

            for (int i = 0; i < _PointsNumber; i++)
                result += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\r\n", (Data[0][i].X + _TimeShift).ToString(NumberFormatInfo.InvariantInfo),
                    Data[0][i].Y.ToString(NumberFormatInfo.InvariantInfo), Data[1][i].Y.ToString(NumberFormatInfo.InvariantInfo),
                    Data[2][i].Y.ToString(NumberFormatInfo.InvariantInfo), Data[3][i].Y.ToString(NumberFormatInfo.InvariantInfo));

            return Encoding.ASCII.GetBytes(result);
        }

        private static async void _TransformAndEmitData()
        {
            while (MeasurementInProcess || !_StringDataQueue.IsEmpty)
            {
                string _Data;

                var _DequeueSuccess = _StringDataQueue.TryDequeue(out _Data);

                if (_DequeueSuccess == true)
                {
                    var resultInt = _DataConverter.ParseDataStringToInt(_Data);
                    var ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);

                    _PointsNumber = (new int[] { ChannelData[0].Count, ChannelData[1].Count, ChannelData[2].Count, ChannelData[3].Count }).Min();

                    byte[] DataToWrite = _GetDataBytes(ChannelData);
                    _TimeShift += ChannelData[0].Last().X;

                    await WriteAcquisitionDataBytesAsync("D:\\SuperMeasurement.dat", DataToWrite);
                }
            }
        }

        private static async Task WriteAcquisitionDataBytesAsync(string __FilePath, byte[] __ToWrite)
        {
            using (var _WriteDataStream = new FileStream(__FilePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await _WriteDataStream.WriteAsync(__ToWrite, 0, __ToWrite.Length);
            };
        }

        public static List<Point> MakeSingleShot(int NumberOfChannel)
        {
            _Channels.DisableAllChannelsForContiniousDataAcquisition();
            _Channels.ChannelArray[NumberOfChannel - 1].Enabled = true;
            _Channels.ChannelArray[NumberOfChannel - 1].ChannelProperties.isAC = true;
            _Channels.Read_AI_Channel_Status();
            ACQ_Rate = _Channels.ACQ_Rate;

            _Channels.AcquireSingleShot();
            while ((!_Channels.CheckSingleShotAcquisitionStatus())) ;
            
            string result = AI_Channels.Instance.AcquireStringWithData();
            Int16[] resultInt = _DataConverter.ParseDataStringToInt(result);
            List<Point>[] ChannelData = _DataConverter.ParseIntArrayIntoChannelData(resultInt, ACQ_Rate);

            return ChannelData[NumberOfChannel - 1];
        }

        public static void ContiniousAcquisition()
        {
            Agilent_DigitalOutput_LowLevel.Instance.AllToZero();

            _Channels.ACQ_Rate = 5000;
            _Channels.PointsPerBlock = 1000;

            _Channels.Read_AI_Channel_Status();

            ACQ_Rate = _Channels.ACQ_Rate;

            _Channels.DisableAllChannelsForContiniousDataAcquisition();

            _Channels.SetChannelsToDC();
            _Channels.SetChannelsToAC();

            foreach(var ch in _Channels.ChannelArray)
                ch.Enabled = true;

            _Channels.StartAnalogAcqusition();

            _DataTransformingAndSendingThread = new Thread(_TransformAndEmitData);
            _DataTransformingAndSendingThread.Priority = ThreadPriority.Normal;
            _DataTransformingAndSendingThread.Start();

            while (MeasurementInProcess)
            {
                while (!_Channels.CheckAcquisitionStatus()) ;
                string result = _Channels.AcquireStringWithData();
                _StringDataQueue.Enqueue(result);
            }

            _Channels.StopAnalogAcqusition();
        }

        static void Main(string[] args)
        {
            System.Timers.Timer a = new System.Timers.Timer(2000);
            a.Elapsed += a_Elapsed;
            a.Start();
            
            ContiniousAcquisition();

            a.Stop();
        }

        static void a_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MeasurementInProcess = false;
        }
    }
}
