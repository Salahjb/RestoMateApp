using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql.EntityFrameworkCore.PostgreSQL; 

namespace RestoMateApp.Data
{
    // Factory for EF Core Tools (Add-Migration, Update-Database) using PostgreSQL
    public class RestoMateDbContextFactory : IDesignTimeDbContextFactory<RestoMateDbContext>
    {
        public RestoMateDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RestoMateDbContext>();

            // --- PostgreSQL Configuration ---
            // IMPORTANT: Use the SAME connection details as MauiProgram.cs
            const string host = "192.168.122.1";       // YOUR POSTGRESQL SERVER IP
            const string port = "5432";                // Default PostgreSQL port
            const string database = "resto";           // YOUR DB NAME
            const string username = "postgres"; // YOUR POSTGRESQL USER
            //const string password = "your_postgres_password"; // YOUR POSTGRESQL PASSWORD

            // Construct the PostgreSQL connection string
            // Note the keyword differences: Host, Port, Database, Username, Password
            string connectionString = $"Host={host};Port={port};Database={database};Username={username};";

            // Log the connection string being used by the factory (mask password)
            Console.WriteLine($"[DesignTimeFactory] Using Connection: Host={host};Port={port};Database={database};Username={username};Password=***");

            // Using Npgsql PostgreSQL Provider
            optionsBuilder.UseNpgsql(connectionString);

            // Optional: Add logging for EF Core tools if needed for debugging
            // optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

            // return the context using the configured options:
            return new RestoMateDbContext(optionsBuilder.Options);
        }
    }
}