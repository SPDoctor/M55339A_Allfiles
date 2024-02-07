using System.Windows;

namespace DesignView
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

        private void btnGetCoffee_Click(object sender, RoutedEventArgs e)
        {
            lblResult.Content = "Your coffee is on its way!";
        }
    }
}
