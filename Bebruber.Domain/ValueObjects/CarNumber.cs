using System.Collections.Generic;
using Bebruber.Domain.Tools;
using Bebruber.Utility.Extensions;

namespace Bebruber.Domain.ValueObjects;

public class CarNumber : ValueObject<CardNumber>
{
    public CarNumber(CarNumberRegistrationSeries series, CarNumberRegionCode regionCode)
    {
        RegistrationSeries = series.ThrowIfNull();
        RegionCode = regionCode.ThrowIfNull();
    }

    public CarNumberRegistrationSeries RegistrationSeries { get; private init; }
    public CarNumberRegionCode RegionCode { get; private init; }

    public override string ToString()
        => $"{RegistrationSeries} {RegionCode}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return RegistrationSeries;
        yield return RegionCode;
    }
}