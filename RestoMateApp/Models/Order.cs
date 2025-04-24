using RestoMate.Models;
using RestoMate.Models.User;

public class Order
{
    public int Id { get; set; }

    public int TableId { get; set; }
        
    public Table Table { get; set; }
        
    public string Status { get; set; } // Pending, Preparing, Served, Paid
        
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        
    public double TotalAmount => Items.Sum(item => item.Subtotal);
        
    public int? CustomerId { get; set; }
        
    public Customer Customer { get; set; }
        
    public DateTime OrderDate { get; set; } = DateTime.Now;
        
    public DateTime? CompletedDate { get; set; }
        
    public string PaymentMethod { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
        
    public int OrderId { get; set; }
        
    public Order Order { get; set; }
        
    public int DishId { get; set; }
        
    public Dish Dish { get; set; }
        
    public string DishName { get; set; }
        
    public int Quantity { get; set; }
        
    public double UnitPrice { get; set; }
        
    public double Subtotal => Quantity * UnitPrice;
        
    public string SpecialInstructions { get; set; }
        
    public string Status { get; set; } // Ordered, Preparing, Ready, Served
}