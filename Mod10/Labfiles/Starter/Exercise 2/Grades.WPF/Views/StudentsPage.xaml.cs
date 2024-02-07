using Grades.DataModel;
using Grades.WPF.Controls;
using Grades.WPF.Services;
using Grades.WPF.ViewModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grades.WPF
{
    public partial class StudentsPage : UserControl
    {
        #region Constructor
        public StudentsPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Members
        public delegate void StudentSelectionHandler(object sender, StudentEventArgs e);
        public event StudentSelectionHandler StudentSelected;

        // TODO: Exercise 2: Task 3a: Add the StartBusy public event

        // TODO: Exercise 2: Task 3b: Add the EndBusy public event

        #endregion

        #region Refresh
        public async void Refresh()
        {
            // TODO: Exercise 2: Task 3f: Raise the StartBusy event

            // Find all students for the current teacher
            ServiceUtils utils = new ServiceUtils();

            await utils.GetStudentsByTeacher(SessionContext.UserName, OnGetStudentsByTeacherComplete);

            // TODO: Exercise 2: Task 3g: Raise the EndBusy event

        }
        #endregion

        #region Callbacks
        private void OnGetStudentsByTeacherComplete(IEnumerable<DataModel.Student> students)
        {
            // Iterate through the returned set of students, construct a local student object list
            var resultData = new List<ViewModel.Student>();

            foreach (DataModel.Student s in students)
            {
                var student = new ViewModel.Student()
                {
                    Record = s
                };
                resultData.Add(student);
            }

            // Use a Dispatcher object to update the UI
            this.Dispatcher.Invoke(() =>
            {
                list.ItemsSource = resultData;

                txtClass.Text = String.Format("Class {0}", SessionContext.CurrentTeacher.Class);
            });
        }
        #endregion

        #region Events

        // TODO: Exercise 2: Task 3c: Implement the StartBusyEvent method to raise the StartBusy event

        // TODO: Exercise 2: Task 3d: Implement the EndBusyEvent method to raise the EndBusy event

        private void Student_MouseEnter(object sender, MouseEventArgs e)
        {
            // Call the OnMouseEnter event handler on the specific photograph currently under the mouse pointer
            ((StudentPhoto)sender).OnMouseEnter();
        }

        private void Student_MouseLeave(object sender, MouseEventArgs e)
        {
            // Call the OnMouseLeave event handler on the specific photograph currently under the mouse pointer
            ((StudentPhoto)sender).OnMouseLeave();
        }

        //Animate the photo as the user moves the mouse over the "delete" image
        private void RemoveStudent_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Opacity = 1.0;
            StudentPhoto photo = ((Grid)grid.Parent).Children[0] as StudentPhoto;
            photo.Opacity = 0.6;
        }

        // Animate the photo as the user moves the mouse away from the "delete" image
        private void RemoveStudent_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Opacity = 0.3;
            StudentPhoto photo = ((Grid)grid.Parent).Children[0] as StudentPhoto;
            photo.Opacity = 1.0;
        }

        // If the user clicks a photo, raise the StudentSelected event to display the details of the student
        private void Student_Click(object sender, MouseButtonEventArgs e)
        {
            if (StudentSelected != null)
                StudentSelected(sender, new StudentEventArgs((sender as StudentPhoto).DataContext as ViewModel.Student));
        }

        private void StudentText_Click(object sender, MouseButtonEventArgs e)
        {
            if (StudentSelected != null)
                StudentSelected(sender, new StudentEventArgs((sender as TextBlock).Tag as ViewModel.Student));
        }

        private void AddStudent_Click(object sender, MouseButtonEventArgs e)
        {
            var dialog = new AssignStudentDialog();
            dialog.Closed += new EventHandler(dialog_Closed);
            dialog.ShowDialog();
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            Refresh();
        }

        // Remove the student from the class if the user clicks the remove icon
        private void RemoveStudent_Click(object sender, MouseButtonEventArgs e)
        {
            var student = (sender as Grid).Tag as ViewModel.Student;

            MessageBoxResult button = MessageBox.Show("Would you like to remove the student?", "Student", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (button == MessageBoxResult.Yes)
            {
                ServiceUtils utils = new ServiceUtils();
                utils.RemoveStudent(SessionContext.CurrentTeacher, student.Record);
                Refresh();
            }
        }
        #endregion

    }

    public class StudentEventArgs : EventArgs
    {
        public ViewModel.Student Child { get; set; }

        public StudentEventArgs(ViewModel.Student s)
        {
            Child = s;
        }
    }
}
