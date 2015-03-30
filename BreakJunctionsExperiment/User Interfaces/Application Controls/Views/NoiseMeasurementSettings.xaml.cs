using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for NoiseMeasurementSettings.xaml
    /// </summary>
    public partial class NoiseMeasurementSettings : UserControl
    {
        #region MVVM interactions

        private NoiseMeasurementDataModel _MeasurementSettings;
        public NoiseMeasurementDataModel MeasurementSettings
        {
            get { return _MeasurementSettings; }
        }

        #endregion

        public NoiseMeasurementSettings()
        {
            InitializeComponent();

            #region MVVM setup

            _MeasurementSettings = new NoiseMeasurementDataModel();
            this.DataContext = _MeasurementSettings;

            #endregion
        }
    }
}
