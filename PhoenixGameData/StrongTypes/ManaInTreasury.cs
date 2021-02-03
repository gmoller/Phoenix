using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct ManaInTreasury : IComparable<ManaInTreasury>, IEquatable<ManaInTreasury>
    {
        public int Value { get; }

        public ManaInTreasury(int value)
        {
            Value = value;
        }

        public bool Equals(ManaInTreasury other) => Value.Equals(other.Value);
        public int CompareTo(ManaInTreasury other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ManaInTreasury other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator ==(ManaInTreasury a, ManaInTreasury b) => a.CompareTo(b) == 0;
        public static bool operator !=(ManaInTreasury a, ManaInTreasury b) => !(a == b);
    }
}