using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.API.Database;
using FoodDelivery.API.Models;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FoodDeliveryDBContext _context;

        public OrdersController(FoodDeliveryDBContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        /*// PUT: api/Orders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Orders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderCreate input)
        {
            int createdOrderId;

            try
            {
                using (var transaction = _context.Database.BeginTransaction()) {

                    var order = new Order()
                    {
                        CreatedAt = DateTime.Now,
                        ShipTo = input.ShipTo,
                        OrderedItems = new List<OrderItem>()
                    };
                    
                    var orderEntity = _context.Orders.Add(order);

                    _context.SaveChanges();

                    foreach (var orderItem in input.OrderedItems) {
                        var foodItem = _context.FoodItems.Find(orderItem.ItemOrderedId);
                        if (foodItem.Quantity - orderItem.Quantity < 0) {
                            throw new Exception($"Insufficient quantity for food item: {foodItem.Name}");
                        }
                        var orderItemEntity = new OrderItem() {
                            ItemOrdered = foodItem,
                            Price = orderItem.Price,
                            Quantity = orderItem.Quantity,
                        };

                        foodItem.Quantity -= orderItem.Quantity;

                        _context.OrderItems.Add(orderItemEntity);

                        order.OrderedItems.Add(orderItemEntity);
                    }

                    _context.SaveChanges();

                    createdOrderId = orderEntity.Entity.Id;

                    transaction.Commit();
                }

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                        

            return CreatedAtAction("GetOrder", new { id = createdOrderId }, input);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
