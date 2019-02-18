using Microsoft.AspNetCore.Mvc;

namespace GuidBase64.TestWebApi.Controllers
{
    [Route("api/values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> GetWithRouteParameter(Base64Guid id)
        {
            return id.Guid.ToString();
        }

        [HttpGet]
        public ActionResult<string> GetWithQueryParameter(Base64Guid id)
        {
            return id.Guid.ToString();
        }
    }
}
