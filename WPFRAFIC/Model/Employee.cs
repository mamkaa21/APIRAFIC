using System;
using System.Collections.Generic;

namespace WPFRAFIC.Model;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public sbyte? IsBlocked { get; set; }

    public DateTime? Lastlogin { get; set; }

    public virtual Role? Role { get; set; }
}
