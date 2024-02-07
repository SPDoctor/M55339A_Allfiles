using GradesPrototype.Data;

namespace GradesPrototype.Services
{
    // Global context for operations performed by MainWindow
    public static class SessionContext
    {
        public static int UserID;
        public static string UserName;
        public static Role UserRole;
        public static Student CurrentStudent;
        public static Teacher CurrentTeacher;
    }
}
