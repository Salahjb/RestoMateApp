using Microsoft.EntityFrameworkCore;
using RestoMate.Data;

namespace RestoMateApp.Services;

// Order service
public interface IOrderService
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<List<Order>> GetActiveOrdersAsync();
    Task<Order> GetOrderByIdAsync(int id);
    Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
    Task<List<Order>> GetOrdersByTableIdAsync(int tableId);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<Order> AddItemToOrderAsync(int orderId, OrderItem item);
    Task<bool> RemoveItemFromOrderAsync(int orderId, int itemId);
    Task<Order> UpdateOrderStatusAsync(int orderId, string status);
    Task<bool> DeleteOrderAsync(int id);
}

public class OrderService : IOrderService
{
    private readonly RestoMateDbContext _context;
    
    public OrderService(RestoMateDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Dish)
            .Include(o => o.Customer)
            .Include(o => o.Table)
            .ToListAsync();
    }
    
    public async Task<List<Order>> GetActiveOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Dish)
            .Include(o => o.Customer)
            .Include(o => o.Table)
            .Where(o => o.Status != "Paid" && o.Status != "Cancelled")
            .ToListAsync();
    }
    
    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Dish)
            .Include(o => o.Customer)
            .Include(o => o.Table)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
    
    public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Dish)
            .Include(o => o.Table)
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }
    
    public async Task<List<Order>> GetOrdersByTableIdAsync(int tableId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Dish)
            .Include(o => o.Customer)
            .Where(o => o.TableId == tableId && o.Status != "Paid" && o.Status != "Cancelled")
            .ToListAsync();
    }
    
    public async Task<Order> CreateOrderAsync(Order order)
    {
        // Update dish names and prices from current values
        foreach (var item in order.Items)
        {
            var dish = await _context.Dishes.FindAsync(item.DishId);
            if (dish != null)
            {
                item.DishName = dish.Name;
                item.UnitPrice = dish.Price;
            }
        }
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        // Update table status
        var table = await _context.Tables.FindAsync(order.TableId);
        if (table != null)
        {
            table.Status = "Occupied";
            await _context.SaveChangesAsync();
        }
        
        return order;
    }
    
    public async Task<Order> UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
    
    public async Task<Order> AddItemToOrderAsync(int orderId, OrderItem item)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException("Order not found");
            
        // Set dish name and price from the current dish
        var dish = await _context.Dishes.FindAsync(item.DishId);
        if (dish != null)
        {
            item.DishName = dish.Name;
            item.UnitPrice = dish.Price;
        }
        
        item.OrderId = orderId;
        order.Items.Add(item);
        await _context.SaveChangesAsync();
        
        return order;
    }
    
    public async Task<bool> RemoveItemFromOrderAsync(int orderId, int itemId)
    {
        var item = await _context.OrderItems
            .FirstOrDefaultAsync(i => i.OrderId == orderId && i.Id == itemId);
            
        if (item == null)
            return false;
            
        _context.OrderItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<Order> UpdateOrderStatusAsync(int orderId, string status)
    {
        var order = await GetOrderByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException("Order not found");
            
        order.Status = status;
        
        // If status is "Paid", update completed date and table status
        if (status == "Paid")
        {
            order.CompletedDate = DateTime.Now;
            
            // Check if table has no other active orders
            var otherActiveOrders = await _context.Orders
                .AnyAsync(o => o.TableId == order.TableId && o.Id != orderId && o.Status != "Paid" && o.Status != "Cancelled");
                
            if (!otherActiveOrders)
            {
                var table = await _context.Tables.FindAsync(order.TableId);
                if (table != null)
                {
                    table.Status = "Available";
                }
            }
        }
        
        await _context.SaveChangesAsync();
        return order;
    }
    
    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return false;
            
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}