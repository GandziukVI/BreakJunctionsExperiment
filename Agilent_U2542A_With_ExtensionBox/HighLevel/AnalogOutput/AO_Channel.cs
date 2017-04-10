using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A_With_ExtensionBox.Classes
{
    public class AO_Channel
    {
        private int _id;
        private Agilent_AnalogOutput_LowLevel _AO;
        private DAQ_Bit A0, A1, A2, EN;
        public AO_Channel(int id)
        {
            _id = id;
            _AO = new Agilent_AnalogOutput_LowLevel(id);
            if (id == 201)
            {
                A0 = new DAQ_Bit(501, 4);
                A1 = new DAQ_Bit(501, 5);
                A2 = new DAQ_Bit(501, 6);
                EN = new DAQ_Bit(501, 7);
                this.OutputNumber = 1;
            }
            else
            {
                if (id == 202)
                {
                    A0 = new DAQ_Bit(501, 0);
                    A1 = new DAQ_Bit(501, 1);
                    A2 = new DAQ_Bit(501, 2);
                    EN = new DAQ_Bit(501, 3);
                    this.OutputNumber = 9;
                }
                else throw new Exception("Not Correct number of AO channel");
            }
            EN.value = 1;
            DCOffset=0;
         }
        private int _OutputNumber;
        
        public int OutputNumber {
            
            set
            {
               /* if (_id == 201)
                    if (!((value <= 8) && (value >= 1))) return;
                if (_id == 202)
                    if (!((value <= 16) && (value >= 9))) return;*/
                _OutputNumber=value;
                  
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
            get { return _OutputNumber; }

        }
        
        private bool _Enabled;
        public bool Enabled
        {
            set
            {
                _Enabled = value;
                if (_Enabled) { _AO.Enable(); EN.value = 1; }
                else { _AO.Disable(); EN.value = 0; };
            }
            get
            {
                return _Enabled;
            }
        }

        private double _DCVoltage;
        public double DCVoltage
        {
            set
            {
                _AO.SetDCVoltage(value);
                _DCVoltage = value;
            }
            get
            {
                return _DCVoltage;
            }
        }
        public double DCOffset;

        private int _AC_Frequency;
        public int AC_Frequency
        {
            get { return _AC_Frequency; }
            set { _AC_Frequency = value;
            _AO.SetFrequency(value);
            }
        }
        private double _AC_Amplitude;
        public double AC_Amplitude
        {
            set 
            {
               
                _AC_Amplitude = value;
                _AO.applySine(_AC_Amplitude, DCOffset);
            }
            get
            {
                return _AC_Amplitude;
            }
        }
        private bool _SineOut;
        public bool SineOut
        {
            set
            {
                _SineOut = value;
                if (SineOut) _AO.OutputON();
                else _AO.OutputOFF();
            }
            get
            {
                return _SineOut;
            }
        }
        private int _iterations;
        public int  Iterations
        {
            get { return _iterations; }
            set 
            {
               _iterations = value;
               _AO.SetIterations(_iterations);
            }
        }
    }
}
