using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPFRAFIC.Model;

public partial class Employee
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; } = 0;

    public DateTime? RegistrationDate { get; set; }

    public sbyte? IsBlocked { get; set; } = 0;

    public DateTime? Lastlogin { get; set; }

    // public virtual Role? Role { get; set; }

    [NotMapped]
    public string Blocked { get => IsBlocked != null ? (IsBlocked.Value == 1 ? "Заблокирован" : " ") : null; }
}
