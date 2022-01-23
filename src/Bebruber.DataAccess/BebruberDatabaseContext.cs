using Bebruber.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bebruber.DataAccess;

public class BebruberDatabaseContext : DbContext
{
    public BebruberDatabaseContext(DbContextOptions<BebruberDatabaseContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Client> Clients { get; private set; } = null!;
    public DbSet<CardInfo> PaymentInfos { get; private set; } = null!;
    public DbSet<Ride> Rides { get; private set; } = null!;
    public DbSet<Driver> Drivers { get; private set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAssemblyMarker).Assembly);
    }
}