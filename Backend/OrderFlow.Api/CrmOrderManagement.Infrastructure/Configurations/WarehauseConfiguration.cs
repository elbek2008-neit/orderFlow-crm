using CrmOrderManagement.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehause>
{
    public void Configure(EntityTypeBuilder<Warehause> builder)
    {
        builder.ToTable("Warehouses");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd();

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(w => w.Address)
            .HasMaxLength(500);

        builder.Property(w => w.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Связи many-to-many с продуктами
        builder.HasMany(w => w.WarehauseProducts)
            .WithOne(wp => wp.Warehause)
        .HasForeignKey(wp => wp.WarehauseId);

        // Seed данные для складов
        builder.HasData(
        new Warehause { Id = 1, Name = "Main Warehouse", Address = "123 Main St, City", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Warehause { Id = 2, Name = "Secondary Warehouse", Address = "456 Second St, City", IsActive = true, CreatedAt = DateTime.UtcNow }
        );
    }
}
