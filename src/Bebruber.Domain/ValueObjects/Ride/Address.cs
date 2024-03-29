﻿using System.Collections.Generic;
using Bebruber.Domain.Tools;

namespace Bebruber.Domain.ValueObjects.Ride;

public class Address : ValueObject<Address>
{
    public Address(string country, string city, string street, string houseNumber)
    {
        Country = country;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }

    protected Address() { }

    public string Country { get; protected init; }
    public string City { get; protected init; }
    public string Street { get; private init; }
    public string HouseNumber { get; private init; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
        yield return HouseNumber;
    }
}