using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct StackId : IComparable<StackId>, IEquatable<StackId>
    {
        public int Value { get; }

        public StackId(int value)
        {
            Value = value;
        }

        public bool Equals(StackId other) => Value.Equals(other.Value);
        public int CompareTo(StackId other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StackId other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value.ToString();

        public static bool operator == (StackId a, StackId b) => a.CompareTo(b) == 0;
        public static bool operator != (StackId a, StackId b) => !(a == b);
    }
}