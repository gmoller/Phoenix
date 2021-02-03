using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct ProductionAccrued : IComparable<ProductionAccrued>, IEquatable<ProductionAccrued>
    {
        public int Value { get; }

        public ProductionAccrued(int value)
        {
            Value = value;
        }

        public bool Equals(ProductionAccrued other) => Value.Equals(other.Value);
        public int CompareTo(ProductionAccrued other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ProductionAccrued other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (ProductionAccrued a, ProductionAccrued b) => a.CompareTo(b) == 0;
        public static bool operator != (ProductionAccrued a, ProductionAccrued b) => !(a == b);
    }
}