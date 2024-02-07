namespace Grades.DataModel;

public partial class Application
{
    public string ApplicationName { get; set; } = null!;

    public Guid ApplicationId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Membership> Memberships { get; } = new List<Membership>();

    public virtual ICollection<Role> Roles { get; } = new List<Role>();

    public virtual ICollection<User> Users { get; } = new List<User>();
}
