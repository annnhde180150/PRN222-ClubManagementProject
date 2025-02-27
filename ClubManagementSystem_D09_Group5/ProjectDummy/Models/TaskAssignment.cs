using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class TaskAssignment
{
    public int AssignmentId { get; set; }

    public int TaskId { get; set; }

    public int MembershipId { get; set; }

    public DateTime? AssignedAt { get; set; }

    public virtual ClubMember Membership { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
