using BreakJunctions.Events;
using BreakJunctions.Plotting;
using Devices;
using Devices.SMU;
using E_755_PI_Controller;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions.Motion
{
    class PI_E755_MotionController : MotionController
    {
        #region Constructor / Destructor

        public PI_E755_MotionController(string comPort = "COM1", int baud = 57600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            var _COM_Device = new COM_Device(comPort, baud, parity, dataBits, stopBits, returnToken) as IExperimentalDevice;
            _Motor = new E_755(ref _COM_Device);

            InitDevice();

            _Motor.COM_Device.COM_Port.DataReceived += COM_Port_DataReceived;
            AllEventsHandler.Instance.TimeTraceBothChannelsPointsReceived += Instance_TimeTraceBothChannelsPointsReceived;           
        }

        ~PI_E755_MotionController()
        {
            Dispose();
        }

        #endregion

        #region Motion settings

        private double _MinMotionLimit = -2500.0;
        public double MinMotionLimit
        {
            get { return _MinMotionLimit; }
            set { _MinMotionLimit = value; }
        }

        private double _MaxMotionLimit = 2500.0;
        public double MaxMotionLimit
        {
            get { return _MaxMotionLimit; }
            set { _MaxMotionLimit = value; }
        }

        private double ConvertPositionToMotorUnits(double __Position)
        {
            var result = __Position * 1000000.0 - 2500.0;

            if (result < _MinMotionLimit)
                result = _MinMotionLimit;
            else if (result > _MaxMotionLimit)
                result = _MaxMotionLimit;

            return result;
        }

        #endregion

        #region Motor

        private E_755 _Motor;
        public E_755 Motor { get { return _Motor; } }

        public override bool InitDevice()
        {
            var isInitSuccess = _Motor.TheDevice.InitDevice();
            if (isInitSuccess == true)
                _Motor.SetServoControlMode(AxisIdentifier._1, ServoControlModes.ON_CurrentPos);

            return isInitSuccess;
        }

        #endregion

        #region Motor answer received

        private void COM_Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motionPort = sender as SerialPort;
            var responce = motionPort.ReadExisting();

            //Checking, if the motor reached target position
            if (responce.Contains("1=1"))
                AllEventsHandler.Instance.OnMotion(this, new Motion_EventArgs(CurrentPosition));

            if(IsMotionInProcess)
                _Motor.GetOnTargetStatus(AxisIdentifier._1);
        }

        void Instance_TimeTraceBothChannelsPointsReceived(object sender, TimeTraceBothChannelsPointsReceived_EventArgs e)
        {
            var positionIncrement = 0.001 / PointsPerMilimeter;

            switch (CurrentMotionKind)
            {
                case MotionKind.Single:
                    {
                        if ((CurrentPosition <= FinalDestination) && (IsMotionInProcess == true) && (CurrentDirection == MotionDirection.Up))
                        {
                            CurrentPosition += positionIncrement;
                            _Motor.MoveAbsolute(AxisIdentifier._1, ConvertPositionToMotorUnits(CurrentPosition));
                        }
                        else if ((CurrentPosition > FinalDestination) && (IsMotionInProcess == true) && (CurrentDirection == MotionDirection.Down))
                        {
                            CurrentPosition -= positionIncrement;
                            _Motor.MoveAbsolute(AxisIdentifier._1, ConvertPositionToMotorUnits(CurrentPosition));
                        }
                        else
                            StopMotion();
                    } break;
                case MotionKind.Repetitive:
                    {
                        //Checking if measurement is completed
                        if (CurrentIteration >= NumberOfRepetities)
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

                            //CurrentPosition += (CurrentDirection == MotionDirection.Up ? 1 : -1) * positionIncrement;
                            if (CurrentDirection == MotionDirection.Up)
                                CurrentPosition += positionIncrement;
                            else
                                CurrentPosition = StartPosition;

                            _Motor.MoveAbsolute(AxisIdentifier._1, ConvertPositionToMotorUnits(CurrentPosition));
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
            this.CurrentMotionKind = motionKind;

            if (this.StartPosition <= this.FinalDestination)
                SetDirection(MotionDirection.Up);
            else SetDirection(MotionDirection.Down);

            this.IsMotionInProcess = true;

            //Going to the start position
            _Motor.MoveAbsolute(AxisIdentifier._1, ConvertPositionToMotorUnits(StartPosition));
            _Motor.GetOnTargetStatus(AxisIdentifier._1);
        }

        public override void StartMotion(TimeSpan FinalTime)
        {
            throw new NotImplementedException();
        }

        public override void StartMotion(double StartPosition, double FixedR, double AllowableDeviation, ChannelsToInvestigate SelectedChannel)
        {
            throw new NotImplementedException();
        }

        public override void MoveToZeroPosition()
        {
            _Motor.MoveAbsolute(AxisIdentifier._1, _MinMotionLimit);
        }

        public override void StopMotion()
        {
            _Motor.StopAllAxes();
            IsMotionInProcess = false;
            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(false));
        }

        public override void ContinueMotion()
        {
            throw new NotImplementedException();
        }

        public override void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits)
        {
            switch (VelosityUnits)
            {
                case MotionVelosityUnits.rpm:
                    throw new Exception("rpm mode is not supported for this kind of controller!");
                case MotionVelosityUnits.MilimetersPerMinute:
                    {
                        _Motor.SetVelosity(AxisIdentifier._1, 1000.0 / 60.0 * VelosityValue);
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
            throw new NotImplementedException();
        }

        private void TimeTraceMeasurementsStateChanged(object sender, TimeTraceMeasurementStateChanged_EventArgs e)
        {
            IsMotionInProcess = e.TimeTrace_MeasurementState;
        }

        #endregion

        #region Correctly disposing the instance

        public override void Dispose()
        {
            if (_Motor != null)
                _Motor.Dispose();

            AllEventsHandler.Instance.TimeTraceBothChannelsPointsReceived -= Instance_TimeTraceBothChannelsPointsReceived;
            AllEventsHandler.Instance.TimeTraceMeasurementsStateChanged -= TimeTraceMeasurementsStateChanged;
        }

        #endregion

        public override void EnableDevice()
        {
            throw new NotImplementedException();
        }

        public override void DisableDevice()
        {
            throw new NotImplementedException();
        }
    }
}
