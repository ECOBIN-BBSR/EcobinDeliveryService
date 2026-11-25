using System;
using EcobinDeliveryService.Repostiory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcobinDeliveryService.Repostiory.Persistence.Configurations;

public class DeliveryStatusOptionConfiguration : IEntityTypeConfiguration<DeliveryStatusHistory>
{
    public void Configure(EntityTypeBuilder<DeliveryStatusHistory> builder)
    {
        builder.ToTable("delivery_status_histories");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.PreviousStatus)
            .HasConversion<int?>();

        builder.Property(h => h.NewStatus)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(h => h.ChangedByUserId)
            .IsRequired();

        builder.Property(h => h.ChangedAt)
            .IsRequired();

        builder.Property(h => h.Remarks)
            .HasMaxLength(1000);

        builder.HasIndex(h => h.DeliveryId);
    }
}
