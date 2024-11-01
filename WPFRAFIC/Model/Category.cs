using System;
using System.Collections.Generic;

namespace WPFRAFIC.Model;

public partial class Category
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
