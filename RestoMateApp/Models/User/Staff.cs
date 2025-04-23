using System.ComponentModel.DataAnnotations;

namespace RestoMate.Models.User;

public class Staff : UserBase
{
    // Staff identification
    [Required]
    public string EmployeeId { get; set; }
    
    [Required] public StaffRole Role { get; set; } = StaffRole.Admin;
    
    // Employment details
    public DateTime HireDate { get; set; }
    public decimal HourlyWage { get; set; }
    
    public override string GetGreeting() => $"Staff #{Id}: {Username} ({Role})";
}

public enum StaffRole
{
    Waiter,
    Chef,
    Supervisor,
    Manager,
    Admin
}