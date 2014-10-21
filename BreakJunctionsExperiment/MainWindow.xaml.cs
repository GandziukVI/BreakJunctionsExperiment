﻿using System;
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

using SMU;
using SMU.KEITHLEY_2602A;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

using Microsoft.Win32;

using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;
using Motion;

using Aids.Graphics;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Axes.Numeric;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

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

        NumberStyles numberStyle = NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent;
        CultureInfo culture = new CultureInfo("en-US");

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

        private List<PointD> _CurrentIV_CurveChannel_01;
        public List<PointD> CurrentIV_CurveChannel_01
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

        private SaveFileDialog _SaveIV_MeasureDialogChannel_01;
        private string _SaveIV_MeasuremrentFileNameChannel_01;
        private static int _IV_FilesCounterChannel_01 = 0;

        #endregion

        #region 2-nd channel

        private List<PointD> _CurrentIV_CurveChannel_02;
        public List<PointD> CurrentIV_CurveChannel_02
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

        private SaveFileDialog _SaveIV_MeasureDialogChannel_02;
        private string _SaveIV_MeasuremrentFileNameChannel_02;
        private static int _IV_FilesCounterChannel_02 = 0;

        #endregion

        IV_MeasurementSettingsDataModel _IV_ExperimentSettings;

        #endregion

        #region Time Trace data handling and presenting

        #region 1-st channel

        private List<PointD> _CurrentTimeTraceChannel_01;
        public List<PointD> CurrentTimeTraceChannel_01
        {
            get { return _CurrentTimeTraceChannel_01; }
            set { _CurrentIV_CurveChannel_01 = value; }
        }

        private ExperimentalTimeTraceDataSource _experimentalTimeTraceDataSourceChannel_01;
        private LineGraph _TimeTraceLineGraphChannel_01;

        MeasureTimeTrace TimeTraceCurveChannel_01;
        BackgroundWorker backgroundTimeTraceMeasureChannel_01;

        private TimeTraceMeasurementLog _TimeTraceMeasurementLogChannel_01;
        private TimeTraceSingleMeasurement _TimeTraceSingleMeasurementChannel_01;

        private SaveFileDialog _SaveTimeTraceMeasureDialogChannel_01;
        private string _SaveTimeTraceMeasuremrentFileNameChannel_01;
        private static int _TimeTraceFilesCounterChannel_01 = 0;

        #endregion

        #region 2-nd channel

        private List<PointD> _CurrentTimeTraceChannel_02;
        public List<PointD> CurrentTimeTraceChannel_02
        {
            get { return _CurrentTimeTraceChannel_02; }
            set { _CurrentIV_CurveChannel_02 = value; }
        }

        private ExperimentalTimeTraceDataSource _experimentalTimeTraceDataSourceChannel_02;
        private LineGraph _TimeTraceLineGraphChannel_02;

        MeasureTimeTrace TimeTraceCurveChannel_02;
        BackgroundWorker backgroundTimeTraceMeasureChannel_02;

        private TimeTraceMeasurementLog _TimeTraceMeasurementLogChannel_02;
        private TimeTraceSingleMeasurement _TimeTraceSingleMeasurementChannel_02;

        private SaveFileDialog _SaveTimeTraceMeasureDialogChannel_02;
        private string _SaveTimeTraceMeasuremrentFileNameChannel_02;
        private static int _TimeTraceFilesCounterChannel_02 = 0;

        #endregion

        private MeasureTimeTraceChannelController _ChannelController;

        private TimeTraceMeasurementSettingsDataModel _TimeTraceExperimentSettings;

        #endregion

        #region Real Time Time Trace data handling and presenting

        #region Sample 01

        private List<PointD> _RealTimeTimeTraceSample_01;
        public List<PointD> RealTimeTimeTraceSample_01
        {
            get { return _RealTimeTimeTraceSample_01; }
            set { _RealTimeTimeTraceSample_01 = value; }
        }

        private Experimental_RealTime_TimeTrace_DataSource_Sample _ExperimentalRealTimeTimetraceDataSourceSample_01;
        private LineGraph _RealTimeTimeTraceLineGraphSample_01;

        #endregion

        #region Sample 02

        private List<PointD> _RealTimeTimeTraceSample_02;
        public List<PointD> RealTimeTimeTraceSample_02
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

        private BackgroundWorker backgroundRealTimeTimeTraceMeasurementSamples;
        private MeasureRealTimeTimeTrace RealTimeTimeTraceCurve_Samples;

        private SaveFileDialog _SaveRealTimeTraceMeasureDialogSamples;
        private string _SaveRealTimeTraceMeasuremrentFileNameSamples;
        private static int _RealTimeTraceFilesCounterSamples = 0;

        #endregion

        public MainWindow()
        {
            #region Modifying current process to have real time priority

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            #endregion

            #region Initializing graphical components

            this.InitializeComponent();

            #endregion

            #region Removing Legend From Charts

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

            #endregion

            #region Implementing logarithmic Y scale for TimeTrace Approach

            chartTimeTraceChannel_01.DataTransform = new Log10YTransform();
            VerticalAxis LogarifmicTimeTraceAxis_Channel_01 = new VerticalAxis
            {
                TicksProvider = new LogarithmNumericTicksProvider(10),
                LabelProvider = new UnroundingLabelProvider()
            };

            chartTimeTraceChannel_01.MainVerticalAxis = LogarifmicTimeTraceAxis_Channel_01;
            chartTimeTraceChannel_01.AxisGrid.DrawVerticalMinorTicks = true;

            chartTimeTraceChannel_02.DataTransform = new Log10YTransform();
            VerticalAxis LogarifmicTimeTraceAxis_Channel_02 = new VerticalAxis
            {
                TicksProvider = new LogarithmNumericTicksProvider(10),
                LabelProvider = new UnroundingLabelProvider()
            };

            chartTimeTraceChannel_02.MainVerticalAxis = LogarifmicTimeTraceAxis_Channel_02;
            chartTimeTraceChannel_02.AxisGrid.DrawVerticalMinorTicks = true;

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

            #endregion

            #region Real time TimeTrace Model-view interactions

            controlRealTimeTimeTraceMeasurementSettings.cmdQuickSampleCheck.Click += on_cmdRealTime_TimeTrace_QuickSampleCheckClick;
            controlRealTimeTimeTraceMeasurementSettings.cmdStartMeasurement.Click += on_cmdRealTime_TimeTraceStartMeasurementClick;
            controlRealTimeTimeTraceMeasurementSettings.cmdStopMeasurement.Click += on_cmdRealTime_TimeTraceStopMeasurementClick;

            AllEventsHandler.Instance.RealTime_TimeTrace_AveragedDataArrived_Sample_01 += OnRealTime_TimeTrace_AveragedDataArrived_Sample_01;
            AllEventsHandler.Instance.RealTime_TimeTrace_AveragedDataArrived_Sample_02 += OnRealTime_TimeTrace_AveragedDataArrived_Sample_02;

            #endregion

            AllEventsHandler.Instance.Motion += OnMotionPositionMeasured;

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

            backgroundRealTimeTimeTraceMeasurementSamples = new BackgroundWorker();
            backgroundRealTimeTimeTraceMeasurementSamples.WorkerSupportsCancellation = true;
            backgroundRealTimeTimeTraceMeasurementSamples.WorkerReportsProgress = true;
            backgroundRealTimeTimeTraceMeasurementSamples.DoWork += backgroundRealTime_TimeTraceMeasureDoWork;

            #endregion

            #region Save I-V data to file dialog configuration

            #region 1-st channel

            _SaveIV_MeasuremrentFileNameChannel_01 = string.Empty;

            _SaveIV_MeasureDialogChannel_01 = new SaveFileDialog();
            _SaveIV_MeasureDialogChannel_01.FileName = "IV_MeasurementChannel_01_";
            _SaveIV_MeasureDialogChannel_01.DefaultExt = ".dat";
            _SaveIV_MeasureDialogChannel_01.Filter = "Measure data (.dat)|*.dat";

            #endregion

            #region 2-nd channel

            _SaveIV_MeasuremrentFileNameChannel_02 = string.Empty;

            _SaveIV_MeasureDialogChannel_02 = new SaveFileDialog();
            _SaveIV_MeasureDialogChannel_02.FileName = "IV_MeasurementChannel_02_";
            _SaveIV_MeasureDialogChannel_02.DefaultExt = ".dat";
            _SaveIV_MeasureDialogChannel_02.Filter = "Measure data (.dat)|*.dat";

            #endregion

            #endregion

            #region Save Time Trace data to file dialog configuration

            _SaveTimeTraceMeasuremrentFileNameChannel_01 = string.Empty;

            _SaveTimeTraceMeasureDialogChannel_01 = new SaveFileDialog();
            _SaveTimeTraceMeasureDialogChannel_01.FileName = "TimeTraceMeasurementChannel_01_";
            _SaveTimeTraceMeasureDialogChannel_01.DefaultExt = ".dat";
            _SaveTimeTraceMeasureDialogChannel_01.Filter = "Measure data (.dat)|*.dat";

            _SaveTimeTraceMeasuremrentFileNameChannel_02 = string.Empty;

            _SaveTimeTraceMeasureDialogChannel_02 = new SaveFileDialog();
            _SaveTimeTraceMeasureDialogChannel_02.FileName = "TimeTraceMeasurementChannel_02_";
            _SaveTimeTraceMeasureDialogChannel_02.DefaultExt = ".dat";
            _SaveTimeTraceMeasureDialogChannel_02.Filter = "Measure data (.dat)|*.dat";

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
                _CurrentIV_CurveChannel_01 = new List<PointD>();
                _experimentalIV_DataSourceChannel_01 = new ExperimentalIV_DataSourceChannel(_CurrentIV_CurveChannel_01, Channels.Channel_01);
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
                _CurrentIV_CurveChannel_02 = new List<PointD>();
                _experimentalIV_DataSourceChannel_02 = new ExperimentalIV_DataSourceChannel(_CurrentIV_CurveChannel_02, Channels.Channel_02);
                _experimentalIV_DataSourceChannel_02.AttachPointReceiveEvent();
                _IV_LineGraphChannel_02 = new LineGraph(_experimentalIV_DataSourceChannel_02);
                _IV_LineGraphChannel_02.AddToPlotter(chartIV_CurvesChannel_02);

                #endregion

                #endregion

                //Getting SMU device

                /*      Better implementation for lot of SMU kinds neened     */

                DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings.Device;
                DeviceChannel_02 = sourceDeviceConfigurationChannel_02.Keithley2602A_DeviceSettings.Device;

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

                #endregion

                #region 1-st channel settings

                var StartValueChannel_01 = _IV_ExperimentSettings.IV_MeasurementStartValueWithMultiplierChannel_01;
                var EndValueChannel_01 = _IV_ExperimentSettings.IV_MeasurementEndValueWithMultiplierChannel_01;
                var StepChannel_01 = _IV_ExperimentSettings.IV_MeasurementStepWithMultiplierChannel_01;

                IV_CurveChannel_01 = new MeasureIV(StartValueChannel_01, EndValueChannel_01, StepChannel_01, NumberOfAverages, TimeDelay, DeviceSourceMode, DeviceChannel_01, Channels.Channel_01);

                #endregion

                #region 2-nd channel settings

                var StartValueChannel_02 = _IV_ExperimentSettings.IV_MeasurementStartValueWithMultiplierChannel_02;
                var EndValueChannel_02 = _IV_ExperimentSettings.IV_MeasurementEndValueWithMultiplierChannel_02;
                var StepChannel_02 = _IV_ExperimentSettings.IV_MeasurementStepWithMultiplierChannel_02;

                IV_CurveChannel_02 = new MeasureIV(StartValueChannel_02, EndValueChannel_02, StepChannel_02, NumberOfAverages, TimeDelay, DeviceSourceMode, DeviceChannel_02, Channels.Channel_02);

                #endregion

                #endregion

                #region Saving I-V data into files

                var _IV_FileNumberChannel_01 = String.Format("_{0}{1}{2}", (_IV_FilesCounterChannel_01 / 100) % 10, (_IV_FilesCounterChannel_01 / 10) % 10, _IV_FilesCounterChannel_01 % 10);
                var newFileNameChannel_01 = string.Empty;

                var _IV_FileNumberChannel_02 = String.Format("_{0}{1}{2}", (_IV_FilesCounterChannel_02 / 100) % 10, (_IV_FilesCounterChannel_02 / 10) % 10, _IV_FilesCounterChannel_02 % 10);
                var newFileNameChannel_02 = string.Empty;

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_01) && !string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_02))
                {
                    _IV_MeasurementLogChannel_01 = new IV_MeasurementLog((new FileInfo(_SaveIV_MeasuremrentFileNameChannel_01)).DirectoryName + "\\IV_MeasurementLogChannel_01.dat");
                    _IV_MeasurementLogChannel_02 = new IV_MeasurementLog((new FileInfo(_SaveIV_MeasuremrentFileNameChannel_02)).DirectoryName + "\\IV_MeasurementLogChannel_02.dat");
                    
                    newFileNameChannel_01 = _SaveIV_MeasuremrentFileNameChannel_01.Insert(_SaveIV_MeasuremrentFileNameChannel_01.LastIndexOf('.'), _IV_FileNumberChannel_01);
                    newFileNameChannel_02 = _SaveIV_MeasuremrentFileNameChannel_02.Insert(_SaveIV_MeasuremrentFileNameChannel_02.LastIndexOf('.'), _IV_FileNumberChannel_02);
                    
                    ++_IV_FilesCounterChannel_01;
                    ++_IV_FilesCounterChannel_02;
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
                    double micrometricBoltPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceMotionCurrentPosition;

                    string comment = _IV_ExperimentSettings.IV_MeasurementDataComment;

                    _IV_MeasurementLogChannel_01.AddNewIV_MeasurementLog(fileNameChannel_01, sourceMode, micrometricBoltPosition, comment);
                    _IV_MeasurementLogChannel_02.AddNewIV_MeasurementLog(fileNameChannel_02, sourceMode, micrometricBoltPosition, comment);
                }

                if ((_IV_SingleMeasurementChannel_01 != null) && (_IV_SingleMeasurementChannel_02 != null))
                {
                    _IV_SingleMeasurementChannel_01.Dispose();
                    _IV_SingleMeasurementChannel_02.Dispose();
                }


                _IV_SingleMeasurementChannel_01 = new IV_SingleMeasurement(newFileNameChannel_01, Channels.Channel_01);
                _IV_SingleMeasurementChannel_02 = new IV_SingleMeasurement(newFileNameChannel_02, Channels.Channel_02);

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
            if(backgroundIV_MeasureChannel_01.IsBusy==true || backgroundIV_MeasureChannel_02.IsBusy == true)
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
            if(backgroundIV_MeasureChannel_01.IsBusy == true)
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
            Nullable<bool> dialogResult = _SaveIV_MeasureDialogChannel_01.ShowDialog();

            if (dialogResult == true)
            {
                _IV_FilesCounterChannel_01 = 0;
                _SaveIV_MeasuremrentFileNameChannel_01 = _SaveIV_MeasureDialogChannel_01.FileName;
                this.controlIV_MeasurementSettings.textBoxIV_FileNameChannel_01.Text = _SaveIV_MeasureDialogChannel_01.SafeFileName;
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
            Nullable<bool> dialogResult = _SaveIV_MeasureDialogChannel_02.ShowDialog();

            if (dialogResult == true)
            {
                _IV_FilesCounterChannel_02 = 0;
                _SaveIV_MeasuremrentFileNameChannel_02 = _SaveIV_MeasureDialogChannel_02.FileName;
                this.controlIV_MeasurementSettings.textBoxIV_FileNameChannel_02.Text = _SaveIV_MeasureDialogChannel_02.SafeFileName;
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
                    _CurrentTimeTraceChannel_01.Clear();
                }

                _CurrentTimeTraceChannel_01 = new List<PointD>();
                _experimentalTimeTraceDataSourceChannel_01 = new ExperimentalTimetraceDataSourceChannel(_CurrentTimeTraceChannel_01, Channels.Channel_01);
                _experimentalTimeTraceDataSourceChannel_01.AttachPointReceiveEvent();
                _TimeTraceLineGraphChannel_01 = new LineGraph(_experimentalTimeTraceDataSourceChannel_01);
                _TimeTraceLineGraphChannel_01.AddToPlotter(chartTimeTraceChannel_01);

                #endregion

                #region 2-nd channel

                if (_TimeTraceLineGraphChannel_02 != null)
                {
                    _experimentalTimeTraceDataSourceChannel_02.DetachPointReceiveEvent();
                    _TimeTraceLineGraphChannel_02.RemoveFromPlotter();
                    _CurrentTimeTraceChannel_02.Clear();
                }

                _CurrentTimeTraceChannel_02 = new List<PointD>();
                _experimentalTimeTraceDataSourceChannel_02 = new ExperimentalTimetraceDataSourceChannel(_CurrentTimeTraceChannel_02, Channels.Channel_02);
                _experimentalTimeTraceDataSourceChannel_02.AttachPointReceiveEvent();
                _TimeTraceLineGraphChannel_02 = new LineGraph(_experimentalTimeTraceDataSourceChannel_02);
                _TimeTraceLineGraphChannel_02.AddToPlotter(chartTimeTraceChannel_02);

                #endregion

                #endregion

                //Configurations for all kinds of SMUs should be listed here

                if(sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings != null)
                    DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings.Device;
                if (sourceDeviceConfigurationChannel_02.Keithley2602A_DeviceSettings != null)
                    DeviceChannel_02 = sourceDeviceConfigurationChannel_02.Keithley2602A_DeviceSettings.Device;

                #region Time trace measurement configuration

                _TimeTraceExperimentSettings = controlTimeTraceMeasurementSettings.MeasurementSettings;

                //Configurations for all kinds of motors should be listed here

                if (motorConfiguration.Faulhaber_2036_U012V_Settings != null)
                    _MotionController = motorConfiguration.Faulhaber_2036_U012V_Settings.motionController;

                _MotionController.NotificationsPerMilimeter = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceNotificationsPerRevolution;

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
                            var motionStartPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceMotionStartPosition;
                            var motionFinalDestination = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceMotionFinalDestination;

                            if (isTimeTraceChannel_01_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_01_ValueThroughTheStructure, Channels.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_01_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_01_ValueThroughTheStructure, Channels.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }

                            if (isTimeTraceChannel_02_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_02, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_02_ValueThroughTheStructure, Channels.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_02_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionStartPosition, motionFinalDestination, DeviceChannel_02, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_02_ValueThroughTheStructure, Channels.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                        } break;
                    case "Distance (Repetitive)":
                        {
                            var motionRepetitiveStartPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceRepetitiveStartPosition;
                            var motionRepetitiveEndPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceRepetitiveEndPosition;

                            if (isTimeTraceChannel_01_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_01_ValueThroughTheStructure, Channels.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_01_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_01_ValueThroughTheStructure, Channels.Channel_01, _ChannelController, ref backgroundTimeTraceMeasureChannel_01);
                                TimeTraceCurveChannel_01.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }

                            if (isTimeTraceChannel_02_VoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_02, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_02_ValueThroughTheStructure, Channels.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceChannel_02_CurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_02 = new MeasureTimeTrace(_MotionController, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_02, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, timeTraceChannel_02_ValueThroughTheStructure, Channels.Channel_02, _ChannelController, ref backgroundTimeTraceMeasureChannel_02);
                                TimeTraceCurveChannel_02.NumberOfAverages = _TimeTraceExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_02.TimeDelay = _TimeTraceExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                        } break;
                    case "Time":
                        {
                        } break;
                    case "Fixed R":
                        {
                        } break;
                    default:
                        break;
                }

                #endregion

                #region Saving Time Trace data into files

                var _TimeTraceChannel_01_FileNumber = String.Format("_{0}{1}{2}", (_TimeTraceFilesCounterChannel_01 / 100) % 10, (_TimeTraceFilesCounterChannel_01 / 10) % 10, _TimeTraceFilesCounterChannel_01 % 10);
                var newFileNameChannel_01 = string.Empty;

                var _TimeTraceChannel_02_FileNumber = String.Format("_{0}{1}{2}", (_TimeTraceFilesCounterChannel_02 / 100) % 10, (_TimeTraceFilesCounterChannel_02 / 10) % 10, _TimeTraceFilesCounterChannel_02 % 10);
                var newFileNameChannel_02 = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_01) && !string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_02))
                {
                    _TimeTraceMeasurementLogChannel_01 = new TimeTraceMeasurementLog((new FileInfo(_SaveTimeTraceMeasuremrentFileNameChannel_01)).DirectoryName + "\\TimeTraceMeasurementLogChannel_01.dat");
                    _TimeTraceMeasurementLogChannel_02 = new TimeTraceMeasurementLog((new FileInfo(_SaveTimeTraceMeasuremrentFileNameChannel_02)).DirectoryName + "\\TimeTraceMeasurementLogChannel_02.dat");

                    newFileNameChannel_01 = _SaveTimeTraceMeasuremrentFileNameChannel_01.Insert(_SaveTimeTraceMeasuremrentFileNameChannel_01.LastIndexOf('.'), _TimeTraceChannel_01_FileNumber);
                    newFileNameChannel_02 = _SaveTimeTraceMeasuremrentFileNameChannel_02.Insert(_SaveTimeTraceMeasuremrentFileNameChannel_02.LastIndexOf('.'), _TimeTraceChannel_02_FileNumber);

                    ++_TimeTraceFilesCounterChannel_01;
                    ++_TimeTraceFilesCounterChannel_02;
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

                    var micrometricBoltPosition = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceMotionCurrentPosition;

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

                _TimeTraceSingleMeasurementChannel_01 = new TimeTraceSingleMeasurement(newFileNameChannel_01, _sourceModeChannel_01, Channels.Channel_01);
                _TimeTraceSingleMeasurementChannel_02 = new TimeTraceSingleMeasurement(newFileNameChannel_02, _sourceModeChannel_02, Channels.Channel_02);

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
                on_cmdRealTime_TimeTraceStopMeasurementClick(sender, e);
                Thread.Sleep(1000);
            }

            var timeTtraceMeasurementsInitSuccess = InitTimeTraceMeasurements();

            if (timeTtraceMeasurementsInitSuccess)
            {
                backgroundTimeTraceMeasureChannel_01.RunWorkerAsync();
                backgroundTimeTraceMeasureChannel_02.RunWorkerAsync();

                AllEventsHandler.Instance.OnTimeTraceMeasurementsStateChanged(this, new TimeTraceMeasurementStateChanged_EventArgs(true));
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

        #region 1-st Channel Background Work

        private void backgroundTimeTraceMeasureDoWorkChannel_01(object sender, DoWorkEventArgs e)
        {
            if ((DeviceChannel_01 != null) && (DeviceChannel_01.InitDevice()))
            {

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings;
                var numerCycles = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveNumberCycles;

                var selectedTimeTraceModeHeader = ExperimentSettings.TimeTraceMeasurementSelectedTabIndex;

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
            this.progressBarMeasurementProgressChannel_01.Value = e.ProgressPercentage;
        }

        private void backgroundTimeTrace_RunWorkerCompletedChannel_01(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion

        #region 2-nd Channel Background Work

        private void backgroundTimeTraceMeasureDoWorkChannel_02(object sender, DoWorkEventArgs e)
        {
            if ((DeviceChannel_02 != null) && (DeviceChannel_02.InitDevice()))
            {

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings;
                var numerCycles = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveNumberCycles;

                var selectedTimeTraceModeHeader = ExperimentSettings.TimeTraceMeasurementSelectedTabIndex;

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
        }

        #endregion

        #region General settings

        private void on_cmdTimeTraceDataFileNameBrowseClickChannel_01(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            Nullable<bool> dialogResult = _SaveTimeTraceMeasureDialogChannel_01.ShowDialog();

            if (dialogResult == true)
            {
                _TimeTraceFilesCounterChannel_01 = 0;
                _SaveTimeTraceMeasuremrentFileNameChannel_01 = _SaveTimeTraceMeasureDialogChannel_01.FileName;
                this.controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_01_DataFileName = _SaveTimeTraceMeasureDialogChannel_01.SafeFileName;
            }
        }

        private void on_cmdTimeTraceDataFileNameBrowseClickChannel_02(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            Nullable<bool> dialogResult = _SaveTimeTraceMeasureDialogChannel_02.ShowDialog();

            if (dialogResult == true)
            {
                _TimeTraceFilesCounterChannel_02 = 0;
                _SaveTimeTraceMeasuremrentFileNameChannel_02 = _SaveTimeTraceMeasureDialogChannel_02.FileName;
                this.controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementChannel_02_DataFileName = _SaveTimeTraceMeasureDialogChannel_02.SafeFileName;
            }
        }

        #endregion

        private void OnMotionPositionMeasured(object sender, Motion_EventArgs e)
        {
            this.controlTimeTraceMeasurementSettings.MotionParameters.MeasurementSettings.TimeTraceMeasurementDistanceMotionCurrentPosition = e.Position;
            this.controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementMicrometricBoltPosition = e.Position;
        }

        #endregion

        #region Real Time Time Trace Measurement Interface Interactions

        private void InitRealTime_TimeTraceMeasurement()
        {
            #region Chart rendering settings

            #region Sample 01

            if(_RealTimeTimeTraceLineGraphSample_01 != null)
            {
                _ExperimentalRealTimeTimetraceDataSourceSample_01.DetachPointReceiveEvent();
                _RealTimeTimeTraceLineGraphSample_01.RemoveFromPlotter();
                _RealTimeTimeTraceSample_01.Clear();
            }

            _RealTimeTimeTraceSample_01 = new List<PointD>();
            _ExperimentalRealTimeTimetraceDataSourceSample_01 = new Experimental_RealTime_TimeTrace_DataSource_Sample(_RealTimeTimeTraceSample_01, Samples.Sample_01);
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

            _RealTimeTimeTraceSample_02 = new List<PointD>();
            _ExperimentalRealTimeTimetraceDataSourceSample_02 = new Experimental_RealTime_TimeTrace_DataSource_Sample(_RealTimeTimeTraceSample_02, Samples.Sample_02);
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
                    _ExperimentalRealTimeTimetraceDataSourceSample_01.DetachPointReceiveEvent();
                    _RealTimeTimeTraceLineGraphSample_01.RemoveFromPlotter();
                    _RealTimeTimeTraceSample_01.Clear();
                }
                if (_RealTimeTimeTraceLineGraphSample_02 != null)
                {
                    _ExperimentalRealTimeTimetraceDataSourceSample_02.DetachPointReceiveEvent();
                    _RealTimeTimeTraceLineGraphSample_02.RemoveFromPlotter();
                    _RealTimeTimeTraceSample_02.Clear();
                }

                AllEventsHandler.Instance.RealTime_TimeTraceDataArrived += OnRealTime_TimeTrace_DataArrived;

                AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(true));
                backgroundRealTimeTimeTraceMeasurementSamples.RunWorkerAsync();
            }
            else 
            {
                AllEventsHandler.Instance.RealTime_TimeTraceDataArrived -= OnRealTime_TimeTrace_DataArrived;
                AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(false));
                backgroundRealTimeTimeTraceMeasurementSamples.CancelAsync();
            }
        }

        private void OnRealTime_TimeTrace_DataArrived(object sender, RealTime_TimeTrace_DataArrived_EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate() 
                {
                    if (e.Data != null)
                    {
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

        private void on_cmdRealTime_TimeTraceStartMeasurementClick(object sender, RoutedEventArgs e)
        {
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartStopEnabled = true;
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartMeasurementButtonEnabled = false;
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStopMeasurementButtonEnabled = true;

            InitRealTime_TimeTraceMeasurement();
            
            backgroundRealTimeTimeTraceMeasurementSamples.RunWorkerAsync();
        }

        private void on_cmdRealTime_TimeTraceStopMeasurementClick(object sender, RoutedEventArgs e)
        {
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartStopEnabled = false;
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStartMeasurementButtonEnabled = true;
            this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.IsStopMeasurementButtonEnabled = false;

            backgroundRealTimeTimeTraceMeasurementSamples.CancelAsync();
            AllEventsHandler.Instance.OnRealTime_TimeTraceMeasurementStateChanged(this, new RealTime_TimeTraceMeasurementStateChanged_EventArgs(false));
        }

        private void OnRealTime_TimeTrace_AveragedDataArrived_Sample_01(object sender, RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_01 e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate() 
                {
                    this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Resistance_1st_Sample = e.Averaged_RealTime_TimeTrace_Data;
            }));
        }

        private void OnRealTime_TimeTrace_AveragedDataArrived_Sample_02(object sender, RealTime_TimeTrace_AveragedDataArrived_EventArgs_Sample_02 e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.controlRealTimeTimeTraceMeasurementSettings.MeasurementSettings.Resistance_2nd_Sample = e.Averaged_RealTime_TimeTrace_Data;
            }));
        }

        #region Background work

        private void backgroundRealTime_TimeTraceMeasureDoWork(object sender, DoWorkEventArgs e)
        {
            if (_RealTime_TimeTraceSingleMeasurementSamples != null)
                _RealTime_TimeTraceSingleMeasurementSamples.Dispose();
            if (RealTimeTimeTraceCurve_Samples != null)
                RealTimeTimeTraceCurve_Samples.Dispose();

            _RealTime_TimeTraceSingleMeasurementSamples = new RealTime_TimeTraceSingleMeasurement("E:\\MegaMeasurement.txt", 0.02, "");
            RealTimeTimeTraceCurve_Samples = new MeasureRealTimeTimeTrace();
            RealTimeTimeTraceCurve_Samples.StartMeasurement(sender, e, MotionKind.Single);
        }

        #endregion

        #endregion

        #region Checking User Input

        private void IntegerPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            HandlingUserInput.IntegerPastingHandler(ref sender, ref e);
        }

        private void OnIntegerTextChanged(object sender, TextChangedEventArgs e)
        {
            HandlingUserInput.OnIntegerTextChanged(ref sender, ref e);
        }

        private void FloatingPointPastingHandler(object sender, DataObjectPastingEventArgs e) 
        {
            HandlingUserInput.FloatingPointPastingHandler(ref sender, ref e);
        }

        private void OnFloatingPointTextChanged(object sender, TextChangedEventArgs e)
        {
            HandlingUserInput.OnFloatingPointTextChanged(ref sender, ref e);
        }

        #endregion
    }
}