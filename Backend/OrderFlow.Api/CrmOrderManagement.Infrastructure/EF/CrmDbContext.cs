using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CrmOrderManagement.Core.Entities;

namespace CrmOrderManagement.Infrastructure.EF
{
    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrdersProduct { get; set; }
        public DbSet<Warehause> Warehauses { get; set; }
        public DbSet<WarehauseProduct> WarehauseProducts { get; set; }
        public DbSet<Auditlog> Auditlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Применяем все конфигурации из текушей сборки
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Настройка составных ключей для связующих таблиц
            ConfigureCompositeKeys(modelBuilder);

            // Настройка индексов
            ConfigureIndexes(modelBuilder);

            // Настройка каскадных удалений
            ConfigureCascadeDeletes(modelBuilder);
        }

        private void ConfigureCompositeKeys(ModelBuilder modelBuilder)
        {
            // Составной ключ для UserRole 
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            // Составной ключ для OrderProduct
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            // Составной ключ для WareHouseProduct
            modelBuilder.Entity<WarehauseProduct>()
                .HasKey(wp => new { wp.WarehauseId, wp.ProductId });
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            //Индексы для User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique()
                .HasDatabaseName("IX_Users_UserName");

            // Индексы для Product SKU
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique()
                .HasDatabaseName("IX_Products_SKU");

            // Индексы для Order номер
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique()
                .HasDatabaseName("IX_Orders_OrderNumber");

            // Индексы для поиска заказов по клиенту
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.ClientId)
                .IsUnique()
                .HasDatabaseName("IX_Orders_ClientId");

            // Индекс для поиска заказов по менеджеру
            modelBuilder.Entity<Order>()
                .HasIndex(o => o.UserId)
                .IsUnique()
                .HasDatabaseName("IX_Order_UserId");

            // Индексы для AuditLog
            modelBuilder.Entity<Auditlog>()
                .HasIndex(a => new { a.EntityName, a.EntityId })
                .IsUnique()
                .HasDatabaseName("IX_AuditLogs_Entity");

            // Составной индекс для поисков товаров на складе
            modelBuilder.Entity<WarehauseProduct>()
                .HasIndex(wp => new { wp.WarehauseId, wp.ProductId });
        }

        private void ConfigureCascadeDeletes(ModelBuilder modelBuilder)
        {
            // Отключаем каскадное удаление для некоторых связей
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Client)
                .WithMany(c => c.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            // AuditLog не должен удаляться при удалении пользователя
            modelBuilder.Entity<Auditlog>()
                .HasOne(a => a.User )
                .WithMany(u => u.AuditLogs)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
        { 
            //Аытоматическое обновление новостей Updatedat для всех сущностей
            var entries = ChangeTracker.Entries()
                .Where(entries => entries.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity.GetType().GetProperty("UpdatedAt") != null)
                {
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
