using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FaulhaberMinimotors;

namespace Motion
{
    public class FaulhaberMinimotor_SA_2036U012V_K1155_MotionController : MotionController
    {
        #region Motion settings

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

        private int _NotificationsPerRevolution = 1000; //The higher is the value, the slower is the motion
        /// <summary>
        /// Gets or sets the number of notifications
        /// per revolution
        /// </summary>
        public int NotificationsPerRevolution
        {
            get { return _NotificationsPerRevolution; }
            set { _NotificationsPerRevolution = value; }
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

        #endregion

        private FaulhaberMinimotor_SA_2036U012V_K1155 _Motor;

        public override bool InitDevice()
        {
            throw new NotImplementedException();
        }

        public override void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, int numberOfRepetities = 1)
        {
            //Setting motion parameters
            _StartPosition = StartPosition;
            _CurrentPosition = StartPosition;
            _FinalDestination = FinalDestination;
            _NumberRepetities = numberOfRepetities;
            _motionVelosityUnits = motionVelosityUnits;
            _MotionKind = motionKind;

            if (_StartPosition <= _FinalDestination)
                SetDirection(MotionDirection.Up);
            else SetDirection(MotionDirection.Down);

            _IsMotionInProcess = true;

            //Going to the start position
            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(_StartPosition));
            _Motor.NotifyPosition();
            _Motor.InitiateMotion();
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
            _Motor.LoadAbsolutePosition(0);
            _Motor.InitiateMotion();
        }

        public override void StopMotion()
        {
            _IsMotionInProcess = false;
        }

        public override void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits)
        {
            throw new NotImplementedException();
        }

        public override void SetDirection(MotionDirection motionDirection)
        {
            throw new NotImplementedException();
        }
    }
}
