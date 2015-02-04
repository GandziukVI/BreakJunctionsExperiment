using BreakJunctions.Events;
using BreakJunctions.Plotting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakJunctions.DataHandling
{
    class NoiseSingleMeasurement : IDisposable
    {
        #region NoiseSingleMeasurement settings

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private FileStream _NoiseData_StreamWriter;
        private ChannelsToInvestigate _SelectedChannel;
        private List<Point> _CalibrationData;

        #endregion

        #region Constructor / Destructor

        public NoiseSingleMeasurement(string __FileName, ChannelsToInvestigate __SelectedChannel, ref NoiseCalibrationSingleMeasurement __CalibrationMeasurement)
        {
            _FileName = __FileName;
            _SelectedChannel = __SelectedChannel;
            _CalibrationData = __CalibrationMeasurement.CalibrationData;

            __CalibrationMeasurement.Detach_PointRecieveEvent();
        }

        ~NoiseSingleMeasurement()
        {
            Dispose();
        }

        #endregion

        #region Functionality implementation

        public void Attach_DataRecieveEvent()
        {
            switch (_SelectedChannel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_01_DataArrived += On_Noise_LastSpectrum_DataArrived;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_02_DataArrived += On_Noise_LastSpectrum_DataArrived;
                    } break;
                default:
                    break;
            }
        }

        public void Detach_PointRecieveEvent()
        {
            switch (_SelectedChannel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_01_DataArrived -= On_Noise_LastSpectrum_DataArrived;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_02_DataArrived -= On_Noise_LastSpectrum_DataArrived;
                    } break;
                default:
                    break;
            }
        }

        private async Task WriteChannelData(List<Point> _Data)
        {
            var ToWrite = new List<Point>();
            for (int i = 0; i < _Data.Count; i++)
                ToWrite.Add(new Point(_Data[i].X, _Data[i].Y - _CalibrationData[i].Y));

            using (_NoiseData_StreamWriter = new FileStream(_FileName, FileMode.Open, FileAccess.Write,
                FileShare.None, bufferSize: 4096, useAsync: true))
            {
                var result = string.Empty;
                foreach (var _DataPoint in ToWrite)
                    result += String.Format("{0}\t{1}\r\n", _DataPoint.X.ToString(NumberFormatInfo.InvariantInfo), _DataPoint.Y.ToString(NumberFormatInfo.InvariantInfo));

                var resultBytes = Encoding.ASCII.GetBytes(result);

                await _NoiseData_StreamWriter.WriteAsync(resultBytes, 0, resultBytes.Length);
            };
        }

        private async void On_Noise_LastSpectrum_DataArrived(object sender, LastNoiseSpectra_Channel_01_DataArrived_EventArgs e)
        {
            await WriteChannelData(e.SpectraData);
        }

        private async void On_Noise_LastSpectrum_DataArrived(object sender, LastNoiseSpectra_Channel_02_DataArrived_EventArgs e)
        {
            await WriteChannelData(e.SpectraData);
        }

        #endregion

        #region Correctly disposing the instance

        public void Dispose()
        {
            Detach_PointRecieveEvent();

            if (_NoiseData_StreamWriter != null)
                _NoiseData_StreamWriter.Dispose();
        }

        #endregion
    }
}
