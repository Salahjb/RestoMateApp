using System.Security.Authentication;
using Microsoft.EntityFrameworkCore;
using RestoMate.Data;
using RestoMate.Models.User;

namespace RestoMateApp.Services;

// Authentication service
public interface IAuthService
{
    Task<UserBase> LoginAsync(string username, string password);
    Task<Customer> RegisterCustomerAsync(Customer customer, string password);
    Task<Staff> RegisterStaffAsync(Staff staff, string password, string adminPassword);
    Task LogoutAsync();
    Task<UserBase> GetCurrentUserAsync();
}

public class AuthService : IAuthService
{
    private readonly RestoMateDbContext _context;
    private UserBase _currentUser;
    
    public AuthService(RestoMateDbContext context)
    {
        _context = context;
    }
    
    public async Task<UserBase> LoginAsync(string username, string password)
    {
        // First check customers
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Username == username);
            
        if (customer != null && BCrypt.Net.BCrypt.Verify(password, customer.passwordHash))
        {
            _currentUser = customer;
            return customer;
        }
        
        // Then check staff
        var staff = await _context.Staff
            .FirstOrDefaultAsync(s => s.Username == username);
            
        if (staff != null && BCrypt.Net.BCrypt.Verify(password, staff.passwordHash))
        {
            _currentUser = staff;
            return staff;
        }
        
        // Then check admins
        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.Username == username);
            
        if (admin != null && BCrypt.Net.BCrypt.Verify(password, admin.passwordHash))
        {
            _currentUser = admin;
            return admin;
        }
        
        throw new AuthenticationException("Invalid username or password");
    }
    
    public async Task<Customer> RegisterCustomerAsync(Customer customer, string password)
    {
        // Check if username already exists
        var exists = await _context.Customers.AnyAsync(c => c.Username == customer.Username);
        if (exists)
        {
            throw new InvalidOperationException("Username already exists");
        }
        
        // Hash password
        customer.passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        
        // Add to database
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        
        return customer;
    }
    
    public async Task<Staff> RegisterStaffAsync(Staff staff, string password, string adminPassword)
    {
        // Verify admin password
        var admin = await _context.Admins.FirstOrDefaultAsync();
        if (admin == null || !BCrypt.Net.BCrypt.Verify(adminPassword, admin.passwordHash))
        {
            throw new AuthenticationException("Admin authentication failed");
        }
        
        // Check if username already exists
        var exists = await _context.Staff.AnyAsync(s => s.Username == staff.Username);
        if (exists)
        {
            throw new InvalidOperationException("Username already exists");
        }
        
        // Hash password
        staff.passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        
        // Add to database
        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();
        
        return staff;
    }
    
    public Task LogoutAsync()
    {
        _currentUser = null;
        return Task.CompletedTask;
    }
    
    public Task<UserBase> GetCurrentUserAsync()
    {
        return Task.FromResult(_currentUser);
    }
}