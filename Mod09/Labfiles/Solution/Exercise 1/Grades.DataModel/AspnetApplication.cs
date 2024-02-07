namespace Grades.DataModel;

public partial class AspnetApplication
{
    public string ApplicationName { get; set; } = null!;

    public string LoweredApplicationName { get; set; } = null!;

    public Guid ApplicationId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AspnetMembership> AspnetMemberships { get; } = new List<AspnetMembership>();

    public virtual ICollection<AspnetPath> AspnetPaths { get; } = new List<AspnetPath>();

    public virtual ICollection<AspnetRole> AspnetRoles { get; } = new List<AspnetRole>();

    public virtual ICollection<AspnetUser> AspnetUsers { get; } = new List<AspnetUser>();
}
