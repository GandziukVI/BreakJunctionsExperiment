using BreakJunctions.Events;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BreakJunctions.Plotting
{
    #region Experimental TimeTrace data source implementation

    public class ExperimentalTimeTraceDataSource : IPointDataSource
    {
        private double _ResistanceValueOverflow = 10000000000.0;
        private double _ScaledConductanceOverflow = (1.0 / 10000000000.0) / 0.00007748091734625;
        /// <summary>
        /// All measured values, higher then this would
        /// be ignored either in displaying or in saving on HDD
        /// </summary>
        public double ResistanceValueOverflow
        {
            get { return _ResistanceValueOverflow; }
            set { _ResistanceValueOverflow = value; }
        }

        private LinkedList<Point> _ExperimentalData;
        public LinkedList<Point> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<Point> _ExperimentalDataSource;

        public ExperimentalTimeTraceDataSource()
        {
            _ExperimentalData = new LinkedList<Point>();
            _ExperimentalDataSource = new EnumerableDataSource<Point>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            _Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(System.Windows.DependencyObject context)
        {
            return new EnumerablePointEnumerator<Point>(_ExperimentalDataSource);
        }

        public virtual void AttachPointReceiveEvent() { }

        public virtual void DetachPointReceiveEvent() { }

        public async void OnTimeTracePointReceived(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
        {
            if (_ExperimentalData.Count > 10000)
                _ExperimentalData.RemoveFirst();

            if (e.Y <= _ResistanceValueOverflow)
            {
                _ExperimentalData.AddLast(new Point(e.X, e.Y));

                await _Dispatcher.BeginInvoke(new Action(delegate()
                {
                    try
                    {
                        DataChanged(sender, new EventArgs());
                    }
                    catch { }
                }));
            }
        }
        public async void OnTimeTracePointReceived(object sender, TimeTracePointReceivedChannel_02_EventArgs e)
        {
            if (_ExperimentalData.Count > 10000)
                _ExperimentalData.RemoveFirst();

            if (e.Y >= _ScaledConductanceOverflow)
            {
                _ExperimentalData.AddLast(new Point(e.X, e.Y));

                await _Dispatcher.BeginInvoke(new Action(delegate()
                {
                    try
                    {
                        DataChanged(sender, new EventArgs());
                    }
                    catch { }
                }));
            }
        }
    }

    public class ExperimentalTimetraceDataSourceChannel : ExperimentalTimeTraceDataSource
    {
        private ChannelsToInvestigate _Channel;

        public ExperimentalTimetraceDataSourceChannel(ChannelsToInvestigate Channel)
            : base()
        {
            _Channel = Channel;
        }

        public override void AttachPointReceiveEvent()
        {
            switch (_Channel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 += OnTimeTracePointReceived;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 += OnTimeTracePointReceived;
                    } break;
                default:
                    break;
            }
        }

        public override void DetachPointReceiveEvent()
        {
            switch (_Channel)
            {
                case ChannelsToInvestigate.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 -= OnTimeTracePointReceived;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_02 -= OnTimeTracePointReceived;
                    } break;
                default:
                    break;
            }
        }
    }

    #endregion
}
