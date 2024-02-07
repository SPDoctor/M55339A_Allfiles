using Grades.DataModel;
using Grades.WPF.Services;
using System.Windows;

namespace Grades.WPF
{
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            ServiceUtils utils = new ServiceUtils();
            InitializeComponent();

            GotoLogon();
        }
        #endregion

        private void StartBusy(object sender, EventArgs e)
        {
            busyIndicator.Visibility = Visibility.Visible;
        }

        private void EndBusy(object sender, EventArgs e)
        {
            busyIndicator.Visibility = Visibility.Hidden;
        }

        #region Events
        private void logonPage_LogonSuccess(object sender, EventArgs e)
        {
            Refresh();
        }

        private void studentsPage_StudentSelected(object sender, StudentEventArgs e)
        {
            SessionContext.CurrentStudent = e.Child.Record;
            GotoStudentProfile();
        }

        private void childrenPage_ChildSelected(object sender, ChildEventArgs e)
        {
            SessionContext.CurrentStudent = e.Child.Record;
            GotoStudentProfile();
        }

        private void studentProfile_Back(object sender, EventArgs e)
        {
            GotoStudentsPage();
        }

        private void Logoff_Click(object sender, RoutedEventArgs e)
        {
            logonPage.Logoff();
            GotoLogon();
        }
        #endregion

        #region Navigate
        public void GotoLogon()
        {
            gridLoggedIn.Visibility = Visibility.Collapsed;
            logonPage.Visibility = Visibility.Visible;
            studentsPage.Visibility = Visibility.Collapsed;
            childrenPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Collapsed;

            logoLogon.Visibility = Visibility.Visible;
            logoMain.Visibility = Visibility.Collapsed;
        }

        public void GotoStudentsPage()
        {
            LoadSecondaryPage();

            studentsPage.Visibility = Visibility.Visible;
            childrenPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Collapsed;

            studentsPage.Refresh();
        }

        public void GotoChildrenPage()
        {
            LoadSecondaryPage();

            studentsPage.Visibility = Visibility.Collapsed;
            childrenPage.Visibility = Visibility.Visible;
            studentProfile.Visibility = Visibility.Collapsed;

            childrenPage.Refresh();
        }

        public void GotoStudentProfile()
        {
            LoadSecondaryPage();

            studentsPage.Visibility = Visibility.Collapsed;
            childrenPage.Visibility = Visibility.Collapsed;
            studentProfile.Visibility = Visibility.Visible;

            if (SessionContext.UserRole == RoleType.Student)
                studentProfile.Refresh(SessionContext.UserName);
            else
                studentProfile.Refresh(SessionContext.CurrentStudent.FirstName, SessionContext.CurrentStudent.LastName);
        }

        public void LoadSecondaryPage()
        {
            gridLoggedIn.Visibility = Visibility.Visible;
            logonPage.Visibility = Visibility.Collapsed;

            logoLogon.Visibility = Visibility.Collapsed;
            logoMain.Visibility = Visibility.Visible;
        }
        #endregion

        #region Refresh

        public void Refresh()
        {
            if (SessionContext.UserRole == RoleType.None)
            {
                GotoLogon();
                return;
            }

            // Databind Name
            ServiceUtils utils = new ServiceUtils();

            try
            {
                switch (SessionContext.UserRole)
                {
                    case RoleType.Student:
                        // Get the details of the current user (which must be a student)
                        txtName.Text = string.Format("Welcome {0} {1}", SessionContext.CurrentStudent.FirstName, SessionContext.CurrentStudent.LastName);

                        // Display the details of the student
                        GotoStudentProfile();
                        break;

                    case RoleType.Parent:
                        // Get the details of the current user (which must be a parent)
                        var parent = SessionContext.CurrentParent;
                        txtName.Text = String.Format("Welcome {0} {1}!", SessionContext.CurrentParent.FirstName, SessionContext.CurrentParent.LastName);

                        // Display the details of the student
                        GotoChildrenPage();
                        break;

                    case RoleType.Teacher:
                        // Display the details for the teacher
                        txtName.Text = String.Format("Welcome {0} {1}!", SessionContext.CurrentTeacher.FirstName, SessionContext.CurrentTeacher.LastName);

                        // Display the students in the class taught by this teacher
                        GotoStudentsPage();
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error fetching details", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
    }
}