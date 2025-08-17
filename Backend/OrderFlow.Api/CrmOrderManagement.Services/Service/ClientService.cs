using AutoMapper;
using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Interfaces;
using CrmOrderManagement.Infrastructure.EF;
using CrmOrderManagement.Infrastructure.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using CrmOrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmOrderManagement.Services.Service
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CrmDbContext _dbContext;

        public ClientService(IUnitOfWork unitOfWork, IMapper mapper, CrmDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<Client?> GetClientByIdAsync(int id) 
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            return client;
        }

        public async Task<Client?> GetClientByCompanyNameAsync(string companyName) 
        { 
            var client =  await _dbContext.Clients.FirstOrDefaultAsync(c => c.CompanyName == companyName);
            return client;
        }

        public async Task<Client> CreateClientAsync(Client client) 
        {
            _unitOfWork.Clients.Add(client);
            await _unitOfWork.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClientAsync(int id, Client updateClientDto) 
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) throw new Exception("User not found");

            _unitOfWork.Clients.Update(client);
            await _unitOfWork.SaveChangesAsync();

            return client;
        }

        public async Task<bool> DeleteClientByIdtAsync(int id) 
        {
            var client = _unitOfWork.Clients.GetById(id);
            if (client == null) return false;

            _unitOfWork.Clients.Remove(client);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync() 
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();
            return clients;
        }

    }
}
