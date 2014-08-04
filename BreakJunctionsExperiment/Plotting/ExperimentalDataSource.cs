using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using System.Windows.Threading;
using System.Windows.Controls;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

using Hardware;

using BreakJunctions.Events;
using System.Windows;

namespace BreakJunctions.Plotting
{
    /// <summary>
    /// Represents Point of double accuracy
    /// </summary>
    public struct PointD
    {
        public double X;
        public double Y;

        public PointD(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

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

        private Dispatcher dispatcher;
        private EnumerableDataSource<PointD> _ExperimentalDataSource;

        public ExperimentalIV_DataSource(List<PointD> data)
        {
            _ExperimentalData = data;
            _ExperimentalDataSource = new EnumerableDataSource<PointD>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(System.Windows.DependencyObject context)
        {
            return new EnumerablePointEnumerator<PointD>(_ExperimentalDataSource);
        }

        public void AttachPointReceiveEvent()
        {
            AllEventsHandler.Instance.IV_PointReceived += OnIV_PointReceived;
        }

        public void DetachPointReceiveEvent()
        {
            AllEventsHandler.Instance.IV_PointReceived -= OnIV_PointReceived;
        }

        private void OnIV_PointReceived(object sender, IV_PointReceived_EventArgs e)
        {
            _ExperimentalData.Add(new PointD(e.X, e.Y));
            _ExperimentalDataSource.RaiseDataChanged();

            dispatcher.BeginInvoke(new Action(delegate() {
                DataChanged(sender, new EventArgs());
            }));
        }
    }

    public class ExperimentalTimetraceDataSource : IPointDataSource
    {
        private List<PointD> _ExperimentalData;
        public List<PointD> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher dispatcher;
        private EnumerableDataSource<PointD> _ExperimentalDataSource;

        public ExperimentalTimetraceDataSource(List<PointD> data)
        {
            _ExperimentalData = data;
            _ExperimentalDataSource = new EnumerableDataSource<PointD>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(System.Windows.DependencyObject context)
        {
            return new EnumerablePointEnumerator<PointD>(_ExperimentalDataSource);
        }

        public void AttachPointReceiveEvent()
        {
            AllEventsHandler.Instance.TimetracePointReceived += OnTimeTracePointReceived;
        }

        public void DetachPointReceiveEvent()
        {
            AllEventsHandler.Instance.TimetracePointReceived -= OnTimeTracePointReceived;
        }

        private void OnTimeTracePointReceived(object sender, TimeTracePointReceived_EventArgs e)
        {
            _ExperimentalData.Add(new PointD(e.X, e.Y));
            _ExperimentalDataSource.RaiseDataChanged();

            dispatcher.BeginInvoke(new Action(delegate()
            {
                DataChanged(sender, new EventArgs());
            }));
        }
    }
}
