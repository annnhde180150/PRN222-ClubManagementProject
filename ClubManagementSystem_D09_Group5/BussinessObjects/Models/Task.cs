using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Pending";

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("ClubMember")]
        public int CreatedBy { get; set; }

        public ClubMember ClubMember { get; set; }

        public ICollection<TaskAssignment> TaskAssignments { get; set; }
    }
}
