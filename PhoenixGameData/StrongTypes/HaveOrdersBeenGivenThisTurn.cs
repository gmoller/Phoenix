using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct HaveOrdersBeenGivenThisTurn : IComparable<HaveOrdersBeenGivenThisTurn>, IEquatable<HaveOrdersBeenGivenThisTurn>
    {
        public bool Value { get; }

        public HaveOrdersBeenGivenThisTurn(bool value)
        {
            Value = value;
        }

        public bool Equals(HaveOrdersBeenGivenThisTurn other) => Value.Equals(other.Value);
        public int CompareTo(HaveOrdersBeenGivenThisTurn other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is HaveOrdersBeenGivenThisTurn other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (HaveOrdersBeenGivenThisTurn a, HaveOrdersBeenGivenThisTurn b) => a.CompareTo(b) == 0;
        public static bool operator != (HaveOrdersBeenGivenThisTurn a, HaveOrdersBeenGivenThisTurn b) => !(a == b);
    }
}