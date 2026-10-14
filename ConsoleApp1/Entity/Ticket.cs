public class Ticket
{
    public int Id { get; set; }
    public double Price { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
}