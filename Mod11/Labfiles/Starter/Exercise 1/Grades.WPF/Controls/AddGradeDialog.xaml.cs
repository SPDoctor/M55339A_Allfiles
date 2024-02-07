using Grades.WPF.Services;
using System.Windows;

namespace Grades.WPF
{
    public partial class AddGradeDialog : Window
    {
        #region Data Members
        private ViewModel.Grade _localgrade;
        #endregion

        #region Constructor
        public AddGradeDialog()
        {
            InitializeComponent();

            _localgrade = new ViewModel.Grade();
            _localgrade.Record = new DataModel.Grade();
            _localgrade.CurrentSubject = ServiceUtils.Subjects[0];
            _localgrade.AssessmentDate = DateTime.Now;
            _localgrade.Assessment = "A";
            this.DataContext = _localgrade;
        }
        #endregion

        #region Events
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceUtils utils = new ServiceUtils();
            utils.AddGrade(_localgrade.Record);
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        #endregion
    }
}

