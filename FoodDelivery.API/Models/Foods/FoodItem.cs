using System.ComponentModel.DataAnnotations;
using FoodDelivery.API.Models.Restaurants;

namespace FoodDelivery.API.Models.Foods
{
    public class FoodItem
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Restaurant Restaurant { get; set; }
    }
}
