using Microsoft.EntityFrameworkCore;

public interface IEventService
{
    Task<List<Event>> GetAllAsync();

    Task<Event?> GetByIdAsync(int id);

    Task AddAsync(Event newEvent);

    Task UpdateAsync(Event updatedEvent);

    Task DeleteAsync(int id);
    Task<List<Event>> GetEventsByTicketCountAsync();
    Task<List<Event>> GetEventsByRevenueAsync();
    Task<List<Event>> GetUpcomingEventsAsync();
    Task<List<Event>> GetPastEventsAsync();
}


public class EventService : IEventService
{
    private readonly AppDbContext _context;

    public EventService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task AddAsync(Event newEvent)
    {
        await _context.Events.AddAsync(newEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event updatedEvent)
    {
        _context.Events.Update(updatedEvent);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);

        if (eventItem == null)
            throw new Exception("Tədbir tapılmadı.");

        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Event>> GetEventsByTicketCountAsync()
    {
        return await _context.Events
            .Include(e => e.Tickets)
            .OrderByDescending(e => e.Tickets.Count)
            .ToListAsync();
    }
    public async Task<List<Event>> GetEventsByRevenueAsync()
    {
        return await _context.Events
            .Include(e => e.Tickets)
            .OrderByDescending(e => e.Tickets.Sum(t => t.Price))
            .ToListAsync();
    }
    public async Task<List<Event>> GetUpcomingEventsAsync()
    {
        return await _context.Events
            .Where(e => e.Date > DateTime.Now)
            .ToListAsync();
    }
    public async Task<List<Event>> GetPastEventsAsync()
    {
        return await _context.Events
            .Where(e => e.Date < DateTime.Now)
            .Include(e => e.Tickets)
            .ToListAsync();
    }
}