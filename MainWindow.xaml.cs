using IntuneWinAppUtilGUI.Properties;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        GenerationManager generationManager = new GenerationManager();
        private bool btnClicked = false;
        public MainWindow()
        {
            InitializeComponent();
            RefreshGenerationList();
            txtOutputDirectory.Text = Properties.Settings.Default.DefaultOutputDirectory;
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            var SetupFilesDirectory = txtSetupDirectory.Text;
            var SetupFile = txtSetupFile.Text;
            var OutputDirectory = txtOutputDirectory.Text;
            Generation generation = new Generation(SetupFilesDirectory, SetupFile, OutputDirectory);
            if (string.IsNullOrWhiteSpace(SetupFilesDirectory) || string.IsNullOrWhiteSpace(SetupFile) || string.IsNullOrWhiteSpace(OutputDirectory))
            {
                string messageBoxText = "One or more required paths are not provided.";
                string caption = "Error";
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Directory.Exists(SetupFilesDirectory) || !Directory.Exists(OutputDirectory) || !File.Exists(System.IO.Path.Combine(SetupFilesDirectory,  SetupFile)))
            {
                string messageBoxText = "One or more specified directories or files do not exist.";
                string caption = "Error";
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string intuneWinAppUtilLocation = Properties.Settings.Default.IntuneWinAppUtilLocation;

            if (string.IsNullOrWhiteSpace(intuneWinAppUtilLocation) || !File.Exists(intuneWinAppUtilLocation))
            {
                string messageBoxText = "IntuneWinAppUtil program does not exist at the specified location.";
                string caption = "Error";
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string arguments = $"-o \"{OutputDirectory}\" -c \"{SetupFilesDirectory}\" -s \"{SetupFile}\" -q";
            ProcessStartInfo startInfo = new ProcessStartInfo(intuneWinAppUtilLocation)
            {
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            };

            using (Process process = new Process() { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                string[] lines = output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length > 0)
                {
                    if(lines[lines.Length - 1] == "INFO   Done!!!")
                    {
                        if (!generationManager.GetGenerationList().Any(gen => gen.SetupFilesDirectory == generation.SetupFilesDirectory && gen.SetupFile == generation.SetupFile))
                        {
                            generationManager.AddGeneration(generation);
                        }
                        RefreshGenerationList();

                    }
                }
                string caption = "IntuneWinAppUtil output";
                MessageBoxResult result;
                result = MessageBox.Show(output, caption, MessageBoxButton.OK, MessageBoxImage.Information);


            }
            
        }

        private void btnChooseSetupDirectory_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFolderDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                txtSetupDirectory.Text = dialog.FolderName;
                if(txtSetupFile.Text == "")
                {
                    txtSetupFile.Text = dialog.FolderName;
                }
                if (txtOutputDirectory.Text == "")
                {
                    txtOutputDirectory.Text = Directory.GetParent(txtSetupDirectory.Text)!.FullName;
                }
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

        private void RefreshGenerationList()
        {
            dataGridGenerationsHistory.ItemsSource = generationManager.GetGenerationList();
            dataGridGenerationsHistory.Items.Refresh();
        }


        private void dataGridGenerationsHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (btnClicked)
            {
                if (dataGridGenerationsHistory.SelectedItem != null)
                {
                    Generation selectedItem = (Generation)dataGridGenerationsHistory.SelectedItem;
                    generationManager.RemoveGeneration(selectedItem);
                    dataGridGenerationsHistory.Items.Refresh();
                }
            }
            else
            {
                if (dataGridGenerationsHistory.SelectedItem != null)
                {
                    Generation selectedItem = (Generation)dataGridGenerationsHistory.SelectedItem;
                    txtSetupDirectory.Text = selectedItem.SetupFilesDirectory;
                    txtSetupFile.Text = selectedItem.SetupFile;
                    txtOutputDirectory.Text = selectedItem.OutputDirectory;
                }
                else
                {
                    CleanTextBoxes();
                }
            }
            
        }

        private void CleanTextBoxes()
        {
            txtSetupDirectory.Text = "";
            txtSetupFile.Text = "";
            txtOutputDirectory.Text = "";
        }

        private void btnCleanPaths_Click(object sender, RoutedEventArgs e)
        {
            CleanTextBoxes();
        }

        private void btnRemoveRow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            btnClicked = true;
        }
    }
}
