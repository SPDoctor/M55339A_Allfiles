using System.Text.RegularExpressions;

namespace Grades.DataModel;

// Types of user
public enum RoleType { None, Teacher, Student, Parent };

public partial class User
{
    public bool SetPassword(RoleType role, string pwd)
    {
        if (role == RoleType.Teacher)
        {
            // Use a regular expression to check that the password contains at least two numeric characters
            Match numericMatch = Regex.Match(pwd, @".*[0-9]+.*[0-9]+.*");

            // If the password provided as the parameter is at least 8 characters long and contains at least two numeric characters then save it and return true
            if (pwd.Length >= 8 && numericMatch.Success)
            {
                UserPassword = pwd;
                return true;
            }
        }
        else if (role == RoleType.Student)
        {
            // If the password provided as the parameter is at least 6 characters long then save it and return true
            if (pwd.Length >= 6)
            {
                UserPassword = pwd;
                return true;
            }
        }
        // If the password is either not complex enough (Teacher) or not long enough (Teacher or Student), then do not save it and return false
        return false;
    }


    public bool VerifyPassword(string pass)
    {
        return (String.Compare(pass, UserPassword) == 0);
    }
}

public partial class Teacher
{
    public void RemoveFromClass(Student student)
    {
        // Verify that the student is actually assigned to the class for this teacher
        if (student.TeacherUserId == UserId)
        {
            // Reset the TeacherID property of the student
            student.TeacherUserId = null;
        }
        else
        {
            // If the student is not assigned to the class for this teacher, throw an ArgumentException
            throw new ArgumentException("Student", "Student is not assigned to this class");
        }
    }
}

