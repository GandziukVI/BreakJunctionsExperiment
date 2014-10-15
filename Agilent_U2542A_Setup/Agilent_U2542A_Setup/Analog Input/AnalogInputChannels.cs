using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ivi.Driver.Interop;
using Agilent.AgilentU254x.Interop;
using System.Threading;

namespace Agilent_U2542A_Setup
{
    public class AnalogInputChannels
    {
        #region AnalogInputChannels settings

        private AgilentU254x _Driver;
        public AgilentU254x Driver
        {
            get { return _Driver; }
        }

        private string[] _AllAnalogInputChannels;
        public string[] AllAnalogInputChannels
        {
            get
            {
                if(_AllAnalogInputChannels == null)
                {
                    _AllAnalogInputChannels = new string[_Driver.AnalogIn.Channels.Count];
                    for (int i = 0; i < _Driver.AnalogIn.Channels.Count; i++)
                        _AllAnalogInputChannels[i] = _Driver.AnalogIn.Channels.get_Name(i + 1);
                }

                return _AllAnalogInputChannels;
            }
        }
        
        private AnalogInputChannel[] _Channels;
        public AnalogInputChannel[] ChannelArray
        {
            get { return _Channels; }
        }

        private int _ACQ_Rate;
        public int ACQ_Rate
        {
            get
            {
                if (_ACQ_Rate == 0)
                {
                    _ACQ_Rate = _Driver.AnalogIn.MultiScan.SampleRate;
                    return _ACQ_Rate;

                }
                return _ACQ_Rate;
            }
            set
            {
                _Driver.AnalogIn.MultiScan.SampleRate = value;
                _ACQ_Rate = value;
            }
        }

        private int _PointsPerBlock;
        public int PointsPerBlock
        {
            get
            {
                if (_PointsPerBlock == 0)
                {
                    this._PointsPerBlock = _Driver.AnalogIn.Acquisition.BufferSize;
                }
                return _PointsPerBlock;
            }
            set
            {
                _Driver.AnalogIn.Acquisition.BufferSize = value;
                _PointsPerBlock = value;
            }
        }

        private int _SingleShotPointsPerBlock;
        public int SingleShotPointsPerBlock
        {
            get
            {
                if (_SingleShotPointsPerBlock == 0)
                {
                    this._SingleShotPointsPerBlock = _Driver.AnalogIn.Acquisition.BufferSize;
                }
                return _SingleShotPointsPerBlock;
            }
            set
            {
                _Driver.AnalogIn.Acquisition.BufferSize = value;
                _SingleShotPointsPerBlock = value;
            }
        }

        #endregion

        #region Singleton pattern implementation

        private static AnalogInputChannels _Instance;
        public static AnalogInputChannels Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new AnalogInputChannels();

                return _Instance;
            }
        }

        private AnalogInputChannels()
        {
            _Driver = AgilentU254xDriver.Instance.Driver;
            _Channels = new AnalogInputChannel[4] { new AnalogInputChannel(1), new AnalogInputChannel(2), new AnalogInputChannel(3), new AnalogInputChannel(4) };
        }

        #endregion

        #region AnalogInputChannels functionality

        public void Read_AI_Channel_Status()
        {
            for (int i = 0; i < _Channels.Length; i++)
            {
                if (_Driver.AnalogIn.Channels.get_Item(AllAnalogInputChannels[i]).Polarity == AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityBipolar)
                    _Channels[i].SetACPolarity(true);
                else if (_Driver.AnalogIn.Channels.get_Item(AllAnalogInputChannels[i]).Polarity == AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityUnipolar)
                    _Channels[i].SetACPolarity(false);
                else 
                    throw new Exception("The polarity returned unknown");

                _Channels[i].SetACRange(_Driver.AnalogIn.Channels.get_Item(AllAnalogInputChannels[i]).Range);

                if (_Driver.AnalogIn.Channels.get_Item(AllAnalogInputChannels[i]).Enabled == true) 
                    _Channels[i].SetEnabled(true);
                else if (_Driver.AnalogIn.Channels.get_Item(AllAnalogInputChannels[i]).Enabled == false)
                    _Channels[i].SetEnabled(false);
                else 
                    throw new Exception("The enabled returned unknown");
                //Channels[i].AcqRate = ACQ_Rate;
            }
        }

        public void SetChannelsToAC()
        {
            foreach (AnalogInputChannel a in ChannelArray)
            {
                if (a.Enabled) a.ChannelProperties.IsAC = true;
            }
        }

        //========================= Binary data acquisition =========================

        public void DisableAllChannelsForContiniousDataAcquisition()
        {
            foreach (var ChName in AllAnalogInputChannels)
                _Driver.AnalogIn.Channels.get_Item(ChName).Enabled = false;
        }

        public void SetSingleShotPointsPerBlockValue(int PointsPerBlock)
        {
            _Driver.AnalogIn.Acquisition.BufferSize = PointsPerBlock;
        }

        public void StartAnalogAcqusition()
        {
            _Driver.AnalogIn.MultiScan.NumberOfScans = -1;
            _Driver.AnalogIn.Acquisition.Start();
        }

        public void AcquireSingleShot()
        {
            _Driver.AnalogIn.Acquisition.Start();            
        }

        public void StopAnalogAcqusition()
        {
            _Driver.AnalogIn.Acquisition.Stop();
        }

        public bool CheckAcquisitionStatus()
        {
            var _AcqisitionStatus = _Driver.AnalogIn.Acquisition.BufferStatus;

            switch (_AcqisitionStatus)
            {
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusDataReady:
                    return true;
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusEmpty:
                    return false;
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusFragment:
                    return false;
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusOverRun:
                    throw new Exception("Device buffer overload");
                default:
                    break;
            }

            return false;
        }

        public bool CheckSingleShotAcquisitionStatus()
        {
            var _AcqisitionStatus = _Driver.AnalogIn.Acquisition.BufferStatus;

            switch (_AcqisitionStatus)
            {
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusDataReady:
                    return true;
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusEmpty:
                    return false;
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusFragment:
                    return false;
                case AgilentU254xBufferStatusEnum.AgilentU254xBufferStatusOverRun:
                    throw new Exception("Device buffer overload");
                default:
                    break;
            }

            return false;
        }

        public short[] AcquireArrayWithData()
        {
            short[] Results = { 0 };
            Thread.Sleep((int)(1000.0 * _Driver.AnalogIn.MultiScan.TimePerScan * (double)PointsPerBlock / (double)ACQ_Rate));
            _Driver.AnalogIn.Acquisition.Fetch(ref Results);

            return Results;
        }

        //========================= Numeric data acquisition =========================

        private int _DC_Average;
        public int DC_Average
        {
            get
            {
                _Driver.System.DirectIO.WriteString("VOLT:AVER?");
                var result = _Driver.System.DirectIO.ReadString();
                _DC_Average = Convert.ToInt32(result);
                
                return _DC_Average;
            }
            set
            {
                if ((value < 1) || (value > 1000))
                    value = 100;

                _Driver.System.DirectIO.WriteString(String.Format("VOLT:AVER {0}", value));
                
                _DC_Average = value;
            }
        }

        public double[] VoltageMeasurement101_104()
        {
            foreach (AnalogInputChannel a in ChannelArray)
            {
                a.ChannelProperties.IsAC = false;
            }
            
            double[] result = new double[4];

            _Driver.AnalogIn.Measurement.ReadMultiple(String.Format("{0},{1},{2},{3}", AllAnalogInputChannels[0], AllAnalogInputChannels[1], AllAnalogInputChannels[2], AllAnalogInputChannels[3]), ref result);

            return result;
        }

        #endregion
    }
}
