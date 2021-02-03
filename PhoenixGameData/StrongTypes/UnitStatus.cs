using System;
using PhoenixGameData.Enumerations;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct Status : IComparable<Status>, IEquatable<Status>
    {
        public UnitStatus Value { get; }

        public Status(UnitStatus value)
        {
            Value = value;
        }

        public bool Equals(Status other) => Value.Equals(other.Value);
        public int CompareTo(Status other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Status other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (Status a, Status b) => a.CompareTo(b) == 0;
        public static bool operator != (Status a, Status b) => !(a == b);
    }
}