using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? ProfilePicture { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();

    public virtual ICollection<ClubRequest> ClubRequests { get; set; } = new List<ClubRequest>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PostInteraction> PostInteractions { get; set; } = new List<PostInteraction>();
}
