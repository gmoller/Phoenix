using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct FactionId : IComparable<FactionId>, IEquatable<FactionId>
    {
        public int Value { get; }

        public FactionId(int value)
        {
            Value = value;
        }

        public bool Equals(FactionId other) => Value.Equals(other.Value);
        public int CompareTo(FactionId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is FactionId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (FactionId a, FactionId b) => a.CompareTo(b) == 0;
        public static bool operator != (FactionId a, FactionId b) => !(a == b);
    }
}