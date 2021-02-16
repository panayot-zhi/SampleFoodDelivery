using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoodDelivery.API.Models.Foods;

namespace FoodDelivery.API.Models.Restaurants
{
    public class Restaurant
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public ICollection<FoodItem> Menu { get; set; }
    }
}
