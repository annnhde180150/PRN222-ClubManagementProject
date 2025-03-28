using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models.Dtos
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageBase64 { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUsername { get; set; } = string.Empty;
    }
}
