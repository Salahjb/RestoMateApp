using System.Windows.Input;
using RestoMate.Models.User;
using RestoMateApp.Services;
using RestoMateApp.Views;

namespace RestoMateApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _serviceProvider;
    
    private string _username;
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }
    
    private string _password;
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }
    
    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    
    public LoginViewModel(IAuthService authService, IServiceProvider serviceProvider)
    {
        Title = "Login";
        _authService = authService;
        _serviceProvider = serviceProvider;
        
        LoginCommand = new Command(async () => await ExecuteLoginCommand());
        RegisterCommand = new Command(async () => await NavigateToRegister());
    }
    
    private async Task ExecuteLoginCommand()
    {
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Username and password are required";
            return;
        }
        
        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;
            
            var user = await _authService.LoginAsync(Username, Password);
            
            // Navigate to appropriate page based on user type
            if (user is Customer)
            {
                await Shell.Current.GoToAsync("//CustomerDashboard");
            }
            else if (user is Staff)
            {
                await Shell.Current.GoToAsync("//StaffDashboard");
            }
            else if (user is Admin)
            {
                await Shell.Current.GoToAsync("//AdminDashboard");
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
    
    private async Task NavigateToRegister()
    {
        try
        {
            // Use the nameof operator to avoid string typos
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
        catch (Exception ex)
        {
            // For debugging purposes - this will display any navigation errors
            ErrorMessage = $"Navigation error: {ex.Message}";
            Console.WriteLine($"Navigation error: {ex}");
        }
    }
}