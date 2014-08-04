using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BreakJunctions
{
    using System.ComponentModel;

    public class IV_VoltageChangedViewModel : INotifyPropertyChanged
    {
        private bool _IV_VoltageModeIsChecked = true;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IV_VoltageModeIsChecked
        {
            get { return _IV_VoltageModeIsChecked; }
            set 
            {
                _IV_VoltageModeIsChecked = value;
                OnPropertyChanged("IV_VoltageModeIsChecked");
            }
        }
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
    public class IV_CurrentChangedViewModel : INotifyPropertyChanged
    {
        private bool _IV_CurrentModeIsChecked = false;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IV_CurrentModeIsChecked
        {
            get { return _IV_CurrentModeIsChecked; }
            set
            {
                _IV_CurrentModeIsChecked = value;
                OnPropertyChanged("IV_CurrentModeIsChecked");
            }
        }
        protected virtual void OnPropertyChanged(string PropertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
