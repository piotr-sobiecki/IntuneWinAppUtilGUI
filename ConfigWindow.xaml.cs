using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace IntuneWinAppUtilGUI
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            txtIntuneWinAppUtilLocation.Text = Properties.Settings.Default.IntuneWinAppUtilLocation;
            txtDefaultOutputDirectory.Text = Properties.Settings.Default.DefaultOutputDirectory;
        }

        private void btnChooseIntuneWinAppUtilLocation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Executable files (.exe)|*.exe";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtIntuneWinAppUtilLocation.Text = dialog.FileName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var contentPrepPath = txtIntuneWinAppUtilLocation.Text;
            if (contentPrepPath == "")
            {
                string messageBoxText = "Win32 Content Prep tool executable is not set.";
                string caption = "Path not set";
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (File.Exists(contentPrepPath))
                {
                    SaveSettings();
                }
                else
                {
                    string messageBoxText = "Specified file not exist.";
                    string caption = "File not exist";
                    MessageBoxResult result;
                    result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void SaveSettings()
        {
            Properties.Settings.Default.IntuneWinAppUtilLocation = txtIntuneWinAppUtilLocation.Text;
            Properties.Settings.Default.DefaultOutputDirectory = txtDefaultOutputDirectory.Text;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
            if (Properties.Settings.Default.IntuneWinAppUtilLocation == null || Properties.Settings.Default.IntuneWinAppUtilLocation == "" || !(File.Exists(Properties.Settings.Default.IntuneWinAppUtilLocation)))
            {
                e.Cancel = true;
                string messageBoxText = "Win32 Content Prep tool executable is not set. If you not set executable path, application will close. Do you want close application?";
                string caption = "Path not set";
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    Owner.Close(); 
                }

            }
            
        }

        private void btnChooseDefaultOutputDirectoryLocation_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtDefaultOutputDirectory.Text = dialog.FolderName;
            }
        }
    }
}
