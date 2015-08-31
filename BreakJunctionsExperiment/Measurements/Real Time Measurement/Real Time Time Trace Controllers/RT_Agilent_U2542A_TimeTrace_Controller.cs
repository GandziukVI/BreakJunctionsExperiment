using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Agilent_ExtensionBox;
using Agilent_ExtensionBox.Internal;
using Agilent_ExtensionBox.IO;

using BreakJunctions.Events;

namespace BreakJunctions.Measurements
{
    class RT_Agilent_U2542A_TimeTrace_Controller : RealTime_TimeTrace_Controller
    {
        BoxController _boxController;

        #region Constructor

        public RT_Agilent_U2542A_TimeTrace_Controller()
        {
            _boxController = new BoxController();
            _boxController.Init("USB0::0x0957::0x1718::TW54334510::0::INSTR");

            AllEventsHandler.Instance.RealTime_TimeTraceMeasurementStateChanged += OnRealTime_TimeTraceMeasurementStateChanged;
        }

        ~RT_Agilent_U2542A_TimeTrace_Controller()
        {
            _boxController.Close();
        }

        private int _SDA_Rate = 5000;
        public int SDA_Rate
        {
            get { return _SDA_Rate; }
            set { _SDA_Rate = value; }
        }

        Thread _DataTransformingAndSendingThread;

        #endregion

        private bool flag = false;
        private void _TransformAndEmitData()
        {
            while (MeasurementInProcess || flag)
            {
                flag = false;

                LinkedList<Point> CH_01, CH_02, CH_03, CH_04;

                var _success_CH_01 = _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn1].ChannelData.TryDequeue(out CH_01);
                var _success_CH_02 = _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn1].ChannelData.TryDequeue(out CH_02);
                var _success_CH_03 = _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn1].ChannelData.TryDequeue(out CH_03);
                var _success_CH_04 = _boxController.AI_ChannelCollection[AnalogInChannelsEnum.AIn1].ChannelData.TryDequeue(out CH_04);

                foreach (var item in _boxController.AI_ChannelCollection)
                    flag |= item.ChannelData.IsEmpty;

                var _channelData = new LinkedList<Point>[4]
                {
                    CH_01,
                    CH_02,
                    CH_03,
                    CH_04
                };

                AllEventsHandler.Instance.OnRealTime_TimeTraceDataArrived(this, new RealTime_TimeTrace_DataArrived_EventArgs(ref _channelData));
            }
        }

        public override void ContiniousAcquisition()
        {
            var _channelConfig = new AI_ChannelConfig[4]
            {
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn1, Enabled = true, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25},
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn2, Enabled = true, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25},
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn3, Enabled = true, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25},
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn4, Enabled = true, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25}
            };

            _boxController.ConfigureAI_Channels(_channelConfig);

            _boxController.StartAnalogAcquisition(_SDA_Rate);

            _DataTransformingAndSendingThread = new Thread(_TransformAndEmitData);
            _DataTransformingAndSendingThread.Priority = ThreadPriority.Normal;
            _DataTransformingAndSendingThread.Start();
        }

        public override List<Point> MakeSingleShot(int NumberOfChannel)
        {
            var _channelConfig = new AI_ChannelConfig[4]
            {
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn1, Enabled = false, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25},
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn2, Enabled = false, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25},
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn3, Enabled = false, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25},
                new AI_ChannelConfig() { ChannelName = AnalogInChannelsEnum.AIn4, Enabled = false, Mode = ChannelModeEnum.DC, Polarity = PolarityEnum.Polarity_Bipolar, Range = RangesEnum.Range_1_25}
            };

            _channelConfig[NumberOfChannel - 1].Mode = ChannelModeEnum.AC;
            _channelConfig[NumberOfChannel - 1].Enabled = true;

            _boxController.ConfigureAI_Channels(_channelConfig);
            _boxController.AcquireSingleShot(_SDA_Rate);

            LinkedList<Point> _result;
            var _success = _boxController.AI_ChannelCollection[(AnalogInChannelsEnum)(NumberOfChannel - 1)].ChannelData.TryDequeue(out _result);
            var result = new List<Point>();
            result.AddRange(_result);

            return _success ? result : new List<Point>();
        }

        public override void Dispose()
        {
            AllEventsHandler.Instance.RealTime_TimeTraceMeasurementStateChanged -= OnRealTime_TimeTraceMeasurementStateChanged;
        }

        public void OnRealTime_TimeTraceMeasurementStateChanged(object sender, RealTime_TimeTraceMeasurementStateChanged_EventArgs e)
        {
            _boxController.AcquisitionInProgress = e.MeasurementInProcess;
            this.MeasurementInProcess = e.MeasurementInProcess;
        }
    }
}
