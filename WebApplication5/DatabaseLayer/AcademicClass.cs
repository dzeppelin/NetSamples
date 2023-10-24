using System;
using System.Collections.Generic;

namespace WebApplication5.DatabaseLayer;

public partial class AcademicClass
{
    public int Id { get; set; }

    public string? ClassName { get; set; }

    public virtual Student IdNavigation { get; set; } = null!;
}
