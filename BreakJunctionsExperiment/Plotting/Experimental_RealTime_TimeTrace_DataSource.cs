using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using BreakJunctions.Events;

using Aids.Graphics;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Threading;

namespace BreakJunctions.Plotting
{
    class Experimental_RealTime_TimeTrace_DataSource_Sample : IPointDataSource
    {
        private List<PointD> _ExperimentalData;
        public List<PointD> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<PointD> _ExperimentalDataSource;

        private SamplesToInvestigate _SampleNumber;

        public Experimental_RealTime_TimeTrace_DataSource_Sample(List<PointD> Data, SamplesToInvestigate SampleNumber)
        {
            _ExperimentalData = Data;
            _ExperimentalDataSource = new EnumerableDataSource<PointD>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            _SampleNumber = SampleNumber;

            _Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(DependencyObject context)
        {
            return new EnumerablePointEnumerator<PointD>(_ExperimentalDataSource);
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

        internal Task SetDataAsync(List<PointD>[] Data, CancellationToken __CancellationToken)
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

                    for (int i = 0; i < DataLendgth; i++)
                    {
                        if (Data[number][i].Y != 0.0)
                            _ExperimentalData.Add(new PointD(Data[number][i].X, Data[number][i].Y / Data[number + 1][i].Y));
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
                            DataChanged(this, new EventArgs());
                        }
                        catch { }
                    }));
                }, __CancellationToken);
        }
    }
}
