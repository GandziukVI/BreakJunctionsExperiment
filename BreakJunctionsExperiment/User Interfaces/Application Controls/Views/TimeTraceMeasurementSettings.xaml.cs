using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BreakJunctions
{
    /// <summary>
    /// Interaction logic for TimeTraceMeasurementSettings.xaml
    /// </summary>
    public partial class TimeTraceMeasurementSettings : UserControl
    {
        #region MVVM interactions

        private TimeTraceMeasurementSettingsDataModel _MeasurementSettings;
        public TimeTraceMeasurementSettingsDataModel MeasurementSettings
        {
            get { return _MeasurementSettings; }
        }

        #endregion

        public TimeTraceMeasurementSettings()
        {
            InitializeComponent();

            #region MVVM setup

            _MeasurementSettings = new TimeTraceMeasurementSettingsDataModel();
            _MeasurementSettings.MotionSettings = this.MotionParameters.DataContext as MVVM_Motion;
            this.DataContext = _MeasurementSettings;

            #endregion
        }

        #region Checking User Input

        private void IntegerPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            HandlingUserInput.IntegerPastingHandler(ref sender, ref e);
        }

        private void OnIntegerTextChanged(object sender, TextChangedEventArgs e)
        {
            HandlingUserInput.OnIntegerTextChanged(ref sender, ref e);
        }

        private void FloatingPointPastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            HandlingUserInput.FloatingPointPastingHandler(ref sender, ref e);
        }

        private void OnFloatingPointTextChanged(object sender, TextChangedEventArgs e)
        {
            HandlingUserInput.OnFloatingPointTextChanged(ref sender, ref e);
        }

        #endregion

        private void MotionParameters_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
