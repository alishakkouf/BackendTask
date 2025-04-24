using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace BackendTask.Data.Models.Identity
{
    public class UserAccount : IdentityUser<long>, IAuditedEntity
    {

        [StringLength(100)]
        [Required]
        public required string FirstName { get; set; }

        [StringLength(100)]
        [Required]
        public string LastName { get; set; }

        [StringLength(20)]
        [Required]
        public override string PhoneNumber { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public long? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public bool? IsDeleted { get; set; } = false;

        public bool IsActive { get; set; } = true;

        internal virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
