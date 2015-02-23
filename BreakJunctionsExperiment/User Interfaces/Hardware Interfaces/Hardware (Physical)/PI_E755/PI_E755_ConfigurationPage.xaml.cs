using BreakJunctions.Motion;
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
using System.Windows.Shapes;

namespace BreakJunctions
{
    /// <summary>
    /// Interaction logic for PI_E755_ConfigurationPage.xaml
    /// </summary>
    public partial class PI_E755_ConfigurationPage : Window
    {
        #region MVVM for PI_E755_ConfigurationPage

        private MVVM_PI_E755_ConfigurationPage _DeviceSettings;
        public MVVM_PI_E755_ConfigurationPage DeviceSettings
        {
            get { return _DeviceSettings; }
        }

        #endregion

        public PI_E755_ConfigurationPage()
        {
            InitializeComponent();

            #region Set MVVM

            _DeviceSettings = new MVVM_PI_E755_ConfigurationPage();
            this.DataContext = _DeviceSettings;

            #endregion
        }

        private IMotionFactory _motionFactory;
        private MotionController _motionController;
        public MotionController motionController
        {
            get { return _motionController; }
        }

        private MotionController SetDevice()
        {
            if (!String.IsNullOrEmpty(_DeviceSettings.SelectedPort))
            {
                _motionFactory = new PI_E755_ControllerFactory(_DeviceSettings.SelectedPort);
                var controller = _motionFactory.GetMotionController();

                return controller;
            }
            else return null;
        }

        private void on_cmdSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            _motionController = SetDevice();
            this.Close();
        }
    }
}
