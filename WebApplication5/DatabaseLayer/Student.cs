using System;
using System.Collections.Generic;

namespace WebApplication5.DatabaseLayer;

public partial class Student
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public string? IpAddress { get; set; }

    public virtual AcademicClass? AcademicClass { get; set; }
}
