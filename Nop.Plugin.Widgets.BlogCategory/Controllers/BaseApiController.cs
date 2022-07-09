using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nop.Plugin.Widgets.BlogCategory.Controllers
{
    [Authorize(Policy = JwtBearerDefaults.AuthenticationScheme, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BaseApiController : Controller
    {
        
        protected IActionResult AccessDenied()
        {
            return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden);
        }

    }
}
