using Microsoft.EntityFrameworkCore;
using RestoMate.Models;
using RestoMateApp.Data;

namespace RestoMateApp.Services;

// Dish service
    public interface IDishService
    {
        Task<List<Dish>> GetAllDishesAsync();
        Task<List<Dish>> GetDishesByCategoryAsync(string category);
        Task<Dish> GetDishByIdAsync(int id);
        Task<Dish> AddDishAsync(Dish dish);
        Task<Dish> UpdateDishAsync(Dish dish);
        Task<bool> DeleteDishAsync(int id);
        Task<List<string>> GetCategoriesAsync();
    }
    
    public class DishService : IDishService
    {
        private readonly RestoMateDbContext _context;
        
        public DishService(RestoMateDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes.ToListAsync();
        }
        
        public async Task<List<Dish>> GetDishesByCategoryAsync(string category)
        {
            return await _context.Dishes
                .Where(d => d.Category == category)
                .ToListAsync();
        }
        
        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes.FindAsync(id);
        }
        
        public async Task<Dish> AddDishAsync(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            return dish;
        }
        
        public async Task<Dish> UpdateDishAsync(Dish dish)
        {
            _context.Dishes.Update(dish);
            await _context.SaveChangesAsync();
            return dish;
        }
        
        public async Task<bool> DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
                return false;
                
            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.Dishes
                .Select(d => d.Category)
                .Distinct()
                .ToListAsync();
        }
    }