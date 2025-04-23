namespace RestoMate.Models.User;

public class Customer : UserBase
{
    public string Email { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        
    public override string GetGreeting() => $"Welcome back, {Username}!";
}