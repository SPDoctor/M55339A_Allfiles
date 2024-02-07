using System;
using System.Collections.Generic;

namespace Grades.DataModel;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; } = new List<Grade>();
}
