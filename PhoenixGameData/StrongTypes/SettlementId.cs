using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct SettlementId : IComparable<SettlementId>, IEquatable<SettlementId>
    {
        public int Value { get; }

        public SettlementId(int value)
        {
            Value = value;
        }

        public bool Equals(SettlementId other) => Value.Equals(other.Value);
        public int CompareTo(SettlementId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SettlementId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (SettlementId a, SettlementId b) => a.CompareTo(b) == 0;
        public static bool operator != (SettlementId a, SettlementId b) => !(a == b);
    }
}