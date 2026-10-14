public class EventExpiredException : Exception
{
    public EventExpiredException()
        : base("Bu tədbirin vaxtı artıq keçib.")
    {
    }
}

public class TicketAlreadyUsedException : Exception
{
    public TicketAlreadyUsedException()
        : base("Bu bilet artıq istifadə olunub.")
    {
    }
}