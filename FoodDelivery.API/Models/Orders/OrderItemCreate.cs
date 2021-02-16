using FoodDelivery.API.Models.Foods;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.Models
{
    public class OrderItemCreate
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int ItemOrderedId { get; set; }
    }
}
