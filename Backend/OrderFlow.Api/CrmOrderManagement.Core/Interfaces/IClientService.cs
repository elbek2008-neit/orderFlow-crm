using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Core.Entities;

namespace CrmOrderManagement.Core.Interfaces
{
    public interface IClientService
    {
        Task<Client?> GetClientByIdAsync(int id);
        Task<Client?> GetClientByCompanyNameAsync(string companyName);
        Task<Client> CreateClientAsync(Client client);
        Task<Client> UpdateClientAsync(int id, Client client);
        Task<bool> DeleteClientByIdtAsync(int id);
        Task<IEnumerable<Client>> GetAllClientsAsync();
    }
}
