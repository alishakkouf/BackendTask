using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendTask.Domain
{
    public interface ICurrentUserService
    {
        /// <summary>
        /// Returns User Id of current logged user
        /// </summary>
        long? GetUserId();

        /// <summary>
        /// Checks if current user has a specific role
        /// </summary>
        bool IsInRole(string role);

        /// <summary>
        /// Check if user has permission save in his claims
        /// </summary>
        bool HasPermission(string permission);

        bool IsAuthenticated();
    }
}
