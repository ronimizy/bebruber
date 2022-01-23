using Bebruber.Domain.Tools;
using Bebruber.Domain.ValueObjects.Exceptions;

namespace Bebruber.Domain.ValueObjects.Ride;

public class Roubles : ValueOf<decimal, Roubles>
{
    public Roubles(decimal value)
        : base(value, d => d >= 0, new InvalidRoublesValueException(value)) { }

    protected Roubles() { }
}