namespace RestoMate.Models;

public class Reservation
{
    public string customerName { get; set; }
    
    public string customerPhone { get; set; }
    
    public int numOfPeople { get; set; }
    
    public DateTime ReservationDate { get; set; }
    
    public int TableNumber { get; set; }

    public TimeSpan ReservationTime { get; set; }
    
    public string SpecialRequests { get; set; }
    
    public string Status { get; set; } // Confirmed, Cancelled, Completed
    
}