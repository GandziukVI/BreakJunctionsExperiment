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
    class FAULHABER_MINIMOTOR_SA : COM_Device, IMotion
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

        private int _IncPerRevolution = 3000;
        public int IncPerRevolution
        {
            get { return _IncPerRevolution; }
            set { _IncPerRevolution = value; }
        }

        private int _TransferFactor = 1526;
        public int TransferFactor
        {
            get { return _TransferFactor; }
            set { _TransferFactor = value; }
        }

        private int _ValuePerRevolution = 4578000;
        /// <summary>
        /// Gets ValuePerRevolution value.
        /// For correct work, specify IncPerRevolution and TransferFactor values!
        /// </summary>
        public int ValuePerRevolution
        {
            get { return _IncPerRevolution * _TransferFactor; }
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
            return Convert.ToInt32(_ValuePerRevolution * _position / _MetersPerRevolution);
        }

        #endregion

        #region Motor device initialization

        public FAULHABER_MINIMOTOR_SA(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
            : base (comPort, baud, parity, dataBits, stopBits, returnToken)
        {           
            this.InitDevice();

            AllEventsHandler.Instance.TimeTraceBothChannelsPointsReceived += OnTimeTraceBothChannelsPointsReceived;
        }

        ~FAULHABER_MINIMOTOR_SA()
        {
            this.Dispose();
        }

        #endregion

        #region Motor answer received

        /// <summary>
        /// Handling motor responce
        /// </summary>
        /// <param name="sender">SerialPort</param>
        /// <param name="e">SerialDataReceivedEventArgs</param>
        public override void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motorPort = sender as SerialPort;
            var responce = motorPort.ReadExisting();

            if (responce.Contains('p'))
                AllEventsHandler.Instance.OnMotion(null, new Motion_EventArgs(_CurrentPosition));
        }

        /// <summary>
        /// Going to the next motor position, if the data
        /// is measured
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeTraceBothChannelsPointsReceived(object sender, TimeTraceBothChannelsPointsReceived_EventArgs e)
        {
            var positionIncrement = _MetersPerRevolution / _NotificationsPerRevolution;

            switch (_MotionKind)
            {
                case MotionKind.Single:
                    {
                        if ((_CurrentPosition <= _FinalDestination) && (_IsMotionInProcess == true) && (_CurrentDirection == MotionDirection.Up))
                        {
                            _CurrentPosition += _MetersPerRevolution / _NotificationsPerRevolution;
                            LoadAbsolutePosition(ConvertPotitionToMotorUnits(_CurrentPosition));
                            NotifyPosition();
                            InitiateMotion();
                        }
                        else if ((_CurrentPosition > _FinalDestination) && (_IsMotionInProcess == true) && (_CurrentDirection == MotionDirection.Down))
                        {
                            _CurrentPosition -= _MetersPerRevolution / _NotificationsPerRevolution;
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

                        if (_IsMotionInProcess == true)
                        {

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
                        }
                    } break;
                default:
                    break;
            }
        }

        #endregion

        #region Motor controlling functions

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

        /// <summary>
        /// Starts the motion of specified kind, velosity and number of repetities
        /// </summary>
        /// <param name="StartPosition">Position whict the motor must reach before
        /// starting measurement [m]</param>
        /// <param name="FinalDestination">Final destination of the motor [m]</param>
        /// <param name="motionKind">Single or repetitive motion</param>
        /// <param name="motionVelosity">Value of motion velosity</param>
        /// <param name="motionVelosityUnits">Motion velosity units</param>
        /// <param name="numberOfRepetities">Number of repetities (for repetitive measurement)</param>
        public void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, int numberOfRepetities = 1)
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
            LoadAbsolutePosition(ConvertPotitionToMotorUnits(_StartPosition));
            NotifyPosition();
            InitiateMotion();
        }
        /// <summary>
        /// Starts the motion with constant velosity and
        /// specified time of motion
        /// </summary>
        /// <param name="FinalTime">Time of moving</param>
        public void StartMotion(TimeSpan FinalTime)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Starts the motion and going on, until the specified value
        /// of resistance is reached, then stops motor and gathering time trace
        /// </summary>
        /// <param name="FixedR">Value of resistance</param>
        public void StartMotion(double FixedR)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Initiates the motion to zero position
        /// </summary>
        public void MoveToZeroPosition()
        {
            LoadAbsolutePosition(0);
            InitiateMotion();
        }
        /// <summary>
        /// Stops the motion
        /// </summary>
        public void StopMotion()
        {
            _IsMotionInProcess = false;
            //Signal that the motion is completed
            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(null, new TimeTraceMeasurementStateChanged_EventArgs(false));
        }
        /// <summary>
        /// Sets the velosity value in responce to
        /// velosity units. Works only for the velosity mode!
        /// </summary>
        /// <param name="VelosityValue">Velosity value</param>
        /// <param name="VelosityUnits">Velosity units (rpm, MetersPerMinute)</param>
        public void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits)
        {
            switch (VelosityUnits)
            {
                case MotionVelosityUnits.rpm:
                    {
                        SendCommandRequest(String.Format("V{0}", Convert.ToInt32(Math.Round(VelosityValue))));
                    } break;
                case MotionVelosityUnits.MetersPerMinute:
                    {
                        var RevolutionPerMinute = 0.0005; //Meters per one revolution
                        var _NewVelosity = Convert.ToInt32(VelosityValue / RevolutionPerMinute);

                        SendCommandRequest(String.Format("V{0}", _NewVelosity));
                    } break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Sets the direction of motor moving
        /// </summary>
        /// <param name="motionDirection">Direction type</param>
        public void SetDirection(MotionDirection motionDirection)
        {
            if (_CurrentDirection != motionDirection)
            {
                _CurrentDirection = motionDirection;
                ++_CurrentIteration;
            }
        }

        #endregion

        #region Disposing of the instance

        /// <summary>
        /// Correctly disposing of the instance
        /// </summary>
        public override void Dispose()
        {
            AllEventsHandler.Instance.TimeTraceBothChannelsPointsReceived -= OnTimeTraceBothChannelsPointsReceived;

            DisableDevice();
            base.Dispose();
        }

        #endregion
    }
}
