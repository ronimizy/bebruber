using System;
using Bebruber.Domain.Tools;
using Bebruber.Domain.ValueObjects.Exceptions;

namespace Bebruber.Domain.ValueObjects;

public class ExpirationDate : ValueObject<ExpirationDate>
{
    public ExpirationDate(int year, int month)
    {
        if (year < 0)
            throw new InvalidExpirationDateComponentException(nameof(Year), year);

        if (month is <= 0 or > 12)
            throw new InvalidExpirationDateComponentException(nameof(Month), month);

        Year = year;
        Month = month;
    }

    public int Year { get; private init; }
    public int Month { get; private init; }

    public override int GetHashCode()
        => HashCode.Combine(Year, Month);

    protected override bool EqualTo(ExpirationDate other)
        => other.Year.Equals(Year) && other.Month.Equals(Month);
}