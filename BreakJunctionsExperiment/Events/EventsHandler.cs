using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class AllEventsHandler
    {
        //General

        private static AllEventsHandler _Instance;
        public static AllEventsHandler Instance
        {
            get
            {
                if (_Instance == null) _Instance = new AllEventsHandler();
                return _Instance;
            }
        }

        #region I-V Curve Measurements Events

        //I-V measurements stateChanged

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

        //Time trace measurements state changed

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

        //Time trace point received

        private readonly object TimeTracePointReceived_EventLock = new object();
        private EventHandler<TimeTracePointReceived_EventArgs> _TimeTracePointReceived;
        public event EventHandler<TimeTracePointReceived_EventArgs> TimetracePointReceived
        {
            add
            {
                lock(TimeTracePointReceived_EventLock)
                {
                    if (_TimeTracePointReceived == null || !_TimeTracePointReceived.GetInvocationList().Contains(value))
                    {
                        _TimeTracePointReceived += value;
                    }
                }
            }
            remove
            {
                lock (TimeTracePointReceived_EventLock)
                {
                    _TimeTracePointReceived -= value;
                }
            }
        }
        public virtual void OnTimeTracePointReceived(object sender, TimeTracePointReceived_EventArgs e)
        {
            EventHandler<TimeTracePointReceived_EventArgs> handler;
            lock (TimeTracePointReceived_EventLock)
            {
                handler = _TimeTracePointReceived;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

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
