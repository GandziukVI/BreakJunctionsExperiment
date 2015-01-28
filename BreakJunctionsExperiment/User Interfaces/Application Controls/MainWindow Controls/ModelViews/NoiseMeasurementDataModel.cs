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

        private int _AmplificationCoefficient = 10000;
        public int AmplificationCoefficient
        {
            get { return _AmplificationCoefficient; }
            set 
            {
                _AmplificationCoefficient = value;
                OnPropertyChanged("AmplificationCoefficient");
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
