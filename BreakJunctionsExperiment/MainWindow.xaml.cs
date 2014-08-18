using System;
using System.Collections.Generic;
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

using Hardware;
using Hardware.KEITHLEY_2602A;

using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;

using Microsoft.Win32;

using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

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

        IMotion Motor;

        #endregion

        #region User interfaces to realize SMUs (get SMU configuration)

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

        #endregion

        #region Time Trace data handling and presenting

        #region 1-st channel

        private List<PointD> _CurrentTimeTraceChannel_01;
        public List<PointD> CurrentTimeTraceChannel_01
        {
            get { return _CurrentTimeTraceChannel_01; }
            set { _CurrentIV_CurveChannel_01 = value; }
        }

        private ExperimentalTimetraceDataSource _experimentalTimeTraceDataSourceChannel_01;
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

        private ExperimentalTimetraceDataSource _experimentalTimeTraceDataSourceChannel_02;
        private LineGraph _TimeTraceLineGraphChannel_02;

        MeasureTimeTrace TimeTraceCurveChannel_02;
        BackgroundWorker backgroundTimeTraceMeasureChannel_02;

        private TimeTraceMeasurementLog _TimeTraceMeasurementLogChannel_02;
        private TimeTraceSingleMeasurement _TimeTraceSingleMeasurementChannel_02;

        private SaveFileDialog _SaveTimeTraceMeasureDialogChannel_02;
        private string _SaveTimeTraceMeasuremrentFileNameChannel_02;
        private static int _TimeTraceFilesCounterChannel_02 = 0;

        #endregion

        #endregion

        public MainWindow()
		{
			this.InitializeComponent();

            #region Interface model-view interactions

            controlIV_MeasurementSettings.cmdIV_DataFileNameBrowse.Click += on_cmdIV_DataFileNameBrowseClickChannel_01;
            controlIV_MeasurementSettings.cmdIV_StartMeasurement.Click += on_cmdIV_StartMeasurementClick;
            controlIV_MeasurementSettings.cmdIV_StopMeasurement.Click += on_cmdIV_StopMeasurementClick;

            controlTimeTraceMeasurementSettings.cmdTimeTraceDataFileNameBrowse.Click += on_cmdTimeTraceDataFileNameBrowseClick;
            //controlTimeTraceMeasurementSettings.cmdTimeTraceDistanceMoveToInitialPosition +=
            controlTimeTraceMeasurementSettings.cmdTimeTraceStartMeasurement.Click += on_cmdTimeTraceStartMeasurementClick;
            controlTimeTraceMeasurementSettings.cmdTimeTraceStopMeasurement.Click += on_cmdTimeTraceStopMeasurementClick;

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

            #region Save I-V data to file dialog configuration

            _SaveIV_MeasuremrentFileNameChannel_01 = string.Empty;

            _SaveIV_MeasureDialogChannel_01 = new SaveFileDialog();
            _SaveIV_MeasureDialogChannel_01.FileName = "IV_Measurement";
            _SaveIV_MeasureDialogChannel_01.DefaultExt = ".dat";
            _SaveIV_MeasureDialogChannel_01.Filter = "Measure data (.dat)|*.dat";

            #endregion

            #region Save Time Trace data to file dialog configuration

            _SaveTimeTraceMeasuremrentFileNameChannel_01 = string.Empty;

            _SaveTimeTraceMeasureDialogChannel_01 = new SaveFileDialog();
            _SaveTimeTraceMeasureDialogChannel_01.FileName = "TimeTraceMeasurement";
            _SaveTimeTraceMeasureDialogChannel_01.DefaultExt = ".dat";
            _SaveTimeTraceMeasureDialogChannel_01.Filter = "Measure data (.dat)|*.dat";

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

		private void onSetSMU_Click(object sender, RoutedEventArgs e)
		{
            sourceDeviceConfigurationChannel_01 = new SourceDeviceConfiguration();
            sourceDeviceConfigurationChannel_01.ShowDialog();
		}

        private void onSetSMU_Channel_02_Click(object sender, RoutedEventArgs e)
        {
            sourceDeviceConfigurationChannel_01 = new SourceDeviceConfiguration();
            sourceDeviceConfigurationChannel_01.ShowDialog();
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
            if (sourceDeviceConfigurationChannel_01 != null)
            {
                #region Chart rendering settings

                //Initializing a new plot on I-V chart
                if (_IV_LineGraphChannel_01 != null)
                {
                    //Detaching receive event from "old" data source
                    _experimentalIV_DataSourceChannel_01.DetachPointReceiveEvent();
                    _IV_LineGraphChannel_01.Remove();
                }
                //Creating new plot and attaching it to the chart
                _CurrentIV_CurveChannel_01 = new List<PointD>();
                _experimentalIV_DataSourceChannel_01 = new ExperimentalIV_DataSource(_CurrentIV_CurveChannel_01);
                _experimentalIV_DataSourceChannel_01.AttachPointReceiveEvent();
                _IV_LineGraphChannel_01 = new LineGraph(_experimentalIV_DataSourceChannel_01);
                _IV_LineGraphChannel_01.AddToPlotter(chartIV_Curves);

                #endregion

                //Getting SMU device
                DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings.Device;

                #region I-V measurement configuration

                var ExperimentSettings = controlIV_MeasurementSettings.MeasurementSettings;

                var StartValue = ExperimentSettings.IV_MeasurementStartValue;                
                var EndValue = ExperimentSettings.IV_MeasurementEndValue;
                var Step = ExperimentSettings.IV_MeasurementStep;
                var NumberOfAverages = ExperimentSettings.IV_MeasurementNumberOfAverages;
                var TimeDelay = ExperimentSettings.IV_MeasurementTimeDelay;

                SourceMode DeviceSourceMode = SourceMode.Voltage;

                if (ExperimentSettings.IsIV_MeasurementVoltageModeChecked == true)
                {
                    DeviceSourceMode = SourceMode.Voltage;
                }
                else if (ExperimentSettings.IsIV_MeasurementCurrentModeChecked == true)
                {
                    DeviceSourceMode = SourceMode.Current;
                }

                IV_CurveChannel_01 = new MeasureIV(StartValue, EndValue, Step, NumberOfAverages, TimeDelay, DeviceSourceMode, DeviceChannel_01);

                #endregion

                #region Saving I-V data into files

                var _IV_FileNumber = String.Format("_{0}{1}{2}", (_IV_FilesCounterChannel_01 / 100) % 10, (_IV_FilesCounterChannel_01 / 10) % 10, _IV_FilesCounterChannel_01 % 10);
                string newFileName = string.Empty;

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_01))
                {
                    _IV_MeasurementLogChannel_01 = new IV_MeasurementLog((new FileInfo(_SaveIV_MeasuremrentFileNameChannel_01)).DirectoryName + "\\IV_MeasurementLog.dat");
                    
                    newFileName = _SaveIV_MeasuremrentFileNameChannel_01.Insert(_SaveIV_MeasuremrentFileNameChannel_01.LastIndexOf('.'), _IV_FileNumber);
                    ++_IV_FilesCounterChannel_01;
                }

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileNameChannel_01))
                {

                    string fileName = (new FileInfo(newFileName)).Name;

                    string sourceMode = string.Empty;

                    if (ExperimentSettings.IsIV_MeasurementVoltageModeChecked == true)
                    {
                        sourceMode = "Source mode: Voltage";
                    }
                    else if (ExperimentSettings.IsIV_MeasurementCurrentModeChecked == true)
                    {
                        sourceMode = "SourceMode: Current";
                    }

                    //Some Comment
                    double micrometricBoltPosition = controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementDistanceMotionCurrentPosition;

                    string comment = ExperimentSettings.IV_MeasurementDataComment;

                    _IV_MeasurementLogChannel_01.AddNewIV_MeasurementLog(fileName, sourceMode, micrometricBoltPosition, comment);
                }

                if (_IV_SingleMeasurementChannel_01 != null)
                    _IV_SingleMeasurementChannel_01.Dispose();


                _IV_SingleMeasurementChannel_01 = new IV_SingleMeasurement(newFileName);

                #endregion

                return true;
            }
            else
            {
                MessageBox.Show("The device is not initialized.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            #endregion
        }

        private void on_cmdIV_StartMeasurementClick(object sender, RoutedEventArgs e)
        {
            var isInitSuccess = InitIV_Measurements();
            
            //Starting I-V measurements in background
            if((isInitSuccess == true) && (backgroundIV_MeasureChannel_01.IsBusy == false))
                backgroundIV_MeasureChannel_01.RunWorkerAsync();
        }

		private void on_cmdIV_StopMeasurementClick(object sender, RoutedEventArgs e)
		{
            //Canceling I-V measures
            if(backgroundIV_MeasureChannel_01.IsBusy == true)
                backgroundIV_MeasureChannel_01.CancelAsync();
		}

        #region 1-st Channel Background Work

        private void backgroundIV_Measure_DoWorkChannel_01(object sender, DoWorkEventArgs e)
        {
            //Updating interface to show that measurement is in process
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.labelMeasurementStatus.Content = "In process...";
            }));

            //Starting measurements
            IV_CurveChannel_01.StartMeasurement(sender, e);
        }

        private void backgroundIV_Measure_ProgressChangedChannel_01(object sender, ProgressChangedEventArgs e)
        {
            //Updating interface to show measurement progress
            this.progressBarMeasurementProgress.Value = e.ProgressPercentage;
        }

        private void backgroundIV_Measure_RunWorkerCompletedChannel_01(object sender, RunWorkerCompletedEventArgs e)
        {
            //Updating interface to show that measurement is completed
            this.labelMeasurementStatus.Content = "Ready";
        }

        private void on_cmdIV_DataFileNameBrowseClickChannel_01(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            Nullable<bool> dialogResult = _SaveIV_MeasureDialogChannel_01.ShowDialog();

            if (dialogResult == true)
            {
                _IV_FilesCounterChannel_01 = 0;
                _SaveIV_MeasuremrentFileNameChannel_01 = _SaveIV_MeasureDialogChannel_01.FileName;
                this.controlIV_MeasurementSettings.textBoxIV_FileName.Text = _SaveIV_MeasureDialogChannel_01.SafeFileName;
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
                this.controlIV_MeasurementSettings.textBoxIV_FileName.Text = _SaveIV_MeasureDialogChannel_02.SafeFileName;
            }
        }

        #endregion

        #endregion

        #region Time Trace Measurements Interface Interactions

        private bool InitTimeTraceMeasurements()
        {
            #region SMU, rendering and save data configurations

            if (sourceDeviceConfigurationChannel_01 != null)
            {
                #region Chart rendering settings

                if (_TimeTraceLineGraphChannel_01 != null)
                {
                    _experimentalTimeTraceDataSourceChannel_01.DetachPointReceiveEvent();
                    _TimeTraceLineGraphChannel_01.Remove();
                    _CurrentTimeTraceChannel_01.Clear();
                }

                _CurrentTimeTraceChannel_01 = new List<PointD>();
                _experimentalTimeTraceDataSourceChannel_01 = new ExperimentalTimetraceDataSource(_CurrentTimeTraceChannel_01);
                _experimentalTimeTraceDataSourceChannel_01.AttachPointReceiveEvent();
                _TimeTraceLineGraphChannel_01 = new LineGraph(_experimentalTimeTraceDataSourceChannel_01);
                _TimeTraceLineGraphChannel_01.AddToPlotter(chartTimeTrace);

                #endregion

                //Getting SMU device
                DeviceChannel_01 = sourceDeviceConfigurationChannel_01.Keithley2602A_DeviceSettings.Device;

                #region Time trace measurement configuration

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MeasurementSettings;

                if (Motor != null)
                    Motor.Dispose();

                var motor = new FAULHABER_MINIMOTOR_SA("COM4"); //Better realization needed
                motor.NotificationsPerRevolution = ExperimentSettings.TimeTraceNotificationsPerRevolution;
                motor.EnableDevice();

                Motor = motor;

                if (TimeTraceCurveChannel_01 != null)
                {
                    TimeTraceCurveChannel_01.Dispose();
                }

                var valueThroughTheStructure = ExperimentSettings.TimeTraceMeasurementValueThrougtTheStructure;
                var isTimeTraceVoltageModeChecked = ExperimentSettings.IsTimeTraceMeasurementVoltageModeChecked;
                var isTimeTraceCurrentModeChecked = ExperimentSettings.IsTimeTraceMeasurementCurrentModeChecked;

                var selectedTimeTraceModeItem = (controlTimeTraceMeasurementSettings.tabControlTimeTraceMeasurementParameters.SelectedItem as TabItem).Header.ToString();

                switch (selectedTimeTraceModeItem)
                {
                    case "Distance":
                        {
                            var motionStartPosition = ExperimentSettings.TimeTraceMeasurementDistanceMotionStartPosition;
                            var motionFinalDestination = ExperimentSettings.TimeTraceMeasurementDistanceMotionFinalDestination;

                            if (isTimeTraceVoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(Motor, motionStartPosition, motionFinalDestination, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                                TimeTraceCurveChannel_01.NumberOfAverages = ExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = ExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceCurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(Motor, motionStartPosition, motionFinalDestination, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                                TimeTraceCurveChannel_01.NumberOfAverages = ExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = ExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                        } break;
                    case "Distance (Repetitive)":
                        {
                            var motionRepetitiveStartPosition = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveStartPosition;
                            var motionRepetitiveEndPosition = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveEndPosition;

                            if (isTimeTraceVoltageModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(Motor, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                                TimeTraceCurveChannel_01.NumberOfAverages = ExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = ExperimentSettings.TimeTraceMeasurementTimeDelay;
                            }
                            else if (isTimeTraceCurrentModeChecked == true)
                            {
                                TimeTraceCurveChannel_01 = new MeasureTimeTrace(Motor, motionRepetitiveStartPosition, motionRepetitiveEndPosition, DeviceChannel_01, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                                TimeTraceCurveChannel_01.NumberOfAverages = ExperimentSettings.TimeTraceMeasurementNumberOfAverages;
                                TimeTraceCurveChannel_01.TimeDelay = ExperimentSettings.TimeTraceMeasurementTimeDelay;
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

                var _TimeTraceFileNumber = String.Format("_{0}{1}{2}", (_TimeTraceFilesCounterChannel_01 / 100) % 10, (_TimeTraceFilesCounterChannel_01 / 10) % 10, _TimeTraceFilesCounterChannel_01 % 10);
                string newFileName = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_01))
                {
                    _TimeTraceMeasurementLogChannel_01 = new TimeTraceMeasurementLog((new FileInfo(_SaveTimeTraceMeasuremrentFileNameChannel_01)).DirectoryName + "\\TimeTraceMeasurementLog.dat");

                    newFileName = _SaveTimeTraceMeasuremrentFileNameChannel_01.Insert(_SaveTimeTraceMeasuremrentFileNameChannel_01.LastIndexOf('.'), _TimeTraceFileNumber);
                    ++_TimeTraceFilesCounterChannel_01;
                }

                string sourceMode = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileNameChannel_01))
                {
                    string fileName = (new FileInfo(newFileName)).Name;

                    if (ExperimentSettings.IsTimeTraceMeasurementVoltageModeChecked == true)
                    {
                        sourceMode = "Source mode: Voltage";
                    }
                    else if (ExperimentSettings.IsTimeTraceMeasurementCurrentModeChecked == true)
                    {
                        sourceMode = "SourceMode: Current";
                    }

                    var micrometricBoltPosition = ExperimentSettings.TimeTraceMeasurementDistanceMotionCurrentPosition;

                    var comment = ExperimentSettings.TimeTraceMeasurementDataComment;

                    _TimeTraceMeasurementLogChannel_01.AddNewTimeTraceMeasurementLog(fileName, sourceMode, valueThroughTheStructure, comment);
                }

                SourceMode _sourceMode = SourceMode.Voltage; //Source mode is voltage by default

                if (sourceMode == "Source mode: Voltage")
                    _sourceMode = SourceMode.Voltage;
                else if (sourceMode == "SourceMode: Current")
                    _sourceMode = SourceMode.Current;

                if (_TimeTraceSingleMeasurementChannel_01 != null)
                    _TimeTraceSingleMeasurementChannel_01.Dispose();

                _TimeTraceSingleMeasurementChannel_01 = new TimeTraceSingleMeasurement(newFileName, _sourceMode);

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
            var timeTtraceMeasurementsInitSuccess = InitTimeTraceMeasurements();

            if (timeTtraceMeasurementsInitSuccess)
            {
                backgroundTimeTraceMeasureChannel_01.RunWorkerAsync();
            }
            else MessageBox.Show("The device was not initialized!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void on_cmdTimeTraceStopMeasurementClick(object sender, RoutedEventArgs e)
		{
            backgroundTimeTraceMeasureChannel_01.CancelAsync();
		}

        #region 1-st Channel Background Work

        private void backgroundTimeTraceMeasureDoWorkChannel_01(object sender, DoWorkEventArgs e)
        {
            if ((DeviceChannel_01 != null) && (DeviceChannel_01.InitDevice()))
            {

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MeasurementSettings;
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
            this.progressBarMeasurementProgress.Value = e.ProgressPercentage;
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

                var ExperimentSettings = controlTimeTraceMeasurementSettings.MeasurementSettings;
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
            this.progressBarMeasurementProgress.Value = e.ProgressPercentage;
        }

        private void backgroundTimeTrace_RunWorkerCompletedChannel_02(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        #endregion


        private void on_cmdTimeTraceDataFileNameBrowseClick(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            Nullable<bool> dialogResult = _SaveTimeTraceMeasureDialogChannel_01.ShowDialog();

            if (dialogResult == true)
            {
                _TimeTraceFilesCounterChannel_01 = 0;
                _SaveTimeTraceMeasuremrentFileNameChannel_01 = _SaveTimeTraceMeasureDialogChannel_01.FileName;
                //this.controlTimeTraceMeasurementSettings.textBoxTimeTraceFileName.Text = _SaveTimeTraceMeasureDialog.SafeFileName;
                this.controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementDataFileName = _SaveTimeTraceMeasureDialogChannel_01.SafeFileName;
            }
        }

        private void OnMotionPositionMeasured(object sender, Motion_EventArgs e)
        {
            this.controlTimeTraceMeasurementSettings.MeasurementSettings.TimeTraceMeasurementDistanceMotionCurrentPosition = e.Position;
            this.controlIV_MeasurementSettings.MeasurementSettings.IV_MeasurementMicrometricBoltPosition = e.Position;
        }
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