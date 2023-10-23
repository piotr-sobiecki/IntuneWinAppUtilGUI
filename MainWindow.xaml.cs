﻿using Microsoft.Win32;
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

namespace IntuneWinAppUtilGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChooseSetupDirectory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChooseSetupFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "All files (.*)|*.*";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtSetupFile.Text = dialog.FileName;
            }
        }

        private void btnChooseOutputDirectory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
