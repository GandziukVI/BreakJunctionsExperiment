using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.IO.Ports;

using System.Threading;
using System.Windows;
using System.Windows.Threading;

using BreakJunctions.Events;
using Hardware;

namespace Hardware
{
    class FAULHABER_MINIMOTOR_SA : COM_Device, IMotion, IDisposable
    {
        #region Motion settings

        private bool _IsMotionInProcess = false;

        private double _MetersPerRevolution = 0.0005;
        public double MetersPerRevolution
        {
            get { return _MetersPerRevolution; }
            set { _MetersPerRevolution = value; }
        }

        private int _IncPerRevolution = 4576118;  //Not exactly! Calibration needed!!!
                                                  //3000 in documentation. WTF?
        public int IncPerRevolution
        {
            get { return _IncPerRevolution; }
            set { _IncPerRevolution = value; }
        }

        private int _NotificationsPerRevolution = 100; //2000 is OK
        public int NitofocationsPerRevolution
        {
            get { return _NotificationsPerRevolution; }
            set { _NotificationsPerRevolution = value; }
        }

        private double _CurrentPosition = 0.0;
        /// <summary>
        /// Gets or sets current micrometric bolt
        /// position in meters
        /// </summary>
        public double CurrentPosition
        {
            get { return _CurrentPosition; }
            set { _CurrentPosition = value; }
        }

        private double _StartPosition = 0.0;
        /// <summary>
        /// Gets or sets start micrometric bolt
        /// position in meters
        /// </summary>
        public double StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }

        private double _FinalDestination = 0.0;
        /// <summary>
        /// Gets or sets final micrometric bolt
        /// position in meters
        /// </summary>
        public double FinalDestination
        {
            get { return _FinalDestination; }
            set { _FinalDestination = value; }
        }

        private int _CurrentIteration = 0;

        private int _NumberRepetities = 0;
        /// <summary>
        /// Gets or sets number of repetities
        /// for repetitive measurement
        /// </summary>
        public int NumberRepetities
        {
            get { return _NumberRepetities; }
            set { _NumberRepetities = value; }
        }

        private MotionVelosityUnits _motionVelosityUnits = MotionVelosityUnits.rpm;
        public MotionVelosityUnits motionVelosityUnits
        {
            get { return _motionVelosityUnits; }
            set { _motionVelosityUnits = value; }
        }

        private double _VelosityValue = 0.0;
        /// <summary>
        /// Gets or sets velosity value. Can only be used in velosity mode!
        /// For correct work motionVelosityUnits must be set
        /// </summary>
        public double VelosityValue
        {
            get { return _VelosityValue; }
            set { _VelosityValue = value; }
        }

        private MotionDirection _CurrentDirection;

        private MotionKind _MotionKind = MotionKind.Single;
        public MotionKind MotionKind
        {
            get { return _MotionKind; }
            set { _MotionKind = value; }
        }

        private int ConvertPotitionToMotorUnits(double _position)
        {
            return Convert.ToInt32(_IncPerRevolution * _position / _MetersPerRevolution);
        }

        #endregion

        #region Motor device initialization

        public FAULHABER_MINIMOTOR_SA(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
            : base (comPort, baud, parity, dataBits, stopBits, returnToken)
        {           
            this.InitDevice();

            AllEventsHandler.Instance.TimetracePointReceived += OnTimeTracePointReceived;
        }

        ~FAULHABER_MINIMOTOR_SA()
        {
            this.Dispose();
        }

        #endregion

        #region Motor answer received

        public override void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motorPort = sender as SerialPort;
            var responce = motorPort.ReadExisting();

            if (responce.Contains('p'))
                AllEventsHandler.Instance.OnMotion(null, new Motion_EventArgs(_CurrentPosition));
        }

        private void OnTimeTracePointReceived(object sender, TimeTracePointReceived_EventArgs e)
        {
            var positionIncrement = _MetersPerRevolution / _NotificationsPerRevolution;

            switch (_MotionKind)
            {
                case MotionKind.Single:
                    {
                        if ((_CurrentPosition <= _FinalDestination) && (_IsMotionInProcess == true))
                        {
                            _CurrentPosition += _MetersPerRevolution / _NotificationsPerRevolution;
                            LoadAbsolutePosition(ConvertPotitionToMotorUnits(_CurrentPosition));
                            NotifyPosition();
                            InitiateMotion();
                        }
                        else StopMotion();
                    } break;
                case MotionKind.Repetitive:
                    {
                        //Checking if measurement is completed
                        if (_CurrentIteration >= _NumberRepetities)
                            this.StopMotion();

                        if (_CurrentPosition >= _FinalDestination - positionIncrement)
                        {
                            this.SetDirection(MotionDirection.Down);
                        }
                        else if (_CurrentPosition <= _StartPosition + positionIncrement)
                        {
                            this.SetDirection(MotionDirection.Up);
                        }

                        _CurrentPosition += (_CurrentDirection == MotionDirection.Up ? 1 : -1) * positionIncrement;

                        LoadAbsolutePosition(ConvertPotitionToMotorUnits(_CurrentPosition));
                        NotifyPosition();
                        InitiateMotion();
                    } break;
                default:
                    break;
            }
        }

        #endregion

        #region Motor controlling functions

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

        #endregion

        #region Motion controlling functions

        public void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, double motionVelosity = 100.0, MotionVelosityUnits motionVelosityUnits = MotionVelosityUnits.rpm, int numberOfRepetities = 1)
        {
            _StartPosition = StartPosition;
            _CurrentPosition = 0.0;
            _FinalDestination = FinalDestination;
            _NumberRepetities = numberOfRepetities;
            _VelosityValue = motionVelosity;
            _motionVelosityUnits = motionVelosityUnits;
            _MotionKind = motionKind;

            _IsMotionInProcess = true;

            LoadAbsolutePosition(ConvertPotitionToMotorUnits(_StartPosition));
            NotifyPosition();
            InitiateMotion();
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
            _IsMotionInProcess = false;
            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(null, new TimeTraceMeasurementStateChanged_EventArgs(false));
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
        }

        #endregion

        public override void Dispose()
        {
            AllEventsHandler.Instance.TimetracePointReceived -= OnTimeTracePointReceived;

            DisableDevice();
            base.Dispose();
        }
    }
}
