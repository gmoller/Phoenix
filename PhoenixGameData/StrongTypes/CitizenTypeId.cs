using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct CitizenTypeId : IComparable<CitizenTypeId>, IEquatable<CitizenTypeId>
    {
        public int Value { get; }

        public CitizenTypeId(int value)
        {
            Value = value;
        }

        public bool Equals(CitizenTypeId other) => Value.Equals(other.Value);
        public int CompareTo(CitizenTypeId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CitizenTypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (CitizenTypeId a, CitizenTypeId b) => a.CompareTo(b) == 0;
        public static bool operator != (CitizenTypeId a, CitizenTypeId b) => !(a == b);
    }
}