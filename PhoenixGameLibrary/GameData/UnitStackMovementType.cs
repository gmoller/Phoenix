using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct UnitStackMovementType : IIdentifiedByIdAndName
    {
        public int Id { get; }
        public string Name { get; }

        private UnitStackMovementType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static UnitStackMovementType Create(int id, string name)
        {
            return new UnitStackMovementType(id, name);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class MovementTypesLoader
    {
        public static NamedDataList<UnitStackMovementType> Load()
        {
            var movementTypes = new List<UnitStackMovementType>
            {
                UnitStackMovementType.Create(0, "Walking"),
                UnitStackMovementType.Create(1, "Swimming"),
                UnitStackMovementType.Create(2, "Flying"),
                UnitStackMovementType.Create(3, "Sailing"),
                UnitStackMovementType.Create(4, "Forester"),
                UnitStackMovementType.Create(5, "Mountaineer"),
                UnitStackMovementType.Create(6, "Pathfinding"),
                UnitStackMovementType.Create(7, "Plane Shift")
            };

            return NamedDataList<UnitStackMovementType>.Create(movementTypes);
        }
    }
}