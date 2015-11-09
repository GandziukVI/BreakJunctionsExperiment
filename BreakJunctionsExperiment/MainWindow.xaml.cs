using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

using System.Threading;

using BreakJunctions.Events;
using BreakJunctions.Plotting;
using BreakJunctions.Measurements;
using BreakJunctions.DataHandling;

using Devices.SMU;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

using Microsoft.Win32;

using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using BreakJunctions.Motion;

using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.Numeric;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;
using BreakJunctions.NotificationSystem;

namespace BreakJunctions
{
    //View models for devices
    public static class ModelViewInteractions
    {
        public static IV_VoltageChangedViewModel IV_VoltageChangedModel = new IV_VoltageChangedViewModel();
        public static IV_CurrentChangedViewModel IV_CurrentChangedModel = new IV_CurrentChangedViewModel();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region For reading settings from form

        NumberStyles numberStyle = NumberStyles.Float;
        CultureInfo culture = CultureInfo.InvariantCulture;

        #endregion

        #region Abstract hardware

        I_SMU DeviceChannel_01;
        I_SMU DeviceChannel_02;

        MotionController _MotionController;

        #endregion

        #region User interfaces to set device settings

        private MotorConfiguration motorConfiguration;

        private SourceDeviceConfiguration sourceDeviceConfigurationChannel_01;
        private SourceDeviceConfiguration sourceDeviceConfigurationChannel_02;

        #endregion

        #region I-V data handling and presenting

        #region 1-st channel

        private List<Point> _CurrentIV_CurveChannel_01;
        public List<Point> CurrentIV_CurveChannel_01
        {
            get { return _CurrentIV_CurveChannel_01; }
            set { _CurrentIV_CurveChannel_01 = value; }
        }

        private ExperimentalIV_DataSource _experimentalIV_DataSourceChannel_01;
        private LineGraph _IV_LineGraphChannel_01;

        MeasureIV IV_CurveChannel_01;
        BackgroundWorker backgroundIV_MeasureChannel_01;

        private IV_MeasurementLog _IV_MeasurementLogChannel_01;
        private IV_SingleMeasurement _IV_SingleMeasurementChannel_01;

        private System.Windows.Forms.FolderBrowserDialog _SaveIV_MeasureDialogChannel_01;
        private string _SaveIV_MeasuremrentFileNameChannel_01;

        #endregion

        #region 2-nd channel

        private List<Point> _CurrentIV_CurveChannel_02;
        public List<Point> CurrentIV_CurveChannel_02
        {
            get { return _CurrentIV_CurveChannel_02; }
            set { _CurrentIV_CurveChannel_02 = value; }
        }

        private ExperimentalIV_DataSource _experimentalIV_DataSourceChannel_02;
        private LineGraph _IV_LineGraphChannel_02;

        MeasureIV IV_CurveChannel_02;
        BackgroundWorker backgroundIV_MeasureChannel_02;

        private IV_MeasurementLog _IV_MeasurementLogChannel_02;
        private IV_SingleMeasurement _IV_SingleMeasurementChannel_02;

        private System.Windows.Forms.FolderBrowserDialog _SaveIV_MeasureDialogChannel_02;
        private string _SaveIV_MeasuremrentFileNameChannel_02;

        #endregion

        IV_MeasurementSettingsDataModel _IV_ExperimentSettings;

        #endregion

        #region Time Trace data handling and presenting

        #region 1-st channel

        private ExperimentalTimeTraceDataSource _experimentalTimeTraceDataSourceChannel_01;
        private LineGraph _TimeTraceLineGraphChannel_01;

        MeasureTimeTrace TimeTraceCurveChannel_01;
        BackgroundWorker backgroundTimeTraceMeasureChannel_01;

        private TimeTraceMeasurementLog _TimeTraceMeasurementLogChannel_01;
        private TimeTraceSingleMeasurement _TimeTraceSingleMeasurementChannel_01;

        private System.Windows.Forms.FolderBrowserDialog _SaveTimeTraceMeasureDialogChannel_01;
        private string _SaveTimeTraceMeasuremrentFileNameChannel_01;

        #endregion

        #region 2-nd channel

        private ExperimentalTimeTraceDataSource _experimentalTimeTraceDataSourceChannel_02;
        private LineGraph _TimeTraceLineGraphChannel_02;

        MeasureTimeTrace TimeTraceCurveChannel_02;
        BackgroundWorker backgroundTimeTraceMeasureChannel_02;

        private TimeTraceMeasurementLog _TimeTraceMeasurementLogChannel_02;
        private TimeTraceSingleMeasurement _TimeTraceSingleMeasurementChannel_02;

        private System.Windows.Forms.FolderBrowserDialog _SaveTimeTraceMeasureDialogChannel_02;
        private string _SaveTimeTraceMeasuremrentFileNameChannel_02;

        #endregion

        private MeasureTimeTraceChannelController _ChannelController;

        private TimeTraceMeasurementSettingsDataModel _TimeTraceExperimentSettings;

        #endregion

        #region Real Time Time Trace data handling and presenting

        #region Sample 01

        private LinkedList<Point> _RealTimeTimeTraceSample_01;
        public LinkedList<Point> RealTimeTimeTraceSample_01
        {
            get { return _RealTimeTimeTraceSample_01; }
            set { _RealTimeTimeTraceSample_01 = value; }
        }

        private Experimental_RealTime_TimeTrace_DataSource_Sample _ExperimentalRealTimeTimetraceDataSourceSample_01;
        private LineGraph _RealTimeTimeTraceLineGraphSample_01;

        #endregion

        #region Sample 02

        private LinkedList<Point> _RealTimeTimeTraceSample_02;
        public LinkedList<Point> RealTimeTimeTraceSample_02
        {
            get { return _RealTimeTimeTraceSample_02; }
            set { _RealTimeTimeTraceSample_02 = value; }
        }

        private Experimental_RealTime_TimeTrace_DataSource_Sample _ExperimentalRealTimeTimetraceDataSourceSample_02;
        private LineGraph _RealTimeTimeTraceLineGraphSample_02;

        #endregion

        private RealTime_TimeTraceMeasurementLog _RealTime_TimeTraceMeasurementLogSamples;
        private RealTime_TimeTraceSingleMeasurement _RealTime_TimeTraceSingleMeasurementSamples;

        private RealTimeTimeTraceMeasurementSettingsDataModel _RealTimeTimeTraceExperimentSettings;

        private BackgroundWorker _Background_RealTime_TimeTrace_Measurement;
        private MeasureRealTimeTimeTrace _RealTime_TimeTrace_Curve;

        private System.Windows.Forms.FolderBrowserDialog _SaveRealTimeTraceMeasureDataDialog;
        private string _SaveRealTimeTraceMeasuremrentDataFileName;

        #endregion

        #region Noise data handling and presenting

        #region Sample 01

        private ExperimentalNoiseSpectra_DataSource _Experimental_Noise_DataSource_Sample_01;
        private LineGraph _Noise_LineGraph_Sample_01;

        #endregion

        #region Sample 02

        private ExperimentalNoiseSpectra_DataSource _Experimental_Noise_DataSource_Sample_02;
        private LineGraph _Noise_LineGraph_Sample_02;

        #endregion

        private BackgroundWorker _Background_NoiseMeasurement;
        private MeasureNoise _Noise_Spectra;

        NoiseCalibrationSingleMeasurement _SingleCalibrationMeasurement_CH_01;
        NoiseCalibrationSingleMeasurement _SingleCalibrationMeasurement_CH_02;

        private string _NoiseCalibrationDataFileName;
        private string _NoiseDataFileName;

        #endregion

        public MainWindow()
        {
            #region Modifying current process to have higjest possible priority

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            #endregion

            #region Registry settings

            BreakJunctionsRegistry.Instance.CreateApplicationRegistry();

            #endregion

            #region Initializing graphical components

            this.InitializeComponent();

            #endregion

            #region Removing Legend From Charts

            chartTimeTraceChannel_01.AxisGrid.DrawVerticalMinorTicks = true;
            chartTimeTraceChannel_02.AxisGrid.DrawVerticalMinorTicks = true;

            chartNoiseSample_01.AxisGrid.DrawVerticalMinorTicks = true;
            chartNoiseSample_02.AxisGrid.DrawVerticalMinorTicks = true;

            chartNoiseSample_01.AxisGrid.DrawHorizontalMinorTicks = true;
            chartNoiseSample_02.AxisGrid.DrawHorizontalMinorTicks = true;

            chartIV_CurvesChannel_01.LegendVisibility = System.Windows.Visibility.Hidden;
            chartIV_CurvesChannel_02.LegendVisibility = System.Windows.Visibility.Hidden;
            chartTimeTraceChannel_01.LegendVisibility = System.Windows.Visibility.Hidden;
            chartTimeTraceChannel_02.LegendVisibility = System.Windows.Visibility.Hidden;

            chartIV_CurvesChannel_01.Children.Remove(chartIV_CurvesChannel_01.Legend);
            chartIV_CurvesChannel_02.Children.Remove(chartIV_CurvesChannel_02.Legend);
            chartTimeTraceChannel_01.Children.Remove(chartTimeTraceChannel_01.Legend);
            chartTimeTraceChannel_02.Children.Remove(chartTimeTraceChannel_02.Legend);

            chartRealTimeTimeTraceSample_01.LegendVisibility = System.Windows.Visibility.Hidden;
            chartRealTimeTimeTraceSample_02.LegendVisibility = System.Windows.Visibility.Hidden;

            chartRealTimeTimeTraceSample_01.Children.Remove(chartRealTimeTimeTraceSample_01.Legend);
            chartRealTimeTimeTraceSample_02.Children.Remove(chartRealTimeTimeTraceSample_02.Legend);

            chartNoiseSample_01.LegendVisibility = System.Windows.Visibility.Hidden;
            chartNoiseSample_02.LegendVisibility = System.Windows.Visibility.Hidden;

            chartNoiseSample_01.Children.Remove(chartNoiseSample_01.Legend);
            chartNoiseSample_02.Children.Remove(chartNoiseSample_02.Legend);

            #endregion

            #region Interface model-view interactions

            #region I-V Model-view interactions

            controlIV_MeasurementSettings.cmdIV_DataFileNameBrowseChannel_01.Click += on_cmdIV_DataFileNameBrowseClickChannel_01;
            controlIV_MeasurementSettings.cmdIV_DataFileNameBrowseChannel_02.Click += on_cmdIV_DataFileNameBrowseClickChannel_02;

            controlIV_MeasurementSettings.cmdIV_StartMeasurement.Click += on_cmdIV_StartMeasurementClick;
            controlIV_MeasurementSettings.cmdIV_StopMeasurement.Click += on_cmdIV_StopMeasurementClick;

            #endregion

            #region TimeTrace Model-view interactions

            controlTimeTraceMeasurementSettings.cmdTimeTraceChannel_01_DataFileNameBrowse.Click += on_cmdTimeTraceDataFileNameBrowseClickChannel_01;
            controlTimeTraceMeasurementSettings.cmdTimeTraceChannel_02_DataFileNameBrowse.Click += on_cmdTimeTraceDataFileNameBrowseClickChannel_02;
            controlTimeTraceMeasurementSettings.MotionParameters.cmdTimeTraceDistanceMoveToInitialPosition.Click += on_cmdTimeTraceDistanceMoveToInitialPosition;
            controlTimeTraceMeasurementSettings.cmdTimeTraceStartMeasurement.Click += on_cmdTimeTraceStartMeasurementClick;
            controlTimeTraceMeasurementSettings.cmdTimeTraceStopMeasurement.Click += on_cmdTimeTraceStopMeasurementClick;
            controlTimeTraceMeasurementSettings.MotionParameters.cmdEnableDisableMotor.Click += cmdEnableDisableMotor_Click;

            #endregion

            #region Real time TimeTrace Model-view interactions

            controlRealTimeTimeTraceMeasurementSettings.cmdQuickSampleCheck.Click += on_cmdRealTime_TimeTrace_QuickSampleCheckClick;
            controlRealTimeTimeTraceMeasurementSettings.cmdStartMeasurement.Click += on_cmdRealTime_TimeTraceStartMeasurementClick;
            controlRealTimeTimeTraceMeasurementSettings.cmdStopMeasurement.Click += on_cmdRealTime_TimeTraceStopMeasurementClick;
            controlRealTimeTimeTraceMeasurementSettings.cmdSaveFile.Click += on_cmdRealTime_TimeTrace_SaveFileClick;

            AllEventsHandler.Instance.RealTime_TimeTrace_AveragedDataArrived_Sample_01 += OnRealTime_TimeTrace_AveragedDataArrived_Sample_01;
            AllEventsHandler.Instance.RealTime_TimeTrace_AveragedDataArrived_Sample_02 += OnRealTime_TimeTrace_AveragedDataArrived_Sample_02;

            AllEventsHandler.Instance.RealTime_TimeTraceMeasurementStateChanged += OnRealTime_TimeTraceMeasurementStateChanged;

            #endregion

            #region Noise Model-view interactions

            controlNoiseTraceMeasurementSettings.cmd_SaveNoiseCalibrationData.Click += On_NoiseCalibration_OpenFile;

            controlNoiseTraceMeasurementSettings.cmd_PerformCalibration.Click += On_NoiseCalibration_Start_Click;

            controlNoiseTraceMeasurementSettings.cmd_SaveNoiseData.Click += On_Noise_OpenFile_Click;
            controlNoiseTraceMeasurementSettings.cmd_NoiseMeasurementStart.Click += On_NoiseMeasurement_Start_Click;

            controlNoiseTraceMeasurementSettings.cmd_NoiseMeasuremntStop.Click += On_NoiseMeasuremntStop_Click;

            #endregion

            AllEventsHandler.Instance.Motion += OnMotionPositionMeasured;
            AllEventsHandler.Instance.Motion_RealTime += OnMotion_RealTime;
            AllEventsHandler.Instance.MotionDirectionChanged += MotionDirectionChanged;
            AllEventsHandler.Instance.ResistanceMeasured += TimeTrace_ResistanceMeasured;

            #endregion

            #region Background I-V Measuremrent

            #region 1-st channel

            backgroundIV_MeasureChannel_01 = new BackgroundWorker();
            backgroundIV_MeasureChannel_01.WorkerSupportsCancellation = true;
            backgroundIV_MeasureChannel_01.WorkerReportsProgress = true;
            backgroundIV_MeasureChannel_01.DoWork += backgroundIV_Measure_DoWorkChannel_01;
            backgroundIV_MeasureChannel_01.ProgressChanged += backgroundIV_Measure_ProgressChangedChannel_01;
            backgroundIV_MeasureChannel_01.RunWorkerCompleted += backgroundIV_Measure_RunWorkerCompletedChannel_01;

            #endregion

            #region 2-nd channel

            backgroundIV_MeasureChannel_02 = new BackgroundWorker();
            backgroundIV_MeasureChannel_02.WorkerSupportsCancellation = true;
            backgroundIV_MeasureChannel_02.WorkerReportsProgress = true;
            backgroundIV_MeasureChannel_02.DoWork += backgroundIV_Measure_DoWorkChannel_02;
            backgroundIV_MeasureChannel_02.ProgressChanged += backgroundIV_Measure_ProgressChangedChannel_02;
            backgroundIV_MeasureChannel_02.RunWorkerCompleted += backgroundIV_Measure_RunWorkerCompletedChannel_02;

            #endregion

            #endregion

            #region Background Time Trace Measurement

            #region 1-st channel

            backgroundTimeTraceMeasureChannel_01 = new BackgroundWorker();
            backgroundTimeTraceMeasureChannel_01.WorkerSupportsCancellation = true;
            backgroundTimeTraceMeasureChannel_01.WorkerReportsProgress = true;
            backgroundTimeTraceMeasureChannel_01.DoWork += backgroundTimeTraceMeasureDoWorkChannel_01;
            backgroundTimeTraceMeasureChannel_01.ProgressChanged += backgroundTimeTraceMeasureProgressChangedChannel_01;
            backgroundTimeTraceMeasureChannel_01.RunWorkerCompleted += backgroundTimeTrace_RunWorkerCompletedChannel_01;

            #endregion

            #region 2-nd channel

            backgroundTimeTraceMeasureChannel_02 = new BackgroundWorker();
            backgroundTimeTraceMeasureChannel_02.WorkerSupportsCancellation = true;
            backgroundTimeTraceMeasureChannel_02.WorkerReportsProgress = true;
            backgroundTimeTraceMeasureChannel_02.DoWork += backgroundTimeTraceMeasureDoWorkChannel_02;
            backgroundTimeTraceMeasureChannel_02.ProgressChanged += backgroundTimeTraceMeasureProgressChangedChannel_02;
            backgroundTimeTraceMeasureChannel_02.RunWorkerCompleted += backgroundTimeTrace_RunWorkerCompletedChannel_02;

            #endregion

            #endregion

            #region Background Real Time Time Trace Measurement

            _Background_RealTime_TimeTrace_Measurement = new BackgroundWorker();
            _Background_RealTime_TimeTrace_Measurement.WorkerSupportsCancellation = true;
            _Background_RealTime_TimeTrace_Measurement.WorkerReportsProgress = true;
            _Background_RealTime_TimeTrace_Measurement.DoWork += backgroundRealTime_TimeTraceMeasureDoWork;
            _Background_RealTime_TimeTrace_Measurement.RunWorkerCompleted += backgroundRealTime_TimeTraceMeasureRunWorkerCompleted;

            #endregion

            #region Background Noise Measurement

            _Background_NoiseMeasurement = new BackgroundWorker();
            _Background_NoiseMeasurement.WorkerSupportsCancellation = true;
            _Background_NoiseMeasurement.WorkerReportsProgress = true;
            _Background_NoiseMeasurement.DoWork += backgroundNoiseMeasure_DoWork;
            _Background_NoiseMeasurement.ProgressChanged += _Background_NoiseMeasurement_ProgressChanged;
            _Background_NoiseMeasurement.RunWorkerCompleted += _Background_NoiseMeasurement_RunWorkerCompleted;

            #endregion

            #region Save I-V data to file dialog configuration

            #region 1-st channel

            _SaveIV_MeasuremrentFileNameChannel_01 = string.Empty;
            _SaveIV_MeasureDialogChannel_01 = new System.Windows.Forms.FolderBrowserDialog();

            #endregion

            #region 2-nd channel

            _SaveIV_MeasuremrentFileNameChannel_02 = string.Empty;
            _SaveIV_MeasureDialogChannel_02 = new System.Windows.Forms.FolderBrowserDialog();

            #endregion

            #endregion

            #region Save Time Trace data to file dialog configuration

            #region 1-st channel

            _SaveTimeTraceMeasuremrentFileNameChannel_01 = string.Empty;
            _SaveTimeTraceMeasureDialogChannel_01 = new System.Windows.Forms.FolderBrowserDialog();

            #endregion

            #region 2-nd channel

            _SaveTimeTraceMeasuremrentFileNameChannel_02 = string.Empty;
            _SaveTimeTraceMeasureDialogChannel_02 = new System.Windows.Forms.FolderBrowserDialog();

            #endregion

            #endregion

            #region Save Real Time Time trace data to fie dialog configuration

            _SaveRealTimeTraceMeasuremrentDataFileName = string.Empty;
            _SaveRealTimeTraceMeasureDataDialog = new System.Windows.Forms.FolderBrowserDialog();

            #endregion
        }

        #region Menu actions realization

        private void onMenuOpenClick(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private void onMenuSaveClick(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private void onMenuExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void onSetMotor_Click(object sender, RoutedEventArgs e)
        {
            motorConfiguration = new MotorConfiguration();
            motorConfiguration.ShowDialog();
        }

        private void onSetSMU_Click(object sender, RoutedEventArgs e)
        {
            sourceDeviceConfigurationChannel_01 = new SourceDeviceConfiguration();
            sourceDeviceConfigurationChannel_01.ShowDialog();
        }

        private void onSetSMU_Channel_02_Click(object sender, RoutedEventArgs e)
        {
            sourceDeviceConfigurationChannel_02 = new SourceDeviceConfiguration();
            sourceDeviceConfigurationChannel_02.ShowDialog();
        }

        #endregion

        #region I-V Measurements Interface Interactions

        /// <summary>
        /// Initializes the new I-V curve measurements
        /// </summary>
        /// <returns>true, if initialization succeed and false otherwise</returns>
        private bool InitIV_Measurements()
        {
            #region SMU, rendering and save data configurations

            //Checking the SMU settings
            if ((sourceDeviceConfigurationChannel_01 != null) && (sourceDeviceConfigurationChannel_02 != null))
            {
                #region Chart rendering settings

                #region 1-st channel

                //Initializing a new plot on I-V chart
                if (_IV_LineGraphChannel_01 != null)
                {
                    //Detaching receive event from "old" data source
                    _experimentalIV_DataSourceChannel_01.DetachPointReceiveEvent();
                    _IV_LineGraphChannel_01.RemoveFromPlotter();
                }
                //Creating new plot and attaching it to the chart
                _experimentalIV_DataSourceChannel_01 = new ExperimentalIV_DataSourceChannel(ChannelsToInvestigate.Channel_01);
                _experimentalIV_DataSourceChannel_01.AttachPointReceiveEvent();
                _IV_LineGraphChannel_01 = new LineGraph(_experimentalIV_DataSourceChannel_01);
                _IV_LineGraphChannel_01.AddToPlotter(chartIV_CurvesChannel_01);

                #endregion

                #region 2-nd channel

                //Initializing a new plot on I-V chart
                if (_IV_LineGraphChannel_02 != null)
                {
                    //Detaching receive event from "old" data source
                    _experimentalIV_DataSourceChannel_02.DetachPointReceiveEvent();
                    _IV_LineGraphChannel_02.RemoveFromPlotter();
                }
                //Creating new plot and attaching it to the chart
                _experimentalIV_DataSourceChannel_02 = new ExperimentalIV_DataSourceChannel(ChannelsToInvestigate.Channel_02);
                _experimentalIV_DataSourceChannel_02.AttachPointReceiveEvent();
                _IV_LineGraphChannel_02 = new LineGraph(_experimentalIV_DataSourceChannel_02);
                _IV_LineGraphChannel_02.AddToPlotter(chartIV_CurvesChannel_02);

                #endregion

                #endregion

                //Getting SMU device

                /*      Better implementation for lot of SMU kinds neened     */

                switch (sourceDeviceConfigurationChannel_01.SelectedSource)
                {
                    case AvailableSources.KEITHLEY_2602A:
                        {
                            DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings.Device;
                        } break;
                    case AvailableSources.KEITHLEY_4200:
                        {
                            DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley4200_DeviceSettings.Device;
                        } break;
                    default:
                        throw new Exception("Non supported source for channel 01 selected!");
                }

                switch (sourceDeviceConfigurationChannel_02.SelectedSource)
                {
                    case AvailableSources.KEITHLEY_2602A:
                        {
                            DeviceChannel_02 = sourceDeviceConfigurationChannel_02.Keithley2602A_DeviceSettings.Device;
                        } break;
                    case AvailableSources.KEITHLEY_4200:
                        {
                            DeviceChannel_02 = sourceDeviceConfigurationChannel_02.Keithley4200_DeviceSettings.Device;
                        } break;
                    default:
                        throw new Exception("Non supported source for channel 01 selected!");
                }

                #region I-V measurement configuration

                #region General configuration

                _IV_ExperimentSettings = controlIV_MeasurementSettings.MeasurementSettings;

                var NumberOfAverages = _IV_ExperimentSettings.IV_MeasurementNumberOfAverages;
                var TimeDelay = _IV_ExperimentSettings.IV_MeasurementTimeDelay;

                SourceMode DeviceSourceMode = SourceMode.Voltage;

                if (_IV_ExperimentSettings.IsIV_MeasurementVoltageModeChecked == true)
                {
                    DeviceSourceMode = SourceMode.Voltage;
                }
                else if (_IV_ExperimentSettings.IsIV_MeasurementCurrentModeChecked == true)
                {
                    DeviceSourceMode = SourceMode.Current;
                }

                var Thread_01_Step = new AutoResetEvent(false);
                var Thread_02_Step = new AutoResetEvent(true);

                #endregion

                #region 1-st channel settings

                var StartValueChannel_01 = _IV_ExperimentSettings.IV_MeasurementStartValueWithMultiplierChannel_01;
                var EndValueChannel_01 = _IV_ExperimentSettings.IV_MeasurementEndValueWithMultiplierChannel_01;
                var StepChannel_01 = _IV_ExperimentSettings.IV_MeasurementStepWithMultiplierChannel_01;

                IV_CurveChannel_01 = new MeasureIV(StartValueChannel_01, EndValueChannel_01, StepChannel_01, NumberOfAverages, TimeDelay, DeviceSourceMode, DeviceChannel_01, ChannelsToInvestigate.Channel_01, ref Thread_01_Step, ref Thread_02_Step);

                #endregion

                #region 2-nd channel settings

                var StartValueChannel_02 = _IV_ExperimentSettings.IV_MeasurementStartValueWithMultiplierChannel_02;
                var EndValueChannel_02 = _IV_ExperimentSettings.IV_MeasurementEndValueWithMultiplierChannel_02;
                var StepChannel_02 = _IV_ExperimentSettings.IV_MeasurementStepWithMultiplierChannel_02;

                IV_CurveChannel_02 = new MeasureIV(StartValueChannel_02, EndValueChannel_02, StepChannel_02, NumberOfAverages, TimeDelay, DeviceSourceMode, DeviceChannel_02, ChannelsToInvestigate.Channel_02, ref Thread_01_Step, ref Thread_02_Step);

                #endregion

                #endregion

                #region Saving I-V data into files

                var newFileNameChannel_01 = string.Empty;
                var newFileNameChannel_02 = string.Empty;

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_01) && !string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_02))
                {
                    _IV_MeasurementLogChannel_01 = new IV_MeasurementLog((new FileInfo(_SaveIV_MeasuremrentFileNameChannel_01)).DirectoryName + "\\IV_MeasurementLogChannel_01.dat");
                    _IV_MeasurementLogChannel_02 = new IV_MeasurementLog((new FileInfo(_SaveIV_MeasuremrentFileNameChannel_02)).DirectoryName + "\\IV_MeasurementLogChannel_02.dat");

                    newFileNameChannel_01 = GetFileNameWithIncrement(_SaveIV_MeasuremrentFileNameChannel_01);
                    newFileNameChannel_02 = GetFileNameWithIncrement(_SaveIV_MeasuremrentFileNameChannel_02);
                }

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_01) && !string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_02))
                {
                    var fileNameChannel_01 = (new FileInfo(newFileNameChannel_01)).Name;
                    var fileNameChannel_02 = (new FileInfo(newFileNameChannel_02)).Name;

                    var sourceMode = string.Empty;

                    if (_IV_ExperimentSettings.IsIV_MeasurementVoltageModeChecked == true)
                    {
                        sourceMode = "Source mode: Voltage";
                    }
                    else if (_IV_ExperimentSettings.IsIV_MeasurementCurrentModeChecked == true)
                    {
                        sourceMode = "SourceMode: Current";
                    }

                    //Some Comment
                    double micrometricBoltPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.CurrentPosition;

                    string comment = _IV_ExperimentSettings.IV_MeasurementDataComment;

                    _IV_MeasurementLogChannel_01.AddNewIV_MeasurementLog(fileNameChannel_01, sourceMode, micrometricBoltPosition, comment);
                    _IV_MeasurementLogChannel_02.AddNewIV_MeasurementLog(fileNameChannel_02, sourceMode, micrometricBoltPosition, comment);
                }

                if ((_IV_SingleMeasurementChannel_01 != null) && (_IV_SingleMeasurementChannel_02 != null))
                {
                    _IV_SingleMeasurementChannel_01.Dispose();
                    _IV_SingleMeasurementChannel_02.Dispose();
                }


                _IV_SingleMeasurementChannel_01 = new IV_SingleMeasurement(newFileNameChannel_01, ChannelsToInvestigate.Channel_01);
                _IV_SingleMeasurementChannel_02 = new IV_SingleMeasurement(newFileNameChannel_02, ChannelsToInvestigate.Channel_02);

                #endregion

                return true;
            }
            else
            {
                MessageBox.Show("The channels of the device are not initialized.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            #endregion
        }

        private void on_cmdIV_StartMeasurementClick(object sender, RoutedEventArgs e)
        {
            if (backgroundIV_MeasureChannel_01.IsBusy == true || backgroundIV_MeasureChannel_02.IsBusy == true)
            {
                on_cmdIV_StopMeasurementClick(sender, e);
                Thread.Sleep(1000);
            }

            var isInitSuccess = InitIV_Measurements();

            //Starting I-V measurements in background
            if ((isInitSuccess == true) && (backgroundIV_MeasureChannel_01.IsBusy == false) && (backgroundIV_MeasureChannel_02.IsBusy == false))
            {
                backgroundIV_MeasureChannel_01.RunWorkerAsync();
                backgroundIV_MeasureChannel_02.RunWorkerAsync();
            }
        }

        private void on_cmdIV_StopMeasurementClick(object sender, RoutedEventArgs e)
        {
            //Canceling I-V measures
            if (backgroundIV_MeasureChannel_01.IsBusy == true)
                backgroundIV_MeasureChannel_01.CancelAsync();
            if (backgroundIV_MeasureChannel_02.IsBusy == true)
                backgroundIV_MeasureChannel_02.CancelAsync();
        }

        #region 1-st Channel Background Work

        private void backgroundIV_Measure_DoWorkChannel_01(object sender, DoWorkEventArgs e)
        {
            //Updating interface to show that measurement is in process
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.labelMeasurementStatusChannel_01.Content = "In process...";
            }));

            //Starting measurements
            IV_CurveChannel_01.StartMeasurement(sender, e);
        }

        private void backgroundIV_Measure_ProgressChangedChannel_01(object sender, ProgressChangedEventArgs e)
        {
            //Updating interface to show measurement progress
            this.progressBarMeasurementProgressChannel_01.Value = e.ProgressPercentage;
        }

        private void backgroundIV_Measure_RunWorkerCompletedChannel_01(object sender, RunWorkerCompletedEventArgs e)
        {
            //Updating interface to show that measurement is completed
            this.labelMeasurementStatusChannel_01.Content = "Ready";
        }

        private void on_cmdIV_DataFileNameBrowseClickChannel_01(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            var dialogResult = _SaveIV_MeasureDialogChannel_01.ShowDialog();

            var _FileName = controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementDataFileNameChannel_01.EndsWith(".dat") ?
                    controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementDataFileNameChannel_01
                    : controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementDataFileNameChannel_01 + ".dat";

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _SaveIV_MeasuremrentFileNameChannel_01 = String.Format("{0}\\{1}", _SaveIV_MeasureDialogChannel_01.SelectedPath, _FileName);
            else
            {
                var _Path = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyy.MM.dd"));
                if (!Directory.Exists(_Path))
                    Directory.CreateDirectory(_Path);

                _SaveIV_MeasuremrentFileNameChannel_01 = String.Format("{0}\\{1}", _Path, _FileName);
            }
        }

        #endregion

        #region 2-nd Channel Background Work

        private void backgroundIV_Measure_DoWorkChannel_02(object sender, DoWorkEventArgs e)
        {
            //Updating interface to show that measurement is in process
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.labelMeasurementStatusChannel_02.Content = "In process...";
            }));

            //Starting measurements
            IV_CurveChannel_02.StartMeasurement(sender, e);
        }

        private void backgroundIV_Measure_ProgressChangedChannel_02(object sender, ProgressChangedEventArgs e)
        {
            //Updating interface to show measurement progress
            this.progressBarMeasurementProgressChannel_02.Value = e.ProgressPercentage;
        }

        private void backgroundIV_Measure_RunWorkerCompletedChannel_02(object sender, RunWorkerCompletedEventArgs e)
        {
            //Updating interface to show that measurement is completed
            this.labelMeasurementStatusChannel_02.Content = "Ready";
        }

        private void on_cmdIV_DataFileNameBrowseClickChannel_02(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            var dialogResult = _SaveIV_MeasureDialogChannel_02.ShowDialog();

            var _FileName = controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementDataFileNameChannel_02.EndsWith(".dat") ?
                    controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementDataFileNameChannel_02
                    : controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementDataFileNameChannel_02 + ".dat";

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _SaveIV_MeasuremrentFileNameChannel_02 = String.Format("{0}\\{1}", _SaveIV_MeasureDialogChannel_02.SelectedPath, _FileName);
            else
            {
                var _Path = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyy.MM.dd"));
                if (!Directory.Exists(_Path))
                    Directory.CreateDirectory(_Path);

                _SaveIV_MeasuremrentFileNameChannel_02 = String.Format("{0}\\{1}", _Path, _FileName);
            }
        }

        #endregion

        #endregion

        #region Time Trace Measurements Interface Interactions

        private bool InitTimeTraceMeasurements()
        {
            #region SMU, rendering and save data configurations

            if ((sourceDeviceConfigurationChannel_01 != null) && (sourceDeviceConfigurationChannel_01 != null))
            {
                #region Chart rendering settings

                #region 1-st channel

                if (_TimeTraceLineGraphChannel_01 != null)
                {
                    _experimentalTimeTraceDataSourceChannel_01.DetachPointReceiveEvent();
                    _TimeTraceLineGraphChannel_01.RemoveFromPlotter();
                }

                _experimentalTimeTraceDataSourceChannel_01 = new ExperimentalTimetraceDataSourceChannel(ChannelsToInvestigate.Channel_01);
                _experimentalTimeTraceDataSourceChannel_01.AttachPointReceiveEvent();
                _TimeTraceLineGraphChannel_01 = new LineGraph(_experimentalTimeTraceDataSourceChannel_01);
                _TimeTraceLineGraphChannel_01.AddToPlotter(chartTimeTraceChannel_01);

                #endregion

                #region 2-nd channel

                if (_TimeTraceLineGraphChannel_02 != null)
                {
                    _experimentalTimeTraceDataSourceChannel_02.DetachPointReceiveEvent();
                    _TimeTraceLineGraphChannel_02.RemoveFromPlotter();
                }

                _experimentalTimeTraceDataSourceChannel_02 = new ExperimentalTimetraceDataSourceChannel(ChannelsToInvestigate.Channel_02);
                _experimentalTimeTraceDataSourceChannel_02.AttachPointReceiveEvent();
                _TimeTraceLineGraphChannel_02 = new LineGraph(_experimentalTimeTraceDataSourceChannel_02);
                _TimeTraceLineGraphChannel_02.AddToPlotter(chartTimeTraceChannel_02);

                #endregion

                #endregion

                //Configurations for all kinds of SMUs should be listed here

                switch (sourceDeviceConfigurationChannel_01.SelectedSource)
                {
                    case AvailableSources.KEITHLEY_2602A:
                        {
                            if (sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings != null)
                                DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings.Device;
                        } break;
                    case AvailableSources.KEITHLEY_4200:
                        {
                            if (sourceDeviceConfigurationChannel_01.Keithley4200_DeviceSettings != null)
                                DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley4200_DeviceSettings.Device;
                        } break;
                    default:
                        throw new Exception("Not supported SMU for channel 1 selected!");
                }

                switch (sourceDeviceConfigurationChannel_02.SelectedSource)
                {
                    case AvailableSources.KEITHLEY_2602A:
                        {
                            if (sourceDeviceConfigurationChannel_02.Keithley2602A_DeviceSettings != null)
                                DeviceChannel_02 = sourceDeviceConfigurationChannel_02.Keithley2602A_DeviceSettings.Device;
                        } break;
                    case AvailableSources.KEITHLEY_4200:
                        {
                            if (sourceDeviceConfigurationChannel_02.Keithley4200_DeviceSettings != null)
                                DeviceChannel_02 = sourceDeviceConfigurationChannel_02.Keithley4200_DeviceSettings.Device;
                        } break;
                    default:
                        throw new Exception("Not supported SMU for channel 2 selected!");
                }

                #region Time trace measurement configuration

                _TimeTraceExperimentSettings = controlTimeTraceMeasurementSettings.MeasurementSettings;

                //Configurations for all kinds of motors should be listed here

                switch (motorConfiguration.SelectedMotionController)
                {
                    case AvailableMotionControllers.Faulhaber_2036_U012V:
                        {
                            if (motorConfiguration.Faulhaber_2036_U012V_Settings != null)
                                _MotionController = motorConfiguration.Faulhaber_2036_U012V_Settings.motionController;
                        } break;
                    case AvailableMotionControllers.PI_E755:
                        {
                            if (motorConfiguration.PI_E755_Settings != null)
                                _MotionController = motorConfiguration.PI_E755_Settings.motionController;
                        } break;
                    default:
                        throw new Exception("Unsupported motion controller selected!");
                }

                _MotionController.PointsPerMilimeter = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.PointsPerMilimeter;

                if ((TimeTraceCurveChannel_01 != null) && (TimeTraceCurveChannel_02 != null))
                {
                    TimeTraceCurveChannel_01.Dispose();
                    TimeTraceCurveChannel_02.Dispose();
                }

                if (_ChannelController != null)
                    _ChannelController.Dispose();

                _ChannelController = new MeasureTimeTraceChannelController();

                var timeTraceChannel_01_ValueThroughTheStructure = _TimeTraceExperimentSettings.TimeTraceMeasurementChannel_01_ValueThrougtTheStructureWithMultiplier;
                var isTimeTraceChannel_01_VoltageModeChecked = _TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_01_VoltageModeChecked;
                var isTimeTraceChannel_01_CurrentModeChecked = _TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_01_CurrentModeChecked;

                var timeTraceChannel_02_ValueThroughTheStructure = _TimeTraceExperimentSettings.TimeTraceMeasurementChannel_02_ValueThrougtTheStructureWithMultiplier;
                var isTimeTraceChannel_02_VoltageModeChecked = _TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_02_VoltageModeChecked;
                var isTimeTraceChannel_02_CurrentModeChecked = _TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_02_CurrentModeChecked;

                var selectedTimeTraceModeItem = (controlTimeTraceMeasurementSettings.MotionParameters.tabControlTimeTraceMeasurementParameters.SelectedItem as TabItem).Header.ToString();

                switch (selectedTimeTraceModeItem)
                {
                    case "Distance":
                        {
                            var motionStartPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.StartPosition;
                            var motionFinalDestination = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.FinalDestination;

                            if (isTimeTraceChannel_01_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_01, SourceMode.Voltage, MeasureMode.Conductance, timeTraceChannel_01_ValueThroughTheStructure, ChannelsToInvestigate.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_01_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_01, SourceMode.Current, MeasureMode.Conductance, timeTraceChannel_01_ValueThroughTheStructure, ChannelsToInvestigate.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }

                            if (isTimeTraceChannel_02_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_02, SourceMode.Voltage, MeasureMode.Conductance, timeTraceChannel_02_ValueThroughTheStructure, ChannelsToInvestigate.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_02_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_02, SourceMode.Current, MeasureMode.Conductance, timeTraceChannel_02_ValueThroughTheStructure, ChannelsToInvestigate.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                        } break;
                    case "Distance (Repetitive)":
                        {
                            var motionRepetitiveStartPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.StartPosition;
                            var motionRepetitiveEndPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.FinalDestination;

                            if (isTimeTraceChannel_01_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_01, SourceMode.Voltage, MeasureMode.Conductance, timeTraceChannel_01_ValueThroughTheStructure, ChannelsToInvestigate.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;

                                TimeTraceCurveChannel_01.Motor.AcquireClosingCurves = _TimeTraceExperimentSettings.EliminateClosing;
                            }
                            else if (isTimeTraceChannel_01_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_01, SourceMode.Current, MeasureMode.Conductance, timeTraceChannel_01_ValueThroughTheStructure, ChannelsToInvestigate.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;

                                TimeTraceCurveChannel_01.Motor.AcquireClosingCurves = _TimeTraceExperimentSettings.EliminateClosing;
                            }

                            if (isTimeTraceChannel_02_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_02, SourceMode.Voltage, MeasureMode.Conductance, timeTraceChannel_02_ValueThroughTheStructure, ChannelsToInvestigate.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;

                                TimeTraceCurveChannel_02.Motor.AcquireClosingCurves = _TimeTraceExperimentSettings.EliminateClosing;
                            }
                            else if (isTimeTraceChannel_02_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_02, SourceMode.Current, MeasureMode.Conductance, timeTraceChannel_02_ValueThroughTheStructure, ChannelsToInvestigate.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;

                                TimeTraceCurveChannel_02.Motor.AcquireClosingCurves = _TimeTraceExperimentSettings.EliminateClosing;
                            }
                        } break;
                    case "Time":
                        {
                        } break;
                    case "Fixed R":
                        {
                            var motionStartPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.StartPosition;
                            var motionFinalDestination = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.FinalDestination;

                            if (isTimeTraceChannel_01_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_01, SourceMode.Voltage, MeasureMode.Conductance, timeTraceChannel_01_ValueThroughTheStructure, ChannelsToInvestigate.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_01_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_01, SourceMode.Current, MeasureMode.Conductance, timeTraceChannel_01_ValueThroughTheStructure, ChannelsToInvestigate.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }

                            if (isTimeTraceChannel_02_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_02, SourceMode.Voltage, MeasureMode.Conductance, timeTraceChannel_02_ValueThroughTheStructure, ChannelsToInvestigate.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_02_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_02, SourceMode.Current, MeasureMode.Conductance, timeTraceChannel_02_ValueThroughTheStructure, ChannelsToInvestigate.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                        } break;
                    default:
                        break;
                }

                TimeTraceCurveChannel_01.CurrentPosition = controlTimeTraceMeasurementSettings.MeasurementSettings.MotionSettings.CurrentPosition;
                TimeTraceCurveChannel_02.CurrentPosition = controlTimeTraceMeasurementSettings.MeasurementSettings.MotionSettings.CurrentPosition;

                #endregion

                #region Saving Time Trace data into files

                var newFileNameChannel_01 = string.Empty;
                var newFileNameChannel_02 = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_01) && !string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_02))
                {
                    _TimeTraceMeasurementLogChannel_01 = new TimeTraceMeasurementLog((new FileInfo(_SaveTimeTraceMeasuremrentFileNameChannel_01)).DirectoryName + "\\TimeTraceMeasurementLogChannel_01.dat");
                    _TimeTraceMeasurementLogChannel_02 = new TimeTraceMeasurementLog((new FileInfo(_SaveTimeTraceMeasuremrentFileNameChannel_02)).DirectoryName + "\\TimeTraceMeasurementLogChannel_02.dat");

                    newFileNameChannel_01 = GetFileNameWithIncrement(_SaveTimeTraceMeasuremrentFileNameChannel_01);
                    newFileNameChannel_02 = GetFileNameWithIncrement(_SaveTimeTraceMeasuremrentFileNameChannel_02);
                }

                var sourceModeChannel_01 = string.Empty;
                var sourceModeChannel_02 = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_01) && !string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_02))
                {
                    var fileNameChannel_01 = (new FileInfo(newFileNameChannel_01)).Name;
                    var fileNameChannel_02 = (new FileInfo(newFileNameChannel_02)).Name;

                    if (_TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_01_VoltageModeChecked == true)
                    {
                        sourceModeChannel_01 = "Source mode: Voltage";
                    }
                    else if (_TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_01_CurrentModeChecked == true)
                    {
                        sourceModeChannel_01 = "SourceMode: Current";
                    }

                    if (_TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_02_VoltageModeChecked == true)
                    {
                        sourceModeChannel_02 = "Source mode: Voltage";
                    }
                    else if (_TimeTraceExperimentSettings.IsTimeTraceMeasurementChannel_02_CurrentModeChecked == true)
                    {
                        sourceModeChannel_02 = "SourceMode: Current";
                    }

                    var micrometricBoltPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.CurrentPosition;

                    var comment = _TimeTraceExperimentSettings.TimeTraceMeasurementDataComment;

                    _TimeTraceMeasurementLogChannel_01.AddNewTimeTraceMeasurementLog(fileNameChannel_01, sourceModeChannel_01, timeTraceChannel_01_ValueThroughTheStructure, comment);
                    _TimeTraceMeasurementLogChannel_02.AddNewTimeTraceMeasurementLog(fileNameChannel_02, sourceModeChannel_02, timeTraceChannel_02_ValueThroughTheStructure, comment);
                }

                SourceMode _sourceModeChannel_01 = SourceMode.Voltage; //Source mode is voltage by default
                SourceMode _sourceModeChannel_02 = SourceMode.Voltage;

                if (sourceModeChannel_01 == "Source mode: Voltage")
                    _sourceModeChannel_01 = SourceMode.Voltage;
                else if (sourceModeChannel_01 == "SourceMode: Current")
                    _sourceModeChannel_01 = SourceMode.Current;

                if (sourceModeChannel_02 == "Source mode: Voltage")
                    _sourceModeChannel_02 = SourceMode.Voltage;
                else if (sourceModeChannel_02 == "SourceMode: Current")
                    _sourceModeChannel_02 = SourceMode.Current;

                if (_TimeTraceSingleMeasurementChannel_01 != null)
                    _TimeTraceSingleMeasurementChannel_01.Dispose();

                if (_TimeTraceSingleMeasurementChannel_02 != null)
                    _TimeTraceSingleMeasurementChannel_02.Dispose();

                _TimeTraceSingleMeasurementChannel_01 = new TimeTraceSingleMeasurement(newFileNameChannel_01, _sourceModeChannel_01, ChannelsToInvestigate.Channel_01);
                _TimeTraceSingleMeasurementChannel_02 = new TimeTraceSingleMeasurement(newFileNameChannel_02, _sourceModeChannel_02, ChannelsToInvestigate.Channel_02);

                #endregion

                return true;
            }
            else
            {
                return false;
            }

            #endregion
        }

        private void on_cmdTimeTraceStartMeasurementClick(object sender, RoutedEventArgs e)
        {
            if (backgroundTimeTraceMeasureChannel_01.IsBusy == true || backgroundTimeTraceMeasureChannel_02.IsBusy == true)
            {
                on_cmdTimeTraceStopMeasurementClick(sender, e);
                Thread.Sleep(1000);
            }

            var timeTtraceMeasurementsInitSuccess = InitTimeTraceMeasurements();

            if (timeTtraceMeasurementsInitSuccess)
            {
                AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(true));

                backgroundTimeTraceMeasureChannel_01.RunWorkerAsync();
                backgroundTimeTraceMeasureChannel_02.RunWorkerAsync();
            }
            else MessageBox.Show("The device was not initialized!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void on_cmdTimeTraceStopMeasurementClick(object sender, RoutedEventArgs e)
        {
            if (backgroundTimeTraceMeasureChannel_01.IsBusy == true)
                backgroundTimeTraceMeasureChannel_01.CancelAsync();

            if (backgroundTimeTraceMeasureChannel_02.IsBusy == true)
                backgroundTimeTraceMeasureChannel_02.CancelAsync();

            AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(false));
        }

        private void on_cmdTimeTraceDistanceMoveToInitialPosition(object sender, RoutedEventArgs e)
        {
            if (_MotionController != null)
            {
                _MotionController.MoveToZeroPosition();
            }
        }

        private void cmdEnableDisableMotor_Click(object sender, RoutedEventArgs e)
        {
            var _cmd = sender as ToggleButton;
            if (_cmd == null || _MotionController == null)
                return;

            if (_cmd.IsChecked == true)
                _MotionController.EnableDevice();
            else
                _MotionController.DisableDevice();
        }

        #region 1-st Channel Background Work

        private void backgroundTimeTraceMeasureDoWorkChannel_01(object sender, DoWorkEventArgs e)
        {
            labelMeasurementStatusChannel_01.Dispatcher.BeginInvoke(new Action(() => { labelMeasurementStatusChannel_01.Content = "In progress..."; }));
            if ((DeviceChannel_01 != null) && (DeviceChannel_01.InitDevice()))
            {

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings;

                var numerCycles = ExperimentSettings.NumberCycles;

                var selectedTimeTraceModeHeader = ExperimentSettings.MeasureModeSelectedIndex;

                switch (selectedTimeTraceModeHeader)
                {
                    case 0: //"Distance" measurement
                        {
                            TimeTraceCurveChannel_01.StartMeasurement(sender, e, MotionKind.Single);
                        } break;
                    case 1: //"Distance (Repetitive)" measurement
                        {
                            TimeTraceCurveChannel_01.StartMeasurement(sender, e, MotionKind.Repetitive, numerCycles);
                        } break;
                    case 2: //"Time" measurement
                        {
                        } break;
                    case 3: //"Fixed R" measurement
                        {
                            if (ExperimentSettings.IsChannel_01_Selected)
                                TimeTraceCurveChannel_01.StartMeasurement(sender, e, ExperimentSettings.R_Value, ExperimentSettings.AllowableDeviation);
                        } break;
                    default:
                        break;
                }
            }
            else MessageBox.Show("The device was not initialized!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void backgroundTimeTraceMeasureProgressChangedChannel_01(object sender, ProgressChangedEventArgs e)
        {
            //Updating interface to show measurement progress
            progressBarMeasurementProgressChannel_01.Value = e.ProgressPercentage;
        }

        private void backgroundTimeTrace_RunWorkerCompletedChannel_01(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarMeasurementProgressChannel_01.Value = 0;
            labelMeasurementStatusChannel_01.Content = "Ready";
        }

        #endregion

        #region 2-nd Channel Background Work

        private void backgroundTimeTraceMeasureDoWorkChannel_02(object sender, DoWorkEventArgs e)
        {
            labelMeasurementStatusChannel_02.Dispatcher.BeginInvoke(new Action(() => { labelMeasurementStatusChannel_02.Content = "In progress..."; }));
            if ((DeviceChannel_02 != null) && (DeviceChannel_02.InitDevice()))
            {

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings;
                var numerCycles = ExperimentSettings.NumberCycles;

                var selectedTimeTraceModeHeader = ExperimentSettings.MeasureModeSelectedIndex;

                switch (selectedTimeTraceModeHeader)
                {
                    case 0: //"Distance" measurement
                        {
                            TimeTraceCurveChannel_02.StartMeasurement(sender, e, MotionKind.Single);
                        } break;
                    case 1: //"Distance (Repetitive)" measurement
                        {
                            TimeTraceCurveChannel_02.StartMeasurement(sender, e, MotionKind.Repetitive, numerCycles);
                        } break;
                    case 2: //"Time" measurement
                        {
                        } break;
                    case 3: //"Fixed R" measurement
                        {
                            if (ExperimentSettings.IsChannel_02_Selected)
                                TimeTraceCurveChannel_02.StartMeasurement(sender, e, ExperimentSettings.R_Value, ExperimentSettings.AllowableDeviation);
                        } break;
                    default:
                        break;
                }
            }
            else MessageBox.Show("The device was not initialized!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void backgroundTimeTraceMeasureProgressChangedChannel_02(object sender, ProgressChangedEventArgs e)
        {
            //Updating interface to show measurement progress
            this.progressBarMeasurementProgressChannel_02.Value = e.ProgressPercentage;
        }

        private void backgroundTimeTrace_RunWorkerCompletedChannel_02(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarMeasurementProgressChannel_02.Value = 0;
            labelMeasurementStatusChannel_02.Content = "Ready";
        }

        #endregion

        #region General settings

        private void on_cmdTimeTraceDataFileNameBrowseClickChannel_01(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            var dialogResult = _SaveTimeTraceMeasureDialogChannel_01.ShowDialog();

            var _FileName = controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_01_DataFileName.EndsWith(".dat") ?
                    controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_01_DataFileName
                    : controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_01_DataFileName + ".dat";

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _SaveTimeTraceMeasuremrentFileNameChannel_01 = String.Format("{0}\\{1}", _SaveTimeTraceMeasureDialogChannel_01.SelectedPath, _FileName);
            else
            {
                var _Path = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyy.MM.dd"));
                if (!Directory.Exists(_Path))
                    Directory.CreateDirectory(_Path);

                _SaveIV_MeasuremrentFileNameChannel_01 = String.Format("{0}\\{1}", _Path, _FileName);
            }
        }

        private void on_cmdTimeTraceDataFileNameBrowseClickChannel_02(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            var dialogResult = _SaveTimeTraceMeasureDialogChannel_02.ShowDialog();

            var _FileName = controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_02_DataFileName.EndsWith(".dat") ?
                    controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_02_DataFileName
                    : controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_02_DataFileName + ".dat";

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _SaveTimeTraceMeasuremrentFileNameChannel_02 = String.Format("{0}\\{1}", _SaveTimeTraceMeasureDialogChannel_02.SelectedPath, _FileName);
            else
            {
                var _Path = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyy.MM.dd"));
                if (!Directory.Exists(_Path))
                    Directory.CreateDirectory(_Path);

                _SaveIV_MeasuremrentFileNameChannel_02 = String.Format("{0}\\{1}", _Path, _FileName);
            }
        }

        #endregion

        private void OnMotionPositionMeasured(object sender, Motion_EventArgs e)
        {
            this.controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.CurrentPosition = e.Position;
            this.controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementMicrometricBoltPosition = e.Position;

            if (TimeTraceCurveChannel_01 != null)
                TimeTraceCurveChannel_01.CurrentPosition = e.Position;
            if (TimeTraceCurveChannel_02 != null)
                TimeTraceCurveChannel_02.CurrentPosition = e.Position;
        }

        private void MotionDirectionChanged(object sender, MotionDirectionChanged_EventArgs e)
        {
            switch (e.NewDirection)
            {
                case MotionDirection.Up:
                    {
                        controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.Direction_UP = true;
                    } break;
                case MotionDirection.Down:
                    {
                        controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.Direction_DOWN = true;
                    } break;
            }
        }

        private void TimeTrace_ResistanceMeasured(object sender, TimeTrace_ResistanceMeasured_EventArgs e)
        {
            this.controlTimeTraceMeasurementSettings.MeasurementSettings.MotionSettings.Current_R_Value = e.ResistanceValue;
        }

        #endregion

        #region Real Time Time Trace Measurement Interface Interactions

        private void InitRealTime_TimeTraceMeasurement()
        {
            #region Chart rendering settings

            #region Sample 01

            if (_RealTimeTimeTraceLineGraphSample_01 != null)
            {
                _ExperimentalRealTimeTimetraceDataSourceSample_01.DetachPointReceiveEvent();
                _RealTimeTimeTraceLineGraphSample_01.RemoveFromPlotter();
                _RealTimeTimeTraceSample_01.Clear();
            }

            _RealTimeTimeTraceSample_01 = new LinkedList<Point>();
            _ExperimentalRealTimeTimetraceDataSourceSample_01 = new Experimental_RealTime_TimeTrace_DataSource_Sample(_RealTimeTimeTraceSample_01, SamplesToInvestigate.Sample_01);
            _ExperimentalRealTimeTimetraceDataSourceSample_01.AttachPointReceiveEvent();
            _RealTimeTimeTraceLineGraphSample_01 = new LineGraph(_ExperimentalRealTimeTimetraceDataSourceSample_01);
            _RealTimeTimeTraceLineGraphSample_01.AddToPlotter(chartRealTimeTimeTraceSample_01);

            #endregion

            #region Sample 02

            if (_RealTimeTimeTraceLineGraphSample_02 != null)
            {
                _ExperimentalRealTimeTimetraceDataSourceSample_02.DetachPointReceiveEvent();
                _RealTimeTimeTraceLineGraphSample_02.RemoveFromPlotter();
                _RealTimeTimeTraceSample_02.Clear();
            }

            _RealTimeTimeTraceSample_02 = new LinkedList<Point>();
            _ExperimentalRealTimeTimetraceDataSourceSample_02 = new Experimental_RealTime_TimeTrace_DataSource_Sample(_RealTimeTimeTraceSample_02, SamplesToInvestigate.Sample_02);
            _ExperimentalRealTimeTimetraceDataSourceSample_02.AttachPointReceiveEvent();
            _RealTimeTimeTraceLineGraphSample_02 = new LineGraph(_ExperimentalRealTimeTimetraceDataSourceSample_02);
            _RealTimeTimeTraceLineGraphSample_02.AddToPlotter(chartRealTimeTimeTraceSample_02);

            #endregion

            #endregion
        }

        private void on_cmdRealTime_TimeTrace_QuickSampleCheckClick(object sender, RoutedEventArgs e)
        {
            var cmdQuickSampleCheck = sender as ToggleButton;

            if (cmdQuickSampleCheck.IsChecked == true)
            {
                if (_RealTimeTimeTraceLineGraphSample_01 != null)
                {
                    _RealTime_TimeTraceSingleMeasurementSamples.DetachPointReceiveEvent();

                    _ExperimentalRealTimeTimetraceDataSourceSample_01.DetachPointReceiveEvent();
                    _RealTimeTimeTraceLineGraphSample_01.RemoveFromPlotter();
                    _RealTimeTimeTraceSample_01.Clear();
                }
                if (_RealTimeTimeTraceLineGraphSample_02 != null)
                {
                    _RealTime_TimeTraceSingleMeasurementSamples.AttachPointDataReceiveEvent();

                    _ExperimentalRealTimeTimetraceDataSourceSample_02.DetachPointReceiveEvent();
                    _RealTimeTimeTraceLineGraphSample_02.RemoveFromPlotter();
                    _RealTimeTimeTraceSample_02.Clear();
                }

                AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived;

                if (_RealTime_TimeTrace_Curve != null)
                    _RealTime_TimeTrace_Curve.Dispose();

                _RealTime_TimeTrace_Curve = new MeasureRealTimeTimeTrace();

                _RealTime_TimeTrace_Curve.StartContiniousAcquisitionInThread();
            }
            else
            {
                AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
                _RealTime_TimeTrace_Curve.StopContiniousAcquisitionInThread();
            }
        }

        private void on_cmdRealTime_TimeTrace_SaveFileClick(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            var dialogResult = _SaveRealTimeTraceMeasureDataDialog.ShowDialog();

            var _FileName = controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.SaveFileName.EndsWith(".dat") ?
                    controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.SaveFileName
                    : controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.SaveFileName + ".dat";

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
                _SaveRealTimeTraceMeasuremrentDataFileName = String.Format("{0}\\{1}", _SaveRealTimeTraceMeasureDataDialog.SelectedPath, _FileName);
            else
            {
                var _Path = String.Format("{0}\\{1}", Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyy.MM.dd"));
                if (!Directory.Exists(_Path))
                    Directory.CreateDirectory(_Path);

                _SaveRealTimeTraceMeasuremrentDataFileName = String.Format("{0}\\{1}", _Path, _FileName);
            }
        }

        private void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
                {
                    if (e.Data != null)
                    {
                        //Checking, if the input has data points
                        var minCount = (new int[4] { e.Data[0].Count, e.Data[1].Count, e.Data[2].Count, e.Data[3].Count }).Min();

                        if (minCount > 0)
                        {
                            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Channel_001_Value = e.Data[0].Average(o => o.Y);
                            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Channel_002_Value = e.Data[1].Average(o => o.Y);
                            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Channel_003_Value = e.Data[2].Average(o => o.Y);
                            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Channel_004_Value = e.Data[3].Average(o => o.Y);
                        }
                    }
                }));
        }

        /// <summary>
        /// Starts the real time time trace measurement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_cmdRealTime_TimeTraceStartMeasurementClick(object sender, RoutedEventArgs e)
        {
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartStopEnabled = true;
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartMeasurementButtonEnabled = false;
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStopMeasurementButtonEnabled = true;

            InitRealTime_TimeTraceMeasurement();

            _Background_RealTime_TimeTrace_Measurement.RunWorkerAsync();
        }

        private void _SetGUI_AfterRT_Measurement()
        {
            controlRealTimeTimeTraceMeasurementSettings.Dispatcher.BeginInvoke(new Action(() =>
            {
                controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartStopEnabled = false;
                controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartMeasurementButtonEnabled = true;
                controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStopMeasurementButtonEnabled = false;
            }));
        }

        /// <summary>
        /// Stops the real time measurement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void on_cmdRealTime_TimeTraceStopMeasurementClick(object sender, RoutedEventArgs e)
        {
            _SetGUI_AfterRT_Measurement();
            _Background_RealTime_TimeTrace_Measurement.CancelAsync();
            AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(false));
        }

        private void OnRealTime_TimeTraceMeasurementStateChanged(object sender, RealTime_TimeTraceMeasurementStateChanged_EventArgs e)
        {
            if (e.MeasurementInProcess == false)
                _SetGUI_AfterRT_Measurement();
        }

        private void OnRealTime_TimeTrace_AveragedDataArrived_Sample_01(object sender, RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01 e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Resistance_1st_Sample = e.Averaged_RealTime_TimeTrace_Data * controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.AmplificationCoefficient;
                }));
        }

        private void OnRealTime_TimeTrace_AveragedDataArrived_Sample_02(object sender, RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02 e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Resistance_2nd_Sample = e.Averaged_RealTime_TimeTrace_Data * controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.AmplificationCoefficient;
            }));
        }

        private void OnMotion_RealTime(object sender, Motion_RealTime_EventArgs e)
        {
            this.controlRealTimeTimeTraceMeasurementSettings.MotionSettings.MeasurementSettings.CurrentPosition = e.Position;
        }

        #region Background work

        private void backgroundRealTime_TimeTraceMeasureDoWork(object sender, DoWorkEventArgs e)
        {
            if (_RealTime_TimeTrace_Curve != null)
                _RealTime_TimeTrace_Curve.Dispose();

            if (_RealTime_TimeTraceSingleMeasurementSamples != null)
                _RealTime_TimeTraceSingleMeasurementSamples.Dispose();

            string RealTime_TimeTrace_CurrentDataFile = GetFileNameWithIncrement(_SaveRealTimeTraceMeasuremrentDataFileName); ;

            _RealTime_TimeTraceSingleMeasurementSamples = new RealTime_TimeTraceSingleMeasurement(RealTime_TimeTrace_CurrentDataFile, 0.02, "");
            _RealTime_TimeTrace_Curve = new MeasureRealTimeTimeTrace();

            #region Motion parameters initialization

            var MotionSettings = controlRealTimeTimeTraceMeasurementSettings.MotionSettings.MeasurementSettings;

            var _StartPosition = 0.0;
            var _FinalDestination = 0.0;
            var _MotionKind = MotionKind.Single;
            var _NumberCycles = 1;

            _StartPosition = MotionSettings.StartPosition;
            _FinalDestination = MotionSettings.FinalDestination;

            switch (MotionSettings.MeasureModeSelectedIndex)
            {
                case 0:
                    _MotionKind = MotionKind.Single;
                    break;
                case 1:
                    {
                        _NumberCycles = 2 * MotionSettings.NumberCycles;
                        _MotionKind = MotionKind.Repetitive;
                    } break;
                default:
                    break;
            }

            _RealTime_TimeTrace_Curve.StartPosition = _StartPosition;
            _RealTime_TimeTrace_Curve.FinalDestination = _FinalDestination;

            _RealTime_TimeTrace_Curve.VelosityMovingUp = MotionSettings.SpeedGoing_UP;
            _RealTime_TimeTrace_Curve.VelosityMovingDown = MotionSettings.SpeedGoing_DOWN;

            _RealTime_TimeTrace_Curve.StartMeasurement(_MotionKind, _NumberCycles);

            #endregion
        }

        private void backgroundRealTime_TimeTraceMeasureRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

        #endregion

        #region Noise Measurement Interface Interactions

        private void InitNoiseMeasurement()
        {
            #region Chart rendering settings

            #region Sample 01

            if (_Noise_LineGraph_Sample_01 != null)
            {
                _Experimental_Noise_DataSource_Sample_01.DetachPointReceiveEvent();
                _Noise_LineGraph_Sample_01.RemoveFromPlotter();
            }

            _Experimental_Noise_DataSource_Sample_01 = new ExperimentalNoiseSpectra_DataSource(SamplesToInvestigate.Sample_01);
            _Experimental_Noise_DataSource_Sample_01.AttachPointReceiveEvent();
            _Noise_LineGraph_Sample_01 = new LineGraph(_Experimental_Noise_DataSource_Sample_01);
            _Noise_LineGraph_Sample_01.AddToPlotter(chartNoiseSample_01);

            #endregion

            #region Sample 02

            if (_Noise_LineGraph_Sample_02 != null)
            {
                _Experimental_Noise_DataSource_Sample_02.DetachPointReceiveEvent();
                _Noise_LineGraph_Sample_02.RemoveFromPlotter();
            }

            _Experimental_Noise_DataSource_Sample_02 = new ExperimentalNoiseSpectra_DataSource(SamplesToInvestigate.Sample_02);
            _Experimental_Noise_DataSource_Sample_02.AttachPointReceiveEvent();
            _Noise_LineGraph_Sample_02 = new LineGraph(_Experimental_Noise_DataSource_Sample_02);
            _Noise_LineGraph_Sample_02.AddToPlotter(chartNoiseSample_02);

            #endregion

            #endregion
        }

        private void backgroundNoiseMeasure_DoWork(object sender, DoWorkEventArgs e)
        {
            if (_Noise_Spectra != null)
                _Noise_Spectra.Dispose();

            var MeasurementSettings = controlNoiseTraceMeasurementSettings.MeasurementSettings;

            _Noise_Spectra = new MeasureNoise(MeasurementSettings.MunberOfSpectra, MeasurementSettings.DisplayUpdateNumber, ref _Background_NoiseMeasurement);

            _Noise_Spectra.AmplificationCoefficient_CH_01 = MeasurementSettings.AmplificationCoefficient_CH1;
            _Noise_Spectra.AmplificationCoefficient_CH_02 = MeasurementSettings.AmplificationCoefficient_CH2;

            AllEventsHandler.Instance.On_NoiseMeasurement_StateChanged(this, new NoiseMeasurement_StateChanged_EventArgs(true));

            _Noise_Spectra.MakeNoiseMeasurement();
        }

        void _Background_NoiseMeasurement_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarNoiseMeasurementProgress.Value = e.ProgressPercentage;
        }

        void _Background_NoiseMeasurement_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBarNoiseMeasurementProgress.Value = 0;
            MessageBox.Show("Noise spectra measurement completed!", "Success!", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void On_Noise_OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Data files (*.dat)|*.dat";
            saveDialog.ShowDialog();

            _NoiseDataFileName = saveDialog.FileName;
            controlNoiseTraceMeasurementSettings.MeasurementSettings.SaveFileName = saveDialog.SafeFileName;
        }

        private void On_NoiseMeasurement_Start_Click(object sender, RoutedEventArgs e)
        {
            var _SingleNoiseMeasurement_CH_01 = new NoiseSingleMeasurement(GetFileNameWithIncrement(_NoiseDataFileName.Insert(_NoiseDataFileName.LastIndexOf('.'), "_CH_01_")), ChannelsToInvestigate.Channel_01, ref _SingleCalibrationMeasurement_CH_01);
            var _SingleNoiseMeasurement_CH_02 = new NoiseSingleMeasurement(GetFileNameWithIncrement(_NoiseDataFileName.Insert(_NoiseDataFileName.LastIndexOf('.'), "_CH_02_")), ChannelsToInvestigate.Channel_02, ref _SingleCalibrationMeasurement_CH_02);

            InitNoiseMeasurement();
            _Background_NoiseMeasurement.RunWorkerAsync();
        }

        private void On_NoiseCalibration_OpenFile(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Data files (*.dat)|*.dat";
            saveDialog.ShowDialog();

            _NoiseCalibrationDataFileName = saveDialog.FileName;
            controlNoiseTraceMeasurementSettings.MeasurementSettings.SaveCalibrationFileName = saveDialog.SafeFileName;
        }

        private void On_NoiseCalibration_Start_Click(object sender, RoutedEventArgs e)
        {
            _SingleCalibrationMeasurement_CH_01 = new NoiseCalibrationSingleMeasurement(GetFileNameWithIncrement(_NoiseCalibrationDataFileName.Insert(_NoiseCalibrationDataFileName.LastIndexOf('.'), "_CH_01_")), ChannelsToInvestigate.Channel_01);
            _SingleCalibrationMeasurement_CH_02 = new NoiseCalibrationSingleMeasurement(GetFileNameWithIncrement(_NoiseCalibrationDataFileName.Insert(_NoiseCalibrationDataFileName.LastIndexOf('.'), "_CH_02_")), ChannelsToInvestigate.Channel_02);

            InitNoiseMeasurement();
            _Background_NoiseMeasurement.RunWorkerAsync();
        }

        private void On_NoiseMeasuremntStop_Click(object sender, RoutedEventArgs e)
        {
            _Background_NoiseMeasurement.CancelAsync();
        }

        #endregion

        #region Checking User Input

        private static int FileCounter;
        private string GetFileNameWithIncrement(string FileName)
        {
            string result;
            FileCounter = 0;

            while (true)
            {
                result = FileName.Insert(FileName.LastIndexOf('.'), String.Format("_{0}{1}{2}", (FileCounter / 100) % 10, (FileCounter / 10) % 10, FileCounter % 10));

                if (!File.Exists(result))
                    break;
                ++FileCounter;
            }

            return result;
        }

        #endregion
    }
}