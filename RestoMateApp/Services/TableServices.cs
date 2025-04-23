using Microsoft.EntityFrameworkCore;
using RestoMate.Data;
using RestoMate.Models;

namespace RestoMateApp.Services;

 // Table service
public interface ITableService
{
    Task<List<Table>> GetAllTablesAsync();
    Task<Table> GetTableByIdAsync(int id);
    Task<Table> GetTableByNumberAsync(int tableNumber);
    Task<Table> AddTableAsync(Table table);
    Task<Table> UpdateTableAsync(Table table);
    Task<bool> DeleteTableAsync(int id);
    Task<Table> UpdateTableStatusAsync(int id, string status);
}

public class TableService : ITableService
{
    private readonly RestoMateDbContext _context;
    
    public TableService(RestoMateDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Table>> GetAllTablesAsync()
    {
        return await _context.Tables.ToListAsync();
    }
    
    public async Task<Table> GetTableByIdAsync(int id)
    {
        return await _context.Tables.FindAsync(id);
    }
    
    public async Task<Table> GetTableByNumberAsync(int tableNumber)
    {
        return await _context.Tables
            .FirstOrDefaultAsync(t => t.TableNumber == tableNumber);
    }
    
    public async Task<Table> AddTableAsync(Table table)
    {
        _context.Tables.Add(table);
        await _context.SaveChangesAsync();
        return table;
    }
    
    public async Task<Table> UpdateTableAsync(Table table)
    {
        _context.Tables.Update(table);
        await _context.SaveChangesAsync();
        return table;
    }
    
    public async Task<bool> DeleteTableAsync(int id)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table == null)
            return false;
            
        _context.Tables.Remove(table);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<Table> UpdateTableStatusAsync(int id, string status)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table == null)
            throw new KeyNotFoundException("Table not found");
            
        table.Status = status;
        await _context.SaveChangesAsync();
        return table;
    }
}