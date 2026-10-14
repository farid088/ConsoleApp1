public interface ITicketService
{
    Task<Ticket> BuyTicketAsync(int eventId);

    Task UseTicketAsync(int ticketId);
}



public class TicketService : ITicketService
{
    private readonly AppDbContext _context;

    public TicketService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket> BuyTicketAsync(int eventId)
    {
        var eventItem = await _context.Events.FindAsync(eventId);

        if (eventItem == null)
            throw new Exception("Tədbir tapılmadı.");

        if (eventItem.Date <= DateTime.Now)
            throw new EventExpiredException();

        var ticket = new Ticket
        {
            EventId = eventId,
            Price = eventItem.TicketPrice
        };

        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        return ticket;
    }

    public async Task UseTicketAsync(int ticketId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);

        if (ticket == null)
            throw new Exception("Bilet tapılmadı.");
    
    }
}