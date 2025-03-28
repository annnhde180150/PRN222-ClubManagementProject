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
    }

    public class PostDetailsDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageBase64 { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByUsername { get; set; } = string.Empty;
        public int ClubId { get; set; }
        public string ClubName { get; set; } = string.Empty;
        public List<RelatedPostDto> RelatedPosts { get; set; } = new();
    }

    public class RelatedPostDto
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string ImageBase64 { get; set; }
    }

    public class PostUpdateDto
    {
        public int PostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string ImageBase64 { get; set; }
    }

}
