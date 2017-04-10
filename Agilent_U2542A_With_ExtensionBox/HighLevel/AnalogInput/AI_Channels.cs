using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Agilent_U2542A_With_ExtensionBox.Classes
{
    public class AI_Channels
    {
        private static AI_Channels _instance;
        public static AI_Channels Instance
        {
            get
            {
                if (_instance == null) _instance = new AI_Channels();
                return _instance;

            }
        }

        private Agilent_AnalogInput_LowLevel _AI;

        private AI_Channel[] _Channels;
        public AI_Channel[] ChannelArray
        {
            get { return _Channels; }
        }

        private AI_Channels()
        {
            this._Channels = new AI_Channel[4] { new AI_Channel(1), new AI_Channel(2), new AI_Channel(3), new AI_Channel(4) };
            _AI = new Agilent_AnalogInput_LowLevel();
        }

        private int _ACQ_Rate;
        public int ACQ_Rate
        {
            set
            {
                _AI.tryToWriteString(String.Format("ACQ:SRAT {0}", value));
                _ACQ_Rate = value;
            }
            get
            {
                if (_ACQ_Rate == 0)
                {
                    string AcquisitionRate = _AI.tryToQueryString("ACQ:SRAT?");
                    int ACQ = Convert.ToInt32(AcquisitionRate);
                    this._ACQ_Rate = ACQ;
                    return ACQ;

                }
                return _ACQ_Rate;
            }
        }

        private int _PointsPerBlock;
        public int PointsPerBlock
        {
            set
            {
                _AI.tryToWriteString(String.Format("WAV:POIN {0}", value));
                _PointsPerBlock = value;
            }
            get
            {
                if (_PointsPerBlock == 0)
                {
                    string PperBlck = _AI.tryToQueryString("WAV:POIN?");
                    this._PointsPerBlock = Convert.ToInt32(PperBlck);
                }
                return _PointsPerBlock;
            }
        }

        private int _SingleShotPointsPerBlock;
        public int SingleShotPointsPerBlock
        {
            set
            {
                _AI.tryToWriteString(String.Format("ACQ:POIN {0}", value));
                _SingleShotPointsPerBlock = value;
            }
            get
            {
                if (_SingleShotPointsPerBlock == 0)
                {
                    string PperBlck = _AI.tryToQueryString("ACQ:POIN?");
                    this._SingleShotPointsPerBlock = Convert.ToInt32(PperBlck);
                }
                return _SingleShotPointsPerBlock;
            }
        }

        public void Read_AI_Channel_Status()
        {

            string devicePolarity = _AI.tryToQueryString("ROUT:CHAN:POL? (@101:104)");
            
            string[] ChannelPolarities = devicePolarity.Split(',');
            string deviceRanges = _AI.tryToQueryString("ROUT:CHAN:RANG? (@101:104)");
            string[] ChannelRanges = deviceRanges.Split(',');
            string DeviceEnabled = _AI.tryToQueryString("ROUT:ENAB? (@101:104)");
            string[] ChannelEnabled = DeviceEnabled.Split(',');

            for (int i = 0; i < _Channels.Length; i++)
            {
                if (ChannelPolarities[i] == "BIP") _Channels[i].SetACPolarity(true);
                else if (ChannelPolarities[i] == "UNIP") _Channels[i].SetACPolarity(false);
                else throw new Exception("The polarity returned unknown");
                _Channels[i].setACRange(Convert.ToDouble(ChannelRanges[i], ImportantConstants.NumberFormat()));

                if (ChannelEnabled[i] == "1") _Channels[i].SetEnabled(true);
                else if (ChannelEnabled[i] == "0") _Channels[i].SetEnabled(false);
                else throw new Exception("The enabled returned unknown");
                //Channels[i].AcqRate = ACQ_Rate;
            }
        }

        public void SetChannelsToAC()
        {
            foreach (AI_Channel a in ChannelArray)
            {
                if (a.Enabled) a.ChannelProperties.isAC = true;
            }
        }

        public void SetChannelsToDC()
        {
            foreach (AI_Channel a in ChannelArray)
                if (a.Enabled) a.ChannelProperties.isAC = false;
        }


        //========================= Binary data acquisition =========================

        public void DisableAllChannelsForContiniousDataAcquisition()
        {
            _AI.tryToWriteString("ROUT:ENAB OFF,(@101:104)");
        }


        public void SetSingleShotPointsPerBlockValue(int PointsPerBlock)
        {
            _AI.tryToWriteString(String.Format("ACQ:POIN {0}", PointsPerBlock));
        }

        public void StartAnalogAcqusition()
        {
            _AI.tryToWriteString("RUN");
        }

        public void AcquireSingleShot()
        {
            _AI.tryToWriteString("DIG");
        }

        public void StopAnalogAcqusition()
        {
            _AI.tryToWriteString("STOP");
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
            set
            {
                if ((value < 1) || (value > 1000)) value = 100;
                _AI.tryToWriteString(String.Format("VOLT:AVER {0}", value));
                _DC_Average = value;
            }
            get
            {
                string result = _AI.tryToQueryString("VOLT:AVER?");
                _DC_Average = Convert.ToInt32(result);
                return _DC_Average;
            }
        }

        public double[] VoltageMeasurement101_104()
        {
            foreach (AI_Channel a in ChannelArray)
            {
                a.ChannelProperties.isAC = false;
                //Thread.Sleep(1000);
            }

            double[] result = new double[4];
            string resultStr = _AI.tryToQueryString("MEAS? (@101:104)");
            string[] parsedResultStr = resultStr.Split(',');

            for (int i = 0; i < 4; i++)
                result[i] = Convert.ToDouble(parsedResultStr[i], ImportantConstants.NumberFormat());

            return result;
        }
    }
}
