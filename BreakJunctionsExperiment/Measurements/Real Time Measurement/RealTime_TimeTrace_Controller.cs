using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using BreakJunctions.Plotting;

namespace BreakJunctions.Measurements
{
    public abstract class RealTime_TimeTrace_Controller : IDisposable
    {
        public bool MeasurementInProcess { get; set; }
        /// <summary>
        /// Realizes the continious time trace acquisition
        /// </summary>
        public abstract void ContiniousAcquisition();
        /// <summary>
        /// Realizes the continious time trace acquisition
        /// with precise voltage measurement
        /// </summary>
        public abstract void ContiniousAcquisitionWithPresiseVoltageMeasurement();
        /// <summary>
        /// Realizes single shot measurements
        /// </summary>
        /// <param name="NumberOfChannel">The number of channel to be measured</param>
        /// <returns></returns>
        public abstract List<Point> MakeSingleShot(int NumberOfChannel);
        /// <summary>
        /// Alternating current autorange in
        /// appropriate channel
        /// </summary>
        /// <param name="NumberOfChannel">Number of channel</param>
        public abstract void startAC_Autorange(int NumberOfChannel);
        /// <summary>
        /// Correctly disposing the instance
        /// </summary>
        public abstract void Dispose();
    }
}
