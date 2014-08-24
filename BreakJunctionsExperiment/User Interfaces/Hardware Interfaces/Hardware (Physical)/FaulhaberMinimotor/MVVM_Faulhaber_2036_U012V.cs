using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace BreakJunctions
{
    public class MVVM_Faulhaber_2036_U012V : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion

        #region COM port

        private string[] _AvaliablePorts = SerialPort.GetPortNames();
        public string[] AvaliablePorts
        {
            get { return _AvaliablePorts; }
            set
            {
                _AvaliablePorts = value;
                OnPropertyChanged("AvaliablePorts");
            }
        }

        private string _SelectedPort;
        public string SelectedPort
        {
            get { return _SelectedPort; }
            set 
            {
                _SelectedPort = value;
                OnPropertyChanged("SelectedPort");
            }
        }

        #endregion
    }
}
