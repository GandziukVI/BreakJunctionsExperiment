using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BreakJunctions
{
    public class MotionParametersDataModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region General settings

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

        private double _TimeTraceMotionSpeed = 4.9;
        public double TimeTraceMotionSpeed
        {
            get { return _TimeTraceMotionSpeed; }
            set
            {
                _TimeTraceMotionSpeed = value;
                OnPropertyChanged("TimeTraceMotionSpeed");
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
    }
}
