using System;
using EcobinDeliveryService.Repostiory.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EcobinDeliveryService.Repostiory.Persistence.Configurations;

public class DeliveryScheduleOptionConfiguration : IEntityTypeConfiguration<DeliveryScheduleOption>
{
    public void Configure(EntityTypeBuilder<DeliveryScheduleOption> builder)
    {
        builder.ToTable("delivery_schedule_options");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.DeliveryId)
            .IsRequired();

        builder.Property(s => s.StartAt)
            .IsRequired();

        builder.Property(s => s.EndAt)
            .IsRequired();

        builder.Property(s => s.CreatedByUserId)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.HasIndex(s => s.DeliveryId);
    }
}
