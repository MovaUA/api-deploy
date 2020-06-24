using System.ComponentModel.DataAnnotations;

namespace Api.Endpoints
{
  public class CreateUserRequest
  {
    [Required]
    [StringLength(10)]
    public string Name { get; set; }
  }
}
