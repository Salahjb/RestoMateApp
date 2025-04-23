using Microsoft.EntityFrameworkCore;
using RestoMate.Models;
using RestoMate.Models.User;

namespace RestoMate.Data;

public class RestaurantDbContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Staff> StaffMembers { get; set; }
    
    public DbSet<Dish> MenuItems { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string databasePath = Path.Combine(FileSystem.AppDataDirectory, "restaurant.db");   
        optionsBuilder.UseSqlite($"Data Source={databasePath}");
    }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships and constraints
        
        // User Inheritance (TPH - Table Per Hierarchy)
        modelBuilder.Entity<UserBase>()
            .HasDiscriminator<string>("Usertype")
            .HasValue<Customer>("Customer")
            .HasValue<Staff>("Admin") ; 
        
        // Order relationships
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.OrderHistory)
            .HasForeignKey(o => o.CustomerId);
        
        // Seed initial data
        modelBuilder.Entity<Staff>().HasData(
            new Staff
            {
                Id = 1,
                Username = "admin",
                passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                EmployeeId = "ADM001",
                Role = StaffRole.Admin
            }
        );
    } 
    
} 