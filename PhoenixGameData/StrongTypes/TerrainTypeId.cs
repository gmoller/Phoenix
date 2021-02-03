using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct TerrainTypeId : IComparable<TerrainTypeId>, IEquatable<TerrainTypeId>
    {
        public int Value { get; }

        public TerrainTypeId(int value)
        {
            Value = value;
        }

        public bool Equals(TerrainTypeId other) => Value.Equals(other.Value);
        public int CompareTo(TerrainTypeId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TerrainTypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator ==(TerrainTypeId a, TerrainTypeId b) => a.CompareTo(b) == 0;
        public static bool operator !=(TerrainTypeId a, TerrainTypeId b) => !(a == b);
    }
}