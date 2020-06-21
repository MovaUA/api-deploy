using System.ComponentModel.DataAnnotations;

namespace Api.Endpoints
{
	public class CreateApplicationRequest
	{
		[Required]
		[StringLength(10)]
		public string Name { get; set; }
	}
}
