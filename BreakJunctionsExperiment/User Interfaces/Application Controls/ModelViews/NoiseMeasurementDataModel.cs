using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakJunctions
{
    public class NoiseMeasurementDataModel : INotifyPropertyChanged
    {
        #region InotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region NoiseMeasurementDataModel inplementation

        private int _NumberOfSpectra = 100;
        public int MunberOfSpectra
        {
            get { return _NumberOfSpectra; }
            set
            {
                _NumberOfSpectra = value;
                OnPropertyChanged("MunberOfSpectra");
            }
        }

        private int _DisplayUpdateNumber = 5;
        public int DisplayUpdateNumber
        {
            get { return _DisplayUpdateNumber; }
            set
            {
                _DisplayUpdateNumber = value;
                OnPropertyChanged("DisplayUpdateNumber");
            }
        }

        private string _Comment = "";
        public string Comment
        {
            get { return _Comment; }
            set
            {
                _Comment = value;
                OnPropertyChanged("Comment");
            }
        }

        private int _AmplificationCoefficient_CH1 = 10000;
        public int AmplificationCoefficient_CH1
        {
            get { return _AmplificationCoefficient_CH1; }
            set 
            {
                _AmplificationCoefficient_CH1 = value;
                OnPropertyChanged("AmplificationCoefficient_CH1");
            }
        }

        private int _AmplificationCoefficient_CH2 = 10000;
        public int AmplificationCoefficient_CH2
        {
            get { return _AmplificationCoefficient_CH2; }
            set
            {
                _AmplificationCoefficient_CH2 = value;
                OnPropertyChanged("AmplificationCoefficient_CH2");
            }
        }

        private string _SaveCalibrationFileName;
        public string SaveCalibrationFileName
        {
            get { return _SaveCalibrationFileName; }
            set
            {
                _SaveCalibrationFileName = value;
                OnPropertyChanged("SaveCalibrationFileName");
            }
        }

        private string _SaveFileName;
        public string SaveFileName
        {
            get { return _SaveFileName; }
            set
            {
                _SaveFileName = value;
                OnPropertyChanged("SaveFileName");
            }
        }

        #endregion
    }
}
