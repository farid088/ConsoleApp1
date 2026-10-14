using System.Globalization;

using var context = new AppDbContext();

IEventService eventService = new EventService(context);
ITicketService ticketService = new TicketService(context);

while (true)
{
    Console.WriteLine("\n===== EVENT MANAGEMENT =====");
    Console.WriteLine("1. Bütün tədbirlər");
    Console.WriteLine("2. Yeni tədbir yarat");
    Console.WriteLine("3. Bilet al");
    Console.WriteLine("4. Biletdən istifadə et");
    Console.WriteLine("5. Bilet sayına görə sırala");
    Console.WriteLine("6. Gəlirə görə sırala");
    Console.WriteLine("7. Gələcək tədbirlər");
    Console.WriteLine("8. Keçmiş tədbirlər");
    Console.WriteLine("0. Çıxış");
    Console.Write("Seçim: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                {
                    var events = await eventService.GetAllAsync();

                    foreach (var item in events)
                    {
                        Console.WriteLine(
                            $"{item.Id} - {item.Name} - {item.Date} - {item.TicketPrice}");
                    }

                    break;
                }

            case "2":
                {
                    Console.Write("Tədbirin adı: ");
                    string name = Console.ReadLine()!;

                    Console.Write("Tarix (dd.MM.yyyy HH:mm): ");

                    DateTime date = DateTime.ParseExact(
                        Console.ReadLine()!,
                        "dd.MM.yyyy HH:mm",
                        CultureInfo.InvariantCulture);

                    Console.Write("Bilet qiyməti: ");

                    if (!double.TryParse(
                        Console.ReadLine(),
                        CultureInfo.InvariantCulture,
                        out double price) || price < 0)
                    {
                        Console.WriteLine("Qiymət düzgün daxil edilməyib.");
                        break;
                    }

                    Event newEvent = new Event
                    {
                        Name = name,
                        Date = date,
                        TicketPrice = price
                    };

                    await eventService.AddAsync(newEvent);

                    Console.WriteLine("Tədbir yaradıldı.");
                    break;
                }

            case "3":
                {
                    var upcomingEvents =
                        await eventService.GetUpcomingEventsAsync();

                    if (upcomingEvents.Count == 0)
                    {
                        Console.WriteLine("Gələcək tədbir yoxdur.");
                        break;
                    }

                    foreach (var item in upcomingEvents)
                    {
                        Console.WriteLine(
                            $"{item.Id} - {item.Name} - {item.TicketPrice}");
                    }

                    int eventId;

                    while (true)
                    {
                        Console.Write("Bilet almaq istədiyiniz Event ID: ");

                        if (!int.TryParse(Console.ReadLine(), out eventId))
                        {
                            Console.WriteLine("Düzgün ID daxil edin.");
                            continue;
                        }

                        var selectedEvent =
                            await eventService.GetByIdAsync(eventId);

                        if (selectedEvent == null ||
                            selectedEvent.Date <= DateTime.Now)
                        {
                            Console.WriteLine("Belə gələcək tədbir yoxdur.");
                            continue;
                        }

                        break;
                    }

                    var ticket =
                        await ticketService.BuyTicketAsync(eventId);

                    Console.WriteLine($"Bilet alındı. Ticket ID: {ticket.Id}");
                    break;
                }

            case "4":
                {
                    Console.Write("Ticket ID: ");

                    if (!int.TryParse(Console.ReadLine(), out int ticketId))
                    {
                        Console.WriteLine("ID düzgün deyil.");
                        break;
                    }

                    await ticketService.UseTicketAsync(ticketId);

                    Console.WriteLine("Bilet istifadə metodu çağırıldı.");
                    break;
                }

            case "5":
                {
                    var events =
                        await eventService.GetEventsByTicketCountAsync();

                    foreach (var item in events)
                    {
                        Console.WriteLine(
                            $"{item.Name} - Satılmış bilet sayı: {item.Tickets.Count}");
                    }

                    break;
                }

            case "6":
                {
                    var events =
                        await eventService.GetEventsByRevenueAsync();

                    foreach (var item in events)
                    {
                        double revenue = 0;

                        foreach (var ticket in item.Tickets)
                        {
                            revenue += ticket.Price;
                        }

                        Console.WriteLine($"{item.Name} - Gəlir: {revenue}");
                    }

                    break;
                }

            case "7":
                {
                    var events =
                        await eventService.GetUpcomingEventsAsync();

                    foreach (var item in events)
                    {
                        Console.WriteLine(
                            $"{item.Id} - {item.Name} - {item.Date}");
                    }

                    break;
                }

            case "8":
                {
                    var events =
                        await eventService.GetPastEventsAsync();

                    foreach (var item in events)
                    {
                        Console.WriteLine(
                            $"{item.Name} - Alınmış bilet sayı: {item.Tickets.Count}");
                    }

                    break;
                }

            case "0":
                return;

            default:
                Console.WriteLine("Yanlış seçim.");
                break;
        }
    }
    catch (EventExpiredException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (TicketAlreadyUsedException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (FormatException)
    {
        Console.WriteLine("Daxil edilən məlumatın formatı yanlışdır.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Xəta: {ex.Message}");
    }
}