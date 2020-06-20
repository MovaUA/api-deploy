using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
  [ApiController]
  public class SettingsController : ControllerBase
  {
    private readonly MongoDbSettings settings;

    public SettingsController(MongoDbSettings settings)
    {
      this.settings = settings;
    }

    [HttpGet]
    [Route("v1/settings")]
    public IActionResult Get() => Ok(this.settings);
  }
}