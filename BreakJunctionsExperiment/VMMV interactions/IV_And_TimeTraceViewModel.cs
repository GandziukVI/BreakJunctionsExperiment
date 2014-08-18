using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BreakJunctions
{
    /// <summary>
    /// Realizes MVVM interactions for main window
    /// </summary>
    /*public class IV_And_TimeTraceViewModel : INotifyPropertyChanged
    {

        #region Singleton object realization

        private static IV_And_TimeTraceViewModel _Instance;
        public static IV_And_TimeTraceViewModel Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new IV_And_TimeTraceViewModel();
                return _Instance;
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region I-V measurement model-view interactions

        //Source mode settings
        private bool _IsIV_MeasurementVoltageModeChecked = true;
        public bool IsIV_MeasurementVoltageModeChecked
        {
            get { return _IsIV_MeasurementVoltageModeChecked; }
            set 
            {
                _IsIV_MeasurementVoltageModeChecked = value;
                OnPropertyChanged("IsIV_MeasurementVoltageModeChecked");
            }
        }
        private bool _IsIV_MeasurementCurrentModeChecked = true;
        public bool IsIV_MeasurementCurrentModeChecked
        {
            get { return _IsIV_MeasurementCurrentModeChecked; }
            set 
            {
                _IsIV_MeasurementCurrentModeChecked = value;
                OnPropertyChanged("IsIV_MeasurementCurrentModeChecked");
            }
        }

        //Measurement value settings
        private double _IV_MeasurementStartValue = 0.0;
        public double IV_MeasurementStartValue
        {
            get 
            {
                return _IV_MeasurementStartValue * HandlingUserInput.GetMultiplier(_IV_MeasurementStartValueMultiplier); 
            }
            set 
            {
                _IV_MeasurementStartValue = value;
                OnPropertyChanged("IV_MeasurementStartValue");
            }
        }
        private string _IV_MeasurementStartValueMultiplier = "None";
        public string IV_MeasurementStartValueMultiplier
        {
            get { return _IV_MeasurementStartValueMultiplier; }
            set 
            {
                _IV_MeasurementStartValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementStartValueMultiplier");
            }
        }

        private double _IV_MeasurementEndValue = 1.0;
        public double IV_MeasurementEndValue
        {
            get 
            {
                return _IV_MeasurementEndValue * HandlingUserInput.GetMultiplier(_IV_MeasurementEndValueMultiplier); 
            }
            set 
            {
                _IV_MeasurementEndValue = value;
                OnPropertyChanged("IV_MeasurementEndValue");
            }
        }
        private string _IV_MeasurementEndValueMultiplier = "None";
        public string IV_MeasurementEndValueMultiplier
        {
            get { return _IV_MeasurementEndValueMultiplier; }
            set 
            {
                _IV_MeasurementEndValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementEndValueMultiplier");
            }
        }

        private double _IV_MeasurementStep = 0.01;
        public double IV_MeasurementStep
        {
            get 
            {
                return _IV_MeasurementStep * HandlingUserInput.GetMultiplier(_IV_MeasurementStepValueMultiplier);
            }
            set 
            {
                _IV_MeasurementStep = value;
                OnPropertyChanged("IV_MeasurementStep");
            }
        }
        private string _IV_MeasurementStepValueMultiplier = "None";
        public string IV_MeasurementStepValueMultiplier
        {
            get { return _IV_MeasurementStepValueMultiplier; }
            set
            {
                _IV_MeasurementStepValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementStepValueMultiplier");
            }
        }

        //Measurement parameters
        private int _IV_MeasurementNumberOfAverages = 2;
        public int IV_MeasurementNumberOfAverages
        {
            get { return _IV_MeasurementNumberOfAverages; }
            set 
            { 
                _IV_MeasurementNumberOfAverages = value;
                OnPropertyChanged("IV_MeasurementNumberOfAverages");
            }
        }
        private double _IV_MeasurementTimeDelay = 0.005;
        public double IV_MeasurementTimeDelay
        {
            get 
            {
                return _IV_MeasurementTimeDelay * HandlingUserInput.GetMultiplier(_IV_MeasurementTimeDelayValueMultiplier);
            }
            set 
            {
                _IV_MeasurementTimeDelay = value;
                OnPropertyChanged("IV_MeasurementTimeDelay");
            }
        }
        private string _IV_MeasurementTimeDelayValueMultiplier = "None";
        public string IV_MeasurementTimeDelayValueMultiplier
        {
            get { return _IV_MeasurementTimeDelayValueMultiplier; }
            set 
            {
                _IV_MeasurementTimeDelayValueMultiplier = value;
                OnPropertyChanged("IV_MeasurementTimeDelayValueMultiplier");
            }
        }

        //Saving data
        private string _IV_MeasurementDataFileName = "IV_Measurement.dat";
        public string IV_MeasurementDataFileName
        {
            get { return _IV_MeasurementDataFileName; }
            set 
            {
                _IV_MeasurementDataFileName = value;
                OnPropertyChanged("IV_MeasurementDataFileName");
            }
        }
        private string _IV_MeasurementDataComment = "";
        public string IV_MeasurementDataComment
        {
            get { return _IV_MeasurementDataComment; }
            set
            { 
                _IV_MeasurementDataComment = value;
                OnPropertyChanged("IV_MeasurementDataComment");
            }
        }

        #endregion

        #region Time trace measurement model-view interactions

        #region General settings

        //Source mode settings
        private bool _IsTimeTraceMeasurementVoltageModeChecked = true;
        public bool IsTimeTraceMeasurementVoltageModeChecked
        {
            get { return _IsTimeTraceMeasurementVoltageModeChecked; }
            set 
            {
                _IsTimeTraceMeasurementVoltageModeChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementVoltageModeChecked");
            }
        }
        private bool _IsTimeTraceMeasurementCurrentModeChecked = false;
        public bool IsTimeTraceMeasurementCurrentModeChecked
        {
            get { return _IsTimeTraceMeasurementCurrentModeChecked; }
            set 
            {
                _IsTimeTraceMeasurementCurrentModeChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementCurrentModeChecked");
            }
        }

        //Measurement value settings
        private double _TimeTraceMeasurementValueThrougtTheStructure = 0.0;
        public double TimeTraceMeasurementValueThrougtTheStructure
        {
            get 
            {
                return _TimeTraceMeasurementValueThrougtTheStructure * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementValueThrougtTheStructureMultiplier);
            }
            set 
            {
                _TimeTraceMeasurementValueThrougtTheStructure = value;
                OnPropertyChanged("TimeTraceMeasurementValueThrougtTheStructure");
            }
        }
        private string _TimeTraceMeasurementValueThrougtTheStructureMultiplier = "None";
        public string TimeTraceMeasurementValueThrougtTheStructureMultiplier
        {
            get { return _TimeTraceMeasurementValueThrougtTheStructureMultiplier; }
            set 
            {
                _TimeTraceMeasurementValueThrougtTheStructureMultiplier = value;
                OnPropertyChanged("TimeTraceMeasurementValueThrougtTheStructureMultiplier");
            }
        }

        //Measurement parameters
        private int _TimeTraceMeasurementNumberOfAverages = 2;
        public int TimeTraceMeasurementNumberOfAverages
        {
            get { return _TimeTraceMeasurementNumberOfAverages; }
            set 
            { 
                _TimeTraceMeasurementNumberOfAverages = value;
                OnPropertyChanged("TimeTraceMeasurementNumberOfAverages");
            }
        }

        private double _TimeTraceMeasurementTimeDelay = 0.005;
        public double TimeTraceMeasurementTimeDelay
        {
            get 
            {
                return _TimeTraceMeasurementTimeDelay * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementTimeDelayMultiplier);
            }
            set 
            {
                _TimeTraceMeasurementTimeDelay = value;
                OnPropertyChanged("TimeTraceMeasurementTimeDelay");
            }
        }
        private string _TimeTraceMeasurementTimeDelayMultiplier = "None";
        public string TimeTraceMeasurementTimeDelayMultiplier
        {
            get { return _TimeTraceMeasurementTimeDelayMultiplier; }
            set 
            {
                _TimeTraceMeasurementTimeDelayMultiplier = value;
                OnPropertyChanged("TimeTraceMeasurementTimeDelayMultiplier");
            }
        }

        private string _TimeTraceMeasurementDataFileName = "TimeTraceMeasurement.dat";
        public string TimeTraceMeasurementDataFileName
        {
            get { return _TimeTraceMeasurementDataFileName; }
            set 
            {
                _TimeTraceMeasurementDataFileName = value;
                OnPropertyChanged("TimeTraceMeasurementDataFileName");
            }
        }
        private string _TimeTraceMeasurementDataComment = "";
        public string TimeTraceMeasurementDataComment
        {
            get { return _TimeTraceMeasurementDataComment; }
            set 
            {
                _TimeTraceMeasurementDataComment = value;
                OnPropertyChanged("TimeTraceMeasurementDataComment");
            }
        }

        //Motion parameters
        private int _TimeTraceNotificationsPerRevolution = 1000;
        public int TimeTraceNotificationsPerRevolution
        {
            get { return _TimeTraceNotificationsPerRevolution; }
            set 
            {
                _TimeTraceNotificationsPerRevolution = value;
                OnPropertyChanged("TimeTraceMeasurementMovingVelosity");
            }
        }

        private int _TimeTraceMeasurementSelectedTabIndex = 0;
        public int TimeTraceMeasurementSelectedTabIndex
        {
            get { return _TimeTraceMeasurementSelectedTabIndex; }
            set 
            {
                _TimeTraceMeasurementSelectedTabIndex = value;
                OnPropertyChanged("TimeTraceMeasurementSelectedTabIndex");
            }
        }

        #endregion

        #region Motion "Distance" parameters

        private bool _IsTimeTraceMeasurementDistanceMotionModeUpChecked = true;
        public bool IsTimeTraceMeasurementDistanceMotionModeUpChecked
        {
            get { return _IsTimeTraceMeasurementDistanceMotionModeUpChecked; }
            set 
            {
                _IsTimeTraceMeasurementDistanceMotionModeUpChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementDistanceMotionModeUpChecked");
            }
        }
        private bool _IsTimeTraceMeasurementDistanceMotionModeDownChecked = false;
        public bool IsTimeTraceMeasurementDistanceMotionModeDownChecked
        {
            get { return _IsTimeTraceMeasurementDistanceMotionModeDownChecked; }
            set 
            {
                _IsTimeTraceMeasurementDistanceMotionModeDownChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementDistanceMotionModeDownChecked");
            }
        }
        private double _TimeTraceMeasurementDistanceMotionStartPosition = 0.0;
        public double TimeTraceMeasurementDistanceMotionStartPosition
        {
            get { return _TimeTraceMeasurementDistanceMotionStartPosition; }
            set 
            {
                _TimeTraceMeasurementDistanceMotionStartPosition = value;
                OnPropertyChanged("TimeTraceMeasurementDistanceMotionStartPosition");
            }
        }
        private double _TimeTraceMeasurementDistanceMotionCurrentPosition = 0.0;
        public double TimeTraceMeasurementDistanceMotionCurrentPosition
        {
            get { return _TimeTraceMeasurementDistanceMotionCurrentPosition; }
            set 
            { 
                _TimeTraceMeasurementDistanceMotionCurrentPosition = value;
                OnPropertyChanged("TimeTraceMeasurementDistanceMotionCurrentPosition");
            }
        }
        private double _TimeTraceMeasurementDistanceMotionFinalDestination = 0.0;
        public double TimeTraceMeasurementDistanceMotionFinalDestination
        {
            get { return _TimeTraceMeasurementDistanceMotionFinalDestination; }
            set 
            {
                _TimeTraceMeasurementDistanceMotionFinalDestination = value;
                OnPropertyChanged("TimeTraceMeasurementDistanceMotionFinalDestination");
            }
        }

        #endregion

        #region Motion "Distance (Repetitive)" parameters

        private double _TimeTraceMeasurementDistanceRepetitiveStartPosition = 0.0;
        public double TimeTraceMeasurementDistanceRepetitiveStartPosition
        {
            get { return _TimeTraceMeasurementDistanceRepetitiveStartPosition; }
            set 
            {
                _TimeTraceMeasurementDistanceRepetitiveStartPosition = value;
                OnPropertyChanged("TimeTraceMeasurementDistanceRepetitiveStartPosition");
            }
        }
        private double _TimeTraceMeasurementDistanceRepetitiveEndPosition = 0.0;
        public double TimeTraceMeasurementDistanceRepetitiveEndPosition
        {
            get { return _TimeTraceMeasurementDistanceRepetitiveEndPosition; }
            set 
            {
                _TimeTraceMeasurementDistanceRepetitiveEndPosition = value;
                OnPropertyChanged("TimeTraceMeasurementDistanceRepetitiveEndPosition");
            }
        }
        private int _TimeTraceMeasurementDistanceRepetitiveNumberCycles = 10;
        public int TimeTraceMeasurementDistanceRepetitiveNumberCycles
        {
            get { return _TimeTraceMeasurementDistanceRepetitiveNumberCycles; }
            set 
            {
                _TimeTraceMeasurementDistanceRepetitiveNumberCycles = value;
                OnPropertyChanged("TimeTraceMeasurementDistanceRepetitiveNumberCycles");
            }
        }

        #endregion

        #region Motion "Time" parameters

        private TimeSpan _TimeTraceMeasurementTime_TimeFinal = TimeSpan.Zero;
        public TimeSpan TimeTraceMeasurementTime_TimeFinal
        {
            get { return _TimeTraceMeasurementTime_TimeFinal; }
            set 
            {
                _TimeTraceMeasurementTime_TimeFinal = value;
                OnPropertyChanged("TimeTraceMeasurementTime_TimeFinal");
            }
        }

        #endregion

        #region Motion "Fixed R" parameters

        private double _TimeTraceMeasurementFixedR_R_Value = 0.0;
        public double TimeTraceMeasurementFixedR_R_Value
        {
            get { return _TimeTraceMeasurementFixedR_R_Value; }
            set 
            {
                _TimeTraceMeasurementFixedR_R_Value = value;
                OnPropertyChanged("TimeTraceMeasurementFixedR_R_Value");
            }
        }

        #endregion

        #endregion
    }*/
}
