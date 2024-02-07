using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class Profile
{
    public Guid UserId { get; set; }

    public string PropertyNames { get; set; } = null!;

    public string PropertyValueStrings { get; set; } = null!;

    public byte[] PropertyValueBinary { get; set; } = null!;

    public DateTime LastUpdatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}
