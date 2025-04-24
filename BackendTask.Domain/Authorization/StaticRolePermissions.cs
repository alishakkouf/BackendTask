using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BackendTask.Domain.Authorization.Permissions;

namespace BackendTask.Domain.Authorization
{
    /// <summary>
    /// Static roles and permissions for them are defined here.
    /// System will seed them on startup.
    /// </summary>
    public static class StaticRolePermissions
    {
        public static readonly List<StaticRolePermissionDomain> Roles =
            new List<StaticRolePermissionDomain>
            {
                new ()
                {
                    Permissions =  Permissions.All,
                    Role = StaticRoleNames.Administrator
                },
                new ()
                {
                    Role = StaticRoleNames.Guest,
                    Permissions =  new List<StaticPermissionDomain>
                    {
                        new StaticPermissionDomain(
                            ToDoTask.View,
                            ToDoTask.ViewPermissionLabel,
                            ToDoTask.Subject,
                            ToDoTask.ViewAction
                        ),
                        new StaticPermissionDomain(
                            ToDoTask.Create,
                            ToDoTask.CreatePermissionLabel,
                            ToDoTask.Subject,
                            ToDoTask.CreateAction
                        )

                    }
                }
            };
    }
}
