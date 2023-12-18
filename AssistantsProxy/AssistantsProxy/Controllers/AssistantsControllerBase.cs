using Microsoft.AspNetCore.Mvc;

namespace AssistantsProxy.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class AssistantsControllerBase : ControllerBase
    {
        protected string? BearerToken
        {
            get { return Request.Headers.Authorization[0]?.Split(' ')[1]; }
        }
    }
}
