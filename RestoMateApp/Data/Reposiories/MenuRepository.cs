using Microsoft.EntityFrameworkCore;
using RestoMate.Data;

namespace RestoMate.Models.User;

public class MenuRepository
{
    private readonly RestaurantDbContext _db;

    public MenuRepository(RestaurantDbContext db)
    {
        _db = db;
    }

    // CRUD Operations for Dishes
    public async Task<Dish> AddDish(Dish dish)
    {
        _db.MenuItems.Add(dish);
        await _db.SaveChangesAsync();
        return dish;
    }

    public async Task<List<Dish>> GetMenuByCategory(string category)
    {
        return await _db.MenuItems
            .Where(d => d.Category == category)
            .OrderBy(d => d.Name)
            .ToListAsync();
    }

    // Inventory Management
    // public async Task UpdateStock(int dishId, int quantityChange)
    // {
    //     var dish = await _db.MenuItems.FindAsync(dishId);
    //     if (dish != null)
    //     {
    //         dish.StockQuantity += quantityChange;
    //         await _db.SaveChangesAsync();
    //     }
    // }
}