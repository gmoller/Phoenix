using System.Collections.Generic;
using System.Diagnostics;
using Utilities;

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
        public bool AppliesToAll { get; }

        private ActionType(int id, string name, string buttonName, bool appliesToAll)
        {
            Id = id;
            Name = name;
            ButtonName = buttonName;
            AppliesToAll = appliesToAll;
        }

        public static ActionType Create(int id, string name, string buttonName, bool appliesToAll)
        {
            return new ActionType(id, name, buttonName, appliesToAll);
        }

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{Id={Id},Name={Name}}}";
    }

    public static class ActionTypesLoader
    {
        public static NamedDataDictionary<ActionType> Load()
        {
            var actionTypes = new List<ActionType>
            {
                ActionType.Create(0, "Done", "Done", true),
                ActionType.Create(1, "Wait", "Wait", true),
                ActionType.Create(2, "Patrol", "Patrol", true),
                ActionType.Create(3, "Fortify", "Fortify", true),
                ActionType.Create(4, "Explore", "Explore", true),
                ActionType.Create(5, "BuildOutpost", "Build", false),
                ActionType.Create(6, "Purify", "Purify", false)
            };

            return NamedDataDictionary<ActionType>.Create(actionTypes);
        }
    }
}