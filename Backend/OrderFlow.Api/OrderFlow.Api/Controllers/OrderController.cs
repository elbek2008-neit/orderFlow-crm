using CrmOrderManagement.Core.Dtos;
using CrmOrderManagement.Core.Enums;
using CrmOrderManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace OrderFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> Get(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet("by-number/{orderNumber}")]
        public async Task<ActionResult<OrderDto>> GetByNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPaged(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] int? clientId = null, [FromQuery] OrderStatus? status = null)
        {
            var (orders, totalCount) = await _orderService.GetOrdersPagedAsync(page, pageSize, clientId, status);
            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
        {
            var order = await _orderService.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] UpdateOrderDto dto)
        {
            var order = await _orderService.UpdateOrderAsync(id, dto);
            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{orderId}/add-product")]
        public async Task<ActionResult> AddProduct(int orderId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            var result = await _orderService.AddProductToOrderAsync(orderId, productId, quantity);
            if (!result) return BadRequest();
            return Ok();
        }

        [HttpPost("{orderId}/remove-product")]
        public async Task<ActionResult> RemoveProduct(int orderId, [FromQuery] int productId)
        {
            var result = await _orderService.RemoveProductFromOrderAsync(orderId, productId);
            if (!result) return BadRequest();
            return Ok();
        }

        [HttpPost("{orderId}/status")]
        public async Task<ActionResult> UpdateStatus(int orderId, [FromQuery] OrderStatus status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (!result) return BadRequest();
            return Ok();
        }

        [HttpGet("{orderId}/total")]
        public async Task<ActionResult<decimal>> GetTotal(int orderId)
        {
            var total = await _orderService.CalculateOrderTotalAsync(orderId);
            return Ok(total);
        }
    }
}
