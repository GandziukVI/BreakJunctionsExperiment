using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using FaulhaberMinimotors;
using BreakJunctions.Events;
using Devices.SMU;

namespace BreakJunctions.Motion
{
    public class FaulhaberMinimotor_SA_2036U012V_K1155_MotionController : MotionController
    {
        #region Motion settings

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

        #endregion

        #region Motor

        private FaulhaberMinimotor_SA_2036U012V_K1155 _Motor;
        /// <summary>
        /// Gets FaulhaberMinimotor_SA_2036U012V_K1155 motor
        /// </summary>
        public FaulhaberMinimotor_SA_2036U012V_K1155 Motor { get { return _Motor; } }

        public override bool InitDevice()
        {
            var isInitSuccess = _Motor.InitDevice();
            if (isInitSuccess == true)
                _Motor.EnableDevice();

            return isInitSuccess;
        }

        #endregion

        #region Constructor / Destructor

        public FaulhaberMinimotor_SA_2036U012V_K1155_MotionController(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            this._Motor = new FaulhaberMinimotor_SA_2036U012V_K1155(comPort, baud, parity, dataBits, stopBits, returnToken);

            InitDevice();

            this._Motor.COM_Port.DataReceived += _COM_Device_DataReceived;
            AllEventsHandler.Instance.TimeTraceBothChannelsPointsReceived += OnTimeTraceBothChannelsPointsReceived;
        }

        ~FaulhaberMinimotor_SA_2036U012V_K1155_MotionController()
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
        public void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motorPort = sender as SerialPort;
            var responce = motorPort.ReadExisting();

            //Checking, if the motor reached notified position
            if (responce.Contains('p'))
                AllEventsHandler.Instance.OnMotion(this, new Motion_EventArgs(CurrentPosition));
        }

        /// <summary>
        /// Going to the next motor position, if the data
        /// is measured
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimeTraceBothChannelsPointsReceived(object sender, TimeTraceBothChannelsPointsReceived_EventArgs e)
        {
            var positionIncrement = _MetersPerRevolution / PointsPerMilimeter * 2;

            switch (CurrentMotionKind)
            {
                case MotionKind.Single:
                    {
                        CurrentIteration = 0;
                        if ((CurrentPosition <= FinalDestination) && (IsMotionInProcess == true) && (CurrentDirection == MotionDirection.Up))
                        {
                            CurrentPosition += positionIncrement;
                            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(CurrentPosition));
                            _Motor.NotifyPosition();
                            _Motor.InitiateMotion();
                        }
                        else if ((CurrentPosition > FinalDestination) && (IsMotionInProcess == true) && (CurrentDirection == MotionDirection.Down))
                        {
                            CurrentPosition -= positionIncrement;
                            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(CurrentPosition));
                            _Motor.NotifyPosition();
                            _Motor.InitiateMotion();
                        }
                        else
                            StopMotion();
                    } break;
                case MotionKind.Repetitive:
                    {
                        //Checking if measurement is completed
                        if (CurrentIteration >= NumberOfRepetities * 2)
                            this.StopMotion();

                        if (IsMotionInProcess == true)
                        {

                            if (CurrentPosition >= FinalDestination - positionIncrement)
                            {
                                this.SetDirection(MotionDirection.Down);
                            }
                            else if (CurrentPosition <= StartPosition + positionIncrement)
                            {
                                this.SetDirection(MotionDirection.Up);
                            }

                            CurrentPosition += (CurrentDirection == MotionDirection.Up ? 1 : -1) * positionIncrement;

                            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(CurrentPosition));
                            _Motor.NotifyPosition();
                            _Motor.InitiateMotion();
                        }
                    } break;
                case MotionKind.FixedR:
                    {
                        var _localPointsPerMilimeter_LowerLimit = 1000.0;
                        var _localPointsPerMilimeter_UpperLimit = 100000.0;

                        switch (SelectedChannel_Val)
                        {
                            case Channels.ChannelA:
                                {
                                    var _Current_R_Val = 1.0 / (e.CH_01_Val * 0.0000774809173);

                                    var _valueInCompliance = ((_Current_R_Val - _Current_R_Val * AllowableDeviation_Val / 100.0) <= FixedR_Val &&
                                        FixedR_Val <= (_Current_R_Val + _Current_R_Val * AllowableDeviation_Val / 100.0)) ? true : false;
                                    
                                    if (_valueInCompliance == true)
                                        StopMotion();

                                    if (IsMotionInProcess == true)
                                    {
                                        if (e.CH_01_Val <= FixedR_Val)
                                            SetDirection(MotionDirection.Up);
                                        else
                                            SetDirection(MotionDirection.Down);
                                    }

                                    var _gradient = 0.0;
                                    if (e.CH_01_Val <= FixedR_Val)
                                        _gradient = 1.0 - (FixedR_Val - e.CH_01_Val) / FixedR_Val;

                                    var _localPointsPerMilimeter = _localPointsPerMilimeter_LowerLimit + ((_localPointsPerMilimeter_UpperLimit - _localPointsPerMilimeter_LowerLimit) * _gradient);

                                    var _localPositionIncrement = _MetersPerRevolution / _localPointsPerMilimeter * 2;

                                    CurrentPosition += (CurrentDirection == MotionDirection.Up ? 1 : -1) * positionIncrement;

                                    _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(CurrentPosition));
                                    _Motor.NotifyPosition();
                                    _Motor.InitiateMotion();
                                } break;
                            case Channels.ChannelB:
                                {
                                    var _Current_R_Val = 1.0 / (e.CH_02_Val * 0.0000774809173);

                                    var _valueInCompliance = ((_Current_R_Val - _Current_R_Val * AllowableDeviation_Val / 100.0) <= FixedR_Val &&
                                        FixedR_Val <= (_Current_R_Val + _Current_R_Val * AllowableDeviation_Val / 100.0)) ? true : false;
                                    
                                    if (_valueInCompliance == true)
                                        StopMotion();

                                    if (IsMotionInProcess == true)
                                    {
                                        if (e.CH_02_Val <= FixedR_Val)
                                            SetDirection(MotionDirection.Up);
                                        else
                                            SetDirection(MotionDirection.Down);
                                    }

                                    var _gradient = 0.0;
                                    if (e.CH_02_Val <= FixedR_Val)
                                        _gradient = 1.0 - (FixedR_Val - e.CH_02_Val) / FixedR_Val;

                                    var _localPointsPerMilimeter = _localPointsPerMilimeter_LowerLimit + ((_localPointsPerMilimeter_UpperLimit - _localPointsPerMilimeter_LowerLimit) * _gradient);

                                    var _localPositionIncrement = _MetersPerRevolution / _localPointsPerMilimeter * 2;

                                    CurrentPosition += (CurrentDirection == MotionDirection.Up ? 1 : -1) * positionIncrement;

                                    _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(CurrentPosition));
                                    _Motor.NotifyPosition();
                                    _Motor.InitiateMotion();
                                } break;
                            default:
                                break;
                        }
                    } break;
                default:
                    break;
            }
        }

        #endregion

        #region Motion functionality implementation

        public override void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, int numberOfRepetities = 1)
        {
            //Setting motion parameters
            this.StartPosition = StartPosition;
            this.CurrentPosition = StartPosition;
            this.FinalDestination = FinalDestination;
            this.NumberOfRepetities = numberOfRepetities;
            this.motionVelosityUnits = motionVelosityUnits;
            this.CurrentMotionKind = motionKind;

            if (this.StartPosition <= this.FinalDestination)
                SetDirection(MotionDirection.Up);
            else SetDirection(MotionDirection.Down);

            this.IsMotionInProcess = true;

            //Going to the start position
            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(this.StartPosition));
            _Motor.NotifyPosition();
            _Motor.InitiateMotion();
        }

        public override void StartMotion(TimeSpan FinalTime)
        {
            throw new NotImplementedException();
        }

        public override void StartMotion(double _StartPosition, double FixedR, double AllowableDeviation, Channels SelectedChannel)
        {
            FixedR_Val = FixedR;
            AllowableDeviation_Val = AllowableDeviation;
            SelectedChannel_Val = SelectedChannel;
            
            StartPosition = _StartPosition;
            CurrentPosition = _StartPosition;

            _Motor.EnableDevice();
            //Going to the start position
            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(this.StartPosition));
            _Motor.NotifyPosition();
            _Motor.InitiateMotion();
        }

        public override void MoveToZeroPosition()
        {
            _Motor.LoadAbsolutePosition(0);
            _Motor.InitiateMotion();
        }

        public override void StopMotion()
        {
            IsMotionInProcess = false;
            _Motor.DisableDevice();
            //Signal that the motion is completed
            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(false));
        }

        public override void ContinueMotion()
        {
            _Motor.EnableDevice();
            _Motor.InitiateMotion();
        }

        public override void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits)
        {
            switch (VelosityUnits)
            {
                case MotionVelosityUnits.rpm:
                    {
                        _Motor.SpeedRPM = Convert.ToInt32(VelosityValue * _Motor.GearFactor);
                    } break;
                case MotionVelosityUnits.MilimetersPerMinute:
                    {
                        _Motor.SpeedRPM = Convert.ToInt32(2 * VelosityValue * _Motor.GearFactor);
                    } break;
                default:
                    break;
            }
        }

        public override void SetDirection(MotionDirection motionDirection)
        {
            if (CurrentDirection != motionDirection)
            {
                CurrentDirection = motionDirection;
                ++CurrentIteration;
                AllEventsHandler.Instance.OnMotionDirectionChanged(this, new MotionDirectionChanged_EventArgs(motionDirection));
            }
        }

        public override double GetCurrentPosition()
        {
            return ConvertMotorUnitsIntoPosition(_Motor.GetPosition());
        }

        #endregion

        #region Disposing the instance

        public override void Dispose()
        {
            AllEventsHandler.Instance.TimeTraceBothChannelsPointsReceived -= OnTimeTraceBothChannelsPointsReceived;

            if (_Motor != null)
            {
                _Motor.DisableDevice();
                _Motor.Dispose();
            }
        }

        #endregion

        public override void EnableDevice()
        {
            _Motor.EnableDevice();
        }

        public override void DisableDevice()
        {
            _Motor.DisableDevice();
        }
    }
}
