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
  public class UserController : ControllerBase
  {
    private readonly IMongoCollection<UserDocument> users;

    private static BsonDocument All { get; }

    static UserController()
    {
      All = new BsonDocument();
    }

    public UserController(IMongoCollection<UserDocument> users)
    {
      this.users = users;
    }

    [HttpGet]
    [Route("v1/users")]
    public async Task<IEnumerable<UserResponse>> Get()
    {
      var users = await this.users.Find(All).ToListAsync().ConfigureAwait(false);
      return users.Select(ToResponse);
    }

    [HttpGet]
    [Route("v1/users/{id:length(32)}")]
    public async Task<IActionResult> Get(
      [FromRoute(Name = "id")] string id
    )
    {
      var user =
        await this.users.Find(x => x.Id == id)
              .FirstOrDefaultAsync()
              .ConfigureAwait(false);

      if (user == null)
        return NotFound();

      return Ok(ToResponse(user));
    }

    [HttpPost]
    [Route("v1/users")]
    public async Task<UserResponse> Post([FromBody] CreateUserRequest request)
    {
      var user = new UserDocument
      {
        Id = Guid.NewGuid().ToString("N"),
        Name = request.Name
      };

      await this.users.InsertOneAsync(user);

      return ToResponse(user);
    }

    private static UserResponse ToResponse(UserDocument user)
    {
      return
        new UserResponse
        {
          Id = user.Id,
          Name = user.Name,
          Version = Environment.GetEnvironmentVariable("APP_VERSION"),
          Host = Environment.MachineName
        };
    }
  }
}
