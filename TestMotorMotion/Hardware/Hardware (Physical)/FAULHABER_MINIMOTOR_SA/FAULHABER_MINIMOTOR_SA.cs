﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Ports;

using System.Threading;

using Hardware;

namespace Hardware
{
    class FAULHABER_MINIMOTOR_SA : COM_Device, IMotion, IDisposable
    {
        #region Motion settings

        private double _CurrentTime = 0.0;

        private double _CurrentPosition = 0.0;
        public double CurrentPosition
        {
            get { return _CurrentPosition; }
            set { _CurrentPosition = value; }
        }

        private double _StartPosition = 0.0;
        public double StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }

        private double _FinalDestination = 0.0;
        public double FinalDestination
        {
            get { return _FinalDestination; }
            set { _FinalDestination = value; }
        }

        private int _CurrentIteration = 0;
        private int _NumberRepetities = 0;
        public int NumberRepetities
        {
            get { return _NumberRepetities; }
            set { _NumberRepetities = value; }
        }

        private double _MotionVelosity = 0.0;
        public double MotionVelosity
        {
            get { return _MotionVelosity; }
            set { _MotionVelosity = value; }
        }

        private MotionVelosityUnits _motionVelosityUnits = MotionVelosityUnits.rpm;
        public MotionVelosityUnits motionVelosityUnits
        {
            get { return _motionVelosityUnits; }
            set { _motionVelosityUnits = value; }
        }

        private double _VelosityValue = 0.0;
        public double VelosityValue
        {
            get { return _VelosityValue; }
            set { _VelosityValue = value; }
        }

        //private DispatcherTimer _MotionSingleMeasurementTimer;
        //private DispatcherTimer _MotionRepetitiveMeasurementTimer;

        private MotionDirection _CurrentDirection;

        #endregion

        public FAULHABER_MINIMOTOR_SA(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
            : base (comPort, baud, parity, dataBits, stopBits, returnToken) { }

        ~FAULHABER_MINIMOTOR_SA()
        {
            this.Dispose();
        }

        public override bool InitDevice()
        {
            var isInitSucceed = base.InitDevice();

            if (isInitSucceed == true)
            {
                SendCommandRequest("EN");
                return true;
            }
            else return false;
        }

        public override void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var a = sender as SerialPort;
            var b = a.ReadExisting();

            Console.WriteLine(b);
        }

        public void AnswerMode(AnswerMode mode)
        {
            SendCommandRequest(String.Format("ANSW{0}", (int)mode));
        }
        public void EnableDevice()
        {
            SendCommandRequest("EN");
        }
        public void DisableDevice()
        {
            SendCommandRequest("DI");
        }

        public void InitiateMotion()
        {
            SendCommandRequest("M");
        }

        public void LoadAbsolutePosition(int Value)
        {
            const int MaxValue = 1800000000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;
            SendCommandRequest(String.Format("LA{0}", Value));
        }

        public void LoadRelativePosition(int Value)
        {
            const int MaxValue = 2140000000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;
            SendCommandRequest(String.Format("LR{0}", Value));
        }

        public void NotifyPosition()
        {
            SendCommandRequest("NP");
        }
        public void NotifyPosition(int Value)
        {
            const int MaxValue = 1800000000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;
            SendCommandRequest(String.Format("NP{0}", Value));
        }
        public void NotifyPositionOff()
        {
            SendCommandRequest("NPOFF");
        }

        public void SelectVelocityMode(int Value)
        {
            int MaxValue = 30000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;
            
            SendCommandRequest(String.Format("V{0}", Value));
        }

        public void NotifyVelocity(int Value)
        {
            int MaxValue = 30000;
            if (Value < -MaxValue)
                Value = -MaxValue;
            if (Value > MaxValue)
                Value = MaxValue;

            SendCommandRequest(String.Format("NV{0}", Value));
        }
        public void NotifyVelocityOff()
        {
            SendCommandRequest("NVOFF");
        }
        public void SetOutputVoltage()
        {
            throw new NotImplementedException();
        }
        public void GoHomingSequence()
        {
            throw new NotImplementedException();
        }

        public void FindHallIndex()
        {
            throw new NotImplementedException();
        }
        public void GoHallIndex()
        {
            throw new NotImplementedException();
        }
        public void GoEncoderIndex()
        {
            throw new NotImplementedException();
        }
        public void DefineHomePosition()
        {
            throw new NotImplementedException();
        }
        public void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, double motionVelosity = 100.0, MotionVelosityUnits motionVelosityUnits = MotionVelosityUnits.rpm, int numberOfRepetities = 1)
        {
            _StartPosition = StartPosition;
            _CurrentPosition = StartPosition;
            _FinalDestination = FinalDestination;
            _NumberRepetities = numberOfRepetities;
            _MotionVelosity = motionVelosity;
            _motionVelosityUnits = motionVelosityUnits;

            switch (motionKind)
            {
                case MotionKind.Single:
                    {
                    } break;
                case MotionKind.Repetitive:
                    {
                    } break;
                default:
                    break;
            }
        }

        public void StartMotion(TimeSpan FinalTime)
        {
            throw new NotImplementedException();
        }

        public void StartMotion(double FixedR)
        {
            throw new NotImplementedException();
        }

        public void StopMotion()
        {
            //if (_MotionSingleMeasurementTimer.IsEnabled == true)
            //    _MotionSingleMeasurementTimer.Stop();
            //if (_MotionRepetitiveMeasurementTimer.IsEnabled == true)
            //    _MotionRepetitiveMeasurementTimer.Stop();

            //AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(null, new TimeTraceMeasurementStateChanged_EventArgs(false));
        }

        public void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits)
        {
            switch (VelosityUnits)
            {
                case MotionVelosityUnits.rpm:
                    {
                        SendCommandRequest(String.Format("V{0}", Convert.ToInt32(Math.Round(VelosityValue))));
                    } break;
                case MotionVelosityUnits.MilimetersPerMinute:
                    {
                        var RevolutionPerMinute = 0.0005; //Meters per one revolution
                        var _NewVelosity = Convert.ToInt32(VelosityValue / RevolutionPerMinute);

                        SendCommandRequest(String.Format("V{0}", _NewVelosity));
                    } break;
                default:
                    break;
            }
        }

        public void SetDirection(MotionDirection motionDirection)
        {
            if (_CurrentDirection != motionDirection)
            {
                _CurrentDirection = motionDirection;
                ++_CurrentIteration;
            }

            switch (motionDirection)
            {
                case MotionDirection.Up:
                    {
                    } break;
                case MotionDirection.Down:
                    {
                        SetVelosity(-1.0 - _MotionVelosity, _motionVelosityUnits);
                    } break;
                default:
                    break;
            }
        }

        void _MotionSingleMeasurementTimer_Tick(object sender, EventArgs e)
        {
            //_CurrentTime += _MotionSingleMeasurementTimer.Interval.Milliseconds;

            //var positionPerTick = _MotionSingleMeasurementTimer.Interval.Milliseconds / 1000;// *_metersPerSecond;

            //if (_CurrentPosition <= _FinalDestination)
            //{
            //    this.SetDirection(MotionDirection.Up);
            //    _CurrentPosition += positionPerTick;
            //    if (_CurrentPosition >= _FinalDestination)
            //        StopMotion();
            //}
            //else
            //{
            //    this.SetDirection(MotionDirection.Down);
            //    _CurrentPosition -= positionPerTick;
            //    if (_CurrentPosition <= _FinalDestination)
            //        StopMotion();
            //}

            //AllEventsHandler.Instance.OnMotion(null, new Motion_EventArgs(_CurrentPosition));
        }

        void _MotionRepettiiveMeasurementTimer_Tick(object sender, EventArgs e)
        {
            
        }

        public override void Dispose()
        {
            DisableDevice();
            base.Dispose();
        }
    }
}