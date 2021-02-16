using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodDelivery.API.Models;
using FoodDelivery.API.Models.Foods;
using FoodDelivery.API.Models.Restaurants;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.API.Database
{
    public class FoodDeliveryDBContext : DbContext
    {
        public DbSet<FoodItem> FoodItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Restaurant> Restaurants { get; set; }

        public FoodDeliveryDBContext(DbContextOptions<FoodDeliveryDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Use Fluent API to configure  

            // Map entities to tables  
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<FoodItem>().ToTable("FoodItems");
            modelBuilder.Entity<Restaurant>().ToTable("Restaurants");

            // Configure Keys  
            modelBuilder.Entity<Order>().HasKey(x => x.Id).HasName("PK_Orders");
            modelBuilder.Entity<OrderItem>().HasKey(x => x.Id).HasName("PK_OrderItems");
            modelBuilder.Entity<OrderItem>().HasOne(p => p.Order).WithMany(b => b.OrderedItems)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FoodItem>().HasKey(x => x.Id).HasName("PK_FoodItems");
            modelBuilder.Entity<Restaurant>().HasKey(x => x.Id).HasName("PK_Restaurant");

            // Configure indexes  
            modelBuilder.Entity<Restaurant>().HasIndex(x => x.Name).IsUnique();

            // Fine grain columns if need be
            modelBuilder.Entity<FoodItem>().Property(x => x.Price).HasColumnType("DECIMAL(18,2)");
            modelBuilder.Entity<OrderItem>().Property(x => x.Price).HasColumnType("DECIMAL(18,2)");

            // Configure relationships  

        }

    }
}
