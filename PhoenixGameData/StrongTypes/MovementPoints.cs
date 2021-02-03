using System;
using System.Globalization;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct MovementPoints : IComparable<MovementPoints>, IEquatable<MovementPoints>
    {
        public float Value { get; }

        public MovementPoints(float value)
        {
            Value = value;
        }

        public bool Equals(MovementPoints other) => Value.Equals(other.Value);
        public int CompareTo(MovementPoints other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MovementPoints other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

        public static bool operator == (MovementPoints a, MovementPoints b) => a.CompareTo(b) == 0;
        public static bool operator != (MovementPoints a, MovementPoints b) => !(a == b);
    }
}