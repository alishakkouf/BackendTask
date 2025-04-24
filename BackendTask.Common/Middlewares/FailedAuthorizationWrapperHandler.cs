using System.Net;
using BackendTask.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Localization;
using Serilog;

namespace BackendTask.Common.Middlewares
{
    /// <summary>
    /// Log and convert authorization failures to <see cref="ErrorResultDto"/> with generic message.
    /// </summary>
    public class FailedAuthorizationWrapperHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler;
        private readonly IStringLocalizer _localizer;

        public FailedAuthorizationWrapperHandler(IStringLocalizerFactory factory)
        {
            _defaultHandler = new AuthorizationMiddlewareResultHandler();
            _localizer = factory.Create(typeof(CommonResource));
        }

        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext httpContext,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult)
        {
            Log.Information($"User Authenticated: {httpContext.User.Identity?.IsAuthenticated}");
            Log.Information($"User Name: {httpContext.User.Identity?.Name}");
            Log.Information($"PolicyAuthorizationResult: Challenged={policyAuthorizationResult.Challenged}, Forbidden={policyAuthorizationResult.Forbidden}");

            if (policyAuthorizationResult.Challenged)
            {
                Log.Warning($"Unauthenticated access to url {httpContext.Request.Path.Value}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(_localizer["Unauthorized"]);
                return;
            }

            if (policyAuthorizationResult.Forbidden)
            {
                Log.Warning($"Unauthorized access by user {httpContext.User.Identity?.Name}, to url {httpContext.Request.GetEncodedUrl()}");
                httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsync(_localizer["Forbidden"]);
                return;
            }

            await _defaultHandler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
        }
    }
}
