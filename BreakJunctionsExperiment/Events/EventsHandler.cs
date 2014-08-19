using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class AllEventsHandler
    {
        #region Singleton class implementation

        private static AllEventsHandler _Instance;
        public static AllEventsHandler Instance
        {
            get
            {
                if (_Instance == null) _Instance = new AllEventsHandler();
                return _Instance;
            }
        }

        #endregion

        #region I-V Curve Measurements Events

        #region I-V measurements state changed

        private readonly object IV_MeasurementsStateChanged_EventLock = new object();
        private EventHandler<IV_MeasurementStateChanged_EventArgs> _IV_MeasurementsStateChanged;
        public event EventHandler<IV_MeasurementStateChanged_EventArgs> IV_MeasurementsStateChanged
        {
            add
            {
                lock (IV_MeasurementsStateChanged_EventLock)
                {
                    if (_IV_MeasurementsStateChanged == null || !_IV_MeasurementsStateChanged.GetInvocationList().Contains(value))
                    {
                        _IV_MeasurementsStateChanged += value;
                    }
                }
            }
            remove
            {
                lock (IV_MeasurementsStateChanged_EventLock)
                {
                    _IV_MeasurementsStateChanged -= value;
                }
            }
        }
        public virtual void OnIV_MeasurementsStateChanged(object sender, IV_MeasurementStateChanged_EventArgs e)
        {
            EventHandler<IV_MeasurementStateChanged_EventArgs> handler;
            lock (IV_MeasurementsStateChanged_EventLock)
            {
                handler = _IV_MeasurementsStateChanged;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #region I-V curve point received

        #region 1-st channel point received events

        private readonly object IV_PointReceivedChannel_01_EventLock = new object();
        private EventHandler<IV_PointReceivedChannel_01_EventArgs> _IV_PointReceivedChannel_01;
        public event EventHandler<IV_PointReceivedChannel_01_EventArgs> IV_PointReceivedChannel_01
        {
            add
            {
                lock (IV_PointReceivedChannel_01_EventLock)
                {
                    if (_IV_PointReceivedChannel_01 == null || !_IV_PointReceivedChannel_01.GetInvocationList().Contains(value))
                    {
                        _IV_PointReceivedChannel_01 += value;
                    }
                }
            }
            remove
            {
                lock (IV_PointReceivedChannel_01_EventLock)
                {
                    _IV_PointReceivedChannel_01 -= value;
                }
            }
        }
        public virtual void OnIV_PointReceivedChannel_01(object sender, IV_PointReceivedChannel_01_EventArgs e)
        {
            EventHandler<IV_PointReceivedChannel_01_EventArgs> handler; 
            lock(IV_PointReceivedChannel_01_EventLock)
            {
                handler = _IV_PointReceivedChannel_01;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #region 2-nd channel point received events

        private readonly object IV_PointReceivedChannel_02_EventLock = new object();
        private EventHandler<IV_PointReceivedChannel_02_EventArgs> _IV_PointReceivedChannel_02;
        public event EventHandler<IV_PointReceivedChannel_02_EventArgs> IV_PointReceivedChannel_02
        {
            add
            {
                lock (IV_PointReceivedChannel_02_EventLock)
                {
                    if (_IV_PointReceivedChannel_02 == null || !_IV_PointReceivedChannel_02.GetInvocationList().Contains(value))
                    {
                        _IV_PointReceivedChannel_02 += value;
                    }
                }
            }
            remove
            {
                lock (IV_PointReceivedChannel_02_EventLock)
                {
                    _IV_PointReceivedChannel_02 -= value;
                }
            }
        }
        public virtual void OnIV_PointReceivedChannel_02(object sender, IV_PointReceivedChannel_02_EventArgs e)
        {
            EventHandler<IV_PointReceivedChannel_02_EventArgs> handler;
            lock (IV_PointReceivedChannel_02_EventLock)
            {
                handler = _IV_PointReceivedChannel_02;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #endregion

        #endregion

        #region TimeTrace Curve Measurements Events

        #region Time trace measurements state changed

        private readonly object TimeTraceMeasurementsStateChanged_EventLock = new object();
        private EventHandler<TimeTraceMeasurementStateChanged_EventArgs> _TimeTraceMeasurementsStateChanged;
        public event EventHandler<TimeTraceMeasurementStateChanged_EventArgs> TimeTraceMeasurementsStateChanged
        {
            add
            {
                lock (TimeTraceMeasurementsStateChanged_EventLock)
                {
                    if (_TimeTraceMeasurementsStateChanged == null || !_TimeTraceMeasurementsStateChanged.GetInvocationList().Contains(value))
                    {
                        _TimeTraceMeasurementsStateChanged += value;
                    }
                }
            }
            remove
            {
                lock (TimeTraceMeasurementsStateChanged_EventLock)
                {
                    _TimeTraceMeasurementsStateChanged -= value;
                }
            }
        }
        public virtual void OnTimeTraceMeasurementsStateChanged(object sender, TimeTraceMeasurementStateChanged_EventArgs e)
        {
            EventHandler<TimeTraceMeasurementStateChanged_EventArgs> handler;
            lock (TimeTraceMeasurementsStateChanged_EventLock)
            {
                handler = _TimeTraceMeasurementsStateChanged;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #region Time trace point received

        #region 1-st channel point received

        private readonly object TimeTracePointReceivedChannel_01_EventLock = new object();
        private EventHandler<TimeTracePointReceivedChannel_01_EventArgs> _TimeTracePointReceivedChannel_01;
        public event EventHandler<TimeTracePointReceivedChannel_01_EventArgs> TimeTracePointReceivedChannel_01
        {
            add
            {
                lock(TimeTracePointReceivedChannel_01_EventLock)
                {
                    if (_TimeTracePointReceivedChannel_01 == null || !_TimeTracePointReceivedChannel_01.GetInvocationList().Contains(value))
                    {
                        _TimeTracePointReceivedChannel_01 += value;
                    }
                }
            }
            remove
            {
                lock (TimeTracePointReceivedChannel_01_EventLock)
                {
                    _TimeTracePointReceivedChannel_01 -= value;
                }
            }
        }
        public virtual void OnTimeTracePointReceivedChannel_01(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
        {
            EventHandler<TimeTracePointReceivedChannel_01_EventArgs> handler;
            lock (TimeTracePointReceivedChannel_01_EventLock)
            {
                handler = _TimeTracePointReceivedChannel_01;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #region 2-nd channel point received

        private readonly object TimeTracePointReceivedChannel_02_EventLock = new object();
        private EventHandler<TimeTracePointReceivedChannel_02_EventArgs> _TimeTracePointReceivedChannel_02;
        public event EventHandler<TimeTracePointReceivedChannel_02_EventArgs> TimeTracePointReceivedChannel_02
        {
            add
            {
                lock (TimeTracePointReceivedChannel_02_EventLock)
                {
                    if (_TimeTracePointReceivedChannel_02 == null || !_TimeTracePointReceivedChannel_02.GetInvocationList().Contains(value))
                    {
                        _TimeTracePointReceivedChannel_02 += value;
                    }
                }
            }
            remove
            {
                lock (TimeTracePointReceivedChannel_02_EventLock)
                {
                    _TimeTracePointReceivedChannel_02 -= value;
                }
            }
        }
        public virtual void OnTimeTracePointReceivedChannel_02(object sender, TimeTracePointReceivedChannel_02_EventArgs e)
        {
            EventHandler<TimeTracePointReceivedChannel_02_EventArgs> handler;
            lock (TimeTracePointReceivedChannel_02_EventLock)
            {
                handler = _TimeTracePointReceivedChannel_02;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #region Both channels point received

        private readonly object TimeTraceBothChannelsPointsReceived_EventLock = new object();
        private EventHandler<TimeTraceBothChannelsPointsReceived_EventArgs> _TimeTraceBothChannelsPointsReceived;
        public event EventHandler<TimeTraceBothChannelsPointsReceived_EventArgs> TimeTraceBothChannelsPointsReceived
        {
            add
            {
                lock (TimeTraceBothChannelsPointsReceived_EventLock)
                {
                    if (_TimeTraceBothChannelsPointsReceived == null || !_TimeTraceBothChannelsPointsReceived.GetInvocationList().Contains(value))
                    {
                        _TimeTraceBothChannelsPointsReceived += value;
                    }
                }
            }
            remove
            {
                lock (TimeTraceBothChannelsPointsReceived_EventLock)
                {
                    _TimeTraceBothChannelsPointsReceived -= value;
                }
            }
        }
        public virtual void OnTimeTraceBothChannelsPointsReceived(object sender, TimeTraceBothChannelsPointsReceived_EventArgs e)
        {
            EventHandler<TimeTraceBothChannelsPointsReceived_EventArgs> handler;
            lock (TimeTraceBothChannelsPointsReceived_EventLock)
            {
                handler = _TimeTraceBothChannelsPointsReceived;
            }
            if(handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #endregion

        //Motion EventArgs

        private readonly object MotionEventLock = new object();
        private EventHandler<Motion_EventArgs> _Motion;
        public event EventHandler<Motion_EventArgs> Motion
        {
            add
            {
                lock (MotionEventLock)
                {
                    if (_Motion == null || !_Motion.GetInvocationList().Contains(value))
                    {
                        _Motion += value;
                    }
                }
            }
            remove
            {
                lock (MotionEventLock)
                {
                    _Motion -= value;
                }
            }
        }
        public virtual void OnMotion(object sender, Motion_EventArgs e)
        {
            EventHandler<Motion_EventArgs> handler;
            lock (MotionEventLock)
            {
                handler = _Motion;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #region Motor Events

        //Motor current position reached

        private readonly object MotorCurrentPositionReachedEventLock = new object();
        private EventHandler<MotorCurrentPositionReached_EventArgs> _MotorCurrentPositionReached;
        public event EventHandler<MotorCurrentPositionReached_EventArgs> MotorCurrentPositionReached
        {
            add
            {
                lock (MotorCurrentPositionReachedEventLock)
                {
                    if (_MotorCurrentPositionReached == null || !_MotorCurrentPositionReached.GetInvocationList().Contains(value))
                    {
                        _MotorCurrentPositionReached += value;
                    }
                }
            }
            remove
            {
                lock(MotorCurrentPositionReachedEventLock)
                {
                    _MotorCurrentPositionReached -= value;
                }
            }
        }
        public virtual void OnMotorCurrentPositionReached(object sender, MotorCurrentPositionReached_EventArgs e)
        {
            EventHandler<MotorCurrentPositionReached_EventArgs> handler;
            lock(MotorCurrentPositionReachedEventLock)
            {
                handler = _MotorCurrentPositionReached;
            }
            if(handler != null)
            {
                handler(sender, e);
            }
        }

        //Motor final destination reached

        private readonly object MotorFinalDestinationReachedEventLock = new object();
        private EventHandler<MotorFinalDestinationReached_EventArgs> _MotorFinalDestinationReached;
        public event EventHandler<MotorFinalDestinationReached_EventArgs> MotorFinalDestinationReached
        {
            add
            {
                lock(MotorFinalDestinationReachedEventLock)
                {
                    if(_MotorFinalDestinationReached == null || !_MotorFinalDestinationReached.GetInvocationList().Contains(value))
                    {
                        _MotorFinalDestinationReached += value;
                    }
                }
            }
            remove
            {
                lock(MotorFinalDestinationReachedEventLock)
                {
                    _MotorFinalDestinationReached -= value;
                }
            }
        }
        public virtual void OnMotorFinalDestinationReached(object sender, MotorFinalDestinationReached_EventArgs e)
        {
            EventHandler<MotorFinalDestinationReached_EventArgs> handler;
            lock(MotorFinalDestinationReachedEventLock)
            {
                handler = _MotorFinalDestinationReached;
            }
            if(handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #endregion
    }
}
