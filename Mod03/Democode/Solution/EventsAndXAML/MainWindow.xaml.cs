using System.Windows;

namespace EventsAndXAML
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

        private void btnGetTime_Click(object sender, RoutedEventArgs e)
        {
            lblShowTime.Content = DateTime.Now.ToLongTimeString();
        }
    }
}
