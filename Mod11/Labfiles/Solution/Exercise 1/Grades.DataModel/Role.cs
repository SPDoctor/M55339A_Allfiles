namespace Grades.DataModel;

public partial class Role
{
    public Guid ApplicationId { get; set; }

    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();
}
