using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
}
