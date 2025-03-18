using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BussinessObjects.Models;

public partial class ClubTask
{
    [Key]
    public int TaskId { get; set; }

    [Required]
    [StringLength(1000)]
    public string TaskDescription { get; set; } = null!;

    [StringLength(50)]
    public string? Status { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? DueDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    public virtual ClubMember CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
