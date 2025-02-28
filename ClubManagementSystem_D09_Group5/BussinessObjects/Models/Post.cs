using System;
using System.Collections.Generic;

namespace BussinessObjects.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int CreatedBy { get; set; }

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public virtual ClubMember CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public virtual ICollection<PostReaction> Reactions { get; set; } = new List<PostReaction>();
}
