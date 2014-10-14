using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ivi.Driver.Interop;
using Agilent.AgilentU254x.Interop;

namespace Agilent_U2542A_Setup
{
    class AnalogOutputChannel
    {
        #region AnalogOutputChannel settings

        private AgilentU254x _Driver;
        public AgilentU254x Driver
        {
            get { return _Driver; }
        }

        private string[] _AllOutputChannels;
        public string[] AllOutputChannels
        {
            get
            {
                if(_AllOutputChannels == null)
                {
                    _AllOutputChannels = new string[_Driver.AnalogOut.ChannelCount];
                    for(int i = 0; i < _AllOutputChannels.Length; i++)
                    {
                        _AllOutputChannels[i] = _Driver.AnalogOut.get_ChannelName(i);
                    }
                }

                return _AllOutputChannels;
            }
        }

        private int _id;
        private DAQ_Bit A0, A1, A2, EN;

        private int _OutputNumber;
        public int OutputNumber
        {
            get { return _OutputNumber; }
            set
            {
                /* if (_id == 201)
                     if (!((value <= 8) && (value >= 1))) return;
                 if (_id == 202)
                     if (!((value <= 16) && (value >= 9))) return;*/
                _OutputNumber = value;

                switch (value)
                {
                    case 16: { A0.TheValue = 0; A1.TheValue = 0; A2.TheValue = 1; break; }
                    case 15: { A0.TheValue = 1; A1.TheValue = 0; A2.TheValue = 1; break; }
                    case 14: { A0.TheValue = 1; A1.TheValue = 1; A2.TheValue = 1; break; }
                    case 13: { A0.TheValue = 0; A1.TheValue = 1; A2.TheValue = 1; break; }
                    case 12: { A0.TheValue = 1; A1.TheValue = 1; A2.TheValue = 0; break; }
                    case 11: { A0.TheValue = 0; A1.TheValue = 1; A2.TheValue = 0; break; }
                    case 10: { A0.TheValue = 1; A1.TheValue = 0; A2.TheValue = 0; break; }
                    case 9: { A0.TheValue = 0; A1.TheValue = 0; A2.TheValue = 0; break; }
                    case 8: { A0.TheValue = 1; A1.TheValue = 1; A2.TheValue = 1; break; }
                    case 7: { A0.TheValue = 0; A1.TheValue = 1; A2.TheValue = 1; break; }
                    case 6: { A0.TheValue = 1; A1.TheValue = 0; A2.TheValue = 1; break; }
                    case 5: { A0.TheValue = 0; A1.TheValue = 0; A2.TheValue = 1; break; }
                    case 4: { A0.TheValue = 1; A1.TheValue = 1; A2.TheValue = 0; break; }
                    case 3: { A0.TheValue = 0; A1.TheValue = 1; A2.TheValue = 0; break; }
                    case 2: { A0.TheValue = 1; A1.TheValue = 0; A2.TheValue = 0; break; }
                    case 1: { A0.TheValue = 0; A1.TheValue = 0; A2.TheValue = 0; break; }
                }

            }
        }

        private bool _Enabled;
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                if (_Enabled) 
                {
                    _Driver.AnalogOut.set_Enabled(AllOutputChannels[_id - 201], true);

                    EN.TheValue = 1; 
                }
                else 
                {
                    _Driver.AnalogOut.set_Enabled(AllOutputChannels[_id - 201], false);

                    EN.TheValue = 0;
                }
            }
        }

        private double _DCVoltage;
        public double DCVoltage
        {
            get { return _DCVoltage; }
            set
            {
                _Driver.AnalogOut.Generation.set_Voltage(AllOutputChannels[_id - 201], value);
                _DCVoltage = value;
            }
        }

        public double DCOffset;

        private int _AC_Frequency;
        public int AC_Frequency
        {
            get { return _AC_Frequency; }
            set
            {
                _Driver.AnalogOut.Waveform.Frequency = value;
                _AC_Frequency = value;
            }
        }

        private double _AC_Amplitude;
        public double AC_Amplitude
        {
            get { return _AC_Amplitude; }
            set
            {
                _Driver.AnalogOut.Waveform.Configure(AllOutputChannels[_id - 201], AgilentU254xWaveformEnum.AgilentU254xWaveformSine, _AC_Frequency, value, DCOffset);
                _AC_Amplitude = value;
            }
        }

        private bool _SineOut;
        public bool SineOut
        {
            get { return _SineOut; }
            set
            {
                _Driver.AnalogOut.Generation.Mode = AgilentU254xAnalogOutModeEnum.AgilentU254xAnalogOutModeWaveform;

                if (value == true)
                    _Driver.AnalogOut.Generation.Start();
                else
                    _Driver.AnalogOut.Generation.Stop();

                _SineOut = value;
            }
        }

        private int _Iterations;
        public int Iterations
        {
            get { return _Iterations; }
            set
            {
                _Iterations = value;
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Constructor

        public AnalogOutputChannel(int outputID)
        {
            _id = outputID;

            _Driver = AgilentU254xDriver.Instance.Driver;

            if (_id == 201)
            {
                A0 = new DAQ_Bit(501, 4);
                A1 = new DAQ_Bit(501, 5);
                A2 = new DAQ_Bit(501, 6);
                EN = new DAQ_Bit(501, 7);
                this.OutputNumber = 1;
            }
            else
            {
                if (_id == 202)
                {
                    A0 = new DAQ_Bit(501, 0);
                    A1 = new DAQ_Bit(501, 1);
                    A2 = new DAQ_Bit(501, 2);
                    EN = new DAQ_Bit(501, 3);
                    this.OutputNumber = 9;
                }
                else throw new Exception("Not Correct number of AO channel");
            }
            EN.TheValue = 1;
            DCOffset = 0;
        }

        #endregion
    }
}
