using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [ForeignKey("ClubMember")]
        public int CreatedBy { get; set; }

        [Required]
        [MaxLength(255)]
        public string EventTitle { get; set; }

        public string EventDescription { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ClubMember ClubMember { get; set; }
    }
}
