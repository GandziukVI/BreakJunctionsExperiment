using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A;

namespace Agilent_U2542A_ExtensionBox
{
    class AnalogOutputChannel
    {
        #region AnalogOutputChannel settings

        private int _id;
        private Agilent_U2542A_AnalogOutput _AO;
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
                    case 16: { A0.value = 0; A1.value = 0; A2.value = 1; break; }
                    case 15: { A0.value = 1; A1.value = 0; A2.value = 1; break; }
                    case 14: { A0.value = 1; A1.value = 1; A2.value = 1; break; }
                    case 13: { A0.value = 0; A1.value = 1; A2.value = 1; break; }
                    case 12: { A0.value = 1; A1.value = 1; A2.value = 0; break; }
                    case 11: { A0.value = 0; A1.value = 1; A2.value = 0; break; }
                    case 10: { A0.value = 1; A1.value = 0; A2.value = 0; break; }
                    case 9: { A0.value = 0; A1.value = 0; A2.value = 0; break; }
                    case 8: { A0.value = 1; A1.value = 1; A2.value = 1; break; }
                    case 7: { A0.value = 0; A1.value = 1; A2.value = 1; break; }
                    case 6: { A0.value = 1; A1.value = 0; A2.value = 1; break; }
                    case 5: { A0.value = 0; A1.value = 0; A2.value = 1; break; }
                    case 4: { A0.value = 1; A1.value = 1; A2.value = 0; break; }
                    case 3: { A0.value = 0; A1.value = 1; A2.value = 0; break; }
                    case 2: { A0.value = 1; A1.value = 0; A2.value = 0; break; }
                    case 1: { A0.value = 0; A1.value = 0; A2.value = 0; break; }
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
                if (_Enabled) { _AO.Enable(); EN.value = 1; }
                else { _AO.Disable(); EN.value = 0; };
            }
        }

        private double _DCVoltage;
        public double DCVoltage
        {
            get { return _DCVoltage; }
            set
            {
                _AO.SetDCVoltage(value);
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
                _AC_Frequency = value;
                _AO.SetFrequency(value);
            }
        }

        private double _AC_Amplitude;
        public double AC_Amplitude
        {
            get { return _AC_Amplitude; }
            set
            {

                _AC_Amplitude = value;
                _AO.applySine(_AC_Amplitude, DCOffset);
            }
        }

        private bool _SineOut;
        public bool SineOut
        {
            get { return _SineOut; }
            set
            {
                _SineOut = value;
                if (SineOut) _AO.OutputON();
                else _AO.OutputOFF();
            }
        }

        private int _Iterations;
        public int Iterations
        {
            get { return _Iterations; }
            set
            {
                _Iterations = value;
                _AO.SetIterations(_Iterations);
            }
        }

        #endregion

        #region Constructor

        public AnalogOutputChannel(int outputID)
        {
            _id = outputID;

            _AO = new Agilent_U2542A_AnalogOutput(_id, ImportantConstants.DeviceID);
            if (_id == 201)
            {
                A0 = new DAQ_Bit(501, 4, ImportantConstants.DeviceID);
                A1 = new DAQ_Bit(501, 5, ImportantConstants.DeviceID);
                A2 = new DAQ_Bit(501, 6, ImportantConstants.DeviceID);
                EN = new DAQ_Bit(501, 7, ImportantConstants.DeviceID);
                this.OutputNumber = 1;
            }
            else
            {
                if (_id == 202)
                {
                    A0 = new DAQ_Bit(501, 0, ImportantConstants.DeviceID);
                    A1 = new DAQ_Bit(501, 1, ImportantConstants.DeviceID);
                    A2 = new DAQ_Bit(501, 2, ImportantConstants.DeviceID);
                    EN = new DAQ_Bit(501, 3, ImportantConstants.DeviceID);
                    this.OutputNumber = 9;
                }
                else throw new Exception("Not Correct number of AO channel");
            }
            EN.value = 1;
            DCOffset = 0;
        }

        #endregion
    }
}
