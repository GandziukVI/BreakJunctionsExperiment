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

        I_SMU Device;
        IMotion Motor;

        #endregion

        #region User interfaces to realize SMU

        private SourceDeviceConfiguration sourceDeviceConfiguration;

        #endregion

        #region I-V data handling and presenting

        private List<PointD> _CurrentIV_Curve;
        public List<PointD> CurrentIV_Curve
        {
            get { return _CurrentIV_Curve; }
            set { _CurrentIV_Curve = value; }
        }

        private ExperimentalIV_DataSource _experimentalIV_DataSource;
        private LineGraph _IV_LineGraph;

        MeasureIV IV_Curve;
        BackgroundWorker backgroundIV_Measure;

        private IV_MeasurementLog _IV_MeasurementLog;
        private IV_SingleMeasurement _IV_SingleMeasurement;

        private SaveFileDialog _SaveIV_MeasureDialog;
        private string _SaveIV_MeasuremrentFileName;
        private static int _IV_FilesCounter = 0;

        #endregion

        #region Time Trace data handling and presenting

        private List<PointD> _CurrentTimeTrace;
        public List<PointD> CurrentTimeTrace
        {
            get { return _CurrentTimeTrace; }
            set { _CurrentIV_Curve = value; }
        }

        private ExperimentalTimetraceDataSource _experimentalTimeTraceDataSource;
        private LineGraph _TimeTraceLineGraph;

        MeasureTimeTrace TimeTraceCurve;
        BackgroundWorker backgroundTimeTraceMeasure;

        private TimeTraceMeasurementLog _TimeTraceMeasurementLog;
        private TimeTraceSingleMeasurement _TimeTraceSingleMeasurement;

        private SaveFileDialog _SaveTimeTraceMeasureDialog;
        private string _SaveTimeTraceMeasuremrentFileName;
        private static int _TimeTraceFilesCounter = 0;

        #endregion

        public MainWindow()
		{
			this.InitializeComponent();

            #region Test commands

            #endregion

            #region Interface model-view interactions

            this.DataContext = IV_And_TimeTraceViewModel.Instance;

            this.radioIVSourceVoltage.DataContext = ModelViewInteractions.IV_VoltageChangedModel;
            this.radioIVSourceCurrent.DataContext = ModelViewInteractions.IV_CurrentChangedModel;

            AllEventsHandler.Instance.Motion += OnMotionPositionMeasured;

            #endregion

            #region Background I-V Measuremrent

            backgroundIV_Measure = new BackgroundWorker();
            backgroundIV_Measure.WorkerSupportsCancellation = true;
            backgroundIV_Measure.WorkerReportsProgress = true;
            backgroundIV_Measure.DoWork += backgroundIV_Measure_DoWork;
            backgroundIV_Measure.ProgressChanged += backgroundIV_Measure_ProgressChanged;
            backgroundIV_Measure.RunWorkerCompleted += backgroundIV_Measure_RunWorkerCompleted;

            #endregion

            #region Background Time Trace Measurement

            backgroundTimeTraceMeasure = new BackgroundWorker();
            backgroundTimeTraceMeasure.WorkerSupportsCancellation = true;
            backgroundTimeTraceMeasure.WorkerReportsProgress = true;
            backgroundTimeTraceMeasure.DoWork += backgroundTimeTraceMeasureDoWork;
            backgroundTimeTraceMeasure.RunWorkerCompleted += backgroundTimeTrace_RunWorkerCompleted;

            #endregion

            #region Save I-V data to file dialog configuration

            _SaveIV_MeasuremrentFileName = string.Empty;

            _SaveIV_MeasureDialog = new SaveFileDialog();
            _SaveIV_MeasureDialog.FileName = "IV_Measurement";
            _SaveIV_MeasureDialog.DefaultExt = ".dat";
            _SaveIV_MeasureDialog.Filter = "Measure data (.dat)|*.dat";

            #endregion

            #region Save Time Trace data to file dialog configuration

            _SaveTimeTraceMeasuremrentFileName = string.Empty;

            _SaveTimeTraceMeasureDialog = new SaveFileDialog();
            _SaveTimeTraceMeasureDialog.FileName = "TimeTraceMeasurement";
            _SaveTimeTraceMeasureDialog.DefaultExt = ".dat";
            _SaveTimeTraceMeasureDialog.Filter = "Measure data (.dat)|*.dat";

            #endregion
        }

        #region Menu actions realization

        private void onMenuOpenClick(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
		}

		private void onMenuSaveClick(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
		}
		
		private void onMenuExitClick(object sender, System.Windows.RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void onSetSMU_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            sourceDeviceConfiguration = new SourceDeviceConfiguration();
            sourceDeviceConfiguration.ShowDialog();
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
            if (sourceDeviceConfiguration != null)
            {
                #region Chart rendering settings

                //Initializing a new plot on I-V chart
                if (_IV_LineGraph != null)
                {
                    //Detaching receive event from "old" data source
                    _experimentalIV_DataSource.DetachPointReceiveEvent();
                    _IV_LineGraph.Remove();
                }
                //Creating new plot and attaching it to the chart
                _CurrentIV_Curve = new List<PointD>();
                _experimentalIV_DataSource = new ExperimentalIV_DataSource(_CurrentIV_Curve);
                _experimentalIV_DataSource.AttachPointReceiveEvent();
                _IV_LineGraph = new LineGraph(_experimentalIV_DataSource);
                _IV_LineGraph.AddToPlotter(chartIV_Curves);

                #endregion

                //Getting SMU device
                Device = sourceDeviceConfiguration.Keithley2602A_DeviceSettings.Device;

                #region I-V measurement configuration

                var ExperimentSettings = IV_And_TimeTraceViewModel.Instance;

                var StartValue = ExperimentSettings.IV_MeasurementStartValue;                
                var EndValue = ExperimentSettings.IV_MeasurementEndValue;
                var Step = ExperimentSettings.IV_MeasurementStep;
                var NumberOfAverages = ExperimentSettings.IV_MeasurementNumberOfAverages;
                var TimeDelay = ExperimentSettings.IV_MeasurementTimeDelay;

                SourceMode DeviceSourceMode = SourceMode.Voltage;

                if (this.radioIVSourceVoltage.IsChecked == true)
                {
                    DeviceSourceMode = SourceMode.Voltage;
                }
                else if (this.radioIVSourceCurrent.IsChecked == true)
                {
                    DeviceSourceMode = SourceMode.Current;
                }

                IV_Curve = new MeasureIV(StartValue, EndValue, Step, NumberOfAverages, TimeDelay, DeviceSourceMode, Device);

                #endregion

                #region Saving I-V data into files

                var _IV_FileNumber = String.Format("_{0}{1}{2}", (_IV_FilesCounter / 100) % 10, (_IV_FilesCounter / 10) % 10, _IV_FilesCounter % 10);
                string newFileName = string.Empty;

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileName))
                {
                    _IV_MeasurementLog = new IV_MeasurementLog((new FileInfo(_SaveIV_MeasuremrentFileName)).DirectoryName + "\\IV_MeasurementLog.dat");
                    
                    newFileName = _SaveIV_MeasuremrentFileName.Insert(_SaveIV_MeasuremrentFileName.LastIndexOf('.'), _IV_FileNumber);
                    ++_IV_FilesCounter;
                }

                if (!string.IsNullOrEmpty(_SaveIV_MeasuremrentFileName))
                {

                    string fileName = (new FileInfo(newFileName)).Name;

                    string sourceMode = string.Empty;

                    if (this.radioIVSourceVoltage.IsChecked == true)
                    {
                        sourceMode = "Source mode: Voltage";
                    }
                    else if (this.radioIVSourceCurrent.IsChecked == true)
                    {
                        sourceMode = "SourceMode: Current";
                    }

                    double micrometricBoltPosition = double.NaN;
                    double.TryParse(this.textBoxIV_MicrometricBoltPosition.Text, numberStyle, culture, out micrometricBoltPosition);

                    string comment = this.textBoxIV_Comment.Text;

                    _IV_MeasurementLog.AddNewIV_MeasurementLog(fileName, sourceMode, micrometricBoltPosition, comment);
                }

                if (_IV_SingleMeasurement != null)
                    _IV_SingleMeasurement.Dispose();


                _IV_SingleMeasurement = new IV_SingleMeasurement(newFileName);

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
            if((isInitSuccess == true) && (backgroundIV_Measure.IsBusy == false))
                backgroundIV_Measure.RunWorkerAsync();
        }
		
		private void on_cmdIV_StopMeasurementClick(object sender, RoutedEventArgs e)
		{
            //Canceling I-V measures
            if(backgroundIV_Measure.IsBusy == true)
                backgroundIV_Measure.CancelAsync();
		}

        private void backgroundIV_Measure_DoWork(object sender, DoWorkEventArgs e)
        {
            //Updating interface to show that measurement is in process
            this.Dispatcher.BeginInvoke(new Action(delegate()
            {
                this.labelMeasurementStatus.Content = "In process...";
            }));
            
            //Starting measurements
            IV_Curve.StartMeasurement(sender, e);
        }

        private void backgroundIV_Measure_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Updating interface to show measurement progress
            this.progressBarMeasurementProgress.Value = e.ProgressPercentage;
        }

        private void backgroundIV_Measure_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Updating interface to show that measurement is completed
            this.labelMeasurementStatus.Content = "Ready";
        }

        private void on_cmdIV_DataFileNameBrowseClick(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            Nullable<bool> dialogResult = _SaveIV_MeasureDialog.ShowDialog();

            if (dialogResult == true)
            {
                _IV_FilesCounter = 0;
                _SaveIV_MeasuremrentFileName = _SaveIV_MeasureDialog.FileName;
                this.textBoxIV_FileName.Text = _SaveIV_MeasureDialog.SafeFileName;
            }
        }

        #endregion

        #region Time Trace Measurements Interface Interactions

        private bool InitTimeTraceMeasurements()
        {
            #region SMU, rendering and save data configurations

            if (sourceDeviceConfiguration != null)
            {
                #region Chart rendering settings

                if (_TimeTraceLineGraph != null)
                {
                    _experimentalTimeTraceDataSource.DetachPointReceiveEvent();
                    _TimeTraceLineGraph.Remove();
                    _CurrentTimeTrace.Clear();
                }

                _CurrentTimeTrace = new List<PointD>();
                _experimentalTimeTraceDataSource = new ExperimentalTimetraceDataSource(_CurrentTimeTrace);
                _experimentalTimeTraceDataSource.AttachPointReceiveEvent();
                _TimeTraceLineGraph = new LineGraph(_experimentalTimeTraceDataSource);
                _TimeTraceLineGraph.AddToPlotter(chartTimeTrace);

                #endregion

                //Getting SMU device
                Device = sourceDeviceConfiguration.Keithley2602A_DeviceSettings.Device;

                #region Time trace measurement configuration

                var pAddress = sourceDeviceConfiguration.Keithley2602A_DeviceSettings.DeviceSettings.PrimaryAddress;
                var sAddress = sourceDeviceConfiguration.Keithley2602A_DeviceSettings.DeviceSettings.SecondaryAddress;
                var bNumber = sourceDeviceConfiguration.Keithley2602A_DeviceSettings.DeviceSettings.BoardNumber;

                if (Motor != null)
                    Motor.Dispose();

                var motor = new FAULHABER_MINIMOTOR_SA("COM4");
                motor.EnableDevice();
                
                Motor = motor;

                if (TimeTraceCurve != null)
                {
                    TimeTraceCurve.Dispose();
                }

                var ExperimentSettings = IV_And_TimeTraceViewModel.Instance;

                var valueThroughTheStructure = ExperimentSettings.TimeTraceMeasurementValueThrougtTheStructure;
                var isTimeTraceVoltageModeChecked = ExperimentSettings.IsTimeTraceMeasurementVoltageModeChecked;
                var isTimeTraceCurrentModeChecked = ExperimentSettings.IsTimeTraceMeasurementCurrentModeChecked;

                var selectedTimeTraceModeItem = (tabControlTimeTraceMeasurementParameters.SelectedItem as TabItem).Header.ToString();

                switch (selectedTimeTraceModeItem)
                {
                    case "Distance":
                        {
                            var motionStartPosition = ExperimentSettings.TimeTraceMeasurementDistanceMotionStartPosition;
                            var motionFinalDestination = ExperimentSettings.TimeTraceMeasurementDistanceMotionFinalDestination;

                            if (isTimeTraceVoltageModeChecked == true)
                                TimeTraceCurve = new MeasureTimeTrace(Motor, motionStartPosition, motionFinalDestination, Device, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                            else if (isTimeTraceCurrentModeChecked == true)
                                TimeTraceCurve = new MeasureTimeTrace(Motor, motionStartPosition, motionFinalDestination, Device, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                        } break;
                    case "Distance (Repetitive)":
                        {
                            var motionRepetitiveStartPosition = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveStartPosition;
                            var motionRepetitiveEndPosition = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveEndPosition;

                            if (isTimeTraceVoltageModeChecked == true)
                                TimeTraceCurve = new MeasureTimeTrace(Motor, motionRepetitiveStartPosition, motionRepetitiveEndPosition, Device, KEITHLEY_2601A_SourceMode.Voltage, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
                            else if (isTimeTraceCurrentModeChecked == true)
                                TimeTraceCurve = new MeasureTimeTrace(Motor, motionRepetitiveStartPosition, motionRepetitiveEndPosition, Device, KEITHLEY_2601A_SourceMode.Current, KEITHLEY_2601A_MeasureMode.Resistance, valueThroughTheStructure);
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

                var _TimeTraceFileNumber = String.Format("_{0}{1}{2}", (_TimeTraceFilesCounter / 100) % 10, (_TimeTraceFilesCounter / 10) % 10, _TimeTraceFilesCounter % 10);
                string newFileName = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileName))
                {
                    _TimeTraceMeasurementLog = new TimeTraceMeasurementLog((new FileInfo(_SaveTimeTraceMeasuremrentFileName)).DirectoryName + "\\TimeTraceMeasurementLog.dat");

                    newFileName = _SaveTimeTraceMeasuremrentFileName.Insert(_SaveTimeTraceMeasuremrentFileName.LastIndexOf('.'), _TimeTraceFileNumber);
                    ++_TimeTraceFilesCounter;
                }

                string sourceMode = string.Empty;

                if (!string.IsNullOrEmpty(_SaveTimeTraceMeasuremrentFileName))
                {
                    string fileName = (new FileInfo(newFileName)).Name;

                    if (this.radioTimeTraceSourceVoltage.IsChecked == true)
                    {
                        sourceMode = "Source mode: Voltage";
                    }
                    else if (this.radioTimeTraceSourceCurrent.IsChecked == true)
                    {
                        sourceMode = "SourceMode: Current";
                    }

                    double micrometricBoltPosition = double.NaN;
                    double.TryParse(this.textBoxTimeTraceDistanceMicrometricBoltCurrentPosition.Text, numberStyle, culture, out micrometricBoltPosition);

                    string comment = this.textBoxTimeTraceComment.Text;

                    _TimeTraceMeasurementLog.AddNewTimeTraceMeasurementLog(fileName, sourceMode, valueThroughTheStructure, comment);
                }

                SourceMode _sourceMode = SourceMode.Voltage; //Source mode is voltage by default

                if (sourceMode == "Source mode: Voltage")
                    _sourceMode = SourceMode.Voltage;
                else if (sourceMode == "SourceMode: Current")
                    _sourceMode = SourceMode.Current;

                if (_TimeTraceSingleMeasurement != null)
                    _TimeTraceSingleMeasurement.Dispose();
                
                _TimeTraceSingleMeasurement = new TimeTraceSingleMeasurement(newFileName, _sourceMode);

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
                backgroundTimeTraceMeasure.RunWorkerAsync();
            }
            else MessageBox.Show("The device was not initialized!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void on_cmdTimeTraceStopMeasurementClick(object sender, RoutedEventArgs e)
		{
            backgroundTimeTraceMeasure.CancelAsync();
		}

        private void backgroundTimeTraceMeasureDoWork(object sender, DoWorkEventArgs e)
        {
            if ((Device != null) && (Device.InitDevice()))
            {

                var ExperimentSettings = IV_And_TimeTraceViewModel.Instance;
                var numerCycles = ExperimentSettings.TimeTraceMeasurementDistanceRepetitiveNumberCycles;

                var selectedTimeTraceModeHeader = ExperimentSettings.TimeTraceMeasurementSelectedTabIndex;

                switch (selectedTimeTraceModeHeader)
                {
                    case 0: //"Distance" measurement
                        {
                            TimeTraceCurve.StartMeasurement(sender, e, MotionKind.Single);
                        } break;
                    case 1: //"Distance (Repetitive)" measurement
                        {
                            TimeTraceCurve.StartMeasurement(sender, e, MotionKind.Repetitive, numerCycles);
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

        private void backgroundTimeTrace_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void on_cmdTimeTraceDataFileNameBrowseClick(object sender, RoutedEventArgs e)
        {
            //Choosing file name to save data
            Nullable<bool> dialogResult = _SaveTimeTraceMeasureDialog.ShowDialog();

            if (dialogResult == true)
            {
                _TimeTraceFilesCounter = 0;
                _SaveTimeTraceMeasuremrentFileName = _SaveTimeTraceMeasureDialog.FileName;
                this.textBoxTimeTraceFileName.Text = _SaveTimeTraceMeasureDialog.SafeFileName;
            }
        }

        private void OnMotionPositionMeasured(object sender, Motion_EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate() 
                {
                    this.textBoxTimeTraceDistanceMicrometricBoltCurrentPosition.Text = e.Position.ToString("E8", culture);
                }));
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