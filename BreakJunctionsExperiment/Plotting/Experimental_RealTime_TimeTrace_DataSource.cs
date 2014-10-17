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

        private Samples _SampleNumber;

        public Experimental_RealTime_TimeTrace_DataSource_Sample(List<PointD> Data, Samples SampleNumber)
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

        public virtual void AttachPointReceiveEvent() { }

        public virtual void DetachPointReceiveEvent() { }

        public void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            _ExperimentalData.Clear();
            var DataLendgth = (new int[4] { e.Data[0].Count, e.Data[1].Count, e.Data[2].Count, e.Data[3].Count }).Min();

            switch (_SampleNumber)
            {
                case Samples.Sample_01:
                    {
                        for (int i = 0; i < DataLendgth; i++)
                        {
                            _ExperimentalData.Add(new PointD(e.Data[0][i].X, e.Data[1][i].Y / e.Data[0][i].Y));
                        }
                    } break;
                case Samples.Sample_02:
                    { 
                    } break;
                default:
                    break;
            }
        }
    }
}
