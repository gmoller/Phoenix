using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct UnitTypeId : IComparable<UnitTypeId>, IEquatable<UnitTypeId>
    {
        public int Value { get; }

        public UnitTypeId(int value)
        {
            Value = value;
        }

        public bool Equals(UnitTypeId other) => Value.Equals(other.Value);
        public int CompareTo(UnitTypeId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UnitTypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (UnitTypeId a, UnitTypeId b) => a.CompareTo(b) == 0;
        public static bool operator != (UnitTypeId a, UnitTypeId b) => !(a == b);
    }
}