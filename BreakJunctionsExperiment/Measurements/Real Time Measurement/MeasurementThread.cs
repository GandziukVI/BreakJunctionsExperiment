using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BreakJunctions.Measurements
{
    class MeasurementThread
    {
        #region MeasurementThread settings

        public delegate void SomeMeasurement();
        public static bool MeasurementInProgress;

        private Thread _Measurement;
        
        public bool MeasurementRunning
        {
            get
            {
                if (_Measurement != null)
                {
                    if (_Measurement.IsAlive) 
                        return true;
                    else if (MeasurementInProgress) 
                        this.StopThread();
                }
                return (MeasurementInProgress);
            }
            set
            {
                if (MeasurementInProgress)
                {
                    if (value == false)
                        this.StopThread();
                }
            }
        }

        #endregion

        #region Singleton pattern implementation

        private static MeasurementThread _Instance;
        public static MeasurementThread Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new MeasurementThread();

                return _Instance;
            }
        }

        #endregion

        #region MeasurementThread functionality

        public void StartThread(SomeMeasurement DesiredFunction)
        {
            _Measurement = new Thread(new ThreadStart(DesiredFunction));
            _Measurement.Priority = ThreadPriority.Highest;
            _Measurement.IsBackground = true;
            MeasurementInProgress = true;
            _Measurement.Start();
        }

        public void StopThread()
        {
            MeasurementInProgress = false;
            bool result = _Measurement.Join(5000);
            if (!result) 
                _Measurement.Abort();
        }

        #endregion
    }
}
