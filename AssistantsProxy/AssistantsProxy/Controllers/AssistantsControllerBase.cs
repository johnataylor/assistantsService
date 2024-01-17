using AssistantsProxy.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    /// <summary>
    /// This is really only about implementing the "proxy" model classes. Otherwise this probably isn't what we would choose to do.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class AssistantsControllerBase : ControllerBase
    {
        protected IActionResult ErrorMessage()
        {
            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            if (exceptionHandlerFeature.Error is ErrorMessageException exception && exception.ErrorMessage != null)
            {
                return StatusCode(exception.StatusCode, exception.ErrorMessage);
            }
            return StatusCode(500);
        }

        protected string? BearerToken
        {
            get { return Request.Headers.Authorization[0]?.Split(' ')[1]; }
        }
    }
}
