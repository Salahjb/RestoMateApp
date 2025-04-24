using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestoMateApp.ViewModels;

namespace RestoMateApp.Views;

public partial class RegisterPage : ContentPage
{
    // Add a parameterless constructor required for MAUI
    public RegisterPage()
    {
        InitializeComponent();
        // Get the ViewModel from the service provider
        BindingContext = Application.Current.Handler.MauiContext.Services.GetService<RegisterViewModel>();
    }
    
    // Keep the existing constructor for direct instantiation
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel; // Set the BindingContext
    }

    // Optional: Clear fields when appearing
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is RegisterViewModel vm)
        {
            // Optionally clear sensitive fields or errors
            vm.Password = string.Empty;
            vm.ConfirmPassword = string.Empty;
            vm.ErrorMessage = string.Empty;
        }
    }
}