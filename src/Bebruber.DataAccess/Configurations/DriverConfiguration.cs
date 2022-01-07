using Bebruber.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bebruber.DataAccess.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.OwnsOne(d => d.Name);
        builder.OwnsOne(d => d.Rating);
        builder.OwnsOne(d => d.PaymentAddress);
        builder.HasOne(d => d.CardInfo);
        builder.Navigation(d => d.Cars).HasField("_cars");
        builder.Navigation(d => d.Rides).HasField("_rides");
    }
}