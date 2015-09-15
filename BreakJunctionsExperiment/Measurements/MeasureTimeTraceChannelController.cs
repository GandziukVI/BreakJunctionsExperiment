using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BreakJunctions.Events;

namespace BreakJunctions.Measurements
{
    class MeasureTimeTraceChannelController : IDisposable
    {
        private bool _FirstChannelGaveResponce = false;
        private bool _SecondChannelGaveResponce = false;

        private bool _BothChannelsGaveResponce = false;

        public MeasureTimeTraceChannelController()
        {
            AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 += OnTimeTracePointReceivedChannel_01;
            AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 += OnTimeTracePointReceivedChannel_02;
        }
        ~MeasureTimeTraceChannelController()
        {
            this.Dispose();
        }

        private double _CH_01_Reading = 0.0;
        private double _CH_02_Reading = 0.0;

        public virtual void OnTimeTracePointReceivedChannel_01(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
        {
            _FirstChannelGaveResponce = true;
            _BothChannelsGaveResponce = _FirstChannelGaveResponce && _SecondChannelGaveResponce;

            if (_BothChannelsGaveResponce == true)
            {
                _FirstChannelGaveResponce = false;
                _SecondChannelGaveResponce = false;
                _BothChannelsGaveResponce = false;

                _CH_01_Reading = e.Y;

                AllEventsHandler.Instance.OnTimeTraceBothChannelsPointsReceived(this, new TimeTraceBothChannelsPointsReceived_EventArgs());
            }
        }

        public virtual void OnTimeTracePointReceivedChannel_02(object sender, TimeTracePointReceivedChannel_02_EventArgs e)
        {
            _SecondChannelGaveResponce = true;
            _BothChannelsGaveResponce = _FirstChannelGaveResponce && _SecondChannelGaveResponce;

            if (_BothChannelsGaveResponce == true)
            {
                _FirstChannelGaveResponce = false;
                _SecondChannelGaveResponce = false;
                _BothChannelsGaveResponce = false;

                _CH_02_Reading = e.Y;

                AllEventsHandler.Instance.OnTimeTraceBothChannelsPointsReceived(this, new TimeTraceBothChannelsPointsReceived_EventArgs());
            }
        }

        public void Dispose()
        {
            AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 += OnTimeTracePointReceivedChannel_01;
            AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 += OnTimeTracePointReceivedChannel_02;
        }
    }
}
