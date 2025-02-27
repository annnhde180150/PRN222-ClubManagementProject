using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class Event
{
    public int EventId { get; set; }

    public int CreatedBy { get; set; }

    public string EventTitle { get; set; } = null!;

    public string? EventDescription { get; set; }

    public DateTime EventDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ClubMember CreatedByNavigation { get; set; } = null!;
}
