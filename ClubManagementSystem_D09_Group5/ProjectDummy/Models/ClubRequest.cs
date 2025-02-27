using System;
using System.Collections.Generic;

namespace ProjectDummy.Models;

public partial class ClubRequest
{
    public int RequestId { get; set; }

    public int UserId { get; set; }

    public string ClubName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
