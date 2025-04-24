using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Shared;

namespace BackendTask.Common
{
    public static class IdentityUserExtensions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var id = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(id) ? (int?)null : int.Parse(id);
        }

        public static string? GetUserName(this ClaimsPrincipal user)
            => user?.FindFirst(ClaimTypes.Name)?.Value;

        public static List<string> GetPermissions(this ClaimsPrincipal user)
        {
            return user?.Claims.Where(c => c.Type == Constants.PermissionsClaimType).Select(c => c.Value).ToList() ?? [];
        }

        public static bool IsWithoutRole(this ClaimsPrincipal user)
        {
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            return string.IsNullOrEmpty(role);
        }

        //public static string GetAccessToken(this HttpRequest request)
        //{
        //    return request.Headers.Authorization[0]["bearer ".Length..];
        //}
    }
}
