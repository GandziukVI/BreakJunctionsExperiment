using BreakJunctions.Plotting;
using Devices.SMU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Motion
{
    public abstract class MotionController : IDisposable
    {
        private bool _IsFirstChannelCompletelyBroken = false;
        public bool IsFirstChannelCompletelyBroken
        {
            get { return _IsFirstChannelCompletelyBroken; }
            set { _IsFirstChannelCompletelyBroken = value; }
        }

        private bool _IsSecondChannelCompletelyBroken = false;
        public bool IsSecondChannelCompletelyBroken
        {
            get { return _IsSecondChannelCompletelyBroken; }
            set { _IsSecondChannelCompletelyBroken = value; }
        }

        private bool _NormalMode = false;
        /// <summary>
        /// Aquiring both breaking and closing curves in full range od distance
        /// </summary>
        public bool NormalMode
        {
            get { return _NormalMode; }
            set
            {
                _NormalMode = value;
                if (value)
                {
                    _EliminateClosing = false;
                    _SmartMode = false;
                }
            }
        }

        private bool _EliminateClosing = false;
        /// <summary>
        /// Not measuring closing curves
        /// </summary>
        public bool EliminateClosing
        {
            get { return _EliminateClosing; }
            set
            {
                _EliminateClosing = value;
                if (value)
                {
                    _NormalMode = false;
                    _SmartMode = false;
                }
            }
        }

        private bool _SmartMode = true;
        /// <summary>
        /// Breaking and closing curves will be acquired in the dynamic range
        /// which is specified by the conductance of the structure
        /// </summary>
        public bool SmartMode
        {
            get { return _SmartMode; }
            set
            {
                _SmartMode = value;
                if (value)
                {
                    _NormalMode = false;
                    _EliminateClosing = false;
                }
            }
        }

        private double _OpenedJunctionConductance_CH_01 = 0.00001;
        public double OpenedJunctionConductance_CH_01
        {
            get { return _OpenedJunctionConductance_CH_01; }
            set { _OpenedJunctionConductance_CH_01 = value; }
        }

        private double _OpenedJunctionConductance_CH_02 = 0.00001;
        public double OpenedJunctionConductance_CH_02
        {
            get { return _OpenedJunctionConductance_CH_02; }
            set { _OpenedJunctionConductance_CH_02 = value; }
        }

        private double _ClosedJunctionConductance = 10.0;
        public double ClosedJunctionConductance
        {
            get { return _ClosedJunctionConductance; }
            set { _ClosedJunctionConductance = value; }
        }

        private int _ConsiderUsingLast = 100;
        public int ConsiderUsingLast
        {
            get { return _ConsiderUsingLast; }
            set { _ConsiderUsingLast = value; }
        }

        public Nullable<bool> Channel_01_Broken { get; protected set; }
        public Nullable<bool> Channel_02_Broken { get; protected set; }

        public LinkedList<double> Channel_01_LastValues { get; protected set; }
        public LinkedList<double> Channel_02_LastValues { get; protected set; }

        public double MotionMin_Position { get; protected set; }
        public double MotionMax_Position { get; protected set; }

        /// <summary>
        /// Gets or sets the motion state
        /// </summary>
        public bool IsMotionInProcess { get; set; }

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
        public int CurrentIteration
        {
            get { return _CurrentIteration; }
            set { _CurrentIteration = value; }
        }

        private int _NumberRepetities = 0;
        /// <summary>
        /// Gets or sets number of repetities
        /// for repetitive measurement
        /// </summary>
        public int NumberOfRepetities
        {
            get { return _NumberRepetities; }
            set { _NumberRepetities = value; }
        }

        private MotionDirection _CurrentDirection;
        public MotionDirection CurrentDirection
        {
            get { return _CurrentDirection; }
            set { _CurrentDirection = value; }
        }

        private MotionKind _MotionKind = MotionKind.Single;
        /// <summary>
        /// Gets or sets motion kind (Single/Repetitive)
        /// </summary>
        public MotionKind CurrentMotionKind
        {
            get { return _MotionKind; }
            set { _MotionKind = value; }
        }

        private double _VelosityMovingUp = 2.5;
        public double VelosityMovingUp
        {
            get { return _VelosityMovingUp; }
            set { _VelosityMovingUp = value; }
        }

        private double _VelosityMovingDown = 3.0;
        public double VelosityMovingDown
        {
            get { return _VelosityMovingDown; }
            set { _VelosityMovingDown = value; }
        }

        /// <summary>
        /// Gets or sets motion velosity units
        /// </summary>
        public MotionVelosityUnits VelosityUnits { get; set; }

        private int _PointsPerMilimeter = 10000;
        /// <summary>
        /// The number of notifications per milimeter
        /// </summary>
        public int PointsPerMilimeter
        {
            get { return _PointsPerMilimeter; }
            set { _PointsPerMilimeter = value; }
        }

        public double FixedR_Val { get; set; }

        public double AllowableDeviation_Val { get; set; }

        public ChannelsToInvestigate SelectedChannel_Val { get; set; }

        /// <summary>
        /// Initializes the device
        /// </summary>
        /// <returns>True, if initialization succeed</returns>
        abstract public bool InitDevice();

        abstract public void EnableDevice();

        abstract public void DisableDevice();

        /// <summary>
        /// Startts the motion from defined position to final
        /// destination with appropriate motion kind
        /// </summary>
        /// <param name="StartPosition">Start position</param>
        /// <param name="FinalDestination">Final destination</param>
        /// <param name="motionKind">Motion kind (Single or Repettitive)</param>
        abstract public void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, int numberOfRepetities = 1);
        /// <summary>
        /// Starts the motion in response to maximum
        /// observation time
        /// </summary>
        /// <param name="FinalTime"></param>
        abstract public void StartMotion(TimeSpan FinalTime);
        /// <summary>
        /// Starts motion with constant velosity
        /// untill needed position is riched
        /// </summary>
        /// <param name="FixedR"></param>
        abstract public void StartMotion(double _StartPosition, double FixedR, double AllowableDeviation, ChannelsToInvestigate SelectedChannel);
        /// <summary>
        /// Initiates the motion to zero position
        /// </summary>
        abstract public void MoveToZeroPosition();
        /// <summary>
        /// Stops the motion ofthe motor
        /// </summary>
        abstract public void StopMotion();
        /// <summary>
        /// Continues the motion
        /// </summary>
        abstract public void ContinueMotion();
        /// <summary>
        /// Sets the velosity of motion
        /// </summary>
        /// <param name="VelosityValue">The value of velosity in appropriate units</param>
        /// <param name="VelosityUnits">Units of the velosity</param>
        abstract public void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits);
        /// <summary>
        /// Sets the direction of motion
        /// </summary>
        /// <param name="motionDirection">The direction of motion</param>
        abstract public void SetDirection(MotionDirection motionDirection);
        /// <summary>
        /// Gets the current position of motor in meters
        /// </summary>
        /// <returns></returns>
        abstract public double GetCurrentPosition();
        /// <summary>
        /// Correctly disposing the instance
        /// </summary>
        abstract public void Dispose();
    }
}
