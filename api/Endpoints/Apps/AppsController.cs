using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
  [ApiController]
  public class AppsController : ControllerBase
  {
    [HttpGet]
    [Route("v1/apps")]
    public IActionResult Get()
    {
      return Ok("apps v1");
    }
  }
}