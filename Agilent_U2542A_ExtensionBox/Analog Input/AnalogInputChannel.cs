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
                if ((value < 5) && (value > 0))     _number = value + 100;
                if ((value < 105) && (value > 100)) _number = value;
                if (_number == 0)                   throw new Exception("Wrong number of channel set");
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

                _AI.SendCommandRequest(String.Format("ROUT:CHAN:RANG {0}, (@{1})", Convert.ToString(value, ImportantConstants.NumberFormat()), number));
                _ACrange = value;
            }
        }

        private bool _enabled;
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value)
                    _AI.SendCommandRequest(String.Format("ROUT:ENAB ON,(@{0})", number));
                else
                    _AI.SendCommandRequest(String.Format("ROUT:ENAB OFF,(@{0})", number));
                _enabled = value;
            }            
        }

        private bool _isBipolarAC;
        public bool isBiPolarAC
        {
            get { return _isBipolarAC; }
            set
            {
                if (value)
                    _AI.SendCommandRequest(String.Format("ROUT:CHAN:POL BIP, (@{0})", number));
                else
                    _AI.SendCommandRequest(String.Format("ROUT:CHAN:POL UNIP, (@{0})", number));

                _isBipolarAC = value;
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

                _AI.SendCommandRequest(String.Format("SENS:VOLT:RANG {0}, (@{1})", ToWrite, number));
                _DCrange = value;
            }
        }

        private bool _isBipolarDC;
        public bool isBiPolarDC
        {
            get { return _isBipolarDC; }
            set
            {
                if (value)
                    _AI.SendCommandRequest(String.Format("VOLT:POL BIP, (@{0})", number));
                else
                    _AI.SendCommandRequest(String.Format("VOLT:POL UNIP, (@{0})", number));

                _isBipolarDC = value;
            }
        }

        #endregion

        #region Constructor

        public AnalogInputChannel(int ChannelNumber, string ID = "USB0::0x0957::0x1718::TW52524501::INSTR")
        {
            number = ChannelNumber;
            ChannelProperties = new AnalogInputChannel_Latch(ChannelNumber);
            _AI = new Agilent_U2542A_AnalogInput(ID);
            _DIO = new Agilent_U2542A_DigitalOutput(ID);
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
            this._enabled = Enabled;
        }

        public void SetACPolarity(bool Polarity)
        {
            _isBipolarAC = Polarity;
        }

        //==============Binary analog Data aqcuisition===============

        private void getACRange()
        {
            string range = _AI.RequestQuery(String.Format("ROUT:CHAN:RANG? (@{0})", _number));
            this._ACrange = Convert.ToDouble(range, ImportantConstants.NumberFormat());
        }
        private void getACPolarity()
        {
            string polarity = _AI.RequestQuery(String.Format("ROUT:CHAN:POL? (@{0})", _number));
            if (polarity == "BIP") this._isBipolarAC = true;
        }
        private void isEnabled()
        {
            string result = _AI.RequestQuery(String.Format("ROUT:ENAB? (@{0})", _number));
            
            if (result[0] == '1') this._enabled = true;
            if (result[0] == '0') this._enabled = false;
        }

        //==============Numeric analog Data aqcuisition===============

        private void getDC_Range()
        {
            string range = _AI.RequestQuery(String.Format("VOLT:RANG? (@{0})", _number));

            if (range == "AUTO")
                this._DCrange = 0;
            else
                this._DCrange = Convert.ToDouble(range, ImportantConstants.NumberFormat());
        }
        private void getDC_Polarity()
        {
            string polarity = _AI.RequestQuery(String.Format("VOLT:POL? (@{0})", _number));
            
            if (polarity == "BIP") 
                this._isBipolarDC = true;
        }

        private double singleVoltageMeasurement()
        {
            string resultStr = _AI.RequestQuery(String.Format("MEAS? (@{0})", number));
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
