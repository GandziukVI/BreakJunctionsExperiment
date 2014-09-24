﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Agilent_U2542A_ExtensionBox;

namespace BreakJunctions.Measurements
{
    class MeasureRealTimeTimeTrace : IDisposable
    {
        #region MeasureRealTimeTimeTrace settings

        private int _PointsPerBlock = 100;
        /// <summary>
        /// The value of points per block for Agilent U2542A
        /// </summary>
        public int PointsPerBlock
        {
            get { return _PointsPerBlock; }
            set { _PointsPerBlock = value; }
        }

        private int _AcquistionRate = 10000;
        /// <summary>
        /// The value of points per second, generated by
        /// simultaneous data acquisition Agilent U2542A
        /// </summary>
        public int AcquistionRate
        {
            get { return _AcquistionRate; }
            set { _AcquistionRate = value; }
        }

        private AnalogInputChannels _Channels;

        #endregion

        #region MeasureRealTimeTimeTrace functionality

        private void _initDAC()
        {

        }

        #endregion

        #region Constructor / Destructor

        

        #endregion

        #region Correctly disposing the instance

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
