using System.ComponentModel.DataAnnotations;

namespace RestoMate.Models.User;

public abstract class UserBase
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required] public  string passwordHash { set; get; }

    [Phone] public  string PhoneNum { set; get; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public virtual string GetGreeting() => $"Hello, {Username}";
}