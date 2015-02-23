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
    class NoiseCalibrationSingleMeasurement : IDisposable
    {
        #region NoiseCalibrationSingleMeasurement settings

        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        private FileStream _NoiseCalibrationData_StreamWriter;
        private ChannelsToInvestigate _SelectedChannel;

        public List<Point> CalibrationData { get; set; }

        #endregion

        #region Constructor / Destructor

        public NoiseCalibrationSingleMeasurement(string __FileName, ChannelsToInvestigate __SelectedChannel)
        {
            _FileName = __FileName;
            _SelectedChannel = __SelectedChannel;

            Attach_DataRecieveEvent();
        }

        ~NoiseCalibrationSingleMeasurement()
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
            AllEventsHandler.Instance.LastNoiseSpectra_Channel_01_DataArrived -= On_Noise_LastSpectrum_DataArrived;
            AllEventsHandler.Instance.LastNoiseSpectra_Channel_02_DataArrived -= On_Noise_LastSpectrum_DataArrived;
        }

        private async Task WriteChannelData(List<Point> _Data)
        {
            CalibrationData = _Data;
            using (_NoiseCalibrationData_StreamWriter = new FileStream(_FileName, FileMode.Create, FileAccess.Write,
                FileShare.None, bufferSize: 4096, useAsync: true))
            {
                var result = string.Empty;
                foreach (var _DataPoint in _Data)
                    result += String.Format("{0}\t{1}\r\n", _DataPoint.X.ToString(NumberFormatInfo.InvariantInfo), _DataPoint.Y.ToString(NumberFormatInfo.InvariantInfo));

                var resultBytes = Encoding.ASCII.GetBytes(result);

                await _NoiseCalibrationData_StreamWriter.WriteAsync(resultBytes, 0, resultBytes.Length);
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

            if (_NoiseCalibrationData_StreamWriter != null)
                _NoiseCalibrationData_StreamWriter.Dispose();
        }

        #endregion
    }
}
