using System;
using System.Collections.Generic;

namespace APIRAFIC;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public virtual Role? Role { get; set; }
}

public partial class EmployeeModel
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateTime? RegistrationDate { get; set; }

}
