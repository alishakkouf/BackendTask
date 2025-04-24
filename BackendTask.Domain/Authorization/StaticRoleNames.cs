using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Domain.Authorization
{
    public static class StaticRoleNames
    {
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string Administrator = nameof(Administrator);
        public const string Guest = nameof(Guest);
    }
}
