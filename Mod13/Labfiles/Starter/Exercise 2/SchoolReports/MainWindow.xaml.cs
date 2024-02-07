using System.IO;
using System.Windows;

namespace School
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Predefined code

        string ReportPath;
        string CombinedReportPath;

        public MainWindow()
        {
            InitializeComponent();
            ReportPath = @"E:\Mod13\Labfiles\Reports\";
            CombinedReportPath = @"ClassReport\ClassReport.xps";
        }

        private void browse_Click(object sender, RoutedEventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.SelectedPath = ReportPath;

                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ReportPath = folderBrowserDialog.SelectedPath.ToString();
                    folderPathTextBox.Text = ReportPath;
                    decrypt.IsEnabled = true;
                }
            }
        }

        #endregion

        private void print_Click(object sender, RoutedEventArgs e)
        {
            // Call the CombineEncryptedReports method.
            using (WordWrapper wordWrapper = new WordWrapper())
            {
                wordWrapper.CombineEncryptedReports(ReportPath, Path.Combine(ReportPath, CombinedReportPath));
                System.Windows.MessageBox.Show("Class report successfully printed.", "The School of Fine Arts", MessageBoxButton.OK);
            }
        }
    }
}
