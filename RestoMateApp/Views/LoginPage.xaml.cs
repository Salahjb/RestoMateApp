using RestoMateApp.ViewModels;
namespace RestoMateApp.Views; 

public partial class LoginPage : ContentPage
{
    // Add a parameterless constructor required for MAUI
    public LoginPage()
    {
        InitializeComponent();
        // Get the ViewModel from the service provider
        BindingContext = Application.Current.Handler.MauiContext.Services.GetService<LoginViewModel>();
    }
    
    // Keep the existing constructor for direct instantiation
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Set the page's BindingContext to the ViewModel instance
    }

    // Optional: Clear password field when the page appears/reappears
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is LoginViewModel vm)
        {
            vm.Password = string.Empty; // Clear password for security/UX
            // vm.ErrorMessage = string.Empty; // Optionally clear previous errors
        }
    }
}