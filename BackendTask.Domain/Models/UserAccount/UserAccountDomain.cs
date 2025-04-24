using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Domain.Models.UserAccount
{
    public class UserAccountDomain
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
