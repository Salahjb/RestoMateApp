using Microsoft.EntityFrameworkCore;
using RestoMate.Data;

namespace RestoMate.Models.User;

public class OrderRepository
{
    private readonly RestaurantDbContext _db;

    public OrderRepository(RestaurantDbContext db)
    {
        
        _db = db;
    }

    public async Task<Order> CreateOrder(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<List<Order>> GetTodayOrders()
    {
        var today = DateTime.Today;
        return await _db.Orders
            .Where(o => o.Date.Date == today)
            .Include(o => o.Items)
            .ThenInclude(i => i.Dish)
            .ToListAsync();
    }

    // Business Logic Example
    public async Task CancelOrder(int orderId)
    {
        var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order != null && order.status != "Completed")
        {
            order.status = "Cancelled";
            await _db.SaveChangesAsync();
        }
    }
}