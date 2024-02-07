using Grades.DataModel;
using GradesPrototype.Controls;
using GradesPrototype.Services;
using System.Windows;
using System.Windows.Controls;

namespace GradesPrototype.Views
{
    /// <summary>
    /// Interaction logic for StudentsPage.xaml
    /// </summary>
    public partial class StudentsPage : UserControl
    {
        public StudentsPage()
        {
            InitializeComponent();
        }

        #region Display Logic
        public void Refresh()
        {
            list.Items.Clear();

            // Find students for the current teacher
            foreach (Student student in SessionContext.DBContext.Students)
            {
                if (student.TeacherUserId == SessionContext.CurrentTeacher.UserId)
                {
                    list.Items.Add(student);
                }
            }

            // Display the class name
            txtClass.Text = String.Format("Class {0}", SessionContext.CurrentTeacher.Class);

        }
        #endregion

        #region Event Members
        public delegate void StudentSelectionHandler(object sender, StudentEventArgs e);
        public event StudentSelectionHandler StudentSelected;
        #endregion

        #region Event Handlers
        private void Student_Click(object sender, RoutedEventArgs e)
        {
            Button itemClicked = sender as Button;
            if (itemClicked != null)
            {
                // Find out which student was clicked
                Guid studentID = (Guid)itemClicked.Tag;
                if (StudentSelected != null)
                {
                    // Find the details of the student by examining the DataContext of the Button
                    Student student = (Student)itemClicked.DataContext;

                    // Raise the StudentSelected event (handled by MainWindow) to display the details for this student
                    StudentSelected(sender, new StudentEventArgs(student));
                }
            }
        }

        // Create a new student with input from the user
        private void NewStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Use the StudentDialog to get the details of the student from the user
                StudentDialog sd = new StudentDialog();

                // Display the form and get the details of the new student
                if (sd.ShowDialog().Value)
                {
                    // When the user closes the form, retrieve the details of the student from the form
                    // and use them to create a new Student object
                    Student newStudent = new Student();
                    newStudent.FirstName = sd.firstName.Text;
                    newStudent.LastName = sd.lastName.Text;
                    newStudent.User = new User();
                    if (!newStudent.User.SetPassword(RoleType.Student, sd.password.Text))
                    {
                        throw new Exception("Password must be at least 6 characters long. Student not created");
                    }

                    // Generate the UserName property - lastname with the initial letter of the first name all converted to lowercase
                    newStudent.User.UserName = (newStudent.LastName + newStudent.FirstName.Substring(0, 1)).ToLower();

                    // Generate a unique ID for the user
                    newStudent.UserId = Guid.NewGuid();

                    // Assign a value for the ImageName field
                    newStudent.ImageName = "No photo";

                    // Generate default values for remaining properties of user object
                    newStudent.User.ApplicationId = (from User u in SessionContext.DBContext.Users select u.ApplicationId).FirstOrDefault();
                    newStudent.User.IsAnonymous = false;
                    newStudent.User.LastActivityDate = DateTime.Now;
                    newStudent.User.UserId = newStudent.UserId;

                    // Add the student to the Students collection
                    SessionContext.DBContext.Students.Add(newStudent);
                    SessionContext.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error creating new student", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Enroll a student in the teacher's class
        private void EnrollStudent_Click(object sender, RoutedEventArgs e)
        {
            // Use the AssignStudentDialog to display unassigned students and add them to the teacher's class
            // All of the work is performed in the code behind the dialog
            AssignStudentDialog asd = new AssignStudentDialog();
            asd.ShowDialog();

            // Refresh the display to show any newly enrolled students
            Refresh();
        }
        #endregion
    }

    // EventArgs class for passing Student information to an event
    public class StudentEventArgs : EventArgs
    {
        public Student Child { get; set; }

        public StudentEventArgs(Student s)
        {
            Child = s;
        }
    }
}
