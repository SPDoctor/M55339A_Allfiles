namespace Grades.DataModel;

public partial class Teacher
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Class { get; set; } = null!;

    public virtual ICollection<Student> Students { get; } = new List<Student>();

    public virtual User User { get; set; } = null!;
}
