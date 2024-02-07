using Grades.DataModel;
using GradesPrototype.Services;
using System.Windows;
using System.Windows.Controls;


namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for LogonPage.xaml
    /// </summary>
    public partial class LogonPage : UserControl
    {
        public LogonPage()
        {
            InitializeComponent();
        }

        #region Event Members
        public event EventHandler LogonSuccess;
        public event EventHandler LogonFailed;
        #endregion

        #region Logon Validation

        // Validate the username and password against the Users collection in the MainWindow window
        private void Logon_Click(object sender, RoutedEventArgs e)
        {
            var user = (from User u in SessionContext.DBContext.Users
                        where u.UserName == username.Text && u.UserPassword == password.Password
                        select u).FirstOrDefault();
            // If the credentials do not match those for a User then they must be invalid
            if (user == null)
            {
                LogonFailed(this, null);
                return;
            }

            var teacher = (from Teacher t in SessionContext.DBContext.Teachers
                           where t.UserId == user.UserId
                           select t).FirstOrDefault();

            if (teacher != null)
            {
                // Save the UserID and Role (teacher or student) and UserName in the global context
                SessionContext.UserID = teacher.UserId;
                SessionContext.UserRole = RoleType.Teacher;
                SessionContext.UserName = user.UserName;
                SessionContext.CurrentTeacher = teacher;

                // Raise the LogonSuccess event and finish
                LogonSuccess(this, null);
                return;
            }
            // If the user is not a teacher, check whether the username and password match those of a student
            else
            {
                var student = (from Student s in SessionContext.DBContext.Students
                               where s.UserId == user.UserId
                               select s).FirstOrDefault();

                if (student != null)
                {
                    // Save the details of the student in the global context
                    SessionContext.UserID = student.UserId;
                    SessionContext.UserRole = RoleType.Student;
                    SessionContext.UserName = user.UserName;
                    SessionContext.CurrentStudent = student;

                    // Raise the LogonSuccess event and finish
                    LogonSuccess(this, null);
                    return;
                }

                // if neither teacher nor student then logon fails
                LogonFailed(this, null);
            }
        }
        #endregion
    }
}
