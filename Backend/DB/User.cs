using System;
using System.Collections.Generic;

namespace Backend;

public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Roleid { get; set; }

    public bool? Adminpanel { get; set; }

    public virtual Role Role { get; set; } = null!;
}
