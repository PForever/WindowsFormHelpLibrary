using System;

namespace WindowsFormHelpLibrary.FilterHelp
{
    public class PropertyNamePosition : IEquatable<PropertyNamePosition>
    {
        public static implicit operator PropertyNamePosition(ValueTuple<int, string> tuple)
        {
            return new PropertyNamePosition(tuple.Item1, tuple.Item2);
        }
        public PropertyNamePosition(int position, string name)
        {
            Position = position;
            Name = name;
        }
        public PropertyNamePosition()
        {
            Position = -1;
            Name = "";
        }
        public int Position { get; set; }
        public string Name { get; set; }

        public bool Equals(PropertyNamePosition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Position == other.Position && string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PropertyNamePosition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}