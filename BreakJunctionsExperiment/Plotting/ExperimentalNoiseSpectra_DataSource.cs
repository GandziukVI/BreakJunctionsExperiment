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
    class ExperimentalNoiseSpectra_DataSource : IPointDataSource
    {
        #region IPointDataSource implementation

        public event EventHandler DataChanged;

        public IPointEnumerator GetEnumerator(System.Windows.DependencyObject context)
        {
            return new EnumerablePointEnumerator<Point>(_ExperimentalDataSource);
        }

        #endregion

        #region ExperimentalNoiseSpectra_DataSource settings

        private List<Point> _ExperimentalData;
        public List<Point> ExperimentalData
        {
            get { return _ExperimentalData; }
            set { _ExperimentalData = value; }
        }

        private Dispatcher _Dispatcher;
        private EnumerableDataSource<Point> _ExperimentalDataSource;

        private SamplesToInvestigate _SampleNumber;

        #endregion

        #region Constructor / Destructor

        public ExperimentalNoiseSpectra_DataSource(SamplesToInvestigate SampleNumber)
        {
            _ExperimentalData = new List<Point>();
            _ExperimentalDataSource = new EnumerableDataSource<Point>(_ExperimentalData);
            _ExperimentalDataSource.SetXMapping(x => x.X);
            _ExperimentalDataSource.SetYMapping(y => y.Y);

            _SampleNumber = SampleNumber;

            _Dispatcher = Dispatcher.CurrentDispatcher;
        }

        #endregion

        #region ExperimentalNoiseSpectra_DataSource functionality

        public virtual void AttachPointReceiveEvent()
        {
            switch (_SampleNumber)
            {
                case SamplesToInvestigate.Sample_01:
                    {
                        AllEventsHandler.Instance.NoiseSpectra_DataArrived_Channel_01 += OnNoiseSpectra_DataArrived;
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_01_DataArrived += OnNoiseSpectra_DataArrived;
                    } break;
                case SamplesToInvestigate.Sample_02:
                    {
                        AllEventsHandler.Instance.NoiseSpectra_DataArrived_Channel_02 += OnNoiseSpectra_DataArrived;
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_02_DataArrived += OnNoiseSpectra_DataArrived;
                    } break;
                default:
                    break;
            }
        }

        public virtual void DetachPointReceiveEvent()
        {
            switch (_SampleNumber)
            {
                case SamplesToInvestigate.Sample_01:
                    {
                        AllEventsHandler.Instance.NoiseSpectra_DataArrived_Channel_01 -= OnNoiseSpectra_DataArrived;
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_01_DataArrived -= OnNoiseSpectra_DataArrived;
                    } break;
                case SamplesToInvestigate.Sample_02:
                    {
                        AllEventsHandler.Instance.NoiseSpectra_DataArrived_Channel_02 -= OnNoiseSpectra_DataArrived;
                        AllEventsHandler.Instance.LastNoiseSpectra_Channel_02_DataArrived -= OnNoiseSpectra_DataArrived;
                    } break;
                default:
                    break;
            }
        }

        public async void OnNoiseSpectra_DataArrived(object sender, NoiseSpectra_DataArrived_Channel_01_EventArgs e)
        {
            await SetDataAsync(e.SpectraData, CancellationToken.None);
        }

        public async void OnNoiseSpectra_DataArrived(object sender, NoiseSpectra_DataArrived_Channel_02_EventArgs e)
        {
            await SetDataAsync(e.SpectraData, CancellationToken.None);
        }

        public async void OnNoiseSpectra_DataArrived(object sender, LastNoiseSpectra_Channel_01_DataArrived_EventArgs e)
        {
            await SetDataAsync(e.SpectraData, CancellationToken.None);
        }

        public async void OnNoiseSpectra_DataArrived(object sender, LastNoiseSpectra_Channel_02_DataArrived_EventArgs e)
        {
            await SetDataAsync(e.SpectraData, CancellationToken.None);
        }

        internal Task SetDataAsync(List<Point> Data, CancellationToken __CancellationToken)
        {
            return Task.Run(() =>
            {
                _ExperimentalData.Clear();

                _ExperimentalData.AddRange(Data);

                _Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        DataChanged(this, new EventArgs());
                    }
                    catch  (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }, __CancellationToken);
        }

        #endregion
    }
}
