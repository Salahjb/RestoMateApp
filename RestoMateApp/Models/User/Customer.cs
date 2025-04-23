namespace RestoMate.Models.User;

public class Customer : UserBase
{
    // Customer-specific properties
    
    public string DeliveryAddress { get; set; }
    public int LoyaltyPoints { get; set; }
    
    // Navigation properties
    public List<Order> OrderHistory { get; set; } = new();
    public List<Reservation> Reservations { get; set; } = new();
    
    // Customer behavior
    public override string GetGreeting() => $"Welcome back, {Username}! You have {LoyaltyPoints} loyalty points";
}