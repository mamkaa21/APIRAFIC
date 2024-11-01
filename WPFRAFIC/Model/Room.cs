using System;
using System.Collections.Generic;

namespace WPFRAFIC.Model;

public partial class Room
{
    public int Id { get; set; }

    public int? Floor { get; set; }

    public int? NumberRoom { get; set; }

    public int? CategoryId { get; set; }

    public int? StatusId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Guest> Guests { get; set; } = new List<Guest>();

    public virtual Status? Status { get; set; }
}
