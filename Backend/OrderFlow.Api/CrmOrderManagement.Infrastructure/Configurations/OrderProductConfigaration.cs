using CrmOrderManagement.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("OrderProducts");

        // Составной ключ
        builder.HasKey(op => new { op.OrderId, op.ProductId });

        builder.Property(op => op.Quantity)
            .IsRequired();

        builder.Property(op => op.UnitPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        // Computed column для TotalPrice (только для чтения)
        builder.Property(op => op.TotalPrice)
            .HasComputedColumnSql("[Quantity] * [UnitPrice]", stored: true);

        // Связи
        builder.HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        builder.HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId);
    }
}
