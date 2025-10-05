using System.Threading;
using System.Threading.Tasks;

namespace BizCardApp.Interfaces;

public interface IBusinessCardService
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);
}