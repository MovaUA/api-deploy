using System.Threading;
using System.Threading.Tasks;

namespace Init.Api
{
  public interface IScript
  {
    Task Apply(CancellationToken cancellationToken = default);
  }
}
