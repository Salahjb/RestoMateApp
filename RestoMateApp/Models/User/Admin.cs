namespace RestoMate.Models.User;

public class Admin : UserBase
{
    public override string GetGreeting() => $"Welcome Administrator {Username}";
}