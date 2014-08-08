using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hardware
{
    public enum MotionKind { Single, Repetitive }
    public enum MotionDirection { Up, Down }
    public enum MotionVelosityUnits { StepsPerMinute, MilimetersPerMinute }

    interface IMotion
    {
        /// <summary>
        /// Initializes the device
        /// </summary>
        /// <returns>True, if initialization succeed</returns>
        bool InitDevice();
        /// <summary>
        /// Startts the motion from defined position to final
        /// destination with appropriate motion kind
        /// </summary>
        /// <param name="StartPosition">Start position</param>
        /// <param name="FinalDestination">Final destination</param>
        /// <param name="motionKind">Motion kind (Single or Repettitive)</param>
        void StartMotion(double StartPosition, double FinalDestination, MotionKind motionKind, double motionVelosity = 100.0, MotionVelosityUnits motionVelosityUnits = MotionVelosityUnits.rpm, int numberOfRepetities = 1);
        /// <summary>
        /// Starts the motion in response to maximum
        /// observation time
        /// </summary>
        /// <param name="FinalTime"></param>
        void StartMotion(TimeSpan FinalTime);
        /// <summary>
        /// Starts motion with constant velosity
        /// untill needed position is riched
        /// </summary>
        /// <param name="FixedR"></param>
        void StartMotion(double FixedR);
        /// <summary>
        /// Stopes the motion ofthe motor
        /// </summary>
        void StopMotion();
        /// <summary>
        /// Sets the velosity of motion
        /// </summary>
        /// <param name="VelosityValue">The value of velosity in appropriate units</param>
        /// <param name="VelosityUnits">Units of the velosity</param>
        void SetVelosity(double VelosityValue, MotionVelosityUnits VelosityUnits);
        /// <summary>
        /// Sets the direction of motion
        /// </summary>
        /// <param name="motionDirection">The direction of motion</param>
        void SetDirection(MotionDirection motionDirection);
    }
}
