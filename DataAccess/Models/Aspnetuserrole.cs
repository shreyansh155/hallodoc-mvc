using System;
using System.Collections.Generic;

namespace HalloDocWeb.Models;

public partial class Aspnetuserrole
{
    public string Userid { get; set; } = null!;

    public int Roleid { get; set; }

    public virtual Aspnetuser User { get; set; } = null!;
}
