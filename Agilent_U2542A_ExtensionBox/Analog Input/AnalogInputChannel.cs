using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    public class AnalogInputChannel
    {
        #region AnalogInputChannelSettings

        private Agilent_U2542A_AnalogInput _AI;
        private Agilent_U2542A_DigitalOutput _DIO;

        private int _number;
        public int number
        {
            get { return _number; }
            set
            {
                _number = 0;

                if ((value < 5) && (value > 0))
                    _number = value + 100;
                if ((value < 105) && (value > 100))
                    _number = value;
                if (_number == 0)
                    throw new Exception("Wrong number of channel set");
            }
        }

        private int _cutOffFrequency;
        public int cutOffFrequency
        {
            get { return _cutOffFrequency; }
            set
            {
                if (ImportantConstants.CutOffFrequencies.Contains(value))
                    _cutOffFrequency = value;
                else _cutOffFrequency = 100;
            }
        }

        public AnalogInputChannel_Latch ChannelProperties;

        private double _ACrange;
        public double AC_Range
        {
            get { return _ACrange; }
            set
            {

                if (!ImportantConstants.Ranges.Contains(value)) throw new Exception("Incorect range " + value + "set for channel" + number);

                _AI.tryToWriteString(String.Format("ROUT:CHAN:RANG {0}, (@{1})", Convert.ToString(value, ImportantConstants.NumberFormat()), number));
                _ACrange = value;
            }
        }

        private bool _Enabled;
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                if (value)
                    _AI.tryToWriteString(String.Format("ROUT:ENAB ON,(@{0})", number));
                else
                    _AI.tryToWriteString(String.Format("ROUT:ENAB OFF,(@{0})", number));
                _Enabled = value;
            }            
        }

        private bool _IsBipolarAC;
        public bool IsBiPolarAC
        {
            get { return _IsBipolarAC; }
            set
            {
                if (value)
                    _AI.tryToWriteString(String.Format("ROUT:CHAN:POL BIP, (@{0})", number));
                else
                    _AI.tryToWriteString(String.Format("ROUT:CHAN:POL UNIP, (@{0})", number));

                _IsBipolarAC = value;
            }
        }

        private double _DCrange;
        public double DC_Range
        {
            get { return _DCrange; }
            set
            {
                double[] Ranges = new double[] { 10, 5, 2.5, 1.25, 0 };
                if (!Ranges.Contains(value)) 
                    throw new Exception("Incorect range " + value + "set for channel" + number);
                string ToWrite = "";
                if (value == 0) 
                    ToWrite = "AUTO";
                else 
                    ToWrite = Convert.ToString(value, ImportantConstants.NumberFormat());

                _AI.tryToWriteString(String.Format("SENS:VOLT:RANG {0}, (@{1})", ToWrite, number));
                _DCrange = value;
            }
        }

        private bool _IsBipolarDC;
        public bool IsBiPolarDC
        {
            get { return _IsBipolarDC; }
            set
            {
                if (value)
                    _AI.tryToWriteString(String.Format("VOLT:POL BIP, (@{0})", number));
                else
                    _AI.tryToWriteString(String.Format("VOLT:POL UNIP, (@{0})", number));

                _IsBipolarDC = value;
            }
        }

        #endregion

        #region Constructor

        public AnalogInputChannel(int ChannelNumber)
        {
            number = ChannelNumber;
            ChannelProperties = new AnalogInputChannel_Latch(ChannelNumber);
            
            _AI = new Agilent_U2542A_AnalogInput();
            _DIO = Agilent_U2542A_DigitalOutput.Instance;
        }

        #endregion

        #region AnalogInputChannel functionality

        public void setACRange(double Range)
        {
            double[] Ranges = new double[] { 10, 5, 2.5, 1.25 };
            if (!Ranges.Contains(Range)) throw new Exception("Incorect range " + Range + "set for channel" + number);
            _ACrange = Range;
        }

        public void SetEnabled(bool Enabled)
        {
            this._Enabled = Enabled;
        }

        public void SetACPolarity(bool Polarity)
        {
            _IsBipolarAC = Polarity;
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
            if (polarity == "BIP") this._IsBipolarAC = true;
        }

        private void isEnabled()
        {
            string result = _AI.tryToQueryString(String.Format("ROUT:ENAB? (@{0})", _number));
            
            if (result[0] == '1') this._Enabled = true;
            if (result[0] == '0') this._Enabled = false;
        }

        //============== Numeric analog Data aqcuisition ===============

        private void getDC_Range()
        {
            string range = _AI.tryToQueryString(String.Format("VOLT:RANG? (@{0})", _number));

            if (range == "AUTO")
                this._DCrange = 0;
            else
                this._DCrange = Convert.ToDouble(range, ImportantConstants.NumberFormat());
        }

        private void getDC_Polarity()
        {
            string polarity = _AI.tryToQueryString(String.Format("VOLT:POL? (@{0})", _number));
            
            if (polarity == "BIP") 
                this._IsBipolarDC = true;
        }

        private double singleVoltageMeasurement()
        {
            string resultStr = _AI.tryToQueryString(String.Format("MEAS? (@{0})", number));
            return Convert.ToDouble(resultStr, ImportantConstants.NumberFormat());
        }

        public void ReadChannelInfo()
        {
            getACRange();
            getACPolarity();
            getDC_Range();
            getDC_Polarity();
        }

        #endregion
    }
}
