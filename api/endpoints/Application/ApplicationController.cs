using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Endpoints
{
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		private readonly IMongoCollection<ApplicationDocument> applications;

		private static BsonDocument All { get; }

		static ApplicationController()
		{
			All = new BsonDocument();
		}

		public ApplicationController(IMongoCollection<ApplicationDocument> applications)
		{
			this.applications = applications;
		}

		[HttpGet]
		[Route("v1/apps")]
		public async Task<IEnumerable<ApplicationResponse>> Get()
		{
			var applications = await this.applications.Find(All).ToListAsync().ConfigureAwait(false);
			return applications.Select(ToResponse);
		}

		[HttpGet]
		[Route("v1/apps/{id:length(32)}")]
		public async Task<IActionResult> Get(
			[FromRoute(Name = "id")] string id
		)
		{
			var application =
				await this.applications.Find(x => x.Id == id)
						  .FirstOrDefaultAsync()
						  .ConfigureAwait(false);

			if (application == null)
				return NotFound();

			return Ok(ToResponse(application));
		}

		[HttpPost]
		[Route("v1/apps")]
		public async Task<ApplicationResponse> Post([FromBody] CreateApplicationRequest request)
		{
			var application = new ApplicationDocument
							  {
								  Id = Guid.NewGuid().ToString("N"),
								  Name = request.Name
							  };

			await this.applications.InsertOneAsync(application);

			return ToResponse(application);
		}

		private static ApplicationResponse ToResponse(ApplicationDocument application)
		{
			return
				new ApplicationResponse
				{
					Id = application.Id,
					Name = application.Name,
					Version = Environment.GetEnvironmentVariable("APP_VERSION"),
					Host = Environment.MachineName
				};
		}
	}
}
