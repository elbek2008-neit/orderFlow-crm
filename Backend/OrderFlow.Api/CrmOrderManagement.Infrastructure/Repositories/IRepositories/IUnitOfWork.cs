using CrmOrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Infrastructure.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Client> Clients { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderProduct> OrderProducts { get; }
        IRepository<Warehause> Warehauses { get; }
        IRepository<WarehauseProduct> WarehauseProducts { get; }
        IRepository<Auditlog> Auditlogs { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task<int> ExecuteTransactionAsync(Func<Task<int>> operation);
    }
}
