using BizCardApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizCardApp.Data.Configuration;

public class BusinessCardConfiguration : IEntityTypeConfiguration<BusinessCard>
{
    public void Configure(EntityTypeBuilder<BusinessCard> builder)
    {
        builder.ToTable("business_cards");

        builder.HasKey(card => card.Id);

        builder.Property(card => card.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(card => card.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(card => card.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(card => card.Company)
            .HasColumnName("company")
            .HasMaxLength(150);

        builder.Property(card => card.JobTitle)
            .HasColumnName("job_title")
            .HasMaxLength(150);

        builder.Property(card => card.Phone)
            .HasColumnName("phone")
            .HasMaxLength(50);

        builder.Property(card => card.Email)
            .HasColumnName("email")
            .HasMaxLength(150);

        builder.Property(card => card.Address)
            .HasColumnName("address")
            .HasMaxLength(255);

        builder.Property(card => card.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();
    }
}