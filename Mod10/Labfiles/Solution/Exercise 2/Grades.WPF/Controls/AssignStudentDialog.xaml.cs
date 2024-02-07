using Grades.DataModel;
using Grades.WPF.Services;
using Grades.WPF.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grades.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AssignStudentDialog.xaml
    /// </summary>
    public partial class AssignStudentDialog : Window
    {
        public AssignStudentDialog()
        {
            InitializeComponent();
        }

        // Refresh the display of unassigned students
        private void Refresh()
        {
            // Find all unassigned students - they have a TeacherID of zero
            SessionContext.DBContext.Students.Load<DataModel.Student>();
            var unassignedStudents = from s in SessionContext.DBContext.Students.Local
                                     where s.TeacherUserId == null
                                     select s;

            // If there are no unassigned students, then display the "No unassigned students" message
            // and hide the list of unassigned students
            if (unassignedStudents.Count() == 0)
            {
                txtMessage.Visibility = Visibility.Visible;
                list.Visibility = Visibility.Collapsed;
            }
            else
            {
                // If there are unassigned students, hide the "No unassigned students" message 
                // and display the list of unassigned students
                txtMessage.Visibility = Visibility.Collapsed;
                list.Visibility = Visibility.Visible;

                // Bind the ItemsControl on the dialog to a local list of unassigned students
                // The names of the students will appear in the ItemsControl on the dialog
                var resultData = new List<ViewModel.Student>();

                foreach (var s in unassignedStudents)
                {
                    var student = new ViewModel.Student()
                    {
                        Record = s
                    };
                    resultData.Add(student);
                }

                list.ItemsSource = resultData;
            }
        }

        private void AssignStudentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        // Enroll a student in the teacher's class
        private void Student_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Determine which student the user clicked using the StudentID held in the Tag property of the Button that the user clicked
                var studentClicked = sender as StudentPhoto;
                Guid studentID = (Guid)studentClicked.Tag;

                // Find this student in the Students collection
                var student = (from s in SessionContext.DBContext.Students
                                   where s.UserId == studentID
                                   select s).First();

                // Prompt the user to confirm that they wish to add this student to their class
                string message = String.Format("Add {0} {1} to your class?", student.FirstName, student.LastName);
                MessageBoxResult reply = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // If the user confirms, add the student to their class
                if (reply == MessageBoxResult.Yes)
                {
                    // Get the ID of the currently logged-on teacher
                    Guid teacherID = SessionContext.CurrentTeacher.UserId;

                    // Call the EnrollInClass method to assign the student to this teacher's class.
                    SessionContext.CurrentTeacher.EnrollInClass(student);
                    SessionContext.Save();

                    // Refresh the display - the newly assigned student should disappear from the list of unassigned students
                    Refresh();
                }
            }
            catch (ClassFullException cfe)
            {
                MessageBox.Show(String.Format("{0}. Class: {1}", cfe.Message, cfe.ClassName), "Error enrolling student", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error enrolling student", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog box
            this.Close();
        }
    }
}
