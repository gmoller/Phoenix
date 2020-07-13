using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct MoveType : IIdentifiedByIdAndName
    {
        public int Id { get; }
        public string Name { get; }

        private MoveType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static MoveType Create(int id, string name)
        {
            return new MoveType(id, name);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class MoveTypesLoader
    {
        public static NamedDataList<MoveType> Load()
        {
            var moveTypes = new List<MoveType>
            {
                MoveType.Create(0, "Ground"),
                MoveType.Create(1, "Air"),
                MoveType.Create(2, "Water")
            };

            return NamedDataList<MoveType>.Create(moveTypes);
        }
    }
}