using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class VwAspnetWebPartStatePath
{
    public Guid ApplicationId { get; set; }

    public Guid PathId { get; set; }

    public string Path { get; set; } = null!;

    public string LoweredPath { get; set; } = null!;
}
