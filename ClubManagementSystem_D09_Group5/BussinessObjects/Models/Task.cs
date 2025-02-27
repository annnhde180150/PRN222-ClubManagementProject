using System;
using System.Collections.Generic;

namespace BussinessObjects.Models;
public partial class Task
{
    public int TaskId { get; set; }

    public string TaskDescription { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public virtual ClubMember CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
