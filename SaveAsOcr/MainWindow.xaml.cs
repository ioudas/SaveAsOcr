using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Microsoft.Win32;
using SaveAsOcr.Properties;
using Application = System.Windows.Application;

namespace SaveAsOcr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public IMainController controller { get; set; }
            
        public MainWindow()
        {
            InitializeComponent();
            txtInputDir.Text = Settings.Default.InputFileDir;
            txtOutputDir.Text = Settings.Default.OutputFileDir;
            txtMatchRegex.Text = Settings.Default.MatchRegex;
            txtReplaceRegex.Text = Settings.Default.ReplaceRegex;

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            txtLogOutput.Text = "";
            SaveUserSettings();
            string intputDir = txtInputDir.Text;
            string outputDir = txtOutputDir.Text;
            string matchRegex = txtMatchRegex.Text;
            string meplaceRegex = txtReplaceRegex.Text;
            Task<SaveResult> task = Task<SaveResult>.Factory.StartNew(
                () =>
                    controller.OnStartClicked(intputDir, outputDir, matchRegex, meplaceRegex));
            task.ContinueWith(x => Application.Current.Dispatcher.Invoke(() => { DisplayResult(x.Result); }));



        }

        private void SaveUserSettings()
        {
            Settings.Default.InputFileDir = txtInputDir.Text;
            Settings.Default.OutputFileDir = txtOutputDir.Text;
            Settings.Default.MatchRegex = txtMatchRegex.Text;
            Settings.Default.ReplaceRegex= txtReplaceRegex.Text;
            Settings.Default.Save();
        }

        private void DisplayResult(SaveResult result)
        {
            if (result.Status == SaveResultStatus.FAILURE)
            {
                txtLogOutput.Text = "";
                txtLogOutput.Foreground = new SolidColorBrush(Colors.OrangeRed);
                txtLogOutput.Text = "Saving failed! Error: " + result.message;
            }
            else
            {
                txtLogOutput.Text = "";
                txtLogOutput.Foreground = new SolidColorBrush(Colors.Black);
                txtLogOutput.Text = "Saving completed. Message: " + result.message;
            }
        }

        private void btnBrowseOutputDir_Click(object sender, RoutedEventArgs e)
        {
            txtOutputDir.Text = OpenFolderPicker();
        }

        private void btwBrowseInputDir_Click(object sender, RoutedEventArgs e)
        {
            txtInputDir.Text = OpenFolderPicker();
        }

        private string OpenFolderPicker()
        {
            var folderDialog = new FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    return folderDialog.SelectedPath;
                default:
                    return String.Empty;
            }
        }
    }
}
