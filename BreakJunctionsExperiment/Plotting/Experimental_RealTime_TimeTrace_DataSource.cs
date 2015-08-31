using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using BreakJunctions.Events;

using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Threading;

namespace BreakJunctions.Plotting
{
    class Experimental_RealTime_TimeTrace_DataSource_Sample : IPointDataSource
    {
        #region IPointDataSource implementation

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(DependencyObject context)
        {
            return new EnumerablePointEnumerator<Point>(_ExperimentalDataSource);
        }

        #endregion

        private LinkedList<Point> _ExperimentalData;
        public LinkedList<Point> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<Point> _ExperimentalDataSource;

        private SamplesToInvestigate _SampleNumber;

        public Experimental_RealTime_TimeTrace_DataSource_Sample(LinkedList<Point> Data, SamplesToInvestigate SampleNumber)
        {
            _ExperimentalData = Data;
            _ExperimentalDataSource = new EnumerableDataSource<Point>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            _SampleNumber = SampleNumber;

            _Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public virtual void AttachPointReceiveEvent() 
        {
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived; 
        }

        public virtual void DetachPointReceiveEvent()
        {
            AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
        }

        public async void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            await SetDataAsync(e.Data, CancellationToken.None);
        }

        internal Task SetDataAsync(LinkedList<Point>[] Data, CancellationToken __CancellationToken)
        {
            return Task.Run(() =>
                {
                    _ExperimentalData.Clear();
                    var DataLendgth = (new int[4] { Data[0].Count, Data[1].Count, Data[2].Count, Data[3].Count }).Min();

                    var number = 0;

                    switch (_SampleNumber)
                    {
                        case SamplesToInvestigate.Sample_01:
                            {
                                number = 0;
                            } break;
                        case SamplesToInvestigate.Sample_02:
                            {
                                number = 2;
                            } break;
                        default:
                            break;
                    }

                    var arr1 = new Point[Data[number].Count];
                    Data[number].CopyTo(arr1, 0);

                    var arr2 = new Point[Data[number + 1].Count];
                    Data[number + 1].CopyTo(arr2, 0);

                    for (int i = 0; i < DataLendgth; i++)
                    {
                        if (arr2[i].Y != 0.0)
                            _ExperimentalData.AddLast(new Point(arr1[i].X, arr1[i].Y / arr2[i].Y));
                    }

                    switch (_SampleNumber)
                    {
                        case SamplesToInvestigate.Sample_01:
                            {
                                if (_ExperimentalData != null && _ExperimentalData.Count > 0)
                                    AllEventsHandler.Instance.OnRealTime_TimeTrace_AveragedDataArrived_Sample_01(this, new RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01(_ExperimentalData.Average(o => o.Y)));
                            } break;
                        case SamplesToInvestigate.Sample_02:
                            {
                                if (_ExperimentalData != null && _ExperimentalData.Count > 0)
                                    AllEventsHandler.Instance.OnRealTime_TimeTrace_AveragedDataArrived_Sample_02(this, new RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02(_ExperimentalData.Average(o => o.Y)));
                            } break;
                        default:
                            break;
                    }

                    _Dispatcher.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            DataChanged(new object(), new EventArgs());
                        }
                        catch { }
                    }));
                }, __CancellationToken);
        }
    }
}
