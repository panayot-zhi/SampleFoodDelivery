using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.API.Models
{
    public class OrderCreate
    {
        [Required]
        public string ShipTo { get; set; }        

        [Required]
        public ICollection<OrderItemCreate> OrderedItems { get; set; }
    }
}
