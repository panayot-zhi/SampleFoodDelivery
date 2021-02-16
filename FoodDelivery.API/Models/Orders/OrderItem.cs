using FoodDelivery.API.Models.Foods;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.Models
{
    public class OrderItem
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public FoodItem ItemOrdered { get; set; }        

        public Order Order { get; set; }

        public int OrderId { get; set; }
    }
}
