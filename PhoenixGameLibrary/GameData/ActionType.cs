using System.Collections.Generic;
using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct ActionType : IIdentifiedByIdAndName
    {
        public int Id { get; }
        public string Name { get; }
        public string ButtonName { get; }

        private ActionType(int id, string name, string buttonName)
        {
            Id = id;
            Name = name;
            ButtonName = buttonName;
        }

        public static ActionType Create(int id, string name, string buttonName)
        {
            return new ActionType(id, name, buttonName);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class ActionTypesLoader
    {
        public static NamedDataList<ActionType> Load()
        {
            var actionTypes = new List<ActionType>
            {
                ActionType.Create(0, "Done", "Done"),
                ActionType.Create(1, "Patrol", "Patrol"),
                ActionType.Create(2, "Wait", "Wait"),
                ActionType.Create(3, "BuildOutpost", "Build"),
                ActionType.Create(4, "Purify", "Purify"),
            };

            return NamedDataList<ActionType>.Create(actionTypes);
        }
    }
}