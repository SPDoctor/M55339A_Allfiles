using Grades.WPF.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grades.WPF
{
    public partial class LogonPage : UserControl
    {
        #region Event Members
        public event EventHandler LogonSuccess;
        public event EventHandler LogonFailed;
        #endregion

        #region Constructor
        public LogonPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            Logon();
        }

        public void Logoff_Click(object sender, RoutedEventArgs e)
        {
            Logoff();
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            if (username.Text != String.Empty && password.Password != String.Empty)
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    e.Handled = true;
                    Logon();
                }
            }
        }
        #endregion

        #region Authentication
        private void Logon()
        {
            if (username.Text == String.Empty || password.Password == String.Empty)
            {
                DisplayError(true);
                return;
            }

            if (SessionContext.Logon(username.Text, password.Password))
            {
                DisplayError(false);
                logonName.Text = SessionContext.UserName;

                if (LogonSuccess != null)
                {
                    LogonSuccess(this, new EventArgs());
                }

                logonForm.Visibility = Visibility.Collapsed;
                logoffForm.Visibility = Visibility.Visible;
            }
            else
            {
                DisplayError(true);
                if (LogonFailed != null)
                {
                    LogonFailed(this, new EventArgs());
                }
            }
        }

        public void Logoff()
        {
            SessionContext.Logoff();

            logoffForm.Visibility = Visibility.Collapsed;
            logonForm.Visibility = Visibility.Visible;
        }
        #endregion

        #region Helper Methods
        private void DisplayError(bool b)
        {
            if (!b)
                txtError.Visibility = Visibility.Collapsed;
            else
            {
                txtError.Text = "Login was unsuccessful.  Please correct the errors and try again.\n The username or password provided is incorrect.";
                txtError.Visibility = Visibility.Visible;
            }
        }
        #endregion   
    }
}
