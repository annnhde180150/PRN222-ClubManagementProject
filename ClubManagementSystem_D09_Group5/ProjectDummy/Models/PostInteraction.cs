using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class PostInteraction
{
    public int InteractionId { get; set; }

    public int PostId { get; set; }

    public int UserId { get; set; }

    public string Type { get; set; } = null!;

    public string? CommentText { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
