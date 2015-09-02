using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions
{
    public class MVVM_Motion : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region General Settings

        private int _pointsPerMilimeter = 2000;
        public int PointsPerMilimeter
        {
            get { return _pointsPerMilimeter; }
            set
            {
                _pointsPerMilimeter = value;
                OnPropertyChanged("PointsPerMilimeter");
            }
        }

        private double _speedGoing_UP = 4.8;
        public double SpeedGoing_UP
        {
            get { return _speedGoing_UP; }
            set
            {
                _speedGoing_UP = value;
                OnPropertyChanged("SpeedGoing_UP");
            }
        }

        private double _speedGoing_DOWN = 4.8;
        public double SpeedGoing_DOWN
        {
            get { return _speedGoing_DOWN; }
            set
            {
                _speedGoing_DOWN = value;
                OnPropertyChanged("SpeedGoing_DOWN");
            }
        }

        private bool _isMotorEnabled = true;
        public bool IsMotorEnabled
        {
            get { return _isMotorEnabled; }
            set
            {
                _isMotorEnabled = value;
                OnPropertyChanged("IsMotorEnabled");
                OnPropertyChanged("CMD_EnableDisable_Motor_Content");
            }
        }

        private double _startPosition = 0.0;
        public double StartPosition
        {
            get { return _startPosition; }
            set
            {
                _startPosition = value;
                OnPropertyChanged("StartPosition");
            }
        }

        private double _currentPosition = 0.0;
        public double CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                _currentPosition = value;
                OnPropertyChanged("CurrentPosition");
            }
        }

        private double _finalDestination = 0.005;
        public double FinalDestination
        {
            get { return _finalDestination; }
            set
            {
                _finalDestination = value;
                OnPropertyChanged("FinalDestination");
            }
        }

        private string _CMD_EnableDisable_Motor_Content = "DISABLE MOTOR";
        public string CMD_EnableDisable_Motor_Content
        {
            get
            {
                if (_isMotorEnabled)
                    _CMD_EnableDisable_Motor_Content = "DISABLE MOTOR";
                else
                    _CMD_EnableDisable_Motor_Content = "ENABLE MOTOR";

                return _CMD_EnableDisable_Motor_Content;
            }
            set
            {
                _CMD_EnableDisable_Motor_Content = value;
                OnPropertyChanged("CMD_EnableDisable_Motor_Content");
            }
        }

        #endregion

        #region Distance

        private bool _direction_UP = true;
        public bool Direction_UP
        {
            get { return _direction_UP; }
            set
            {
                _direction_UP = value;
                OnPropertyChanged("Direction_UP");
            }
        }

        private bool _direction_DOWN = false;
        public bool Direction_DOWN
        {
            get { return _direction_DOWN; }
            set
            {
                _direction_DOWN = value;
                OnPropertyChanged("Direction_DOWN");
            }
        }

        #endregion

        #region Destance Repetitive

        private int _numberCycles = 10;
        public int NumberCycles
        {
            get { return _numberCycles; }
            set
            {
                _numberCycles = value;
                OnPropertyChanged("NumberCycles");
            }
        }

        #endregion

        #region Time

        private TimeSpan _measureTime = new TimeSpan(0, 0, 0);
        public TimeSpan MeasureTime
        {
            get { return _measureTime; }
            set
            {
                _measureTime = value;
                OnPropertyChanged("MeasureTime");
            }
        }

        #endregion

        #region Fixed R

        private double _r_Value = 12900;
        public double R_Value
        {
            get { return _r_Value; }
            set
            {
                _r_Value = value;
                OnPropertyChanged("R_Value");
            }
        }

        private double _Current_R_Value = 12900;
        public double Current_R_Value
        {
            get { return _Current_R_Value; }
            set
            {
                _Current_R_Value = value;
                OnPropertyChanged("Current_R_Value");
            }
        }

        private double _allowableDeviation = 20.0;
        public double AllowableDeviation
        {
            get { return _allowableDeviation; }
            set
            {
                _allowableDeviation = value;
                OnPropertyChanged("AllowableDeviation");
            }
        }

        private bool _isChannel_01_Selected = true;
        public bool IsChannel_01_Selected
        {
            get { return _isChannel_01_Selected; }
            set
            {
                _isChannel_01_Selected = value;
                OnPropertyChanged("IsChannel_01_Selected");
            }
        }

        private bool _isChannel_02_Selected = false;
        public bool IsChannel_02_Selected
        {
            get { return _isChannel_02_Selected; }
            set
            {
                _isChannel_02_Selected = value;
                OnPropertyChanged("IsChannel_02_Selected");
            }
        }

        #endregion
    }
}
