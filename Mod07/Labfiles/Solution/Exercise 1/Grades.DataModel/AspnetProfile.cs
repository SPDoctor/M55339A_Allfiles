using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class AspnetProfile
{
    public Guid UserId { get; set; }

    public string PropertyNames { get; set; } = null!;

    public string PropertyValuesString { get; set; } = null!;

    public byte[] PropertyValuesBinary { get; set; } = null!;

    public DateTime LastUpdatedDate { get; set; }

    public virtual AspnetUser User { get; set; } = null!;
}
