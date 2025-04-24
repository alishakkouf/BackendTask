using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Domain;
using BackendTask.Shared;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BackendTask.Common
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public long? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.GetUserId();
        }

        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User.GetUserName();
        }

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
        }

        public bool HasPermission(string permission)
        {
            return _httpContextAccessor.HttpContext?.User.HasClaim(x => x.Type == Constants.PermissionsClaimType && x.Value == permission) ?? false;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }
}
