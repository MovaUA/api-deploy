using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints
{
	[ApiController]
	public class SettingsController : ControllerBase
	{
		private readonly IMongoDbSettings settings;

		public SettingsController(IMongoDbSettings settings)
		{
			this.settings = settings;
		}

		[HttpGet]
		[Route("v1/settings")]
		public IActionResult Get()
		{
			return Ok(this.settings);
		}
	}
}
