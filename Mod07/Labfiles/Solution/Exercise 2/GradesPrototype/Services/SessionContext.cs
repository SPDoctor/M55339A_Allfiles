using Grades.DataModel;

namespace GradesPrototype.Services
{

    // Global context for operations performed by MainWindow
    public class SessionContext
    {
        public static SchoolGradesDbContext DBContext = new SchoolGradesDbContext();

        public static Guid UserID;
        public static string UserName;
        public static RoleType UserRole;
        public static Student CurrentStudent;
        public static Teacher CurrentTeacher;

        public static void Save()
        {
            DBContext.SaveChanges();
        }
    }
}
