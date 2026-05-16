using Blog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace Blog.Domain.Entities
{
    public class User: BaseEntity
    {
        private readonly List<Post> _posts = new();
        private readonly List<Comment> _comments = new();
        private readonly List<Reaction> _reactions = new();
        private readonly List<Follow> _followers = new();
        private readonly List<Follow> _following = new();

        public required string UserName { get; set; }
        public required Email Email { get; set; }
        public string? Bio { get; set; }

        // Read-only collections for better encapsulation
        public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();
        
        [InverseProperty(nameof(Follow.Follower))]
        public IReadOnlyCollection<Follow> Followers => _followers.AsReadOnly();
        
        [InverseProperty(nameof(Follow.Followee))]
        public IReadOnlyCollection<Follow> Following => _following.AsReadOnly();

        // Domain methods for managing relationships
        public void AddPost(Post post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            _posts.Add(post);
        }

        public void AddComment(Comment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            _comments.Add(comment);
        }

        public void AddReaction(Reaction reaction)
        {
            if (reaction == null) throw new ArgumentNullException(nameof(reaction));
            _reactions.Add(reaction);
        }
    }
}
