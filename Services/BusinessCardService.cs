using BizCardApp.Data;
using BizCardApp.Interfaces;
using BizCardApp.Models;
using BizCardApp.Resources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BizCardApp.Services;

public sealed class BusinessCardService(IDbContextFactory<AppDbContext> factory, IDialogService dialogService) : IBusinessCardService
{
    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);
        return await appDbContext.Database.CanConnectAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BusinessCard>> GetAllBusinessCardsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);

            return await appDbContext.BusinessCards
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch
        {
            await dialogService.ShowMessageAsync(AppStrings.Dialogs.UnableToConnectDatabase, Enums.DialogType.Error);
            Process.GetCurrentProcess().Kill();
            return [];
        }
    }

    public async Task<BusinessCard?> SaveBusinessCardAsync(BusinessCard businessCard, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var appDbContext = await factory.CreateDbContextAsync(cancellationToken);

            if (businessCard.Id == 0)
            {
                var entityEntry = await appDbContext.BusinessCards.AddAsync(businessCard, cancellationToken);

                await appDbContext.SaveChangesAsync(cancellationToken);

                return entityEntry.Entity;
            }
            else
            {
                var existing = await appDbContext.BusinessCards
                    .FirstOrDefaultAsync(card => card.Id == businessCard.Id, cancellationToken);

                if (existing is null)
                    return null;

                existing.FirstName = businessCard.FirstName;
                existing.LastName = businessCard.LastName;
                existing.Company = businessCard.Company;
                existing.JobTitle = businessCard.JobTitle;
                existing.Phone = businessCard.Phone;
                existing.Email = businessCard.Email;
                existing.Address = businessCard.Address;

                await appDbContext.SaveChangesAsync(cancellationToken);

                return existing;
            }
        }
        catch
        {
            await dialogService.ShowMessageAsync(AppStrings.Dialogs.UnableToConnectDatabase, Enums.DialogType.Error);
            Process.GetCurrentProcess().Kill();
            return null;
        }
    }

    public async Task<bool> DeleteBusinessCardAsync(int id, CancellationToken cancellationToken = default)
    {
        try
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
        catch
        {
            await dialogService.ShowMessageAsync(AppStrings.Dialogs.UnableToConnectDatabase, Enums.DialogType.Error);
            Process.GetCurrentProcess().Kill();
            return false;
        }
    }
}