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

namespace BreakJunctions
{
    /// <summary>
    /// Interaction logic for Faulhaber_2036_U012V.xaml
    /// </summary>
    public partial class Faulhaber_2036_U012V : Window
    {
        public Faulhaber_2036_U012V()
        {
            this.DataContext = new MVVM_Faulhaber_2036_U012V();

            InitializeComponent();
        }
    }
}
