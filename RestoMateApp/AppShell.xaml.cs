using RestoMateApp.Views;

namespace RestoMateApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for pages you navigate to using GoToAsync("RouteName")
        // LoginPage is defined in XAML, so it's registered implicitly as the root

        // Register the RegisterPage explicitly
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));

        // Dashboard routes are defined in XAML with ShellContent, so '//RouteName' navigation
        // like '//CustomerDashboard' should work. Explicit registration can be a fallback if needed.
        // Routing.RegisterRoute("CustomerDashboard", typeof(CustomerDashboardPage));
        // Routing.RegisterRoute("StaffDashboard", typeof(StaffDashboardPage));
        // Routing.RegisterRoute("AdminDashboard", typeof(AdminDashboardPage));

        // Add routes for any other pages you need to navigate to programmatically
    }
}