﻿using BreakJunctions.Events;
using FaulhaberMinimotors;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BreakJunctions.Motion
{
    class FaulhaberMinimotor_SA_2036U012V_K1155_RealTime_MotionController : MotionController
    {
        #region FaulhaberMinimotor_SA_2036U012V_K1155_RealTime_MotionController settings

        private double _MetersPerRevolution = 0.0005;
        /// <summary>
        /// Gets or sets the value of meters per revolution value
        /// </summary>
        public double MetersPerRevolution
        {
            get { return _MetersPerRevolution; }
            set { _MetersPerRevolution = value; }
        }

        /// <summary>
        /// Converts motor position from meters to
        /// motor-specified value
        /// </summary>
        /// <param name="_position">Position [m]</param>
        /// <returns></returns>
        private int ConvertPotitionToMotorUnits(double _position)
        {
            return Convert.ToInt32(_Motor.ValuePerRevolution * _position / _MetersPerRevolution);
        }

        private double ConvertMotorUnitsIntoPosition(int MotorUnitsPosition)
        {
            return Convert.ToDouble(MotorUnitsPosition * _MetersPerRevolution / _Motor.ValuePerRevolution);
        }

        private MotionVelosityUnits _motionVelosityUnits = MotionVelosityUnits.rpm;
        /// <summary>
        /// Gets or sets motion velosity units
        /// </summary>
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

        private FaulhaberMinimotor_SA_2036U012V_K1155 _Motor;
        /// <summary>
        /// Gets FaulhaberMinimotor_SA_2036U012V_K1155 motor
        /// </summary>
        public FaulhaberMinimotor_SA_2036U012V_K1155 Motor
        {
            get { return _Motor; }
        }

        private ThreadStart MotorPositionMeasurementStartInfo;
        private Thread MotorPositionMeasurementThread;

        #endregion

        #region Constructor

        public FaulhaberMinimotor_SA_2036U012V_K1155_RealTime_MotionController(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            _Motor = new FaulhaberMinimotor_SA_2036U012V_K1155(comPort, baud, parity, dataBits, stopBits, returnToken);

            InitDevice();

            _Motor.COM_Port.DataReceived += _COM_Device_DataReceived;

            AllEventsHandler.Instance.Motion_RealTime_StartPositionReached += OnMotion_RealTime_StartPositionReached;
            AllEventsHandler.Instance.Motion_RealTime_FinalDestinationReached += OnMotion_RealTime_FinalDestinationreached;
        }

        #endregion

        #region MotionController implementation

        public override bool InitDevice()
        {
            var isInitSuccess = _Motor.InitDevice();
            if (isInitSuccess == true)
                _Motor.EnableDevice();

            return isInitSuccess;
        }

        public override void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, int numberOfRepetities = 1)
        {
            CurrentIteration = 0;
            NumberRepetities = numberOfRepetities;

            MotorPositionMeasurementStartInfo = new ThreadStart(MeasureMotorPositionInThread);
            MotorPositionMeasurementThread = new Thread(MotorPositionMeasurementStartInfo);

            //Moving motor to its start position
            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(StartPosition));
            _Motor.NotifyPosition();
            _Motor.InitiateMotion();

            _IsStartPositionReached = false;
            _IsFinalDestinationReached = false;

            //Sending information about current motor position
            while (!IsMotionInProcess) ;
            MotorPositionMeasurementThread.Priority = ThreadPriority.AboveNormal;
            MotorPositionMeasurementThread.Start();
        }

        public override void StartMotion(TimeSpan FinalTime)
        {
            throw new NotImplementedException();
        }

        public override void StartMotion(double FixedR)
        {
            throw new NotImplementedException();
        }

        public override void MoveToZeroPosition()
        {
            throw new NotImplementedException();
        }

        public override void StopMotion()
        {
            throw new NotImplementedException();
        }

        public override void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits)
        {
            throw new NotImplementedException();
        }

        public override void SetDirection(MotionDirection motionDirection)
        {
            throw new NotImplementedException();
        }

        public override double GetCurrentPosition()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            _Motor.COM_Port.DataReceived -= _COM_Device_DataReceived;

            AllEventsHandler.Instance.Motion_RealTime_StartPositionReached -= OnMotion_RealTime_StartPositionReached;
            AllEventsHandler.Instance.Motion_RealTime_FinalDestinationReached -= OnMotion_RealTime_FinalDestinationreached;

            _Motor.DisableDevice();
            _Motor.Dispose();

            MotorPositionMeasurementThread.Abort();
        }

        #endregion

        #region COM data received

        private bool _IsStartPositionReached = false;
        private bool _IsFinalDestinationReached = false;

        /// <summary>
        /// Handling motor responce
        /// </summary>
        /// <param name="sender">SerialPort</param>
        /// <param name="e">SerialDataReceivedEventArgs</param>
        public void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motorPort = sender as SerialPort;
            var responce = motorPort.ReadExisting();

            if (responce.Contains('p'))
            {
                if (_IsStartPositionReached == false && _IsFinalDestinationReached == false)
                {
                    _IsStartPositionReached = true;

                    AllEventsHandler.Instance.OnMotion_RealTime_StartPositionReached(this, new Motion_RealTime_StartPositionReached_EventArgs());
                }
                else if (_IsStartPositionReached == false && _IsFinalDestinationReached == true)
                {
                    _IsStartPositionReached = true;
                    _IsFinalDestinationReached = false;

                    AllEventsHandler.Instance.OnMotion_RealTime_StartPositionReached(this, new Motion_RealTime_StartPositionReached_EventArgs());
                }
                else if (_IsStartPositionReached == true && _IsFinalDestinationReached == false)
                {
                    _IsStartPositionReached = false;
                    _IsFinalDestinationReached = true;

                    AllEventsHandler.Instance.OnMotion_RealTime_FinalDestinationReached(this, new Motion_RealTime_FinalDestinationReached_EventArgs());
                }
            }
        }

        #endregion

        #region Motor position measure in thread implementtaion

        private void MeasureMotorPositionInThread()
        {
            DateTime StartDateTime = DateTime.Now;
            DateTime CurrentDateTime;
            TimeSpan DateTimeDifference;

            double CurrentMotorPosition;

            while(IsMotionInProcess)
            {
                CurrentMotorPosition = ConvertMotorUnitsIntoPosition(_Motor.GetPosition());
                CurrentDateTime = DateTime.Now;
                DateTimeDifference = CurrentDateTime.Subtract(StartDateTime);

                AllEventsHandler.Instance.OnMotion_RealTime(this, new Motion_RealTime_EventArgs(DateTimeDifference.TotalSeconds, CurrentPosition));
            }
        }

        #endregion

        private void OnMotion_RealTime_StartPositionReached(object sender, Motion_RealTime_StartPositionReached_EventArgs e)
        {
            switch (MotionKind)
            {
                case MotionKind.Single:
                    {
                        IsMotionInProcess = true;
                        _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(FinalDestination));
                        _Motor.NotifyPosition();
                        _Motor.InitiateMotion();
                    } break;
                case MotionKind.Repetitive:
                    {
                        if (CurrentIteration <= NumberRepetities)
                        {
                            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(FinalDestination));
                            _Motor.NotifyPosition();
                            _Motor.InitiateMotion();
                            ++CurrentIteration;
                        }
                        else
                            IsMotionInProcess = false;
                    } break;
                default:
                    break;
            }
        }

        private void OnMotion_RealTime_FinalDestinationreached(object sender, Motion_RealTime_FinalDestinationReached_EventArgs e)
        {
            switch (MotionKind)
            {
                case MotionKind.Single:
                    {
                        IsMotionInProcess = false;
                    } break;
                case MotionKind.Repetitive:
                    {
                        if (CurrentIteration <= NumberRepetities)
                        {
                            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(StartPosition));
                            _Motor.NotifyPosition();
                            _Motor.InitiateMotion();
                            ++CurrentIteration;
                        }
                        else
                            IsMotionInProcess = false;
                    } break;
                default:
                    break;
            }
        }
    }
}
