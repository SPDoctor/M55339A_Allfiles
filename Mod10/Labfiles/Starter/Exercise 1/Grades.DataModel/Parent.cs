namespace Grades.DataModel;

public partial class Parent
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Student> StudentsUsers { get; } = new List<Student>();
}
