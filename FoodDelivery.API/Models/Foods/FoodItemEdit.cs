using System.ComponentModel.DataAnnotations;
using FoodDelivery.API.Models.Restaurants;

namespace FoodDelivery.API.Models.Foods
{
    public class FoodItemEdit
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int? Quantity { get; set; }
        
        public decimal? Price { get; set; }
    }
}
