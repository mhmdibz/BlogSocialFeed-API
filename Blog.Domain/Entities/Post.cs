using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace Blog.Domain.Entities
{
    public class Post : BaseEntity
    {
        public int? UserId { get; set; }
        public required string Content { get; set; }

        // Navigation Properties
        public User? User { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
