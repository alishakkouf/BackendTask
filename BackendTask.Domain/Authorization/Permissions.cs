using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Shared;

namespace BackendTask.Domain.Authorization
{
    /// <summary>
    /// Static permissions of the system are defined here, and they will be added as claims (of type <see cref="Constants.PermissionsClaimType"/>)
    /// to the corresponding roles. Static role permissions are defined in <see cref="StaticRolePermissions"/>.
    /// </summary>
    public static class Permissions
    {
        private const string PermissionsPrefix = Constants.PermissionsClaimType + ".";

        //Permission,                       ===> Permission.User.Create
        //CreatePermissionLabel,            ===> Create User
        //Subject,                          ===> User
        //ActionCreate,                     ===> Create

        public static readonly List<StaticPermissionDomain> All = new()
        {
            new StaticPermissionDomain(
                User.Create,
                User.CreatePermissionLabel,
                User.Subject,
                User.CreateAction
            ),
            new StaticPermissionDomain(
                User.Delete,
                User.DeletePermissionLabel,
                User.Subject,
                User.DeleteAction
            ),
            new StaticPermissionDomain(
                User.Update,
                User.UpdatePermissionLabel,
                User.Subject,
                User.UpdateAction
            ),
            new StaticPermissionDomain(
                User.View,
                User.ViewPermissionLabel,
                User.Subject,
                User.ViewAction
            ),
            new StaticPermissionDomain(
                ToDoTask.View,
                ToDoTask.ViewPermissionLabel,
                ToDoTask.Subject,
                ToDoTask.ViewAction
            )
            ,
            new StaticPermissionDomain(
                ToDoTask.Create,
                ToDoTask.CreatePermissionLabel,
                ToDoTask.Subject,
                ToDoTask.CreateAction
            )
            ,
            new StaticPermissionDomain(
                ToDoTask.Delete,
                ToDoTask.DeletePermissionLabel,
                ToDoTask.Subject,
                ToDoTask.DeleteAction
            )
            ,
            new StaticPermissionDomain(
                ToDoTask.Update,
                ToDoTask.UpdatePermissionLabel,
                ToDoTask.Subject,
                ToDoTask.UpdateAction
            )

        };


        #region User Permissions
        public static class User
        {
            public const string Create = PermissionsPrefix + "User.Create";
            public const string View = PermissionsPrefix + "User.View";
            public const string Update = PermissionsPrefix + "User.Update";
            public const string Delete = PermissionsPrefix + "User.Delete";

            public const string Subject = "User";

            public const string CreatePermissionLabel = "Create User";
            public const string ViewPermissionLabel = "View User";
            public const string UpdatePermissionLabel = "Update User";
            public const string DeletePermissionLabel = "Delete User";

            public const string CreateAction = "Create";
            public const string ViewAction = "View";
            public const string UpdateAction = "Update";
            public const string DeleteAction = "Delete";
        }
        #endregion
        //ToDoTask
        public static class ToDoTask
        {
            public const string Create = PermissionsPrefix + "ToDoTask.Create";
            public const string View = PermissionsPrefix + "ToDoTask.View";
            public const string Update = PermissionsPrefix + "ToDoTask.Update";
            public const string Delete = PermissionsPrefix + "ToDoTask.Delete";

            public const string Subject = "ToDoTask";

            public const string CreatePermissionLabel = "Create ToDoTask";
            public const string ViewPermissionLabel = "View ToDoTask";
            public const string UpdatePermissionLabel = "Update ToDoTask";
            public const string DeletePermissionLabel = "Delete ToDoTask";

            public const string CreateAction = "Create";
            public const string ViewAction = "View";
            public const string UpdateAction = "Update";
            public const string DeleteAction = "Delete";
        }
    }
}

