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
    #region Experimental I-V data source implementation

    /// <summary>
    /// Represents data source for I-V measurements
    /// </summary>
    public class ExperimentalIV_DataSource : IPointDataSource
    {
        private List<PointD> _ExperimentalData;
        public List<PointD> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<PointD> _ExperimentalDataSource;

        public ExperimentalIV_DataSource(List<PointD> _Data)
        {
            _ExperimentalData = _Data;
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

        public void OnIV_PointReceived(object sender, IV_PointReceivedChannel_01_EventArgs e)
        {
            if (_ExperimentalData.Count > 10000)
                _ExperimentalData.RemoveAt(0);

            _ExperimentalData.Add(new PointD(e.X, e.Y));
            //_ExperimentalDataSource.RaiseDataChanged();

            _Dispatcher.BeginInvoke(new Action(delegate()
            {
                DataChanged(sender, new EventArgs());
            }));
        }

        public void OnIV_PointReceived(object sender, IV_PointReceivedChannel_02_EventArgs e)
        {
            _ExperimentalData.Add(new PointD(e.X, e.Y));
            _ExperimentalDataSource.RaiseDataChanged();

            _Dispatcher.BeginInvoke(new Action(delegate()
            {
                DataChanged(sender, new EventArgs());
            }));
        }
    }

    public class ExperimentalIV_DataSourceChannel : ExperimentalIV_DataSource
    {
        private Channels _Channel;

        public ExperimentalIV_DataSourceChannel(List<PointD> data, Channels Channel)
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
                        AllEventsHandler.Instance.IV_PointReceivedChannel_01 += OnIV_PointReceived;
                    } break;
                case Channels.Channel_02:
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
                case Channels.Channel_01:
                    {
                        AllEventsHandler.Instance.IV_PointReceivedChannel_01 -= OnIV_PointReceived;
                    } break;
                case Channels.Channel_02:
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
