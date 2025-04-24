using System.ComponentModel.DataAnnotations;

namespace RestoMate.Models;

public class Dish
{
    public int Id { get; set; }
        
    [Required]
    public string Name { get; set; }
        
    public string Description { get; set; }
        
    [Required]
    public double Price { get; set; }
        
    public string Category { get; set; } // Appetizer, Main Course, Dessert, etc.
        
    public bool IsAvailable { get; set; } = true;
        
    public string ImagePath { get; set; }
        
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}