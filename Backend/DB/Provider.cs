using System;
using System.Collections.Generic;

namespace Backend;

public partial class Provider
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Info { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Supply> Supplies { get; set; } = new List<Supply>();
}
