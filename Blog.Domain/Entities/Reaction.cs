using Blog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Reaction:BaseEntity
    {
        // Foreign Keys
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public int? UserId { get; set; }

        public ReactionKind Kind { get; set; }

        // Navigation Properties
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
        public User? User { get; set; }
    }
}
