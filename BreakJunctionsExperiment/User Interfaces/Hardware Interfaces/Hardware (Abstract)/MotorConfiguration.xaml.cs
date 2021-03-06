﻿using System;
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
using System.Windows.Shapes;

namespace BreakJunctions
{
    public enum AvailableMotionControllers
    {
        Faulhaber_2036_U012V,
        PI_E755
    }

    /// <summary>
    /// Interaction logic for MotorConfiguration.xaml
    /// </summary>
    public partial class MotorConfiguration : Window
    {
        private AvailableMotionControllers _SelectedMotionController;
        public AvailableMotionControllers SelectedMotionController { get { return _SelectedMotionController; } }

        //List of avaliable motor settings should be here
        private Faulhaber_2036_U012V _Faulhaber_2036_U012V_Settings = null;
        public Faulhaber_2036_U012V Faulhaber_2036_U012V_Settings { get { return _Faulhaber_2036_U012V_Settings; } }

        private PI_E755_ConfigurationPage _PI_E755_Settings = null;
        public PI_E755_ConfigurationPage PI_E755_Settings { get { return _PI_E755_Settings; } }

        public MotorConfiguration()
        {
            InitializeComponent();
        }

        public void on_cmdConfigureDeviceClick(object sender, RoutedEventArgs e)
        {
            switch (this.comboBoxSelectMotorDevice.Text)
            {
                case "Faulhaber minimotor SA 2036U012V K1155":
                    {
                        this.Close();
                        _SelectedMotionController = AvailableMotionControllers.Faulhaber_2036_U012V;
                        _Faulhaber_2036_U012V_Settings = new Faulhaber_2036_U012V();
                        _Faulhaber_2036_U012V_Settings.ShowDialog();
                    } break;
                case "PI E755":
                    {
                        this.Close();
                        _SelectedMotionController = AvailableMotionControllers.PI_E755;
                        _PI_E755_Settings = new PI_E755_ConfigurationPage();
                        _PI_E755_Settings.ShowDialog();
                    } break;
                default:
                    break;
            }
        }
    }
}
