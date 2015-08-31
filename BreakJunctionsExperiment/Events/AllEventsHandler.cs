using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions.Events
{
    class AllEventsHandler
    {
        #region Singleton class implementation

        private static readonly object _InstanceCreated_Lock = new object();

        private static AllEventsHandler _Instance;
        public static AllEventsHandler Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_InstanceCreated_Lock) 
                    {
                        if (_Instance == null)
                            _Instance = new AllEventsHandler();
                    }
                }

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
            lock (IV_PointReceivedChannel_01_EventLock)
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
                lock (TimeTracePointReceivedChannel_01_EventLock)
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
            if (handler != null)
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

        #region Motion Events

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
                lock (MotorCurrentPositionReachedEventLock)
                {
                    _MotorCurrentPositionReached -= value;
                }
            }
        }
        public virtual void OnMotorCurrentPositionReached(object sender, MotorCurrentPositionReached_EventArgs e)
        {
            EventHandler<MotorCurrentPositionReached_EventArgs> handler;
            lock (MotorCurrentPositionReachedEventLock)
            {
                handler = _MotorCurrentPositionReached;
            }
            if (handler != null)
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
                lock (MotorFinalDestinationReachedEventLock)
                {
                    if (_MotorFinalDestinationReached == null || !_MotorFinalDestinationReached.GetInvocationList().Contains(value))
                    {
                        _MotorFinalDestinationReached += value;
                    }
                }
            }
            remove
            {
                lock (MotorFinalDestinationReachedEventLock)
                {
                    _MotorFinalDestinationReached -= value;
                }
            }
        }
        public virtual void OnMotorFinalDestinationReached(object sender, MotorFinalDestinationReached_EventArgs e)
        {
            EventHandler<MotorFinalDestinationReached_EventArgs> handler;
            lock (MotorFinalDestinationReachedEventLock)
            {
                handler = _MotorFinalDestinationReached;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object MotionDirectionChanged_EventLock = new object();
        private EventHandler<MotionDirectionChanged_EventArgs> _MotionDirectionChanged;
        public event EventHandler<MotionDirectionChanged_EventArgs> MotionDirectionChanged
        {
            add
            {
                lock(MotionDirectionChanged_EventLock)
                {
                    if (_MotionDirectionChanged == null || !_MotionDirectionChanged.GetInvocationList().Contains(value))
                        _MotionDirectionChanged += value;
                }
            }
            remove
            {
                lock(MotionDirectionChanged_EventLock)
                {
                    _MotionDirectionChanged -= value;
                }
            }
        }
        public virtual void OnMotionDirectionChanged(object sender, MotionDirectionChanged_EventArgs e)
        {
            EventHandler<MotionDirectionChanged_EventArgs> handler;
            lock (MotionDirectionChanged_EventLock)
            {
                handler = _MotionDirectionChanged;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #endregion

        #region Real Time TimeTrace Curve Measureemnts Events

        private readonly object RealTime_TimeTraceDataArrived_EventLock = new object();
        private EventHandler<RealTime_TimeTrace_DataArrived_EventArgs> _RealTime_TimeTraceDataArrived;
        public event EventHandler<RealTime_TimeTrace_DataArrived_EventArgs> RealTime_TimeTraceDataArrived
        {
            add
            {
                lock (RealTime_TimeTraceDataArrived_EventLock)
                {
                    if (_RealTime_TimeTraceDataArrived == null || !_RealTime_TimeTraceDataArrived.GetInvocationList().Contains(value))
                    {
                        _RealTime_TimeTraceDataArrived += value;
                    }
                }
            }
            remove
            {
                lock (RealTime_TimeTraceDataArrived_EventLock)
                {
                    _RealTime_TimeTraceDataArrived -= value;
                }
            }
        }
        public virtual void OnRealTime_TimeTraceDataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            EventHandler<RealTime_TimeTrace_DataArrived_EventArgs> handler;
            lock (RealTime_TimeTraceDataArrived_EventLock)
            {
                handler = _RealTime_TimeTraceDataArrived;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object RealTime_TimeTraceMeasurementStateChanged_EventLock = new object();
        private EventHandler<RealTime_TimeTraceMeasurementStateChanged_EventArgs> _RealTime_TimeTraceMeasurementStateChanged;
        public event EventHandler<RealTime_TimeTraceMeasurementStateChanged_EventArgs> RealTime_TimeTraceMeasurementStateChanged
        {
            add
            {
                lock (RealTime_TimeTraceMeasurementStateChanged_EventLock)
                {
                    if (_RealTime_TimeTraceMeasurementStateChanged == null || !_RealTime_TimeTraceMeasurementStateChanged.GetInvocationList().Contains(value))
                    {
                        _RealTime_TimeTraceMeasurementStateChanged += value;
                    }
                }
            }
            remove
            {
                lock (RealTime_TimeTraceMeasurementStateChanged_EventLock)
                {
                    _RealTime_TimeTraceMeasurementStateChanged -= value;
                }
            }
        }
        public virtual void OnRealTime_TimeTraceMeasurementStateChanged(object sender, RealTime_TimeTraceMeasurementStateChanged_EventArgs e)
        {
            EventHandler<RealTime_TimeTraceMeasurementStateChanged_EventArgs> handler;
            lock (RealTime_TimeTraceMeasurementStateChanged_EventLock)
            {
                handler = _RealTime_TimeTraceMeasurementStateChanged;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object RealTime_TimeTrace_ResetTimeShift_EventLock = new object();
        private EventHandler<RealTime_TimeTrace_ResetTimeShift_EventArgs> _RealTime_TimeTrace_ResetTimeShift;
        public event EventHandler<RealTime_TimeTrace_ResetTimeShift_EventArgs> RealTime_TimeTrace_ResetTimeShift
        {
            add
            {
                lock (RealTime_TimeTrace_ResetTimeShift_EventLock)
                {
                    if (_RealTime_TimeTrace_ResetTimeShift == null || !_RealTime_TimeTrace_ResetTimeShift.GetInvocationList().Contains(value))
                    {
                        _RealTime_TimeTrace_ResetTimeShift += value;
                    }
                }
            }
            remove
            {
                lock (RealTime_TimeTrace_ResetTimeShift_EventLock)
                {
                    _RealTime_TimeTrace_ResetTimeShift -= value;
                }
            }
        }
        public virtual void OnRealTime_TimeTrace_ResetTimeShift(object sender, RealTime_TimeTrace_ResetTimeShift_EventArgs e)
        {
            EventHandler<RealTime_TimeTrace_ResetTimeShift_EventArgs> handler;
            lock (RealTime_TimeTrace_ResetTimeShift_EventLock)
            {
                handler = _RealTime_TimeTrace_ResetTimeShift;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object RealTime_TimeTrace_AveragedDataArrived_Sample_01_EventLock = new object();
        private EventHandler<RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01> _RealTime_TimeTrace_AveragedDataArrived_Sample_01;
        public event EventHandler<RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01> RealTime_TimeTrace_AveragedDataArrived_Sample_01
        {
            add
            {
                lock (RealTime_TimeTrace_AveragedDataArrived_Sample_01_EventLock)
                {
                    if (_RealTime_TimeTrace_AveragedDataArrived_Sample_01 == null || !_RealTime_TimeTrace_AveragedDataArrived_Sample_01.GetInvocationList().Contains(value))
                        _RealTime_TimeTrace_AveragedDataArrived_Sample_01 += value;
                }
            }
            remove
            {
                lock (RealTime_TimeTrace_AveragedDataArrived_Sample_01_EventLock)
                {
                    _RealTime_TimeTrace_AveragedDataArrived_Sample_01 -= value;
                }
            }
        }
        public virtual void OnRealTime_TimeTrace_AveragedDataArrived_Sample_01(object sender, RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01 e)
        {
            EventHandler<RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01> handler;
            lock (RealTime_TimeTrace_AveragedDataArrived_Sample_01_EventLock)
            {
                handler = _RealTime_TimeTrace_AveragedDataArrived_Sample_01;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object RealTime_TimeTrace_AveragedDataArrived_Sample_02_EventLock = new object();
        private EventHandler<RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02> _RealTime_TimeTrace_AveragedDataArrived_Sample_02;
        public event EventHandler<RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02> RealTime_TimeTrace_AveragedDataArrived_Sample_02
        {
            add
            {
                lock (RealTime_TimeTrace_AveragedDataArrived_Sample_02_EventLock)
                {
                    if (_RealTime_TimeTrace_AveragedDataArrived_Sample_02 == null || !_RealTime_TimeTrace_AveragedDataArrived_Sample_02.GetInvocationList().Contains(value))
                        _RealTime_TimeTrace_AveragedDataArrived_Sample_02 += value;
                }
            }
            remove
            {
                lock (RealTime_TimeTrace_AveragedDataArrived_Sample_02_EventLock)
                {
                    _RealTime_TimeTrace_AveragedDataArrived_Sample_02 -= value;
                }
            }
        }
        public virtual void OnRealTime_TimeTrace_AveragedDataArrived_Sample_02(object sender, RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02 e)
        {
            EventHandler<RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02> handler;
            lock (RealTime_TimeTrace_AveragedDataArrived_Sample_01_EventLock)
            {
                handler = _RealTime_TimeTrace_AveragedDataArrived_Sample_02;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #region RealTime motion events

        private readonly object Motion_RealTime_EventLock = new object();
        private EventHandler<Motion_RealTime_EventArgs> _Motion_RealTime;
        public event EventHandler<Motion_RealTime_EventArgs> Motion_RealTime
        {
            add
            {
                lock (Motion_RealTime_EventLock)
                {
                    if (_Motion_RealTime == null || !_Motion_RealTime.GetInvocationList().Contains(value))
                        _Motion_RealTime += value;
                }
            }
            remove
            {
                lock (Motion_RealTime_EventLock)
                {
                    _Motion_RealTime -= value;
                }
            }
        }
        public virtual void OnMotion_RealTime(object sender, Motion_RealTime_EventArgs e)
        {
            EventHandler<Motion_RealTime_EventArgs> handler;
            lock (Motion_RealTime_EventLock)
            {
                handler = _Motion_RealTime;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object Motion_RealTime_StartPositionReached_EventLock = new object();
        private EventHandler<Motion_RealTime_StartPositionReached_EventArgs> _Motion_RealTime_StartPositionReached;
        public event EventHandler<Motion_RealTime_StartPositionReached_EventArgs> Motion_RealTime_StartPositionReached
        {
            add
            {
                lock (Motion_RealTime_StartPositionReached_EventLock)
                {
                    if (_Motion_RealTime_StartPositionReached == null || !_Motion_RealTime_StartPositionReached.GetInvocationList().Contains(value))
                        _Motion_RealTime_StartPositionReached += value;
                }
            }
            remove
            {
                lock (Motion_RealTime_StartPositionReached_EventLock)
                {
                    _Motion_RealTime_StartPositionReached -= value;
                }
            }
        }
        public virtual void OnMotion_RealTime_StartPositionReached(object sender, Motion_RealTime_StartPositionReached_EventArgs e)
        {
            EventHandler<Motion_RealTime_StartPositionReached_EventArgs> handler;
            lock (Motion_RealTime_StartPositionReached_EventLock)
            {
                handler = _Motion_RealTime_StartPositionReached;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object Motion_RealTime_FinalDestinationReached_EventLock = new object();
        private EventHandler<Motion_RealTime_FinalDestinationReached_EventArgs> _Motion_RealTime_FinalDestinationReached;
        public event EventHandler<Motion_RealTime_FinalDestinationReached_EventArgs> Motion_RealTime_FinalDestinationReached
        {
            add
            {
                lock (Motion_RealTime_FinalDestinationReached_EventLock)
                {
                    if (_Motion_RealTime_FinalDestinationReached == null || !_Motion_RealTime_FinalDestinationReached.GetInvocationList().Contains(value))
                        _Motion_RealTime_FinalDestinationReached += value;
                }
            }
            remove
            {
                lock (Motion_RealTime_FinalDestinationReached_EventLock)
                {
                    _Motion_RealTime_FinalDestinationReached -= value;
                }
            }
        }
        public virtual void OnMotion_RealTime_FinalDestinationReached(object sender, Motion_RealTime_FinalDestinationReached_EventArgs e)
        {
            EventHandler<Motion_RealTime_FinalDestinationReached_EventArgs> handler;
            lock (Motion_RealTime_FinalDestinationReached_EventLock)
            {
                handler = _Motion_RealTime_FinalDestinationReached;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion

        #endregion

        #region Niose Measurements Events

        private readonly object NoiseMeasurement_StateChanged_EventLock = new object();
        private EventHandler<NoiseMeasurement_StateChanged_EventArgs> _NoiseMeasurement_StateChanged;
        public event EventHandler<NoiseMeasurement_StateChanged_EventArgs> NoiseMeasurement_StateChanged
        {
            add
            {
                lock(NoiseMeasurement_StateChanged_EventLock)
                {
                    if(_NoiseMeasurement_StateChanged == null || !_NoiseMeasurement_StateChanged.GetInvocationList().Contains(value))
                    {
                        _NoiseMeasurement_StateChanged += value;
                    }
                }
            }
            remove
            {
                lock(NoiseMeasurement_StateChanged_EventLock)
                {
                    _NoiseMeasurement_StateChanged -= value;
                }
            }
        }
        public virtual void On_NoiseMeasurement_StateChanged(object sender, NoiseMeasurement_StateChanged_EventArgs e)
        {
            EventHandler<NoiseMeasurement_StateChanged_EventArgs> handler;
            lock(NoiseMeasurement_StateChanged_EventLock)
            {
                handler = _NoiseMeasurement_StateChanged;
            }
            if(handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object NoiseSpectra_DataArrived_EventLock_Channel_01 = new object();
        private EventHandler<NoiseSpectra_DataArrived_Channel_01_EventArgs> _NoiseSpectra_DataArrived_Channel_01;
        public event EventHandler<NoiseSpectra_DataArrived_Channel_01_EventArgs> NoiseSpectra_DataArrived_Channel_01
        {
            add
            {
                lock (NoiseSpectra_DataArrived_EventLock_Channel_01)
                {
                    if (_NoiseSpectra_DataArrived_Channel_01 == null || !_NoiseSpectra_DataArrived_Channel_01.GetInvocationList().Contains(value))
                    {
                        _NoiseSpectra_DataArrived_Channel_01 += value;
                    }
                }
            }
            remove
            {
                lock (NoiseSpectra_DataArrived_EventLock_Channel_01)
                {
                    _NoiseSpectra_DataArrived_Channel_01 -= value;
                }
            }
        }
        public virtual void On_NoiseSpectra_DataArrived_Channel_01(object sender, NoiseSpectra_DataArrived_Channel_01_EventArgs e)
        {
            EventHandler<NoiseSpectra_DataArrived_Channel_01_EventArgs> handler;
            lock (NoiseSpectra_DataArrived_EventLock_Channel_01)
            {
                handler = _NoiseSpectra_DataArrived_Channel_01;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object NoiseSpectra_DataArrived_EventLock_Channel_02 = new object();
        private EventHandler<NoiseSpectra_DataArrived_Channel_02_EventArgs> _NoiseSpectra_DataArrived_Channel_02;
        public event EventHandler<NoiseSpectra_DataArrived_Channel_02_EventArgs> NoiseSpectra_DataArrived_Channel_02
        {
            add
            {
                lock (NoiseSpectra_DataArrived_EventLock_Channel_02)
                {
                    if (_NoiseSpectra_DataArrived_Channel_02 == null || !_NoiseSpectra_DataArrived_Channel_02.GetInvocationList().Contains(value))
                    {
                        _NoiseSpectra_DataArrived_Channel_02 += value;
                    }
                }
            }
            remove
            {
                lock (NoiseSpectra_DataArrived_EventLock_Channel_02)
                {
                    _NoiseSpectra_DataArrived_Channel_02 -= value;
                }
            }
        }
        public virtual void On_NoiseSpectra_DataArrived_Channel_02(object sender, NoiseSpectra_DataArrived_Channel_02_EventArgs e)
        {
            EventHandler<NoiseSpectra_DataArrived_Channel_02_EventArgs> handler;
            lock (NoiseSpectra_DataArrived_EventLock_Channel_02)
            {
                handler = _NoiseSpectra_DataArrived_Channel_02;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object LastNoiseSpectra_Channel_01_DataArrived_EventLock = new object();
        private EventHandler<LastNoiseSpectra_Channel_01_DataArrived_EventArgs> _LastNoiseSpectra_Channel_01_DataArrived;
        public event EventHandler<LastNoiseSpectra_Channel_01_DataArrived_EventArgs> LastNoiseSpectra_Channel_01_DataArrived
        {
            add
            {
                lock (LastNoiseSpectra_Channel_01_DataArrived_EventLock)
                {
                    if (_LastNoiseSpectra_Channel_01_DataArrived == null || !_LastNoiseSpectra_Channel_01_DataArrived.GetInvocationList().Contains(value))
                    {
                        _LastNoiseSpectra_Channel_01_DataArrived += value;
                    }
                }
            }
            remove
            {
                lock (LastNoiseSpectra_Channel_01_DataArrived_EventLock)
                {
                    _LastNoiseSpectra_Channel_01_DataArrived -= value;
                }
            }
        }
        public virtual void On_LastNoiseSpectra_Channel_01_DataArrived(object sender, LastNoiseSpectra_Channel_01_DataArrived_EventArgs e)
        {
            EventHandler<LastNoiseSpectra_Channel_01_DataArrived_EventArgs> handler;
            lock (LastNoiseSpectra_Channel_01_DataArrived_EventLock)
            {
                handler = _LastNoiseSpectra_Channel_01_DataArrived;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        private readonly object LastNoiseSpectra_Channel_02_DataArrived_EventLock = new object();
        private EventHandler<LastNoiseSpectra_Channel_02_DataArrived_EventArgs> _LastNoiseSpectra_Channel_02_DataArrived;
        public event EventHandler<LastNoiseSpectra_Channel_02_DataArrived_EventArgs> LastNoiseSpectra_Channel_02_DataArrived
        {
            add
            {
                lock (LastNoiseSpectra_Channel_02_DataArrived_EventLock)
                {
                    if (_LastNoiseSpectra_Channel_02_DataArrived == null || !_LastNoiseSpectra_Channel_02_DataArrived.GetInvocationList().Contains(value))
                    {
                        _LastNoiseSpectra_Channel_02_DataArrived += value;
                    }
                }
            }
            remove
            {
                lock (LastNoiseSpectra_Channel_02_DataArrived_EventLock)
                {
                    _LastNoiseSpectra_Channel_02_DataArrived -= value;
                }
            }
        }
        public virtual void On_LastNoiseSpectra_Channel_02_DataArrived(object sender, LastNoiseSpectra_Channel_02_DataArrived_EventArgs e)
        {
            EventHandler<LastNoiseSpectra_Channel_02_DataArrived_EventArgs> handler;
            lock (LastNoiseSpectra_Channel_02_DataArrived_EventLock)
            {
                handler = _LastNoiseSpectra_Channel_02_DataArrived;
            }
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        #endregion
    }
}
