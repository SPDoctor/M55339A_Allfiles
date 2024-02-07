using GradesPrototype.Views;
using System.Windows;

namespace GradesPrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GotoLogon();
        }

        #region Navigation
        // TODO: Exercise 1: Task 3a: Display the logon view and and hide the list of students and single student view
        public void GotoLogon()
        {

        }

        // TODO: Exercise 1: Task 4c: Display the list of students
        private void GotoStudentsPage()
        {

        }

        // TODO: Exercise 1: Task 4b: Display the details for a single student
        public void GotoStudentProfile()
        {

        }
        #endregion

        #region Event Handlers

        // TODO: Exercise 1: Task 3b: Handle successful logon
        // Update the display and show the data for the logged on user

        // Handle logoff
        private void Logoff_Click(object sender, RoutedEventArgs e)
        {
            // Hide all views apart from the logon page
            gridLoggedIn.Visibility = Visibility.Collapsed;
            studentsPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Collapsed;
            logonPage.Visibility = Visibility.Visible;
        }

        // Handle the Back button on the Student page
        private void studentPage_Back(object sender, EventArgs e)
        {
            GotoStudentsPage();
        }

        // TODO: Exercise 1: Task 5b: Handle the StudentSelected event when the user clicks a student on the Students page
        // Set the global context to the name of the student and call the GotoStudentProfile method to display the details of the student
        private void studentsPage_StudentSelected(object sender, StudentEventArgs e)
        {

        }
        #endregion

        #region Display Logic

        // TODO: Exercise 1: Task 4a: Update the display for the logged on user (student or teacher)
        private void Refresh()
        {

        }
        #endregion
    }
}
