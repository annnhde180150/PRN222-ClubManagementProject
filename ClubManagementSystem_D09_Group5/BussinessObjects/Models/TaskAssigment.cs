using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class TaskAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [ForeignKey("Task")]
        public int TaskId { get; set; }

        [ForeignKey("ClubMember")]
        public int MembershipId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;

        public Task Task { get; set; }
        public ClubMember ClubMember { get; set; }
    }
}
