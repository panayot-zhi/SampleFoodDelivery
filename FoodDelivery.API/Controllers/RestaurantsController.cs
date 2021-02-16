using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.API.Database;
using FoodDelivery.API.Models;
using FoodDelivery.API.Models.Restaurants;
using Microsoft.AspNetCore.Authorization;

namespace FoodDelivery.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly FoodDeliveryDBContext _context;

        public RestaurantsController(FoodDeliveryDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// List all restaurants.
        /// </summary>
        /// <returns>All restaurants in the database.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            return await _context.Restaurants.Select(x =>
                new Restaurant()
                {
                    Id = x.Id,
                    Address = x.Address,
                    Name = x.Name

                }).ToListAsync();
        }

        /// <summary>
        /// Query a restaurant by it's id to return full information about it's menu.
        /// </summary>
        /// <param name="id">The id of the restaurant.</param>
        /// <returns>Not found, if the restaurant with the provided id doesn't exist, or a restaurant with it's menu.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }

        /// <summary>
        /// Modifies a restaurant. TODO: This should be restricted to certain users.
        /// </summary>
        /// <returns>404 NotFound, if no restaurant, or 204 NoContent if modified successfully.</returns>
        [HttpPut("{id}")]
        //[Authorize(Policy = "RestaurantAdministrator")]
        public async Task<IActionResult> PutRestaurant(int id, RestaurantEdit input)
        {
            if (id != input.Id)
            {
                return BadRequest();
            }

            var restaurant = await _context.Restaurants.FindAsync(input.Id);

            restaurant.Name = input.Name;
            restaurant.Address = input.Address;

            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
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

        /// <summary>
        /// Create a restaurant. TODO: This should be restricted to certain users.
        /// </summary>
        /// <returns>What would a GET typically return for this restaurant.</returns>
        [HttpPost]
        //[Authorize(Policy = "RestaurantAdministrator")]
        public async Task<ActionResult<Restaurant>> PostRestaurant(RestaurantCreate input)
        {

            var restaurant = _context.Restaurants.Add(new Restaurant()
            {
                Name = input.Name,
                Address = input.Address
            });

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestaurant", new { id = restaurant.Entity.Id }, restaurant.Entity);
        }

        /// <summary>
        /// Delete a restaurant by it's id. TODO: This should be restricted to certain users.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[Authorize(Policy = "RestaurantAdministrator")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
