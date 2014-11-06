using Aids.Graphics;
using BreakJunctions.Events;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BreakJunctions.Plotting
{
    #region Experimental TimeTrace data source implementation

    public class ExperimentalTimeTraceDataSource : IPointDataSource
    {
        private double _ResistanceValueOverflow = 10000000000.0;
        /// <summary>
        /// All measured values, higher then this would
        /// be ignored either in displaying or in saving on HDD
        /// </summary>
        public double ResistanceValueOverflow
        {
            get { return _ResistanceValueOverflow; }
            set { _ResistanceValueOverflow = value; }
        }

        private List<PointD> _ExperimentalData;
        public List<PointD> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<PointD> _ExperimentalDataSource;

        public ExperimentalTimeTraceDataSource(List<PointD> data)
        {
            _ExperimentalData = data;
            _ExperimentalDataSource = new EnumerableDataSource<PointD>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            _Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(System.Windows.DependencyObject context)
        {
            return new EnumerablePointEnumerator<PointD>(_ExperimentalDataSource);
        }

        public virtual void AttachPointReceiveEvent() { }

        public virtual void DetachPointReceiveEvent() { }

        public void OnTimeTracePointReceived(object sender, TimeTracePointReceivedChannel_01_EventArgs e)
        {
            if (_ExperimentalData.Count > 10000)
                _ExperimentalData.RemoveAt(0);

            if (e.Y <= _ResistanceValueOverflow)
            {
                _ExperimentalData.Add(new PointD(e.X, e.Y));
                //_ExperimentalDataSource.RaiseDataChanged();

                _Dispatcher.BeginInvoke(new Action(delegate()
                {
                    try
                    {
                        DataChanged(sender, new EventArgs());
                    }
                    catch { }
                }));
            }
        }
        public void OnTimeTracePointReceived(object sender, TimeTracePointReceivedChannel_02_EventArgs e)
        {
            if (_ExperimentalData.Count > 10000)
                _ExperimentalData.RemoveAt(0);

            if (e.Y <= _ResistanceValueOverflow)
            {
                _ExperimentalData.Add(new PointD(e.X, e.Y));
                _ExperimentalDataSource.RaiseDataChanged();

                _Dispatcher.BeginInvoke(new Action(delegate()
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
        private Channels _Channel;

        public ExperimentalTimetraceDataSourceChannel(List<PointD> data, Channels Channel)
            : base(data)
        {
            _Channel = Channel;
        }

        public override void AttachPointReceiveEvent()
        {
            switch (_Channel)
            {
                case Channels.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 += OnTimeTracePointReceived;
                    } break;
                case Channels.Channel_02:
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
                case Channels.Channel_01:
                    {
                        AllEventsHandler.Instance.TimeTracePointReceivedChannel_01 -= OnTimeTracePointReceived;
                    } break;
                case Channels.Channel_02:
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
