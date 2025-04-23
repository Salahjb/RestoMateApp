using Microsoft.EntityFrameworkCore;
using RestoMate.Data;
using RestoMate.Models;

namespace RestoMateApp.Services;
// Reservation service
public interface IReservationService
{
    Task<List<Reservation>> GetAllReservationsAsync();
    Task<List<Reservation>> GetReservationsForDateAsync(DateTime date);
    Task<Reservation> GetReservationByIdAsync(int id);
    Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId);
    Task<Reservation> CreateReservationAsync(Reservation reservation);
    Task<Reservation> UpdateReservationAsync(Reservation reservation);
    Task<bool> CancelReservationAsync(int id);
    Task<List<Table>> GetAvailableTablesForTimeAsync(DateTime date, TimeSpan time, int duration, int partySize);
}

public class ReservationService : IReservationService
{
    private readonly RestoMateDbContext _context;
    
    public ReservationService(RestoMateDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .ToListAsync();
    }
    
    public async Task<List<Reservation>> GetReservationsForDateAsync(DateTime date)
    {
        return await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .Where(r => r.ReservationDate.Date == date.Date && r.Status != "Cancelled")
            .ToListAsync();
    }
    
    public async Task<Reservation> GetReservationByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    
    public async Task<List<Reservation>> GetReservationsByCustomerAsync(int customerId)
    {
        return await _context.Reservations
            .Include(r => r.Table)
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();
    }
    
    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        // Validate table availability
        bool isAvailable = await IsTableAvailable(
            reservation.TableId, 
            reservation.ReservationDate, 
            reservation.ReservationTime);
            
        if (!isAvailable)
            throw new InvalidOperationException("The selected table is not available at the specified time.");
            
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        
        return reservation;
    }
    
    public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
    {
        // If changing date/time/table, validate availability
        var existingReservation = await _context.Reservations.FindAsync(reservation.Id);
        
        if (existingReservation.TableId != reservation.TableId ||
            existingReservation.ReservationDate != reservation.ReservationDate ||
            existingReservation.ReservationTime != reservation.ReservationTime)
        {
            bool isAvailable = await IsTableAvailable(
                reservation.TableId, 
                reservation.ReservationDate, 
                reservation.ReservationTime,
                reservation.Id);
                
            if (!isAvailable)
                throw new InvalidOperationException("The selected table is not available at the specified time.");
        }
        
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
        
        return reservation;
    }
    
    public async Task<bool> CancelReservationAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
            return false;
            
        reservation.Status = "Cancelled";
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<Table>> GetAvailableTablesForTimeAsync(DateTime date, TimeSpan time, int duration, int partySize)
    {
        // Get all tables that can accommodate the party size
        var suitableTables = await _context.Tables
            .Where(t => t.Capacity >= partySize)
            .ToListAsync();
            
        var availableTables = new List<Table>();
        
        foreach (var table in suitableTables)
        {
            bool isAvailable = await IsTableAvailable(table.Id, date, time);
            if (isAvailable)
            {
                availableTables.Add(table);
            }
        }
        
        return availableTables.OrderBy(t => t.Capacity).ToList();
    }
    
    private async Task<bool> IsTableAvailable(int tableId, DateTime date, TimeSpan time, int? excludeReservationId = null)
    {
        // Default reservation duration (2 hours)
        TimeSpan duration = TimeSpan.FromHours(2);
        
        // Calculate start and end times for the requested reservation
        var startTime = time;
        var endTime = time.Add(duration);
        
        // Find overlapping reservations for the table
        var overlappingReservations = await _context.Reservations
            .Where(r => 
                r.TableId == tableId && 
                r.ReservationDate.Date == date.Date &&
                r.Status != "Cancelled" &&
                (excludeReservationId == null || r.Id != excludeReservationId) &&
                (
                    // Case 1: New reservation starts during an existing reservation
                    (r.ReservationTime <= startTime && startTime < r.ReservationTime.Add(duration)) ||
                    // Case 2: New reservation ends during an existing reservation
                    (r.ReservationTime < endTime && endTime <= r.ReservationTime.Add(duration)) ||
                    // Case 3: New reservation completely contains an existing reservation
                    (startTime <= r.ReservationTime && r.ReservationTime.Add(duration) <= endTime)
                ))
            .AnyAsync();
            
        return !overlappingReservations;
    }
}
    