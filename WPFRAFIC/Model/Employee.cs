﻿using System;
using System.Collections.Generic;

namespace WPFRAFIC.Model;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public virtual Role? Role { get; set; }
}
