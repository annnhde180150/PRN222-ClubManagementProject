using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Models;

public partial class TaskAssignment
{
    [Key]
    public int AssignmentId { get; set; }

    [Required]
    public int TaskId { get; set; }

    [Required]
    public int MembershipId { get; set; }

    //Declined, Accepted, On Going, Done, Pending
    public string Status { get; set; } = "Pending";

    [DataType(DataType.DateTime)]
    public DateTime? AssignedAt { get; set; }

    [ForeignKey("MembershipId")]
    public virtual ClubMember Membership { get; set; } = null!;

    [ForeignKey("TaskId")]
    public virtual ClubTask Task { get; set; } = null!;
}
