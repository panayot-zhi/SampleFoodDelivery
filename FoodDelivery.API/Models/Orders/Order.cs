using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.Models
{
    public class Order
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ShipTo { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public ICollection<OrderItem> OrderedItems { get; set; }
    }
}
