using System.ComponentModel.DataAnnotations;

namespace RestoMate.Models.User;

public class Staff : UserBase
{
    [Required]
    public string Position { get; set; }
        
    public string ShiftHours { get; set; }
        
    public override string GetGreeting() => $"Hello {Position} {Username}";
}