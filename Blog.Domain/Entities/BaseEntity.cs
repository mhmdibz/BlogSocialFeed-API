using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public bool IsDeleted { get; protected set; }
        public byte[] RowVersion { get; protected set; } = Array.Empty<byte>();

        // Domain methods for better encapsulation
        public virtual void Delete() => IsDeleted = true;
        public virtual void Restore() => IsDeleted = false;
    }
}
