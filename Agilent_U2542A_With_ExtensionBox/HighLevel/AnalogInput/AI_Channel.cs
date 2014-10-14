using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Agilent_U2542A_With_ExtensionBox.Classes
{
    public class AI_Channel
    {
        private Agilent_AnalogInput_LowLevel _AI;
        private Agilent_DigitalOutput_LowLevel _DIO;

        private int _number;
        public int number
        {
            set
            {
                _number = 0;
                if ((value < 5) && (value > 0)) _number = value + 100;
                if ((value < 105) && (value > 100)) _number = value;
                if (_number == 0) throw new Exception("Wrong number of channel set");
            }
            get
            {
                return _number;
            }
        }

        private int _cutOffFrequency;
        public int cutOffFrequency
        {
            set 
            {
                if (ImportantConstants.CutOffFrequencies.Contains(value))
                    _cutOffFrequency = value;
                else _cutOffFrequency = 100;
            }
            get
            {
                return _cutOffFrequency;
            }
        }

        public AI_Channel_Latch ChannelProperties;

        private double _ACrange;
        public double AC_Range
        {
            set
            {
                
                if (!ImportantConstants.Ranges.Contains(value)) throw new Exception("Incorect range " + value + "set for channel" + number);

                _AI.tryToWriteString(String.Format("ROUT:CHAN:RANG {0}, (@{1})", Convert.ToString(value, ImportantConstants.NumberFormat()), number));
                _ACrange = value;
            }
            get
            {
                return _ACrange;
            }
        }
        public void setACRange(double Range)
        {
            double[] Ranges = new double[] { 10, 5, 2.5, 1.25 };
            if (!Ranges.Contains(Range)) throw new Exception("Incorect range " + Range + "set for channel" + number);
            _ACrange = Range;
        }

        private bool _enabled;
        public bool Enabled
        {
            set 
            {
                if(value)
                    _AI.tryToWriteString(String.Format("ROUT:ENAB ON,(@{0})", number));
                else
                    _AI.tryToWriteString(String.Format("ROUT:ENAB OFF,(@{0})", number));
                _enabled = value;
            }
            get
            {
                return _enabled;
            }
        }
        public void SetEnabled(bool Enabled)
        {
            this._enabled = Enabled;
        }

        private bool _isBipolarAC;
        public bool isBiPolarAC
        {
            set
            {
                if (value)
                    _AI.tryToWriteString(String.Format("ROUT:CHAN:POL BIP, (@{0})", number));
                else
                    _AI.tryToWriteString(String.Format("ROUT:CHAN:POL UNIP, (@{0})", number));
                _isBipolarAC = value;
            }
            get
            {
                return _isBipolarAC;
            }
        }
        public void SetACPolarity(bool Polarity)
        {
            _isBipolarAC = Polarity;
        }

        private double _DCrange;
        public double DC_Range
        {
            set
            {
                double[] Ranges = new double[] { 10, 5, 2.5, 1.25, 0 };
                if (!Ranges.Contains(value)) throw new Exception("Incorect range " + value + "set for channel" + number);
                string ToWrite = "";
                if (value == 0) ToWrite = "AUTO";
                else ToWrite = Convert.ToString(value, ImportantConstants.NumberFormat());
                _AI.tryToWriteString(String.Format("SENS:VOLT:RANG {0}, (@{1})", ToWrite, number));
                _DCrange = value;
            }
            get
            {
                return _DCrange;
            }
        }

        private bool _isBipolarDC;
        public bool isBiPolarDC
        {
            set
            {
                if (value)
                    _AI.tryToWriteString(String.Format("VOLT:POL BIP, (@{0})", number));
                else
                    _AI.tryToWriteString(String.Format("VOLT:POL UNIP, (@{0})", number));
                _isBipolarDC = value;
            }
            get
            {
                return _isBipolarDC;
            }
        }

        public AI_Channel(int ChannelNumber)
        {
            number = ChannelNumber;
            ChannelProperties = new AI_Channel_Latch(ChannelNumber);
            _AI = new Agilent_AnalogInput_LowLevel();
            _DIO = Agilent_DigitalOutput_LowLevel.Instance;
        }

        public void ReadChannelInfo()
        {
            getACRange();
            getACPolarity();
            getDC_Range();
            getDC_Polarity();
        }

        //============== Binary analog Data aqcuisition ===============

        private void getACRange()
        {
            string range = _AI.tryToQueryString(String.Format("ROUT:CHAN:RANG? (@{0})", _number));
            this._ACrange = Convert.ToDouble(range, ImportantConstants.NumberFormat());
        }
        private void getACPolarity()
        {
            string polarity = _AI.tryToQueryString(String.Format("ROUT:CHAN:POL? (@{0})", _number));
            if (polarity == "BIP") this._isBipolarAC = true;
        }
        private void isEnabled()
        {
            string result = _AI.tryToQueryString(String.Format("ROUT:ENAB? (@{0})", _number));
            
            if (result[0] == '1') this._enabled = true;
            if (result[0] == '0') this._enabled = false;
        }

        //============== Numeric analog Data aqcuisition ===============

        private void getDC_Range()
        {
            string range = _AI.tryToQueryString(String.Format("VOLT:RANG? (@{0})", _number));
            if (range == "AUTO") this._DCrange = 0;
            else this._DCrange = Convert.ToDouble(range, ImportantConstants.NumberFormat());
        }
        private void getDC_Polarity()
        {
            string polarity = _AI.tryToQueryString(String.Format("VOLT:POL? (@{0})", _number));
            if (polarity == "BIP") this._isBipolarDC = true;
        }
        
        private double singleVoltageMeasurement()
        {
            string resultStr = _AI.tryToQueryString(String.Format("MEAS? (@{0})", number));
            return Convert.ToDouble(resultStr, ImportantConstants.NumberFormat());
        }
    }
}
