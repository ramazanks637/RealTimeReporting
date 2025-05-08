using Microsoft.AspNetCore.Mvc;
using RealTimeReporting.Application.Services;
using RealTimeReporting.Domain.Models;

namespace RealTimeReporting.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {
            if (order == null)
                return BadRequest();

            await _orderService.AddOrderAsync(order);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Order order)
        {
            await _orderService.UpdateOrderAsync(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok();
        }
    }
}
