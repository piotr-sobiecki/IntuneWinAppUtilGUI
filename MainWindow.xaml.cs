using IntuneWinAppUtilGUI.Properties;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            dataGridGenerationsHistory.ItemsSource = Settings.Default.GenerationsHistory;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnChooseSetupDirectory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtSetupDirectory.Text = dialog.FolderName;
                txtSetupFile.Text = dialog.FolderName;
            }
        }

        private void btnChooseSetupFile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            if(Directory.Exists(txtSetupFile.Text)) dialog.InitialDirectory = txtSetupFile.Text;
            dialog.Filter = "All files (.*)|*.*";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtSetupFile.Text = dialog.FileName;
            }
        }

        private void btnChooseOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtOutputDirectory.Text = dialog.FolderName;
            }
        }

        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            ShowSettingsWindow();
        }

        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosed(e);
        }

        

        private void ShowSettingsWindow()
        {
            ConfigWindow configWindow = new ConfigWindow();
            configWindow.Owner = this;
            configWindow.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.IntuneWinAppUtilLocation == null || Properties.Settings.Default.IntuneWinAppUtilLocation == "" || !File.Exists(Properties.Settings.Default.IntuneWinAppUtilLocation))
            {
                ShowSettingsWindow();
            }
        }
    }
}
