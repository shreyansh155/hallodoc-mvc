﻿using System;
using System.Collections.Generic;

namespace DataAccess.DataModels;

public partial class Shiftdetail
{
    public int Shiftdetailid { get; set; }

    public int Shiftid { get; set; }

    public DateOnly Shiftdate { get; set; }

    public int? Regionid { get; set; }

    public DateTime Starttime { get; set; }

    public DateTime Endtime { get; set; }

    public short Status { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime? Modifieddate { get; set; }

    public DateTime? Lastrunningdate { get; set; }

    public string? Eventid { get; set; }

    public bool? Isdeleted { get; set; }

    public bool? Issync { get; set; }

    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    public virtual Shift Shift { get; set; } = null!;

    public virtual ICollection<Shiftdetailregion> Shiftdetailregions { get; set; } = new List<Shiftdetailregion>();
}
