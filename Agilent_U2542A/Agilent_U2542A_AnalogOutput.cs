using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agilent_U2542A
{
    public class Agilent_U2542A_AnalogOutput
    {
        #region Analog output settings

        private int DAQ_Number;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the instance of Agilent_U2542A_AnalogOutput
        /// for managing analog output
        /// </summary>
        /// <param name="channelNumber">Channel number</param>
        /// <param name="ID">Device ID</param>
        public Agilent_U2542A_AnalogOutput(int channelNumber)
        {
            if (!AgilentUSB_Device.Instance.IsAlive)
                AgilentUSB_Device.Instance.InitDevice();
            if (!AgilentUSB_Device.Instance.IsAlive)
                throw new Exception("Device Not Connected");

            DAQ_Number = channelNumber;

            AgilentUSB_Device.Instance.SendCommandRequest("SOUR:VOLT:POL BIP, (@201:202)");
            AgilentUSB_Device.Instance.SendCommandRequest("OUTP:WAV:ITER 0");
            AgilentUSB_Device.Instance.SendCommandRequest("OUTP:WAV:SRAT 0");
        }

        #endregion

        #region Analog output functionality

        /// <summary>
        /// Sets DC voltage to the appropriate output
        /// </summary>
        /// <param name="Voltage">Voltage to be set to the channel</param>
        public void SetDCVoltage(double Voltage)
        {
            if (!((Voltage < 10) && ((Voltage > -10)))) 
                return;

            AgilentUSB_Device.Instance.SendCommandRequest(String.Format("SOUR:VOLT {0}, (@{1})", Voltage.ToString("G3"), DAQ_Number));
        }

        /// <summary>
        /// Sets iterations
        /// </summary>
        /// <param name="Iter"></param>
        public void SetIterations(int Iter)
        {
            AgilentUSB_Device.Instance.SendCommandRequest(String.Format("OUTP:WAV:ITER {0}", Iter));
        }

        /// <summary>
        /// Sets frequency to the appropriate channel
        /// </summary>
        /// <param name="Frequency">Frequency value</param>
        public void SetFrequency(int Frequency)
        {
            if (Frequency > 10000)
                return;
            if (Frequency < 0)
                return;

            AgilentUSB_Device.Instance.SendCommandRequest(String.Format("OUTP:WAV:FREQ {0}", Frequency));
        }

        /// <summary>
        /// Enables appropriate channel
        /// </summary>
        public void Enable()
        {
            AgilentUSB_Device.Instance.SendCommandRequest(String.Format("ROUT:ENAB ON,(@{0})", DAQ_Number));
        }

        /// <summary>
        /// Disables appropriate channel
        /// </summary>
        public void Disable()
        {
            AgilentUSB_Device.Instance.SendCommandRequest(String.Format("ROUT:ENAB OFF,(@{0})", DAQ_Number));
        }

        /// <summary>
        /// Applies Sin voltage to the appropriate output
        /// </summary>
        /// <param name="amplitude">Amplitude of signal</param>
        /// <param name="offset">Offset</param>
        public void applySine(double amplitude, double offset)
        {
            if (amplitude < 0)  return;
            if (amplitude > 10) return;
            if (offset > 10)    return;
            if (offset < -10)   return;

            AgilentUSB_Device.Instance.SendCommandRequest(String.Format("APPL:SIN {0},{1}, (@{2})", amplitude.ToString("G3"), offset.ToString("G3"), DAQ_Number));
        }

        /// <summary>
        /// Switches ON the output
        /// </summary>
        public void OutputON()
        {
            AgilentUSB_Device.Instance.SendCommandRequest("OUTP ON");
        }

        /// <summary>
        /// Switches OFF the output
        /// </summary>
        public void OutputOFF()
        {
            AgilentUSB_Device.Instance.SendCommandRequest("OUTP OFF");
        }

        #endregion
    }
}
