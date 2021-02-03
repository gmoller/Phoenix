using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct BuildingTypeId : IComparable<BuildingTypeId>, IEquatable<BuildingTypeId>
    {
        public int Value { get; }

        public BuildingTypeId(int value)
        {
            Value = value;
        }

        public bool Equals(BuildingTypeId other) => Value.Equals(other.Value);
        public int CompareTo(BuildingTypeId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is BuildingTypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (BuildingTypeId a, BuildingTypeId b) => a.CompareTo(b) == 0;
        public static bool operator != (BuildingTypeId a, BuildingTypeId b) => !(a == b);
    }
}