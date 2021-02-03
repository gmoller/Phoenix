using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct RaceTypeId : IComparable<RaceTypeId>, IEquatable<RaceTypeId>
    {
        public int Value { get; }

        public RaceTypeId(int value)
        {
            Value = value;
        }

        public bool Equals(RaceTypeId other) => Value.Equals(other.Value);
        public int CompareTo(RaceTypeId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RaceTypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (RaceTypeId a, RaceTypeId b) => a.CompareTo(b) == 0;
        public static bool operator != (RaceTypeId a, RaceTypeId b) => !(a == b);
    }
}