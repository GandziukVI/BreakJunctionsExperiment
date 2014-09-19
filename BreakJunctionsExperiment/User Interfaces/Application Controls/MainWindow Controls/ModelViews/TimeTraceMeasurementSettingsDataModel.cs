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
        private double _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure = 0.02;
        public double TimeTraceMeasurementChannel_01_ValueThrougtTheStructure
        {
            get { return _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure; }
            set
            {
                _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_01_ValueThrougtTheStructure");
            }
        }
        public double TimeTraceMeasurementChannel_01_ValueThrougtTheStructureWithMultiplier
        {
            get { return _TimeTraceMeasurementChannel_01_ValueThrougtTheStructure * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementChannel_01_ValueThrougtTheStructureMultiplier); }
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
        private double _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure = 0.02;
        public double TimeTraceMeasurementChannel_02_ValueThrougtTheStructure
        {
            get { return _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure; }
            set
            {
                _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure = value;
                OnPropertyChanged("TimeTraceMeasurementChannel_02_ValueThrougtTheStructure");
            }
        }
        public double TimeTraceMeasurementChannel_02_ValueThrougtTheStructureWithMultiplier
        {
            get { return _TimeTraceMeasurementChannel_02_ValueThrougtTheStructure * HandlingUserInput.GetMultiplier(_TimeTraceMeasurementChannel_02_ValueThrougtTheStructureMultiplier); }
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
        private int _TimeTraceMeasurementNumberOfAverages = 1;
        public int TimeTraceMeasurementNumberOfAverages
        {
            get { return _TimeTraceMeasurementNumberOfAverages; }
            set
            {
                _TimeTraceMeasurementNumberOfAverages = value;
                OnPropertyChanged("TimeTraceMeasurementNumberOfAverages");
            }
        }

        private double _TimeTraceMeasurementTimeDelay = 0.0;
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

        #endregion

        #endregion
    }
}
