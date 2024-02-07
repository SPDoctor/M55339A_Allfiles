using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class Student
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string ImageName { get; set; } = null!;

    public Guid? TeacherUserId { get; set; }

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();

    public virtual Teacher? TeacherUser { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Parent> ParentsUsers { get; } = new List<Parent>();
}
