using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nop.Plugin.Widgets.BlogCategory.Authorization.Requirements;

namespace Nop.Plugin.Widgets.BlogCategory.Authorization.Policies
{
    public class ValidSchemeAuthorizationPolicy : AuthorizationHandler<AuthorizationSchemeRequirement>
    {
        IHttpContextAccessor _httpContextAccessor = null;
        public ValidSchemeAuthorizationPolicy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationSchemeRequirement requirement)
        {
            //var mvcContext = context.Resource as
            //                     AuthorizationFilterContext;
            //if (requirement.IsValid(mvcContext?.HttpContext.Request.Headers))
            //{
            //    context.Succeed(requirement);
            //}
            //else
            //{
            //    context.Fail();
            //}

            if (requirement.IsValid(_httpContextAccessor?.HttpContext.Request.Headers))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
