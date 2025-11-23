using BizCardApp.Data;
using BizCardApp.Interfaces;
using BizCardApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BizCardApp.Services;

public sealed class BusinessCardService(IDbContextFactory<AppDbContext> factory) : IBusinessCardService
{
    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);
        return await appDbContext.Database.CanConnectAsync(cancellationToken);
    }

    public async Task<BusinessCard?> SaveBusinessCardAsync(BusinessCard businessCard, CancellationToken cancellationToken = default)
    {
        await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);

        var entityEntry = await appDbContext.BusinessCards.AddAsync(businessCard, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity;
    }
}