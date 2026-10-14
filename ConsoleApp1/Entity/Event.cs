public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public double TicketPrice { get; set; }
    public List<Ticket> Tickets { get; set; } = new();
}

