using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessObjects.Models
{
    public class PostReaction
    {
        public int ReactionId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        //navigation properties
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
