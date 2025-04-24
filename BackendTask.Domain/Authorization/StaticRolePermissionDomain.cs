using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Domain.Authorization
{
    public class StaticRolePermissionDomain
    {
        public string Role { get; set; }
        public List<StaticPermissionDomain> Permissions { get; set; }
    }

    public class StaticPermissionDomain
    {
        public StaticPermissionDomain(string permission, string permissionLabel,
                                      string subject,  string action)
        {
            Permission = permission;
            PermissionLabel = permissionLabel;
            Subject = subject;
            Action = action;
        }

        public string Permission { get; set; }
        public string PermissionLabel { get; set; }
        public string Subject { get; set; }
        public string Action { get; set; }
    }
}
