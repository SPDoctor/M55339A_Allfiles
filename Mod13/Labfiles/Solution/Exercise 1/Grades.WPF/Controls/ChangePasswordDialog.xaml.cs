using Grades.DataModel;
using Grades.WPF.Services;
using System.Windows;


namespace Grades.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ChangePasswordDialog.xaml
    /// </summary>
    public partial class ChangePasswordDialog : Window
    {
        public ChangePasswordDialog()
        {
            InitializeComponent();
        }

        // If the user clicks OK to change the password, validate the information that the user has provided
        private void ok_Click(object sender, RoutedEventArgs e)
        {
            // Get the details of the current user
            User currentUser;
            if (SessionContext.UserRole == RoleType.Teacher)
            {
                currentUser = SessionContext.CurrentTeacher.User;
            }
            else
            {
                currentUser = SessionContext.CurrentStudent.User;
            }

            // Check that the old password is correct for the current user
            string oldPwd = oldPassword.Password;
            if (!currentUser.VerifyPassword(oldPwd))
            {
                MessageBox.Show("Old password is incorrect", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Check that the new password and confirm password fields are the same
            string newPwd = newPassword.Password;
            string confirmPwd = confirm.Password;

            if (String.Compare(newPwd, confirmPwd) != 0)
            {
                MessageBox.Show("The new password and confirm password fields are different", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Attempt to change the password
            // If the password is not sufficiently complex, display an error message
            if (!currentUser.SetPassword(SessionContext.UserRole, newPwd))
            {
                MessageBox.Show("The new password is not sufficiently complex", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SessionContext.Save();

            // Indicate that the data is valid
            this.DialogResult = true;
        }
    }
}
