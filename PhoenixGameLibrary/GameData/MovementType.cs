using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct MovementType : IIdentifiedByIdAndName
    {
        public int Id { get; }
        public string Name { get; }

        private MovementType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static MovementType Create(int id, string name)
        {
            return new MovementType(id, name);
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class MovementTypesLoader
    {
        public static NamedDataList<MovementType> Load()
        {
            var movementTypes = new List<MovementType>
            {
                MovementType.Create(0, "Walking"),
                MovementType.Create(1, "Swimming"),
                MovementType.Create(2, "Flying"),
                MovementType.Create(3, "Sailing"),
                MovementType.Create(4, "Forester"),
                MovementType.Create(5, "Mountaineer"),
                MovementType.Create(6, "Pathfinding"),
                MovementType.Create(7, "Plane Shift")
            };

            return NamedDataList<MovementType>.Create(movementTypes);
        }
    }
}