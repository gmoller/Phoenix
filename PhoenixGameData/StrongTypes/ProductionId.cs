using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct ProductionId : IComparable<ProductionId>, IEquatable<ProductionId>
    {
        public int Value { get; }

        public ProductionId(int value)
        {
            Value = value;
        }

        public bool Equals(ProductionId other) => Value.Equals(other.Value);
        public int CompareTo(ProductionId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ProductionId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (ProductionId a, ProductionId b) => a.CompareTo(b) == 0;
        public static bool operator != (ProductionId a, ProductionId b) => !(a == b);
    }
}