using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmOrderManagement.Core.Dtos;

namespace CrmOrderManagement.Core.Interfaces
{
    public interface IClientService
    {
        Task<ClientDto?> GetClientByIdAsync(int id);
        Task<ClientDto?> GetClientByCompanyNameAsync(string companyName);
        Task<ClientDto> CreateClientAsync(CreateClientDto createClientDto);
        Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto updateClientDto);
        Task<bool> DeleteClientByIdtAsync(int id);
        Task<IEnumerable<ClientDto>> GetAllClientsAsync();
    }
}
