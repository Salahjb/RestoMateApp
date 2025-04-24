using Microsoft.EntityFrameworkCore;
using RestoMate.Models;
using RestoMate.Models.User;

namespace RestoMateApp.Data
{
    public class RestoMateDbContext : DbContext
    {
        public RestoMateDbContext(DbContextOptions<RestoMateDbContext> options) : base(options)
        {
        }
        
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Table> Tables { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure relationships
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);
                
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Dish)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(oi => oi.DishId);
                
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);
                
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Table)
                .WithMany(t => t.Orders)
                .HasForeignKey(o => o.TableId);
                
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CustomerId);
                
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Table)
                .WithMany(t => t.Reservations)
                .HasForeignKey(r => r.TableId);
                
            // Seed initial data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Add admin user
            modelBuilder.Entity<Admin>().HasData(
                new Admin 
                { 
                    Id = 1, 
                    Username = "admin", 
                    passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    PhoneNum = "123-456-7890",
                    DateCreated = DateTime.UtcNow
                }
            );
            
            // Add some dishes
            modelBuilder.Entity<Dish>().HasData(
                new Dish 
                { 
                    Id = 1, 
                    Name = "Classic Burger", 
                    Description = "Beef patty with lettuce, tomato, and cheese", 
                    Price = 9.99, 
                    Category = "Main Course",
                    IsAvailable = true,
                    ImagePath = "dotnet_bot.png"
                },
                new Dish 
                { 
                    Id = 2, 
                    Name = "Caesar Salad", 
                    Description = "Romaine lettuce with Caesar dressing and croutons", 
                    Price = 7.99, 
                    Category = "Starter",
                    IsAvailable = true,
                    ImagePath = "dotnet_bot.png"
                },
                new Dish 
                { 
                    Id = 3, 
                    Name = "Chocolate Cake", 
                    Description = "Rich chocolate cake with ganache", 
                    Price = 5.99, 
                    Category = "Dessert",
                    IsAvailable = true,
                    ImagePath = "dotnet_bot.png" 
                }
            );
            
            // Add some tables
            modelBuilder.Entity<Table>().HasData(
                new Table { Id = 1, TableNumber = 1, Capacity = 2, Status = "Available" },
                new Table { Id = 2, TableNumber = 2, Capacity = 2, Status = "Available" },
                new Table { Id = 3, TableNumber = 3, Capacity = 4, Status = "Available" },
                new Table { Id = 4, TableNumber = 4, Capacity = 4, Status = "Available" },
                new Table { Id = 5, TableNumber = 5, Capacity = 6, Status = "Available" },
                new Table { Id = 6, TableNumber = 6, Capacity = 8, Status = "Available" }
            );
        }
    }
}