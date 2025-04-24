using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Shared
{
    public interface IAuditedEntity
    {
        public long? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool? IsDeleted { get; set; }
    }

    public class AuditedEntity : IAuditedEntity
    {
        public long? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool? IsDeleted { get; set; } = false;
    }
}
