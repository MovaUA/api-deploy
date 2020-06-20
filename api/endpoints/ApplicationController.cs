using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Api.Endpoints
{
  [ApiController]
  public class ApplicationController : ControllerBase
  {
    private readonly IMongoCollection<Application> applications;

    static BsonDocument All { get; }

    public ApplicationController(IMongoCollection<Application> applications)
    {
      this.applications = applications;
    }

    static ApplicationController()
    {
      All = new BsonDocument();
    }

    [HttpGet]
    [Route("v1/apps")]
    public Task<List<Application>> Get()
    {
      return this.applications.Find(All).ToListAsync();
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

      return Ok(application);
    }

    [HttpPost]
    [Route("v1/apps")]
    public async Task<Application> Post([FromBody] CreateApplicationRequest request)
    {
      var application = new Application
      {
        Id = Guid.NewGuid().ToString("N"),
        Name = request.Name
      };

      await this.applications.InsertOneAsync(application);

      return application;
    }
  }
}