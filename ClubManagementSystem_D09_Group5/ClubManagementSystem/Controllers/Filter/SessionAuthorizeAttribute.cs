using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClubManagementSystem.Controllers.Filter
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly List<string> _roles;

        public SessionAuthorizeAttribute(string roles)
        {
            _roles = roles.Split(',').ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userRole = context.HttpContext.Session.GetString("UserRole");

            var actionDescriptor = context.ActionDescriptor;
            var allowAnonymous = actionDescriptor.EndpointMetadata.Any(em => em is AllowAnonymousAttribute);

            if (allowAnonymous)
            {
                base.OnActionExecuting(context);
                return;
            }
            var actionAttributes = actionDescriptor.EndpointMetadata.OfType<SessionAuthorizeAttribute>().ToList();
            var rolesToCheck = actionAttributes.Any() ? actionAttributes.SelectMany(a => a._roles).ToList() : _roles;

            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else if (!rolesToCheck.Contains(userRole) && userRole != "0")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
