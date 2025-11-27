using System.Threading.Tasks;

namespace BizCardApp.Interfaces;

public interface IAsyncInitializable
{
    Task InitializeAsync();
}