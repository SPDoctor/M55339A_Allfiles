using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class User
{
    public Guid ApplicationId { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public bool IsAnonymous { get; set; }

    public DateTime LastActivityDate { get; set; }

    public string UserPassword { get; set; } = null!;

    public virtual Application Application { get; set; } = null!;

    public virtual Membership? Membership { get; set; }

    public virtual Parent? Parent { get; set; }

    public virtual Profile? Profile { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }

    public virtual ICollection<Role> Roles { get; } = new List<Role>();
}
