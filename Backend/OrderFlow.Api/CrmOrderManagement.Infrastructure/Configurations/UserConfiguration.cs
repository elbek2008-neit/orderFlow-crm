using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CrmOrderManagement.Core.Entities;

namespace CrmOrderManagement.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users"); // Указывает, что сущность будет сопоставлена с таблицей "Users" в базе данных

            builder.HasKey(x => x.Id); //Указываем ключ

            builder.Property(u => u.Id) 
                .ValueGeneratedNever(); // Значение должно быть явно установленно перед сохранением 

            builder.Property(u => u.UserName) // Начинает настройку свойства UserName
                .IsRequired()        // Колонка не может быть Null
                .HasMaxLength(100);  // Ограничение длины строки 

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.FirstName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .HasMaxLength(100);

            builder.Property(u => u.CreatedAt)
                .HasDefaultValue(DateTime.UtcNow);

            // Связи many-to-many с ролями
            builder.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            // Связь один-ко-многим с заказами
            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);
        }
    }
}
