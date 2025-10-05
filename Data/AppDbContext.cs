using BizCardApp.Data.Configuration;
using BizCardApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BizCardApp.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BusinessCard> BusinessCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new BusinessCardConfiguration());
}