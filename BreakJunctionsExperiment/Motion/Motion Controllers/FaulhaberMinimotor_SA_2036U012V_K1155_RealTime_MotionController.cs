using BreakJunctions.Events;
using FaulhaberMinimotors;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
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

        private string _COM_Port;
        private int _Baud;
        private Parity _Parity;
        private int _DataBits;
        private StopBits _StopBits;
        private string _ReturnToken;

        DateTime StartDateTime;
        DateTime CurrentDateTime;
        TimeSpan DateTimeDifference;

        string In_COM_Port = string.Empty;
        string COM_Data_Residue = string.Empty;

        private bool _IsMeasurementInProcess = false;

        #endregion

        #region Constructor

        private void _SetMotor()
        {
            _Motor = new FaulhaberMinimotor_SA_2036U012V_K1155(_COM_Port, _Baud, _Parity, _DataBits, _StopBits, _ReturnToken);
            InitDevice();
            _Motor.COM_Port.DataReceived += _COM_Device_DataReceived;
        }

        public FaulhaberMinimotor_SA_2036U012V_K1155_RealTime_MotionController(string comPort = "COM1", int baud = 115200, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One, string returnToken = ">")
        {
            _StringDataQueue = new ConcurrentQueue<string>();
            _COM_Data_TransformingAndSendingThread = new Thread(_TransformAndEmit_COM_Data);

            _COM_Port = comPort;
            _Baud = baud;
            _Parity = parity;
            _DataBits = dataBits;
            _StopBits = stopBits;
            _ReturnToken = returnToken;

            _SetMotor();

            AllEventsHandler.Instance.Motion_RealTime_StartPositionReached += OnMotion_RealTime_StartPositionReached;
            AllEventsHandler.Instance.Motion_RealTime_FinalDestinationReached += OnMotion_RealTime_FinalDestinationreached;

            AllEventsHandler.Instance.RealTime_TimeTraceMeasurementStateChanged += OnRealTime_TimeTraceMeasurement_StateChanged;
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

            //Moving motor to its start position
            _Motor.LoadAbsolutePosition(ConvertPotitionToMotorUnits(StartPosition));
            _Motor.NotifyPosition();
            _Motor.InitiateMotion();

            _IsStartPositionReached = false;
            _IsFinalDestinationReached = false;
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
            _Motor.DisableDevice();
        }

        public override void ContinueMotion()
        {
            _Motor.EnableDevice();
            _Motor.InitiateMotion();
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
            AllEventsHandler.Instance.Motion_RealTime_StartPositionReached -= OnMotion_RealTime_StartPositionReached;
            AllEventsHandler.Instance.Motion_RealTime_FinalDestinationReached -= OnMotion_RealTime_FinalDestinationreached;

            _Motor.DisableDevice();
            _Motor.Dispose();
        }

        #endregion

        #region COM data received

        private bool _IsStartPositionReached = false;
        private bool _IsFinalDestinationReached = false;

        private ConcurrentQueue<string> _StringDataQueue;
        private Thread _COM_Data_TransformingAndSendingThread;

        private void _TransformAndEmit_COM_Data()
        {
            // Handling the motion controller answer to give correct current
            // position value
            while (_IsMeasurementInProcess)
            {
                string responce;

                var _DequeueSuccess = _StringDataQueue.TryDequeue(out responce);

                if (_DequeueSuccess == true)
                {
                    CurrentDateTime = DateTime.Now;
                    DateTimeDifference = CurrentDateTime.Subtract(StartDateTime);

                    if (!responce.EndsWith("\n"))
                    {
                        In_COM_Port += responce.TrimEnd("\r\np".ToCharArray());
                    }
                    else
                    {
                        var splitData = responce.Split("\r\np".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (splitData.Length > 0)
                            In_COM_Port += splitData[0];

                        if (splitData.Length > 1)
                            COM_Data_Residue = splitData[1];
                        else
                            COM_Data_Residue = string.Empty;

                        if (IsMotionInProcess)
                        {
                            _Motor.SendCommandRequest("POS");

                            int intCurrentMotorPosition;
                            double CurrentMotorPosition;

                            bool success = int.TryParse(In_COM_Port.TrimEnd("\r\np".ToCharArray()), out intCurrentMotorPosition);

                            if (success)
                            {
                                CurrentMotorPosition = ConvertMotorUnitsIntoPosition(intCurrentMotorPosition);
                                AllEventsHandler.Instance.OnMotion_RealTime(this, new Motion_RealTime_EventArgs(DateTimeDifference.TotalSeconds, CurrentMotorPosition));
                            }
                        }

                        In_COM_Port = COM_Data_Residue;
                    }
                }
            }
        }

        /// <summary>
        /// Handling motor responce
        /// </summary>
        /// <param name="sender">SerialPort</param>
        /// <param name="e">SerialDataReceivedEventArgs</param>
        public void _COM_Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var motorPort = sender as SerialPort;
            var responce = motorPort.ReadExisting();

            _StringDataQueue.Enqueue(responce);

            if(!IsMotionInProcess)
                StartDateTime = DateTime.Now;

            // Motion (Single & Repetitive) implementation
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

        #region Motion start and end positions reached implementation

        private void OnMotion_RealTime_StartPositionReached(object sender, Motion_RealTime_StartPositionReached_EventArgs e)
        {
            IsMotionInProcess = true;

            switch (MotionKind)
            {
                case MotionKind.Single:
                    {
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
                        {
                            IsMotionInProcess = false;
                            AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(false));
                        }
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
                        AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(false));
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
                        {
                            IsMotionInProcess = false;
                            AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(false));
                        }
                    } break;
                default:
                    break;
            }
        }

        #endregion

        #region Real time measurement state changed implementation

        private void OnRealTime_TimeTraceMeasurement_StateChanged(object sender, RealTime_TimeTraceMeasurementStateChanged_EventArgs e)
        {
            _IsMeasurementInProcess = e.MeasurementInProcess;

            if(e.MeasurementInProcess == true)
            {
                ContinueMotion();
                _COM_Data_TransformingAndSendingThread = new Thread(_TransformAndEmit_COM_Data);
                _COM_Data_TransformingAndSendingThread.Priority = ThreadPriority.Normal;
                _COM_Data_TransformingAndSendingThread.Start();
                while (!_COM_Data_TransformingAndSendingThread.IsAlive) ;
            }
            else
            {
                StopMotion();
                var success = _COM_Data_TransformingAndSendingThread.Join(1000);
                if (!success)
                    _COM_Data_TransformingAndSendingThread.Abort();
            }
        }

        #endregion
    }
}
