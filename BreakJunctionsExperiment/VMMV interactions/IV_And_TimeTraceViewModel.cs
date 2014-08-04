using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BreakJunctions
{
    public class IV_And_TimeTraceViewModel
    {
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

        #region I-V measurement model-view interactions

        //Source mode settings
        private bool _IsIV_MeasurementVoltageModeChecked = true;
        public bool IsIV_MeasurementVoltageModeChecked
        {
            get { return _IsIV_MeasurementVoltageModeChecked; }
            set { _IsIV_MeasurementVoltageModeChecked = value; }
        }
        private bool _IsIV_MeasurementCurrentModeChecked = true;
        public bool IsIV_MeasurementCurrentModeChecked
        {
            get { return _IsIV_MeasurementCurrentModeChecked; }
            set { _IsIV_MeasurementCurrentModeChecked = value; }
        }

        //Measurement value settings
        private double _IV_MeasurementStartValue = 0.0;
        public double IV_MeasurementStartValue
        {
            get 
            {
                return _IV_MeasurementStartValue * HandlingUserInput.GetMultiplier(_IV_MeasurementStartValueMultiplier); 
            }
            set { _IV_MeasurementStartValue = value; }
        }
        private string _IV_MeasurementStartValueMultiplier = "None";
        public string IV_MeasurementStartValueMultiplier
        {
            get { return _IV_MeasurementStartValueMultiplier; }
            set { _IV_MeasurementStartValueMultiplier = value; }
        }

        private double _IV_MeasurementEndValue = 1.0;
        public double IV_MeasurementEndValue
        {
            get 
            {
                return _IV_MeasurementEndValue * HandlingUserInput.GetMultiplier(_IV_MeasurementEndValueMultiplier); 
            }
            set { _IV_MeasurementEndValue = value; }
        }
        private string _IV_MeasurementEndValueMultiplier = "None";
        public string IV_MeasurementEndValueMultiplier
        {
            get { return _IV_MeasurementEndValueMultiplier; }
            set { _IV_MeasurementEndValueMultiplier = value; }
        }

        private double _IV_MeasurementStep = 0.01;
        public double IV_MeasurementStep
        {
            get 
            {
                return _IV_MeasurementStep * HandlingUserInput.GetMultiplier(_IV_MeasurementStepValueMultiplier);
            }
            set { _IV_MeasurementStep = value; }
        }
        private string _IV_MeasurementStepValueMultiplier = "None";
        public string IV_MeasurementStepValueMultiplier
        {
            get { return _IV_MeasurementStepValueMultiplier; }
            set { _IV_MeasurementStepValueMultiplier = value; }
        }

        //Measurement parameters
        private int _IV_MeasurementNumberOfAverages = 2;
        public int IV_MeasurementNumberOfAverages
        {
            get { return _IV_MeasurementNumberOfAverages; }
            set { _IV_MeasurementNumberOfAverages = value; }
        }
        private double _IV_MeasurementTimeDelay = 0.005;
        public double IV_MeasurementTimeDelay
        {
            get 
            {
                return _IV_MeasurementTimeDelay * HandlingUserInput.GetMultiplier(_IV_MeasurementTimeDelayValueMultiplier);
            }
            set { _IV_MeasurementTimeDelay = value; }
        }
        private string _IV_MeasurementTimeDelayValueMultiplier = "None";
        public string IV_MeasurementTimeDelayValueMultiplier
        {
            get { return _IV_MeasurementTimeDelayValueMultiplier; }
            set { _IV_MeasurementTimeDelayValueMultiplier = value; }
        }

        //Saving data
        private string _IV_MeasurementDataFileName = "IV_Measurement.dat";
        public string IV_MeasurementDataFileName
        {
            get { return _IV_MeasurementDataFileName; }
            set { _IV_MeasurementDataFileName = value; }
        }
        private string _IV_MeasurementDataComment = "";
        public string IV_MeasurementDataComment
        {
            get { return _IV_MeasurementDataComment; }
            set { _IV_MeasurementDataComment = value; }
        }

        #endregion

        #region Time trace measurement model-view interactions

        //Source mode settings
        private bool _IsTimeTraceMeasurementVoltageModeChecked = true;
        public bool IsTimeTraceMeasurementVoltageModeChecked
        {
            get { return _IsTimeTraceMeasurementVoltageModeChecked; }
            set { _IsTimeTraceMeasurementVoltageModeChecked = value; }
        }
        private bool _IsTimeTraceMeasurementCurrentModeChecked = false;
        public bool IsTimeTraceMeasurementCurrentModeChecked
        {
            get { return _IsTimeTraceMeasurementCurrentModeChecked; }
            set { _IsTimeTraceMeasurementCurrentModeChecked = value; }
        }

        //Measurement value settings
        private double _TimeTraceMeasurementValueThrougtTheStructure = 0.0;
        public double TimeTraceMeasurementValueThrougtTheStructure
        {
            get 
            {
                return _TimeTraceMeasurementValueThrougtTheStructure * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementValueThrougtTheStructureMultiplier);
            }
            set { _TimeTraceMeasurementValueThrougtTheStructure = value; }
        }
        private string _TimeTraceMeasurementValueThrougtTheStructureMultiplier = "None";
        public string TimeTraceMeasurementValueThrougtTheStructureMultiplier
        {
            get { return _TimeTraceMeasurementValueThrougtTheStructureMultiplier; }
            set { _TimeTraceMeasurementValueThrougtTheStructureMultiplier = value; }
        }

        //Measurement parameters
        private int _TimeTraceMeasurementNumberOfAverages = 2;
        public int TimeTraceMeasurementNumberOfAverages
        {
            get { return _TimeTraceMeasurementNumberOfAverages; }
            set { _TimeTraceMeasurementNumberOfAverages = value; }
        }

        private double _TimeTraceMeasurementTimeDelay = 0.005;
        public double TimeTraceMeasurementTimeDelay
        {
            get 
            {
                return _TimeTraceMeasurementTimeDelay * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementTimeDelayMultiplier);
            }
            set { _TimeTraceMeasurementTimeDelay = value; }
        }
        private string _TimeTraceMeasurementTimeDelayMultiplier = "None";
        public string TimeTraceMeasurementTimeDelayMultiplier
        {
            get { return _TimeTraceMeasurementTimeDelayMultiplier; }
            set { _TimeTraceMeasurementTimeDelayMultiplier = value; }
        }

        private string _TimeTraceMeasurementDataFileName = "TimeTraceMeasurement.dat";
        public string TimeTraceMeasurementDataFileName
        {
            get { return _TimeTraceMeasurementDataFileName; }
            set { _TimeTraceMeasurementDataFileName = value; }
        }
        private string _TimeTraceMeasurementDataComment = "";
        public string TimeTraceMeasurementDataComment
        {
            get { return _TimeTraceMeasurementDataComment; }
            set { _TimeTraceMeasurementDataComment = value; }
        }

        //Motion parameters
        private int _TimeTraceMeasurementMovingVelosity = 0;
        public int TimeTraceMeasurementMovingVelosity
        {
            get { return _TimeTraceMeasurementMovingVelosity; }
            set { _TimeTraceMeasurementMovingVelosity = value; }
        }

        private int _TimeTraceMeasurementSelectedTabIndex = 0;
        public int TimeTraceMeasurementSelectedTabIndex
        {
            get { return _TimeTraceMeasurementSelectedTabIndex; }
            set { _TimeTraceMeasurementSelectedTabIndex = value; }
        }

        #region Motion "Distance" parameters

        private bool _IsTimeTraceMeasurementMotionModeUpChecked = true;
        public bool IsTimeTraceMeasurementMotionModeUpChecked
        {
            get { return _IsTimeTraceMeasurementMotionModeUpChecked; }
            set { _IsTimeTraceMeasurementMotionModeUpChecked = value; }
        }
        private bool _IsTimeTraceMeasurementMotionModeDownChecked = true;
        public bool IsTimeTraceMeasurementMotionModeDownChecked
        {
            get { return _IsTimeTraceMeasurementMotionModeDownChecked; }
            set { _IsTimeTraceMeasurementMotionModeDownChecked = value; }
        }
        private double _TimeTraceMeasurementMotionCurrentDestination = 0.0;
        public double TimeTraceMeasurementMotionCurrentDestination
        {
            get { return _TimeTraceMeasurementMotionCurrentDestination; }
            set { _TimeTraceMeasurementMotionCurrentDestination = value; }
        }
        private double _TimeTraceMeasurementMotionFinalDestination = 0.0;
        public double TimeTraceMeasurementMotionFinalDestination
        {
            get { return _TimeTraceMeasurementMotionFinalDestination; }
            set { _TimeTraceMeasurementMotionFinalDestination = value; }
        }

        #endregion

        #region Motion "Distance (Repetitive)" parameters

        private double _TimeTraceMeasurementDistanceRepetitiveStartPosition = 0.0;
        public double TimeTraceMeasurementDistanceRepetitiveStartPosition
        {
            get { return _TimeTraceMeasurementDistanceRepetitiveStartPosition; }
            set { _TimeTraceMeasurementDistanceRepetitiveStartPosition = value; }
        }
        private double _TimeTraceMeasurementDistanceRepetitiveEndPosition = 0.0;
        public double TimeTraceMeasurementDistanceRepetitiveEndPosition
        {
            get { return _TimeTraceMeasurementDistanceRepetitiveEndPosition; }
            set { _TimeTraceMeasurementDistanceRepetitiveEndPosition = value; }
        }
        private int _TimeTraceMeasurementDistanceRepetitiveNumberCycles = 10;
        public int TimeTraceMeasurementDistanceRepetitiveNumberCycles
        {
            get { return _TimeTraceMeasurementDistanceRepetitiveNumberCycles; }
            set { _TimeTraceMeasurementDistanceRepetitiveNumberCycles = value; }
        }

        #endregion

        #region Motion "Time" parameters

        private TimeSpan _TimeTraceMeasurementTime_TimeFinal = TimeSpan.Zero;
        public TimeSpan TimeTraceMeasurementTime_TimeFinal
        {
            get { return _TimeTraceMeasurementTime_TimeFinal; }
            set { _TimeTraceMeasurementTime_TimeFinal = value; }
        }

        #endregion

        #region Motion "Fixed R" parameters

        private double _TimeTraceMeasurementFixedR_R_Value = 0.0;
        public double TimeTraceMeasurementFixedR_R_Value
        {
            get { return _TimeTraceMeasurementFixedR_R_Value; }
            set { _TimeTraceMeasurementFixedR_R_Value = value; }
        }

        #endregion

        #endregion
    }
}
