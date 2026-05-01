using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Omia.PL.Middlewares
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            // Skip if the endpoint allows anonymous access
            var allowAnonymous = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();
            if (allowAnonymous != null)
            {
                await _next(context);
                return;
            }

            var authorizeRolesAttr = endpoint.Metadata.GetMetadata<AuthorizeRolesAttribute>();

            // No AuthorizeRoles attribute — let it pass through (other [Authorize] still works)
            if (authorizeRolesAttr == null)
            {
                await _next(context);
                return;
            }

            // User must be authenticated
            if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"isSuccess\":false,\"message\":\"Unauthorized: You must be logged in.\"}");
                return;
            }

            // Get user role from claims
            var userRoleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userRoleClaim))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"isSuccess\":false,\"message\":\"Forbidden: No role assigned to this user.\"}");
                return;
            }

            // Check if the user's role matches any of the allowed roles
            var allowedRoleNames = authorizeRolesAttr.Roles.Select(r => r.ToString()).ToList();

            if (!allowedRoleNames.Contains(userRoleClaim, StringComparer.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"isSuccess\":false,\"message\":\"Forbidden: You do not have the required role to access this resource.\"}");
                return;
            }

            await _next(context);
        }
    }
}
