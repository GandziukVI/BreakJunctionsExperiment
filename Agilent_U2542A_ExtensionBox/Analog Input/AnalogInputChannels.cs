using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    public class AnalogInputChannels
    {
        #region AnalogInputChannels settings

        private AgilentUSB_Device _Device = AgilentUSB_Device.Instance;

        private Agilent_U2542A_AnalogInput _AI;
        
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
                    string AcquisitionRate = _AI.RequestQuery("ACQ:SRAT?");
                    int ACQ = Convert.ToInt32(AcquisitionRate);
                    this._ACQ_Rate = ACQ;
                    return ACQ;

                }
                return _ACQ_Rate;
            }
            set
            {
                _Device.SendCommandRequest(String.Format("ACQ:SRAT {0}", value));
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
                    string PperBlck = _AI.RequestQuery("WAV:POIN?");
                    this._PointsPerBlock = Convert.ToInt32(PperBlck);
                }
                return _PointsPerBlock;
            }
            set
            {
                _Device.SendCommandRequest(String.Format("WAV:POIN {0}", value));
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
                    string PperBlck = _AI.RequestQuery("ACQ:POIN?");
                    this._SingleShotPointsPerBlock = Convert.ToInt32(PperBlck);
                }
                return _SingleShotPointsPerBlock;
            }
            set
            {
                _Device.SendCommandRequest(String.Format("ACQ:POIN {0}", value));
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
            _Channels = new AnalogInputChannel[4] { new AnalogInputChannel(1), new AnalogInputChannel(2), new AnalogInputChannel(3), new AnalogInputChannel(4) };
            _AI = new Agilent_U2542A_AnalogInput();
        }

        #endregion

        #region AnalogInputChannels functionality

        public void Read_AI_Channel_Status()
        {
            string devicePolarity = _AI.RequestQuery("ROUT:CHAN:POL? (@101:104)");
            string[] ChannelPolarities = devicePolarity.Split(',');
            string deviceRanges = _AI.RequestQuery("ROUT:CHAN:RANG? (@101:104)");
            string[] ChannelRanges = deviceRanges.Split(',');
            string DeviceEnabled = _AI.RequestQuery("ROUT:ENAB? (@101:104)");
            string[] ChannelEnabled = DeviceEnabled.Split(',');


            for (int i = 0; i < _Channels.Length; i++)
            {
                if (ChannelPolarities[i] == "BIP") 
                    _Channels[i].SetACPolarity(true);
                else if (ChannelPolarities[i] == "UNIP") 
                    _Channels[i].SetACPolarity(false);
                else 
                    throw new Exception("The polarity returned unknown");
                
                _Channels[i].setACRange(Convert.ToDouble(ChannelRanges[i], ImportantConstants.NumberFormat()));

                if (ChannelEnabled[i] == "1") 
                    _Channels[i].SetEnabled(true);
                else if (ChannelEnabled[i] == "0") 
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
                if (a.Enabled) a.ChannelProperties.isAC = true;
            }
        }

        //========================= Binary data acquisition =========================

        public void DisableAllChannelsForContiniousDataAcquisition()
        {
            _Device.SendCommandRequest("ROUT:ENAB OFF,(@101:104)");
        }


        public void SetSingleShotPointsPerBlockValue(int PointsPerBlock)
        {
            _Device.SendCommandRequest(String.Format("ACQ:POIN {0}", PointsPerBlock));

        }
        public void StartAnalogAcqusition()
        {
            _Device.SendCommandRequest("RUN");
        }

        public void AcquireSingleShot()
        {
            _Device.SendCommandRequest("DIG");
        }
        public void StopAnalogAcqusition()
        {
            _Device.SendCommandRequest("STOP");
        }
        public bool CheckAcquisitionStatus()
        {
            return _AI.CheckAcquisitionStatus();
        }
        public bool CheckSingleShotAcquisitionStatus()
        {
            return _AI.CheckSingleShotAcquisitionStatus();
        }
        public string AcquireStringWithData()
        {
            return _AI.AcquireRawADC_Data();
        }

        //========================= Numeric data acquisition =========================

        private int _DC_Average;
        public int DC_Average
        {
            get
            {
                string result = _AI.RequestQuery("VOLT:AVER?");
                _DC_Average = Convert.ToInt32(result);
                return _DC_Average;
            }
            set
            {
                if ((value < 1) || (value > 1000))
                    value = 100;

                _Device.SendCommandRequest(String.Format("VOLT:AVER {0}", value));
                _DC_Average = value;
            }
        }

        public double[] VoltageMeasurement101_104()
        {
            foreach (AnalogInputChannel a in ChannelArray)
            {
                a.ChannelProperties.isAC = false;
                //Thread.Sleep(1000);
            }
            
            double[] result = new double[4];
            string resultStr = _AI.RequestQuery("MEAS? (@101:104)");
            string[] parsedResultStr = resultStr.Split(',');
            
            for (int i = 0; i < 4; i++)
                result[i] = Convert.ToDouble(parsedResultStr[i], ImportantConstants.NumberFormat());
            return result;
        }

        #endregion
    }
}
