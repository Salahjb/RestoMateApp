using Microsoft.EntityFrameworkCore;
using RestoMate.Data;

namespace RestoMate.Models.User;

public class UserRepository
{
    private readonly RestaurantDbContext _db;

    public UserRepository(RestaurantDbContext db)
    {
        _db = db;
    }

    // Customer Operations
    public async Task<Customer> CreateCustomer(Customer customer)
    {
        customer.passwordHash = BCrypt.Net.BCrypt.HashPassword(customer.passwordHash);
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    // Staff Operations
    public async Task<Staff> CreateStaff(Staff staff)
    {
        staff.passwordHash = BCrypt.Net.BCrypt.HashPassword(staff.passwordHash);
        _db.StaffMembers.Add(staff);
        await _db.SaveChangesAsync();
        return staff;
    }

    // Authentication
    public async Task<UserBase?> Authenticate(string username, string password)
    {
        var user = await _db.Customers
            .Cast<UserBase>()
            .Concat(_db.StaffMembers)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.passwordHash))
        {
            //user.LastLogin = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return user;
        }
        return null;
    }

    // Example Query: Get staff by role
    public async Task<List<Staff>> GetStaffByRole(StaffRole role)
    {
        return await _db.StaffMembers
            .Where(s => s.Role == role)
            .ToListAsync();
    }
}