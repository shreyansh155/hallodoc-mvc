﻿using System;
using System.Collections.Generic;

namespace DataAccess.DataModels;

public partial class Role
{
    public int Roleid { get; set; }

    public string Name { get; set; } = null!;

    public short Accounttype { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime Createddate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public string? Ip { get; set; }

    public bool? Isdeleted { get; set; }
}
