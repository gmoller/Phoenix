using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct Amount : IComparable<Amount>, IEquatable<Amount>
    {
        public int Value { get; }

        public Amount(int value)
        {
            Value = value;
        }

        public bool Equals(Amount other) => Value.Equals(other.Value);
        public int CompareTo(Amount other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Amount other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (Amount a, Amount b) => a.CompareTo(b) == 0;
        public static bool operator != (Amount a, Amount b) => !(a == b);
    }
}