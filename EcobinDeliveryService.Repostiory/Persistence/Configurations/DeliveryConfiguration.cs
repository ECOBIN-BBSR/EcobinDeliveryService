using System;
using EcobinDeliveryService.Repostiory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcobinDeliveryService.Repostiory.Persistence.Configurations;

public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
{
    public void Configure(EntityTypeBuilder<Delivery> builder)
    {
        builder.ToTable("deliveries");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.ProductId)
            .IsRequired();

        builder.Property(d => d.SellerId)
            .IsRequired();

        builder.Property(d => d.CollectorId)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.PickupAddress)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(d => d.Notes)
            .HasMaxLength(1000);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .IsRequired();

        builder.HasIndex(d => d.SellerId);

        builder.HasIndex(d => d.CollectorId);

        builder.HasIndex(d => d.ProductId);

        builder.HasOne(d => d.SelectedSchedule)
            .WithMany()
            .HasForeignKey(d => d.SelectedScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.ScheduleOptions)
            .WithOne(s => s.Delivery)
            .HasForeignKey(s => s.DeliveryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.StatusHistories)
            .WithOne(h => h.Delivery)
            .HasForeignKey(h => h.DeliveryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}