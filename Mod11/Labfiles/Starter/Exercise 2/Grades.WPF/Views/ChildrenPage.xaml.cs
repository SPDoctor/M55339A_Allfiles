using Grades.WPF.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace Grades.WPF
{
    public partial class ChildrenPage : UserControl
    {
        #region Constructor
        public ChildrenPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Members
        public delegate void ChildSelectionHandler(object sender, ChildEventArgs e);
        public event ChildSelectionHandler ChildSelected;
        #endregion

        #region Refresh
        public void Refresh()
        {
            // Find all children for the current parent
            ServiceUtils utils = new ServiceUtils();

            var children = utils.GetStudentsByParent(SessionContext.UserID);

            // Iterate through the returned set of students, construct a local student object list
            var resultData = new List<ViewModel.Student>();

            foreach (var c in children)
            {
                var student = new ViewModel.Student()
                {
                    Record = c
                };
                resultData.Add(student);
            }

            list.ItemsSource = resultData;
        }
        #endregion

        #region Callbacks

        #endregion

        #region Events

        // If the user clicks a photo, raise the ChildSelected event to display the details of the student
        private void Child_Click(object sender, MouseButtonEventArgs e)
        {
            if (ChildSelected != null)
                ChildSelected(sender, new ChildEventArgs((sender as StudentPhoto).DataContext as ViewModel.Student));
        }

        private void ChildText_Click(object sender, MouseButtonEventArgs e)
        {
            if (ChildSelected != null)
                ChildSelected(sender, new ChildEventArgs((sender as TextBlock).Tag as ViewModel.Student));
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            Refresh();
        }

        #endregion

    }

    public class ChildEventArgs : EventArgs
    {
        public ViewModel.Student Child { get; set; }

        public ChildEventArgs(ViewModel.Student s)
        {
            Child = s;
        }
    }
}
