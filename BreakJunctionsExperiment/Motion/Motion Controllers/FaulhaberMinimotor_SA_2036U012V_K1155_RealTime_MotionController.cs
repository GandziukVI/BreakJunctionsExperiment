using BreakJunctions.Events;
using FaulhaberMinimotors;
using Motion;
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

        private bool _IsMotionInProcess = false;
        /// <summary>
        /// Gets the motion state
        /// </summary>
        public bool IsMotionInProcess
        {
            get { return _IsMotionInProcess; }
        }

        private double _MetersPerRevolution = 0.0005;
        /// <summary>
        /// Gets or sets the value of meters per revolution value
        /// </summary>
        public double MetersPerRevolution
        {
            get { return _MetersPerRevolution; }
            set { _MetersPerRevolution = value; }
        }

        private double _CurrentPosition = 0.0;
        /// <summary>
        /// Gets or sets current micrometric bolt
        /// position in meters [m]
        /// </summary>
        public double CurrentPosition
        {
            get { return _CurrentPosition; }
            set { _CurrentPosition = value; }
        }

        private double _StartPosition = 0.0;
        /// <summary>
        /// Gets or sets start micrometric bolt
        /// position in meters [m]
        /// </summary>
        public double StartPosition
        {
            get { return _StartPosition; }
            set { _StartPosition = value; }
        }

        private double _FinalDestination = 0.0;
        /// <summary>
        /// Gets or sets final micrometric bolt
        /// position in meters [m]
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

        private MotionDirection _CurrentDirection;

        private MotionKind _MotionKind = MotionKind.Single;
        /// <summary>
        /// Gets or sets motion kind (Single/Repetitive)
        /// </summary>
        public MotionKind MotionKind
        {
            get { return _MotionKind; }
            set { _MotionKind = value; }
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
            var MotorPositionMeasurementStartInfo = new ThreadStart(MeasureMotorPositionInThread);
            var MotorPositionMeasurementThread = new Thread(MotorPositionMeasurementStartInfo);

            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(StartPosition));
            _Motor.NotifyPosition();
            _Motor.InitiateMotion();

            _IsStartPositionReached = false;
            _IsFinalDestinationReached = false;

            while (!_IsMotionInProcess) ;
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
            throw new NotImplementedException();
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

                    
                }
                else if (_IsStartPositionReached == false && _IsFinalDestinationReached == true)
                {
                    _IsStartPositionReached = true;
                    _IsFinalDestinationReached = false;

                    
                }
                else if (_IsStartPositionReached == true && _IsFinalDestinationReached == false)
                {
                    _IsStartPositionReached = false;
                    _IsFinalDestinationReached = true;
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

            while(_IsMotionInProcess)
            {
                CurrentMotorPosition = ConvertMotorUnitsIntoPosition(_Motor.GetPosition());
                CurrentDateTime = DateTime.Now;
                DateTimeDifference = CurrentDateTime.Subtract(StartDateTime);

                AllEventsHandler.Instance.OnMotion_RealTime(this, new Motion_RealTime_EventArgs(DateTimeDifference.TotalSeconds, CurrentPosition));
            }
        }

        #endregion
    }
}
