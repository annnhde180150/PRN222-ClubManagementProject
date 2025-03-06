using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Models;

public partial class Post
{
    [Key]
    public int PostId { get; set; }

    [Required]
    public int CreatedBy { get; set; }

    [Required]
    [StringLength(2000)]
    public string Content { get; set; } = null!;

    [Url]
    public string? ImageUrl { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [StringLength(50)]
    public string? Status { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual ClubMember CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<PostReaction> Reactions { get; set; } = new List<PostReaction>();
}
