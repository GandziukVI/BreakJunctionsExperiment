using System;
using System.Collections.Generic;
using System.ComponentModel;
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
	/// Логика взаимодействия для IV_MeasurementSettings.xaml
	/// </summary>
	public partial class IV_MeasurementSettings : UserControl
    {
        #region MVVM interactions

        private IV_MeasurementSettingsDataModel _MeasurementSettings;
        public IV_MeasurementSettingsDataModel MeasurementSettings
        {
            get { return _MeasurementSettings; }
        }

        #endregion

        public IV_MeasurementSettings()
		{
			this.InitializeComponent();

            #region MVVM setup

            _MeasurementSettings = new IV_MeasurementSettingsDataModel();
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
    }
}