namespace RestoMate.Models;

public class Table
{
    public int Id { get; set; }
        
    public int TableNumber { get; set; }
        
    public int Capacity { get; set; }
        
    public string Status { get; set; } // Available, Occupied, Reserved
        
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        
    public List<Order> Orders { get; set; } = new List<Order>();
}