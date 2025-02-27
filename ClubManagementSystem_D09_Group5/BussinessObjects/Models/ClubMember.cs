using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class ClubMember
{
    public int MembershipId { get; set; }

    public int ClubId { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User User { get; set; } = null!;
}
