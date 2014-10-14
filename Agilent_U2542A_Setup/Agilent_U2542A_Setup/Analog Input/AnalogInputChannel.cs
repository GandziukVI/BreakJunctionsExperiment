using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ivi.Driver.Interop;
using Agilent.AgilentU254x.Interop;

namespace Agilent_U2542A_Setup
{
    public class AnalogInputChannel
    {
        #region AnalogInputChannelSettings

        private AgilentU254x _Driver;
        /// <summary>
        /// Gets the device driver
        /// </summary>
        public AgilentU254x Driver
        {
            get { return _Driver; }
        }

        private int _ChannelNumber;
        /// <summary>
        /// The number of the channel
        /// </summary>
        public int ChannelNumber
        {
            get { return _ChannelNumber; }
            set
            {
                _ChannelNumber = 0;

                if ((value < 5) && (value > 0))
                    _ChannelNumber = value + 100;
                if ((value < 105) && (value > 100))
                    _ChannelNumber = value;
                if (_ChannelNumber == 0)
                    throw new Exception("Wrong number of channel set");
            }
        }

        private int _CutOffFrequency;
        /// <summary>
        /// CutOff frequency
        /// </summary>
        public int CutOffFrequency
        {
            get { return _CutOffFrequency; }
            set
            {
                if (ImportantConstants.CutOffFrequencies.Contains(value))
                    _CutOffFrequency = value;
                else _CutOffFrequency = 100;
            }
        }

        public AnalogInputChannel_Latch ChannelProperties;

        private bool _Enabled;
        /// <summary>
        /// Enables or disables the appropriate channel
        /// </summary>
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                string ChName = _GetChannelNameByNumber();

                if (value == true)
                    _Driver.AnalogIn.Channels.get_Item(ChName).Enabled = true;
                else
                    _Driver.AnalogIn.Channels.get_Item(ChName).Enabled = false;

                _Enabled = value;
            }            
        }

        private bool _IsBipolarAC;
        /// <summary>
        /// Indicates, whether the appropriate channel
        /// has "Bipolar" AC polarity
        /// </summary>
        public bool IsBiPolarAC
        {
            get { return _IsBipolarAC; }
            set
            {
                string ChName = _GetChannelNameByNumber();

                if (value == true)
                    _Driver.AnalogIn.Channels.get_Item(ChName).Polarity = AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityBipolar;
                else
                    _Driver.AnalogIn.Channels.get_Item(ChName).Polarity = AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityUnipolar;

                _IsBipolarAC = value;
            }
        }

        private bool _IsBipolarDC;
        /// <summary>
        /// Indicates, whether the appropriate channel
        /// has "Bipolar" DC polarity
        /// </summary>
        public bool IsBiPolarDC
        {
            get { return _IsBipolarDC; }
            set
            {
                string ChName = _GetChannelNameByNumber();

                if (value == true)
                    _Driver.AnalogIn.Channels.get_Item(ChName).Polarity = AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityBipolar;
                else
                    _Driver.AnalogIn.Channels.get_Item(ChName).Polarity = AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityUnipolar;

                _IsBipolarDC = value;
            }
        }

        private double _ACrange;
        /// <summary>
        /// Gets or sets the AC range of the appropriate channel
        /// </summary>
        public double AC_Range
        {
            get { return _ACrange; }
            set
            {
                string ChName = _GetChannelNameByNumber();

                if (!ImportantConstants.Ranges.Contains(value)) 
                    throw new Exception("Incorect range " + value + " set for channel " + ChannelNumber);

                _Driver.AnalogIn.Channels.get_Item(ChName).Range = value;
                _ACrange = value;
            }
        }
        
        private double _DCrange;
        /// <summary>
        /// Gets or sets the AC range of the appropriate channel
        /// </summary>
        public double DC_Range
        {
            get { return _DCrange; }
            set
            {
                string ChName = _GetChannelNameByNumber();
                double[] Ranges = new double[] { 10, 5, 2.5, 1.25, 0 };

                if (!Ranges.Contains(value)) 
                    throw new Exception("Incorect range " + value + "set for channel" + ChannelNumber);

                if (value == 0)
                    _Driver.System.DirectIO.WriteString(String.Format("SENS:VOLT:RANG AUTO,(@{0})", _ChannelNumber));
                else
                    _Driver.AnalogIn.Channels.get_Item(ChName).Range = value;

                _DCrange = value;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the new instance of AanlogInputChannel
        /// for appropriate channel
        /// </summary>
        /// <param name="__ChannelNumber">Channel number</param>
        public AnalogInputChannel(int __ChannelNumber)
        {
            this.ChannelNumber = __ChannelNumber;
            this.ChannelProperties = new AnalogInputChannel_Latch(ChannelNumber);

            this._Driver = AgilentU254xDriver.Instance.Driver;
            this._Driver.AnalogIn.Measurement.AutoScaleEnabled = true;
        }

        #endregion

        #region AnalogInputChannel functionality

        /// <summary>
        /// Gets appropriate channel name it the tersm of device names
        /// </summary>
        /// <returns></returns>
        private string _GetChannelNameByNumber()
        {
            string _ChannelName = string.Empty;
            switch(_ChannelNumber)
            {
                case 501:
                    {
                        _ChannelName = _Driver.AnalogIn.Channels.get_Name(1);
                    }break;
                case 502:
                    {
                        _ChannelName = _Driver.AnalogIn.Channels.get_Name(2);
                    } break;
                case 503:
                    {
                        _ChannelName = _Driver.AnalogIn.Channels.get_Name(3);
                    } break;
                case 504:
                    {
                        _ChannelName = _Driver.AnalogIn.Channels.get_Name(4);
                    } break;
            }

            return _ChannelName;
        }

        public void SetACRange(double Range)
        {
            double[] Ranges = new double[] { 10, 5, 2.5, 1.25 };

            if (!Ranges.Contains(Range)) 
                throw new Exception("Incorect range " + Range + " set for channel " + ChannelNumber);

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

        private void _GetACRange()
        {
            var ChName = _GetChannelNameByNumber();
            var range = _Driver.AnalogIn.Channels.get_Item(ChName).Range;

            this._ACrange = range;
        }

        private void _GetACPolarity()
        {
            var ChName = _GetChannelNameByNumber();
            var polarity = _Driver.AnalogIn.Channels.get_Item(ChName).Polarity;

            switch (polarity)
            {
                case AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityBipolar:
                    {
                        this._IsBipolarAC = true;
                    } break;
                case AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityUnipolar:
                    {
                        this._IsBipolarAC = false;
                    } break;
            }
        }

        private void _IsEnabled()
        {
            var ChName = _GetChannelNameByNumber();
            this._Enabled = _Driver.AnalogIn.Channels.get_Item(ChName).Enabled;
        }

        //============== Numeric analog Data aqcuisition ===============

        private void _GetDC_Range()
        {
            var ChName = _GetChannelNameByNumber();
            var range = _Driver.AnalogIn.Channels.get_Item(ChName).Range;

            this._DCrange = range;
        }

        private void _GetDC_Polarity()
        {
            var ChName = _GetChannelNameByNumber();
            var polarity = _Driver.AnalogIn.Channels.get_Item(ChName).Polarity;

            switch (polarity)
            {
                case AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityBipolar:
                    {
                        this._IsBipolarDC = true;
                    } break;
                case AgilentU254xAnalogPolarityEnum.AgilentU254xAnalogPolarityUnipolar:
                    {
                        this._IsBipolarDC = false;
                    } break;
            }
        }

        private double _SingleVoltageMeasurement()
        {
            var ChName = _GetChannelNameByNumber();
            double result = 0.0;
            _Driver.AnalogIn.Measurement.ReadSingle(ChName, ref result);

            return result;
        }

        public void ReadChannelInfo()
        {
            _GetACRange();
            _GetACPolarity();
            _GetDC_Range();
            _GetDC_Polarity();
        }

        #endregion
    }
}
