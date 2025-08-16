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

        public async Task<ClientDto?> GetClientByIdAsync(int id) 
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto?> GetClientByCompanyNameAsync(string companyName) 
        { 
            var client =  await _dbContext.Clients.FirstOrDefaultAsync(c => c.CompanyName == companyName);
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> CreateClientAsync(CreateClientDto createClientDto) 
        {
            var client = _mapper.Map<Client>(createClientDto);
            _unitOfWork.Clients.Add(client);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> UpdateClientAsync(int id, UpdateClientDto updateClientDto) 
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) throw new Exception("User not found");

            _mapper.Map(updateClientDto, client);
            _unitOfWork.Clients.Update(client);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<bool> DeleteClientByIdtAsync(int id) 
        {
            var client = _unitOfWork.Clients.GetById(id);
            if (client == null) return false;

            _unitOfWork.Clients.Remove(client);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ClientDto>> GetAllClientsAsync() 
        {
            var clients = await _unitOfWork.Clients.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

    }
}
