using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;

namespace FourthCoffee.MessageSafe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        const string _messageFileName = "protected_message.txt";

        Encryptor _encryptor;
        FileInfo _protectedFile;
        bool _isMessageLoaded;

        public MainWindow()
        {
            InitializeComponent();

            _encryptor = new Encryptor("74519A8D-1519-4E14-A66A-9F9F9BE95860");
            _protectedFile = new FileInfo(GetPathToProtectedMessageFile());

            _isMessageLoaded = !_protectedFile.Exists;
            ResetUI();

            status.Content = "Ready....";
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveMessage();

            status.Content = "Message saved..";
        }

        private void load_Click(object sender, RoutedEventArgs e)
        {
            bool loadSuccessful = false;

            try
            {
                if (string.IsNullOrEmpty(password.Password))
                {
                    status.Content = "Load unsuccessful. You must provide a password...";
                }
                else
                {
                    LoadMessage();
                    loadSuccessful = true;
                }
            }
            catch
            {
                status.Content = "Load unsuccessful. Please ensure you provide the correct password...";
            }

            if (loadSuccessful)
            {
                _isMessageLoaded = true;
                ResetUI();
                status.Content = "Message loaded...";
            }
        }

        private static string GetPathToProtectedMessageFile()
        {
            var dataDirectory = ConfigurationManager.AppSettings["EncryptedFilePath"];

            var pathToProtectedFile = System.IO.Path.Combine(dataDirectory, _messageFileName);

            return pathToProtectedFile;
        }

        private void LoadMessage()
        {
            if (!_protectedFile.Exists)
                return;

            var encryptedBytes = File.ReadAllBytes(_protectedFile.FullName);

            var decryptedBytes = _encryptor.Decrypt(encryptedBytes, password.Password);

            var decryptedText = Encoding.Default.GetString(decryptedBytes);

            messageText.Text = decryptedText;
        }

        private void SaveMessage()
        {
            var decryptedBytes = Encoding.Default.GetBytes(messageText.Text);

            var encryptedBytes = _encryptor.Encrypt(decryptedBytes, password.Password);

            File.WriteAllBytes(_protectedFile.FullName, encryptedBytes);
        }

        private void ResetUI()
        {
            if (_isMessageLoaded)
            {
                save.IsEnabled = true;
                messageText.IsEnabled = true;
                load.IsEnabled = false;

            }
            else
            {
                save.IsEnabled = false;
                messageText.IsEnabled = false;
                load.IsEnabled = true;
            }
        }

        #region IDisposable Members

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _protectedFile = null;

                if (_encryptor != null)
                {
                    _encryptor.Dispose();
                }
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
