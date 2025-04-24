using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;      // Already here
using Npgsql.EntityFrameworkCore.PostgreSQL; // <-- Add Npgsql provider namespace
using RestoMateApp.Data;                  // Your DbContext namespace
//using RestoMateApp.Services;              // Your Services namespace

namespace RestoMateApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // --- Database & Service Configuration ---

        // IMPORTANT: Use the SAME connection details as any potential DbContextFactory
        // Ensure the server IP (e.g., 192.168.122.1) is reachable from your Android emulator/device
        // For emulators, 'localhost' or '127.0.0.1' often refers to the emulator itself.
        // You might need to use your machine's actual network IP or a special IP like '10.0.2.2' for the default Android emulator to reach your host machine.
        const string host = "192.168.122.1"; // YOUR ACTUAL POSTGRESQL SERVER IP (reachable from device/emulator)
        const string port = "5432";          // Default PostgreSQL port
        const string database = "resto";     // YOUR DB NAME
        const string username = "postgres"; // YOUR POSTGRESQL USER (often 'postgres' initially, but create a specific app user)
        //const string password = "your_postgres_password"; // YOUR POSTGRESQL PASSWORD (Consider safer storage!)

        // Construct the PostgreSQL connection string
        // Note the keyword differences: Host, Port, Database, Username, Password
        string connectionString = $"Host={host};Port={port};Database={database};Username={username};";

        // Register DbContext for PostgreSQL using Npgsql provider
        builder.Services.AddDbContext<RestoMateDbContext>(options =>
            options.UseNpgsql(connectionString) // <-- Use UseNpgsql instead of UseMySql
                   // Optional: Add logging in Debug mode to see EF Core interactions
                   // #if DEBUG
                   // .LogTo(Console.WriteLine, LogLevel.Information)
                   // .EnableSensitiveDataLogging() // Be careful with sensitive data logging
                   // .EnableDetailedErrors()
                   // #endif
        );


        // Register your application services that use the DbContext
        // Lifetimes remain the same unless you have specific reasons to change them
        //builder.Services.AddTransient<AuthService>();
        //builder.Services.AddTransient<DishService>();
        //builder.Services.AddTransient<OrderService>();
        //builder.Services.AddTransient<ReservationService>();
        //builder.Services.AddTransient<TableServices>();

        // --- End Database & Service Configuration ---


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}