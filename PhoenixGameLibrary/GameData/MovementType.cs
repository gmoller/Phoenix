using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct MovementType : IIdentifiedByIdAndName
    {
        #region State
        public int Id { get; }
        public string Name { get; }
        public int IncrementSightBy { get; }
        #endregion

        private MovementType(int id, string name, int incrementSightBy)
        {
            Id = id;
            Name = name;
            IncrementSightBy = incrementSightBy;
        }

        public static MovementType Create(int id, string name, int incrementSightBy)
        {
            return new MovementType(id, name, incrementSightBy);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class MovementTypesLoader
    {
        public static NamedDataDictionary<MovementType> Load()
        {
            var movementTypes = new List<MovementType>
            {
                MovementType.Create(0, "Walking", 0),
                MovementType.Create(1, "Swimming", 0),
                MovementType.Create(2, "Flying", 1),
                MovementType.Create(3, "Sailing", 0),
                MovementType.Create(4, "Forester", 0),
                MovementType.Create(5, "Mountaineer", 0),
                MovementType.Create(6, "Pathfinding", 0),
                MovementType.Create(7, "PlaneShift", 0)
            };

            return NamedDataDictionary<MovementType>.Create(movementTypes);
        }
    }
}