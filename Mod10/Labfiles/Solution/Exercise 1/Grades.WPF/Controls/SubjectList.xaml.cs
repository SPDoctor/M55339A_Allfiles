using Grades.WPF.Services;
using System.Windows.Controls;

namespace Grades.WPF
{
    public partial class SubjectList : UserControl
    {
        #region Constructor
        public SubjectList()
        {
            InitializeComponent();

            Init();
        }
        #endregion

        #region Public Methods
        public void Init()
        {
            if (list.Items.Count == 0 && ServiceUtils.Subjects != null)
                list.ItemsSource = ServiceUtils.Subjects;

            if (ServiceUtils.Subjects != null && ServiceUtils.Subjects.Count > 0)
                list.SelectedIndex = 0;
        }
        #endregion
    }
}