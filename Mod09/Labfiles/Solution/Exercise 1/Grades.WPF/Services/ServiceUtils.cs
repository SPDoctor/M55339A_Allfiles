using Grades.DataModel;

namespace Grades.WPF.Services
{
    class ServiceUtils
    {
        #region Data Members
        public static SchoolGradesDbContext DBContext;
        //public static GradesEntities DBContext;
        public static List<Subject> Subjects { get; set; }
        #endregion

        #region Constructor
        public ServiceUtils()
        {
            if (DBContext == null)
            {
                DBContext = new SchoolGradesDbContext();

                if (Subjects == null)
                {
                    GetSubjects();
                }
            }
        }
        #endregion

        #region Teacher
        public Teacher GetTeacher(string userName)
        {
            if (!IsConnected())
                return null;

            var teacher = (from t in DBContext.Teachers
                           where t.User.UserName == userName
                           select t).FirstOrDefault();

            return teacher;
        }

        public List<Student> GetStudentsByTeacher(string teacherName)
        {
            if (!IsConnected())
                return null;

            // Fetch students by using the GradesService service
            var students = (from s in DBContext.Students
                            where s.TeacherUser.User.UserName == teacherName
                            select s).OrderBy(s => s.LastName).ToList();

            return students;
        }

        public void AddStudent(Teacher teacher, Student student)
        {
            if (!IsConnected())
                return;

            if (student == null || teacher == null)
                return;

            student.TeacherUser = teacher;
            student.TeacherUserId = teacher.UserId;
            DBContext.Update(student);
            Save();
        }

        public void RemoveStudent(Teacher teacher, Student student)
        {
            if (!IsConnected())
                return;

            if (student == null || teacher == null)
                return;

            student.TeacherUser = null;
            student.TeacherUserId = null;
            DBContext.Update(student);
            Save();
        }

        #endregion

        #region Parent
        public Parent GetParent(string userName)
        {
            if (!IsConnected())
                return null;

            var parent = (from p in DBContext.Parents
                          where p.User.UserName == userName
                          select p).FirstOrDefault();

            return parent;
        }

        public IEnumerable<Student> GetStudentsByParent(Guid parentID)
        {
            if (!IsConnected()) return null;

            var parent = (from Parent p in SessionContext.DBContext.Parents
                          where p.UserId == parentID
                          select p).FirstOrDefault();
            if (parent != null) return parent.StudentsUsers;
            return null;                                                                                                            //var students = DBContext.Execute<Student>(url);
        }
        #endregion

        #region Student
        public List<Student> GetUnassignedStudents()
        {
            if (!IsConnected())
                return null;

            var students = (from s in DBContext.Students
                            where s.TeacherUserId == null
                            select s).OrderBy(s => s.LastName).ToList();

            return students;
        }

        public Student GetStudent(string userName)
        {
            if (!IsConnected())
                return null;

            var student = (from s in DBContext.Students
                           where s.User.UserName == userName
                           select s).FirstOrDefault();

            return student;
        }

        public Student GetStudent(string firstname, string lastname)
        {
            if (!IsConnected())
                return null;

            var student = (from s in DBContext.Students
                           where s.FirstName == firstname && s.LastName == lastname
                           select s).FirstOrDefault();

            return student;
        }

        public string GetClassNameByStudent(Guid studentID)
        {
            if (!IsConnected())
                return null;

            var classname = (from s in DBContext.Students
                             where s.User.UserId == studentID
                             select s.TeacherUser.Class).FirstOrDefault();

            return classname;
        }

        public List<Grade> GetGradesByStudent(Guid studentID)
        {
            if (!IsConnected())
                return null;

            var grades = (from g in DBContext.Grades
                          where g.StudentUserId == studentID
                          select g).ToList();

            return grades;
        }
        #endregion

        #region Grades
        private List<Subject> GetSubjects()
        {
            if (!IsConnected())
                return null;

            // Find all the subjects in the dataservice for the current student and add them to the Subjects collection
            var subs = (from s in DBContext.Subjects
                        select s).OrderBy(s => s.Name).ToList();

            Subjects = new List<Subject>();

            foreach (Subject s in subs)
                Subjects.Add(s);

            return Subjects;
        }

        public static Subject GetSubject(int id)
        {
            if (Subjects.Count == 0)
                return null;

            return Subjects.First(s => s.Id == id);
        }

        public void AddGrade(Grade grade)
        {
            grade.StudentUserId = SessionContext.CurrentStudent.UserId;
            DBContext.Grades.Add(grade);
            Save();
        }

        public void UpdateGrade(Grade grade)
        {
            if (!IsConnected())
                return;

            DBContext.Update(grade);
            Save();
        }
        #endregion

        #region Helper Methods
        public bool IsConnected()
        {
            return DBContext != null;
        }

        public void Save()
        {
            DBContext.SaveChanges();
        }
        #endregion
    }
}