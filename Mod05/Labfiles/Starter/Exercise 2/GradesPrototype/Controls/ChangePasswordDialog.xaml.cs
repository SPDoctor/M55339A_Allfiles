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
            // TODO: Exercise 2: Task 4a: Get the details of the current user

            // TODO: Exercise 2: Task 4b: Check that the old password is correct for the current user

            // TODO: Exercise 2: Task 4c: Check that the new password and confirm password fields are the same

            // TODO: Exercise 2: Task 4d: Attempt to change the password
            // If the password is not sufficiently complex, display an error message

            // Indicate that the data is valid
            this.DialogResult = true;
        }
    }
}
