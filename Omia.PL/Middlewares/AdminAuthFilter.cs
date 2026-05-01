using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Omia.PL.Helpers;


namespace Omia.PL.Middlewares
{
    public class AdminAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Skip auth check for Login action
            var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(m => m is Omia.PL.Middlewares.AllowAnonymousActionAttribute);

            if (hasAllowAnonymous)
            {
                base.OnActionExecuting(context);
                return;
            }

            var session = context.HttpContext.Session;
            var adminId = session.GetString(AdminSessionHelper.AdminIdKey);

            if (string.IsNullOrEmpty(adminId))
            {
                context.Result = new RedirectToActionResult("Login", "CentralAdmin", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
