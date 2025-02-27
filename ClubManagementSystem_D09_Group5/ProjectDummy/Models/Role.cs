using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
}
