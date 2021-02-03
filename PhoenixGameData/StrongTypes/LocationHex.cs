using System;
using Zen.Utilities;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct LocationHex : IComparable<LocationHex>, IEquatable<LocationHex>
    {
        public PointI Value { get; }

        public LocationHex(PointI value)
        {
            Value = value;
        }

        public bool Equals(LocationHex other) => Value.Equals(other.Value);
        public int CompareTo(LocationHex other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is LocationHex other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (LocationHex a, LocationHex b) => a.CompareTo(b) == 0;
        public static bool operator != (LocationHex a, LocationHex b) => !(a == b);
    }
}