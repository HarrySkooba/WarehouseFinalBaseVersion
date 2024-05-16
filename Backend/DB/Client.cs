﻿using System;
using System.Collections.Generic;

namespace Backend;

public partial class Client
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Info { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
