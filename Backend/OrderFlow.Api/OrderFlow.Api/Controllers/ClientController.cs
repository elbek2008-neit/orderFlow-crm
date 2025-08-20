using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Entities;
using CrmOrderManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpGet("name/{companyName}")]
        public async Task<IActionResult> GetByCompanyName(string companyName)
        { 
            var client = await _clientService.GetClientByCompanyNameAsync(companyName);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientDto dto)
        { 
            var client = await _clientService.CreateClientAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateClientDto dto)
        {
            var updated = await _clientService.UpdateClientAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        { 
            var result = await _clientService.DeleteClientByIdtAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        { 
            var clients = await _clientService.GetAllClientsAsync();
            return Ok(clients);
        }
    }
}
