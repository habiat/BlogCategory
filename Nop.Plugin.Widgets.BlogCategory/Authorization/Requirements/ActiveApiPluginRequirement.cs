using Microsoft.AspNetCore.Authorization;

namespace Nop.Plugin.Widgets.BlogCategory.Authorization.Requirements
{
    public class ActiveApiPluginRequirement : IAuthorizationRequirement
    {
        public bool IsActive()
        {
            

            return true;
        }
    }
}
