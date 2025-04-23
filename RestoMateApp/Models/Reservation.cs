using RestoMate.Models.User;

namespace RestoMate.Models;

public class Reservation
{
    public int Id { get; set; }
        
    public string CustomerName { get; set; }
        
    public string CustomerPhone { get; set; }
        
    public int NumOfPeople { get; set; }
        
    public DateTime ReservationDate { get; set; }
        
    public int TableId { get; set; }
        
    public Table Table { get; set; }
        
    public TimeSpan ReservationTime { get; set; }
        
    public string SpecialRequests { get; set; }
        
    public string Status { get; set; } // Confirmed, Cancelled, Completed
        
    public int? CustomerId { get; set; } // Nullable for walk-in reservations
        
    public Customer Customer { get; set; }
}
