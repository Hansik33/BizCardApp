using BizCardApp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BizCardApp.Interfaces;

public interface IBusinessCardService
{
    Task<bool> CanConnectAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<BusinessCard>> GetAllBusinessCardsAsync(CancellationToken cancellationToken = default);
    Task<BusinessCard?> SaveBusinessCardAsync(BusinessCard businessCard, CancellationToken cancellationToken = default);
    Task<bool> DeleteBusinessCardAsync(int id, CancellationToken cancellationToken = default);
}
