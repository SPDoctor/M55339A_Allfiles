using Grades.DataModel;

namespace Grades.WPF.Services
{

    // Global context for operations performed by MainWindow
    public class SessionContext
    {
        public static SchoolGradesDbContext DBContext = new SchoolGradesDbContext();

        public static Guid UserID { get; set; }
        public static string UserName { get; set; }
        public static RoleType UserRole { get; set; }
        public static Student CurrentStudent { get; set; }
        public static Teacher CurrentTeacher { get; set; }
        public static Parent CurrentParent { get; set; }
        public static List<ViewModel.Grade> CurrentGrades { get; set; }

        public static bool Logon(string username, string password)
        {
            var user = (from User u in SessionContext.DBContext.Users
                        where u.UserName == username && u.UserPassword == password
                        select u).FirstOrDefault();
            // If the credentials do not match those for a User then they must be invalid
            if (user == null)
            {
                return false;
            }

            var teacher = (from Teacher t in SessionContext.DBContext.Teachers
                           where t.UserId == user.UserId
                           select t).FirstOrDefault();

            if (teacher != null)
            {
                // Save the UserID and Role (teacher or student) and UserName in the global context
                UserID = teacher.UserId;
                UserRole = RoleType.Teacher;
                UserName = user.UserName;
                CurrentTeacher = teacher;
                return true; // logon succeeded
            }
            // If the user is not a teacher, check whether the username and password match those of a student
            var student = (from Student s in SessionContext.DBContext.Students
                           where s.UserId == user.UserId
                           select s).FirstOrDefault();

            if (student != null)
            {
                // Save the details of the student in the global context
                UserID = student.UserId;
                UserRole = RoleType.Student;
                UserName = user.UserName;
                CurrentStudent = student;
                return true; // logon succeeded
            }

            // If the user is not a student, check whether the username and password match those of a parent
            var parent = (from Parent p in SessionContext.DBContext.Parents
                          where p.UserId == user.UserId
                          select p).FirstOrDefault();

            if (parent != null)
            {
                // Save the details of the student in the global context
                UserID = parent.UserId;
                UserRole = RoleType.Parent;
                UserName = user.UserName;
                CurrentParent = parent;
                return true; // logon succeeded
            }

            // if no valid user type then logon fails
            return false;
        }

        public static void Logoff()
        {
            UserName = "";
            UserRole = RoleType.None;
            CurrentStudent = null;
            CurrentTeacher = null;
            CurrentParent = null;
        }

        public static void Save()
        {
            DBContext.SaveChanges();
        }
    }
}
