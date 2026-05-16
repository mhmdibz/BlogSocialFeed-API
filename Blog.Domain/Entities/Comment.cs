using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Comment: BaseEntity
    {
        public required string Content { get; set; }

        // Foreign Keys
        public int? PostId { get; set; }
        public int? UserId { get; set; }


        // Navigation Properties
        public Post? Post { get; set; }
        public User? User { get; set; }
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
