using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Motion
{
    public abstract class MotionController : IDisposable
    {
        private int _NotificationsPerMilimeter = 10000;
        /// <summary>
        /// The number of notifications per milimeter
        /// </summary>
        public int NotificationsPerMilimeter
        {
            get { return _NotificationsPerMilimeter; }
            set { _NotificationsPerMilimeter = value; }
        }
        /// <summary>
        /// Initializes the device
        /// </summary>
        /// <returns>True, if initialization succeed</returns>
        abstract public bool InitDevice();
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
        abstract public void StartMotion(double FixedR);
        /// <summary>
        /// Initiates the motion to zero position
        /// </summary>
        abstract public void MoveToZeroPosition();
        /// <summary>
        /// Stops the motion ofthe motor
        /// </summary>
        abstract public void StopMotion();
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
