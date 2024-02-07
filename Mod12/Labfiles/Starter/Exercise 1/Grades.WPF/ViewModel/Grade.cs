using Grades.DataModel;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Grades.WPF.ViewModel
{
    // TODO: Exercise 1: Task 2: Add the IncludeInReport attribute to the approriate properties in the Grade class
    public class Grade : IDataErrorInfo
    {
        #region Properties
        public DataModel.Grade Record { get; set; }

        #region Readonly Properties

        public string SubjectName
        {
            get { return Record.Subject != null ? Record.Subject.Name : ""; }
        }

        public string AssessmentDateString
        {
            get { return Record.AssessmentDate.ToShortDateString(); }
        }
        #endregion

        #region Form Properties
        public DateTime AssessmentDate
        {
            get { return Record.AssessmentDate; }
            set { Record.AssessmentDate = value; }
        }

        public Subject CurrentSubject
        {
            get { return Record.Subject; }
            set
            {
                Record.Subject = value;
                Record.SubjectId = value.Id;
            }
        }

        public string Assessment
        {
            get { return Record.Assessment; }
            set { Record.Assessment = value; }
        }

        public string Comments
        {
            get { return Record.Comments; }
            set { Record.Comments = value; }
        }
        #endregion
        #endregion

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;

                if (columnName == "AssessmentDate")
                {
                    if (AssessmentDate.Year < 2000)
                        result = "Invalid Date";
                }
                else if (columnName == "Assessment")
                {
                    // Use a regular expression to check that the supplied
                    // assessment grade is in the range A-E. Also allow a - or a +
                    if (Assessment != null)
                    {
                        Match regMatch = Regex.Match(Assessment, @"^[A-E][+-]?$");
                        if (!regMatch.Success)
                        {
                            result = "An assessment must be A - E, with optional + or -";
                        }
                    }
                }

                return result;
            }
        }
    }
}
