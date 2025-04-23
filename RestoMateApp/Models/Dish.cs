using System.ComponentModel.DataAnnotations;

namespace RestoMate.Models;

public class Dish
{
    [Required]
    public int Id { get; set;  }  
    
    public string Name { get; set; }  
    
    public string Description { get; set; }  
    
    public string Image { get; set; } 
    
    public int Price { get; set; } 
    
    public string  Category { get; set; }

    public bool isAvailable { get; set; } = true; 
}