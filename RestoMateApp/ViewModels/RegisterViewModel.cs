using System.Windows.Input;
using RestoMate.Models.User;
using RestoMateApp.Services;

namespace RestoMateApp.ViewModels;

 public class RegisterViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        
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
        
        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }
        
        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set => SetProperty(ref _phoneNumber, value);
        }
        
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        
        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        
        public ICommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }
        
        public RegisterViewModel(IAuthService authService)
        {
            Title = "Register";
            _authService = authService;
            
            RegisterCommand = new Command(async () => await ExecuteRegisterCommand());
            CancelCommand = new Command(async () => await ExecuteCancelCommand());
        }
        
        private async Task ExecuteRegisterCommand()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || 
                string.IsNullOrWhiteSpace(ConfirmPassword) || string.IsNullOrWhiteSpace(PhoneNumber))
            {
                ErrorMessage = "Please fill in all required fields";
                return;
            }
            
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match";
                return;
            }
            
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;
                
                var customer = new Customer
                {
                    Username = Username,
                    PhoneNum = PhoneNumber,
                    Email = Email
                };
                
                await _authService.RegisterCustomerAsync(customer, Password);
                
                // Automatically login after registration
                await _authService.LoginAsync(Username, Password);
                
                // Navigate to dashboard
                await Shell.Current.GoToAsync("//CustomerDashboard");
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
        
        private async Task ExecuteCancelCommand()
        {
            await Shell.Current.GoToAsync("..");
        }
    }