using Microsoft.AspNetCore.Mvc.Filters;

namespace ClubManagementSystem.Controllers.Filter
{
    public class ClubAuthorizationFilter : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
