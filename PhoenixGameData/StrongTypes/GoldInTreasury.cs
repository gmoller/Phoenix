using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct GoldInTreasury : IComparable<GoldInTreasury>, IEquatable<GoldInTreasury>
    {
        public int Value { get; }

        public GoldInTreasury(int value)
        {
            Value = value;
        }

        public bool Equals(GoldInTreasury other) => Value.Equals(other.Value);
        public int CompareTo(GoldInTreasury other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is GoldInTreasury other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator ==(GoldInTreasury a, GoldInTreasury b) => a.CompareTo(b) == 0;
        public static bool operator !=(GoldInTreasury a, GoldInTreasury b) => !(a == b);
    }
}