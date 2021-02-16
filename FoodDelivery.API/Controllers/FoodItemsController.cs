using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.API.Database;
using FoodDelivery.API.Models;
using FoodDelivery.API.Models.Foods;
using FoodDelivery.API.Models.Restaurants;

namespace FoodDelivery.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodItemsController : ControllerBase
    {
        private readonly FoodDeliveryDBContext _context;

        public FoodItemsController(FoodDeliveryDBContext context)
        {
            _context = context;
        }

        // GET: api/FoodItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodItem>> GetFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return foodItem;
        }

        // GET: api/FoodItems/5
        [HttpGet("ByRestaurant/{id}")]
        public async Task<ActionResult<IEnumerable<FoodItem>>> GetFoodItems(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null) {
                return NotFound();
            }

            var foodItems = _context.FoodItems
                .Where(x => x.Restaurant.Id == restaurant.Id)
                /*.Select(x => new FoodItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Restaurant = new Restaurant() {  Id = x.Restaurant.Id }
                })*/;

            return foodItems.ToList();
        }

        // PUT: api/FoodItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        //[Authorize(Policy = "RestaurantAdministrator")]
        public async Task<IActionResult> PutFoodItem(int id, FoodItemEdit input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            var foodItem = await _context.FoodItems.FindAsync(input.Id);
            if (foodItem == null) {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(input.Name))
            {
                foodItem.Name = input.Name;
            }

            if (input.Price.HasValue)
            {
                foodItem.Price = input.Price.Value;
            }

            if (input.Quantity.HasValue)
            {
                foodItem.Quantity = input.Quantity.Value;
            }

            _context.Entry(foodItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FoodItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        //[Authorize(Policy = "RestaurantAdministrator")]
        public async Task<ActionResult<FoodItem>> PostFoodItem(FoodItemCreate input)
        {

            var restaurant = await _context.Restaurants.FindAsync(input.RestaurantId);
            if (restaurant == null) {
                return BadRequest();
            }

            var foodItem = await _context.FoodItems.AddAsync(new FoodItem() { 
                 Name = input.Name,
                 Price = input.Price,
                 Quantity = input.Quantity,
                 Restaurant = restaurant
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodItem", new { id = foodItem.Entity.Id }, new { 
                Id = foodItem.Entity.Id,
                Name = input.Name,
                Price = input.Price,
                Quantity = input.Quantity,
                RestaurantId = restaurant.Id
            });
        }

        // DELETE: api/FoodItems/5
        [HttpDelete("{id}")]
        //[Authorize(Policy = "RestaurantAdministrator")]
        public async Task<ActionResult<FoodItem>> DeleteFoodItem(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(foodItem);
            await _context.SaveChangesAsync();

            return foodItem;
        }

        private bool FoodItemExists(int id)
        {
            return _context.FoodItems.Any(e => e.Id == id);
        }
    }
}
