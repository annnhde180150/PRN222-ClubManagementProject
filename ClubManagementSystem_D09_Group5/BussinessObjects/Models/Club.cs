using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BussinessObjects.Models;

public partial class Club
{
    [Key]
    public int ClubId { get; set; }

    [Required]
    [StringLength(100)]
    public string ClubName { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();
}
