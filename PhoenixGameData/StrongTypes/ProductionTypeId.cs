using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct ProductionTypeId : IComparable<ProductionTypeId>, IEquatable<ProductionTypeId>
    {
        public int Value { get; }

        public ProductionTypeId(int value)
        {
            Value = value;
        }

        public bool Equals(ProductionTypeId other) => Value.Equals(other.Value);
        public int CompareTo(ProductionTypeId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ProductionTypeId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (ProductionTypeId a, ProductionTypeId b) => a.CompareTo(b) == 0;
        public static bool operator != (ProductionTypeId a, ProductionTypeId b) => !(a == b);
    }
}