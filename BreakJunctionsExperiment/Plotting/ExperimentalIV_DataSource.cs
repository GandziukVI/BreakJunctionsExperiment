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
    #region Experimental I-V data source implementation

    /// <summary>
    /// Represents data source for I-V measurements
    /// </summary>
    public class ExperimentalIV_DataSource : IPointDataSource
    {
        private LinkedList<Point> _ExperimentalData;
        public LinkedList<Point> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<Point> _ExperimentalDataSource;

        public ExperimentalIV_DataSource()
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

        public async void OnIV_PointReceived(object sender, IV_PointReceivedChannel_01_EventArgs e)
        {
            if (_ExperimentalData.Count > 5000)
                _ExperimentalData.RemoveFirst();

            _ExperimentalData.AddLast(new Point(e.X, e.Y));

            await _Dispatcher.BeginInvoke(new Action(delegate()
            {
                DataChanged(sender, new EventArgs());
            }));
        }

        public async void OnIV_PointReceived(object sender, IV_PointReceivedChannel_02_EventArgs e)
        {
            if (_ExperimentalData.Count > 5000)
                _ExperimentalData.RemoveFirst();

            _ExperimentalData.AddLast(new Point(e.X, e.Y));
            _ExperimentalDataSource.RaiseDataChanged();

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

    public class ExperimentalIV_DataSourceChannel : ExperimentalIV_DataSource
    {
        private ChannelsToInvestigate _Channel;

        public ExperimentalIV_DataSourceChannel(ChannelsToInvestigate Channel)
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
                        AllEventsHandler.Instance.IV_PointReceivedChannel_01 += OnIV_PointReceived;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_02 += OnIV_PointReceived;
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
                        AllEventsHandler.Instance.IV_PointReceivedChannel_01 -= OnIV_PointReceived;
                    } break;
                case ChannelsToInvestigate.Channel_02:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_02 -= OnIV_PointReceived;
                    } break;
                default:
                    break;
            }
        }
    }

    #endregion
}
