using BizCardApp.Data;
using BizCardApp.Interfaces;
using BizCardApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

    public async Task<IReadOnlyList<BusinessCard>> GetAllBusinessCardsAsync(CancellationToken cancellationToken = default)
    {
        await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);

        return await appDbContext.BusinessCards
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<BusinessCard?> SaveBusinessCardAsync(BusinessCard businessCard, CancellationToken cancellationToken = default)
    {
        await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);

        var entityEntry = await appDbContext.BusinessCards.AddAsync(businessCard, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return entityEntry.Entity;
    }

    public async Task<bool> DeleteBusinessCardAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);

        var entity = await appDbContext.BusinessCards
            .FirstOrDefaultAsync(businessCard => businessCard.Id == id, cancellationToken);

        if (entity is null)
            return false;

        appDbContext.BusinessCards.Remove(entity);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}