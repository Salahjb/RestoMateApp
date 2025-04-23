using RestoMate.Models.User;

namespace RestoMate.Models;

public class Order
{
    public int Id { get; set;  }

    public int TableNumber { get; set; }
    
    public string status { get; set; }
    
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    
    public decimal TotalAmount => Items.Sum(item => item.Subtotal);
    
    public int CustomerId { get; set; }       
    
    public DateTime Date { get; set; } 
    
    public Customer Customer { get; set; }
}

public class OrderItem
{
    public int DishId { get; set; }
    
    public string DishName { get; set; }
    
    public Dish Dish { get; set; }
    
    public int quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    public decimal Subtotal => quantity * UnitPrice;
    
}
