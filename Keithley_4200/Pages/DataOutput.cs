using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keithley_4200.Pages
{
    public class DataOutput
    {
        #region DataOutput settings

        private IExperimentalDevice _TheDevice;

        #endregion

        #region Constructor

        public DataOutput(ref IExperimentalDevice __TheDevice)
        {
            _TheDevice = __TheDevice;
        }

        #endregion

        #region Functionality

        public List<double> ObtainOutputData(string __MeasurementChannelName)
        {
            var MeasurementChannelName = (__MeasurementChannelName.Length <= 6) ? __MeasurementChannelName : __MeasurementChannelName.Substring(0, 6);
            var query = String.Format("DO \'{0}\'", MeasurementChannelName);
            var responce = _TheDevice.RequestQuery(query).Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var result = new List<double>();
            for (int i = 0; i < responce.Length; i++)
            {
                var Data = 0.0;
                var IsReadingSuccess = double.TryParse(responce[i].Substring(1), DataFormatting.NumberStyle, DataFormatting.NumberFormat, out Data);
                if (IsReadingSuccess)
                    result.Add(Data);
            }

            return result;
        }

        public void SaveFile(FileType __FileType, string __FileName, string __Comment)
        {
            var Type = (__FileType == FileType.ProgramFile) ? "P" : "D";
            var FileName = (__FileName.Length <= 6) ? __FileName : __FileName.Substring(0, 6);
            var Comment = (__Comment.Length <= 6) ? __Comment : __Comment.Substring(0, 6);

            var command = String.Format("SV \'{0} {1} {2}\'", Type, FileName, Comment);
            _TheDevice.SendCommandRequest(command);
        }

        public void GetFile(FileType __FileType, string __FileName)
        {
            var Type = (__FileType == FileType.ProgramFile) ? "P" : "D";
            var FileName = (__FileName.Length <= 6) ? __FileName : __FileName.Substring(0, 6);

            var command = String.Format("GT \'{0} {1}\'", Type, FileName);
            _TheDevice.SendCommandRequest(command);
        }

        public void MapChannel(SMUs __SelectedChannel, ChannelType __ChannelType, SMUs __SelectedNumber)
        {
            var Type = String.Empty;
            switch (__ChannelType)
            {
                case ChannelType.SMU:
                    {
                        Type = "SMU";
                    } break;
                case ChannelType.VS:
                    {
                        Type = "VS";
                    } break;
                case ChannelType.VM:
                    {
                        Type = "VM";
                    } break;
                default:
                    {
                        Type = "SMU";
                    } break;
            }

            var command = String.Format("MP {0}, {1}{2}", (int)__SelectedChannel, Type, (int)__SelectedNumber);
            _TheDevice.SendCommandRequest(command);
        }

        public void SetSourceRange(SMUs __SelectedChennel, SourceRanges __Range)
        {
            var command = String.Format("SR {0}, {1}", (int)__SelectedChennel, (int)__Range);
            _TheDevice.SendCommandRequest(command);
        }

        #endregion
    }
}
