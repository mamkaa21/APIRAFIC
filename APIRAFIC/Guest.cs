using System;
using System.Collections.Generic;

namespace APIRAFIC;

public partial class Guest
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Patronymic { get; set; }

    public int? RoleId { get; set; }

    public int? RoomId { get; set; }

    public DateOnly? EntryDate { get; set; }

    public DateOnly? DepartureDate { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Room? Room { get; set; }
}
