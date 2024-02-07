using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class AspnetPath
{
    public Guid ApplicationId { get; set; }

    public Guid PathId { get; set; }

    public string Path { get; set; } = null!;

    public string LoweredPath { get; set; } = null!;

    public virtual AspnetApplication Application { get; set; } = null!;

    public virtual AspnetPersonalizationAllUser? AspnetPersonalizationAllUser { get; set; }

    public virtual ICollection<AspnetPersonalizationPerUser> AspnetPersonalizationPerUsers { get; } = new List<AspnetPersonalizationPerUser>();
}
