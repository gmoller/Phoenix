using System;

namespace PhoenixGameData.StrongTypes
{
    public readonly struct Name : IComparable<Name>, IEquatable<Name>
    {
        public string Value { get; }

        public Name(string value)
        {
            Value = value;
        }

        public bool Equals(Name other) => Value.Equals(other.Value);
        public int CompareTo(Name other) => Value.CompareTo(other.Value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Name other && Equals(other);
        }

        public override int GetHashCode() => Value.GetHashCode();
        public override string ToString() => Value;

        public static bool operator == (Name a, Name b) => a.CompareTo(b) == 0;
        public static bool operator != (Name a, Name b) => !(a == b);
    }
}