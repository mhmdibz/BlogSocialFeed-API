using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Follow
    {
        // Foreign Keys
        public int? FollowerId { get; set; }
        public int? FolloweeId { get; set; }

        // Navigation Properties
        public User? Follower { get; set; }
        public User? Followee { get; set; }
    }
}
