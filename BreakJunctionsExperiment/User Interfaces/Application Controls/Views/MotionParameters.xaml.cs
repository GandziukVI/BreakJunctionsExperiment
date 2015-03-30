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
    /// Interaction logic for MotionParameters.xaml
    /// </summary>
    public partial class MotionParameters : UserControl
    {
        #region MVVM interactions

        private MotionParametersDataModel _MeasurementSettings;
        public MotionParametersDataModel MeasurementSettings
        {
            get { return _MeasurementSettings; }
        }

        #endregion

        public MotionParameters()
        {
            InitializeComponent();

            #region MVVM setup

            _MeasurementSettings = new MotionParametersDataModel();
            this.DataContext = _MeasurementSettings;

            #endregion
        }
    }
}
