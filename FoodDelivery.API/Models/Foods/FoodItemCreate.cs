using System.ComponentModel.DataAnnotations;
using FoodDelivery.API.Models.Restaurants;

namespace FoodDelivery.API.Models.Foods
{
    public class FoodItemCreate
    {
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int RestaurantId { get; set; }
    }
}
