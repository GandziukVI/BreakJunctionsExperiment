using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using BreakJunctions.Motion;

namespace BreakJunctions
{
    /// <summary>
    /// Interaction logic for Faulhaber_2036_U012V.xaml
    /// </summary>
    public partial class Faulhaber_2036_U012V : Window
    {
        #region MVVM for Faulhaber_2036_U012V

        private MVVM_Faulhaber_2036_U012V _DeviceSettings;
        public MVVM_Faulhaber_2036_U012V DeviceSettings
        {
            get { return _DeviceSettings; }
        }

        #endregion

        #region Constructor

        public Faulhaber_2036_U012V()
        {
            InitializeComponent();

            #region Set MVVM

            _DeviceSettings = new MVVM_Faulhaber_2036_U012V();
            this.DataContext = _DeviceSettings;

            #endregion
        }

        #endregion

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
                _motionFactory = new FaulhaberMinimotor_SA_2036U012V_K1155_ControllerFactory(_DeviceSettings.SelectedPort);
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
