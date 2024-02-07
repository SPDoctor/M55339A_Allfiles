using Grades.DataModel;
using Grades.Utilities;
using Grades.WPF.Services;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace Grades.WPF
{
    public partial class StudentProfile : UserControl
    {
        #region Data Members
        private ServiceUtils _context;
        private FilterCriteria _filter;
        #endregion

        #region Event Members
        public event EventHandler Back;
        #endregion

        #region Constructor
        public StudentProfile()
        {
            InitializeComponent();

            _context = new ServiceUtils();
            _filter = new FilterCriteria();
        }
        #endregion

        #region Refresh
        private void InitRefresh()
        {
            btnBack.Visibility = (SessionContext.UserRole == RoleType.Teacher) ? Visibility.Visible : Visibility.Collapsed;
            profileView.Visibility = (SessionContext.UserRole != RoleType.Teacher) ? Visibility.Visible : Visibility.Collapsed;
            separatorVertical.Visibility = (SessionContext.UserRole != RoleType.Teacher) ? Visibility.Visible : Visibility.Collapsed;
            profileEdit.Visibility = (SessionContext.UserRole == RoleType.Teacher) ? Visibility.Visible : Visibility.Collapsed;
            listChild.Visibility = (SessionContext.UserRole == RoleType.Parent) ? Visibility.Visible : Visibility.Collapsed;
            txtAddGrade.Visibility = (SessionContext.UserRole == RoleType.Teacher) ? Visibility.Visible : Visibility.Collapsed;

            cbFilterDate.IsChecked = false;
            cbFilterSubject.IsChecked = false;

            _filter.CurrentSubject = ServiceUtils.Subjects[0];
            _filter.StartDate = DateTime.Now;
            _filter.EndDate = DateTime.Now;
            formSearch.DataContext = _filter;

            txtError.Visibility = Visibility.Collapsed;

            flipControl.ToFront();
        }

        public void Refresh(string studentid)
        {
            InitRefresh();

            // Get the details of the student
            var student = _context.GetStudent(studentid);

            // Update the display with the details and grades of the student
            DisplayStudentDetailsAndGrades(student);

            // If the current user is a parent, fetch the names of any other children and display them
            if (SessionContext.CurrentParent != null)
            {
                var students = _context.GetStudentsByParent(SessionContext.UserID);
                DisplayStudentsForParent(students);
            }
        }

        public void Refresh(string firstname, string lastname)
        {
            InitRefresh();

            var student = _context.GetStudent(firstname, lastname);
            DisplayStudentDetailsAndGrades(student);

            if (SessionContext.CurrentParent != null)
            {
                var students = _context.GetStudentsByParent(SessionContext.UserID);
                DisplayStudentsForParent(students);
            }
        }
        #endregion

        #region Events
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (SessionContext.UserRole != RoleType.Teacher)
                return;

            if (Back != null)
                Back(sender, e);
        }

        private void Child_Click(object sender, MouseButtonEventArgs e)
        {
            var child = (sender as TextBlock).Tag as ViewModel.Student;
            Refresh(child.FirstName, child.LastName);
            SessionContext.CurrentStudent = child.Record;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            var grade = (sender as GrungeButton).Tag as ViewModel.Grade;

            Grid parentGrid = (sender as GrungeButton).Parent as Grid;
            DateTime dt = (parentGrid.Children[0] as DatePicker).SelectedDate.Value;

            ServiceUtils utils = new ServiceUtils();
            utils.UpdateGrade(grade.Record);
        }

        private void Flip_Click(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Grid).Tag.ToString() == "Front")
                flipControl.ToFront();
            else
                flipControl.ToBack();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            bool dateFilter = false;
            bool subjectFilter = false;

            if (cbFilterDate.IsChecked == false && cbFilterSubject.IsChecked == false)
            {
                txtError.Visibility = Visibility.Collapsed;
                ResetGrades();
                flipControl.ToFront();
                return;
            }

            if (cbFilterDate.IsChecked == true)
            {
                if (_filter.StartDate <= _filter.EndDate)
                    dateFilter = true;
                else
                {
                    txtError.Text = "Invalid Date Range.  Start date must be before End date.";
                    txtError.Visibility = Visibility.Visible;
                    return;
                }
            }

            txtError.Visibility = Visibility.Collapsed;

            if (cbFilterSubject.IsChecked == true)
                subjectFilter = true;

            if (dateFilter || subjectFilter)
            {
                if (dateFilter && subjectFilter)
                    FilterGrades(_filter.StartDate, _filter.EndDate, _filter.CurrentSubject.Name);
                else if (dateFilter)
                    FilterGrades(_filter.StartDate, _filter.EndDate, String.Empty);
                else
                    FilterGrades(null, null, _filter.CurrentSubject.Name);

                flipControl.ToFront();
            }
        }

        private void FilterSubject_Checked(object sender, RoutedEventArgs e)
        {
            listSubject.Init();
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 1)
            {
                dpEndDate.DisplayDateStart = null;
                return;
            }

            dpEndDate.DisplayDateStart = (DateTime)e.AddedItems[0];
        }

        private void AddGrade_Click(object sender, MouseButtonEventArgs e)
        {
            AddGradeDialog dialog = new AddGradeDialog();
            dialog.Closed += new EventHandler(dialog_Closed);
            dialog.ShowDialog();
        }

        // Generate the grades report for the currently selected student
        private void SaveReport_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Use a SaveFileDiaolog to prompt the user for a filename to save the report as (must be a Word document)
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Word documents|*.docx";

                // Set the default filename to Grades.txt
                dialog.FileName = "Grades";
                dialog.DefaultExt = ".docx";

                // Display the dialog and get a filename from the user
                Nullable<bool> result = dialog.ShowDialog();

                // If the user selected a file, then generate the report
                if (result.HasValue && result.Value)
                {
                    try
                    {
                        Task.Run(() => GenerateStudentReport(SessionContext.CurrentStudent, dialog.FileName));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error generating report", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Generating Report", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dialog_Closed(object sender, EventArgs e)
        {
            // Find the new set of grades for the currently displayed student (including any new and updated grades resulting from the dialog)
            var grades = _context.GetGradesByStudent(SessionContext.UserID);

            // Display the new set of grades
            DisplayGrades(grades);
        }
        #endregion

        #region Filter
        private void RefreshGradeVisibility()
        {
            var grades = studentDetails.DataContext as List<ViewModel.Grade>;
            if (grades == null || grades.Count == 0)
            {
                txtNoGrades.Visibility = Visibility.Visible;
                gridProfile.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtNoGrades.Visibility = Visibility.Collapsed;
                gridProfile.Visibility = Visibility.Visible;
            }
        }

        private void ResetGrades()
        {
            ListColorConverter.Reset();
            studentDetails.DataContext = null;
            studentDetails.DataContext = SessionContext.CurrentGrades;
            RefreshGradeVisibility();
        }

        private void FilterGrades(DateTime? startDate, DateTime? endDate, string subject)
        {
            if (SessionContext.CurrentGrades == null || SessionContext.CurrentGrades.Count == 0)
                return;

            if (!startDate.HasValue && !endDate.HasValue && subject == String.Empty)
                ResetGrades();
            else
            {
                List<ViewModel.Grade> grades = null;

                if (startDate.HasValue && subject != String.Empty)
                {
                    // Both filters
                    grades = SessionContext.CurrentGrades.Where(g => g.AssessmentDate >= startDate && g.AssessmentDate < endDate && g.SubjectName == subject).ToList();
                }
                else if (startDate.HasValue && subject == String.Empty)
                {
                    // Date only
                    grades = SessionContext.CurrentGrades.Where(g => g.AssessmentDate >= startDate && g.AssessmentDate < endDate).ToList();
                }
                else if (!startDate.HasValue && subject != String.Empty)
                {
                    // subject only
                    grades = SessionContext.CurrentGrades.Where(g => g.SubjectName == subject).ToList();
                }

                if (grades != null)
                {
                    ListColorConverter.Reset();
                    studentDetails.DataContext = grades;
                    RefreshGradeVisibility();
                }
            }
        }
        #endregion

        #region Utility and Helper Methods

        // Update the display with the details of the specified student and retrieve the grades for this student
        private void DisplayStudentDetailsAndGrades(DataModel.Student student)
        {
            try
            {
                var child = new ViewModel.Student()
                {
                    Record = student
                };

                // Find the grades for the student and display them
                var grades = _context.GetGradesByStudent(student.UserId);
                DisplayGrades(grades);
            }
            catch (Exception e)
            {
                MessageBox.Show(String.Format("Error: {0} - {1}", e.Message));
            }
        }

        // Display a list of grades
        private void DisplayGrades(IEnumerable<DataModel.Grade> gradeResults)
        {
            // Convert the list of grades from the format provided by the dataservice to the format displayed by the application
            var grades = new List<ViewModel.Grade>();

            foreach (var g in gradeResults)
            {
                var vmGrade = new ViewModel.Grade() { Record = g };
                grades.Add(vmGrade);
            }

            // Display the grades
            SessionContext.CurrentGrades = grades;
            ResetGrades();
        }

        // Display the names of the students for a parent
        private void DisplayStudentsForParent(IEnumerable<DataModel.Student> students)
        {
            // Convert the list of students from the format provided by the dataservice to the format displayed by the application
            var studentList = new List<ViewModel.Student>();
            foreach (DataModel.Student s in students)
            {
                var child = new ViewModel.Student() { Record = s };
                studentList.Add(child);
            }

            // USe databinding to display the student names
            listChild.ItemsSource = studentList;
        }

        // Format a list of grades as an XML document and write it to a MemoryStream
        private MemoryStream FormatAsXMLStream(List<ViewModel.Grade> grades)
        {
            // Save the XML document to a MemoryStream by using an XmlWriter
            MemoryStream ms = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(ms);

            // The document root has the format <Grades Student="Eric Gruber">
            writer.WriteStartDocument();
            writer.WriteStartElement("Grades");
            writer.WriteAttributeString("Student", String.Format("{0} {1}", SessionContext.CurrentStudent.FirstName, SessionContext.CurrentStudent.LastName));

            // Format the grades for the student and add them as child elements of the root node
            // Grade elements have the format <Grade Date="01/01/2012" Subject="Math" Assessment="A-" Comments="Good" />
            foreach (ViewModel.Grade grade in grades)
            {
                writer.WriteStartElement("Grade");
                writer.WriteAttributeString("Date", grade.AssessmentDate.ToString("d"));
                writer.WriteAttributeString("Subject", grade.SubjectName);
                writer.WriteAttributeString("Assessment", grade.Assessment);
                writer.WriteAttributeString("Comments", grade.Comments);
                writer.WriteEndElement();
            }

            // Finish the XML document with the appropriate end elements
            writer.WriteEndElement();
            writer.WriteEndDocument();

            // Flush the XmlWriter and close it to ensure that all the data is written to the MemoryStream
            writer.Flush();
            writer.Close();

            // The MemoryStream now contains the formatted data
            // Reset the MemoryStream so it can be read from the start and then return it
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        // Format the XML data in the stream as a neatly constructed string
        private string FormatXMLData(Stream stream)
        {
            // Use a StringBuilder to construct the string
            StringBuilder builder = new StringBuilder();

            // Use an XmlTextReader to read the XML data from the stream
            XmlTextReader reader = new XmlTextReader(stream);

            // Read and process the XML data a node at a time
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.XmlDeclaration:
                        // The node is an XML declaration such as <?xml version='1.0'>
                        builder.Append(String.Format("<?{0} {1}>\n", reader.Name, reader.Value));
                        break;

                    case XmlNodeType.Element:
                        // The node is an element (enclosed between '<' and '/>')
                        builder.Append(String.Format("<{0}", reader.Name));
                        if (reader.HasAttributes)
                        {
                            // Output each of the attributes of the element in the form "name='value'"
                            while (reader.MoveToNextAttribute())
                            {
                                builder.Append(String.Format(" {0}='{1}'", reader.Name, reader.Value));
                            }
                        }
                        builder.Append(">\n");
                        break;

                    case XmlNodeType.EndElement:
                        // The node is the closing tag at the end of an element
                        builder.Append(String.Format("</{0}>", reader.Name));
                        break;
                }
            }

            // Reset the stream
            stream.Seek(0, SeekOrigin.Begin);

            // Return the string containing the formatted data
            return builder.ToString();
        }

        // Generate a student grade report as a Word document.
        public void GenerateStudentReport(DataModel.Student studentData, string reportPath)
        {
            WordWrapper wrapper = new WordWrapper();

            // Create a new Word document in memory
            wrapper.CreateBlankDocument();

            // Add a heading to the document
            wrapper.AppendHeading(String.Format("Grade Report: {0} {1}", studentData.FirstName, studentData.LastName));
            wrapper.InsertCarriageReturn();
            wrapper.InsertCarriageReturn();

            // Output the details of each grade for the student
            foreach (var grade in SessionContext.CurrentGrades)
            {
                wrapper.AppendText(grade.SubjectName, true, true);
                wrapper.InsertCarriageReturn();
                wrapper.AppendText("Assessment: " + grade.Assessment, false, false);
                wrapper.InsertCarriageReturn();
                wrapper.AppendText("Date: " + grade.AssessmentDateString, false, false);
                wrapper.InsertCarriageReturn();
                wrapper.AppendText("Comment: " + grade.Comments, false, false);
                wrapper.InsertCarriageReturn();
                wrapper.InsertCarriageReturn();
            }

            // Save the Word document
            wrapper.SaveAs(reportPath);
        }

        #endregion
    }
}
