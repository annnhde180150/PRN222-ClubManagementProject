using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }
        //navigation properties
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
