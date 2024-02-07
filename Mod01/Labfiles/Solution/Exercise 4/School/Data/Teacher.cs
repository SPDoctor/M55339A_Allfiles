using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace School
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Class { get; set; }
        public virtual ICollection<Student> Students
          { get; set; } = new ObservableCollection<Student>();
    }
}
