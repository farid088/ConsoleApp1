using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=.\SQLEXPRESS;Database=F7;Trusted_Connection=True;TrustServerCertificate=True");
    }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Event> Events { get; set; } 
}