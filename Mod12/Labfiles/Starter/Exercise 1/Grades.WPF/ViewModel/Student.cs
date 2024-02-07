namespace Grades.WPF.ViewModel
{
    public class Student
    {
        public DataModel.Student Record { get; set; }

        public Guid ID
        {
            get { return Record.UserId; }
        }

        public string Name
        {
            get { return string.Format("{0} {1}", Record.FirstName, Record.LastName); }
        }

        public string File
        {
            get
            {
                return string.Format(@"{0}images/{1}", @"http://localhost:5104/", Record.ImageName);
            }
        }

        public string FirstName
        {
            get { return Record.FirstName; }
        }

        public string LastName
        {
            get { return Record.LastName; }
        }
    }
}
