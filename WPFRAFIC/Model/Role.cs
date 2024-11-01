using System;
using System.Collections.Generic;

namespace WPFRAFIC.Model;

public partial class Role
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();
}
