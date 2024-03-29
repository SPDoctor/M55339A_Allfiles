﻿using GradesPrototype.Data;
using GradesPrototype.Services;
using System.Windows;

namespace GradesPrototype.Controls
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
            if (SessionContext.UserRole == Role.Teacher)
            {
                currentUser = SessionContext.CurrentTeacher;
            }
            else
            {
                currentUser = SessionContext.CurrentStudent;
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

            // If the password is not sufficiently complex, display an error message
            if (!currentUser.SetPassword(newPwd))
            {
                MessageBox.Show("The new password is not sufficiently complex", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Indicate that the data is valid
            this.DialogResult = true;
        }
    }
}
