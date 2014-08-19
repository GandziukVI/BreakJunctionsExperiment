using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BreakJunctions
{
    public class TimeTraceMeasurementSettingsDataModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region Time trace measurement model-view interactions

        #region 1-st channel settings

        //Source mode settings
        private bool _IsTimeTraceMeasurementChannel_01_VoltageModeChecked = true;
        public bool IsTimeTraceMeasurementChannel_01_VoltageModeChecked
        {
            get { return _IsTimeTraceMeasurementChannel_01_VoltageModeChecked; }
            set
            {
                _IsTimeTraceMeasurementChannel_01_VoltageModeChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementChannel_01_VoltageModeChecked");
            }
        }
        private bool _IsTimeTraceMeasurementChannel_01_CurrentModeChecked = false;
        public bool IsTimeTraceMeasurementChannel_01_CurrentModeChecked
        {
            get { return _IsTimeTraceMeasurementChannel_01_CurrentModeChecked; }
            set
            {
                _IsTimeTraceMeasurementChannel_01_CurrentModeChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementChannel_01_CurrentModeChecked");
            }
        }

        //Measurement value settings
        private double _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure = 0.02;
        public double TimeTraceMeasurementChannel_01_ValueThrougtTheStructure
        {
            get
            {
                return _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier);
            }
            set
            {
                _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_01_ValueThrougtTheStructure");
            }
        }
        private string _TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier = "None";
        public string TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier
        {
            get { return _TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier; }
            set
            {
                _TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier");
            }
        }

        //File name settings
        private string _TimeTraceMeasurementChannel_01_DataFileName = "TimeTraceMeasurementChannel_01_.dat";
        public string TimeTraceMeasurementChannel_01_DataFileName
        {
            get { return _TimeTraceMeasurementChannel_01_DataFileName; }
            set
            {
                _TimeTraceMeasurementChannel_01_DataFileName = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_01_DataFileName");
            }
        }

        #endregion

        #region 2-nd channel settings

        //Source mode settings
        private bool _IsTimeTraceMeasurementChannel_02_VoltageModeChecked = true;
        public bool IsTimeTraceMeasurementChannel_02_VoltageModeChecked
        {
            get { return _IsTimeTraceMeasurementChannel_02_VoltageModeChecked; }
            set
            {
                _IsTimeTraceMeasurementChannel_02_VoltageModeChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementChannel_02_VoltageModeChecked");
            }
        }
        private bool _IsTimeTraceMeasurementChannel_02_CurrentModeChecked = false;
        public bool IsTimeTraceMeasurementChannel_02_CurrentModeChecked
        {
            get { return _IsTimeTraceMeasurementChannel_02_CurrentModeChecked; }
            set
            {
                _IsTimeTraceMeasurementChannel_02_CurrentModeChecked = value;
                OnPropertyChanged("IsTimeTraceMeasurementChannel_02_CurrentModeChecked");
            }
        }

        //Measurement value settings
        private double _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure = 0.02;
        public double TimeTraceMeasurementChannel_02_ValueThrougtTheStructure
        {
            get
            {
                return _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier);
            }
            set
            {
                _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_02_ValueThrougtTheStructure");
            }
        }
        private string _TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier = "None";
        public string TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier
        {
            get { return _TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier; }
            set
            {
                _TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier");
            }
        }

        //File name settings
        private string _TimeTraceMeasurementChannel_02_DataFileName = "TimeTraceMeasurementChannel_02_.dat";
        public string TimeTraceMeasurementChannel_02_DataFileName
        {
            get { return _TimeTraceMeasurementChannel_02_DataFileName; }
            set
            {
                _TimeTraceMeasurementChannel_02_DataFileName = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_02_DataFileName");
            }
        }

        #endregion

        #region General settings

        //Measurement settings
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

        //Comments
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
    }
}
