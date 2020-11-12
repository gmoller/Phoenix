using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Zen.Utilities;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public readonly struct ActionType : IIdentifiedByIdAndName
    {
        #region State
        public int Id { get; }
        public string Name { get; }
        public string ButtonName { get; }
        public bool AppliesToAll { get; }
        #endregion End State

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

    public struct ActionTypeForDeserialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ButtonName { get; set; }
        public bool AppliesToAll { get; set; }
    }

    public static class ActionTypesLoader
    {
        public static List<ActionType> Load()
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

            return actionTypes;
        }

        public static List<ActionType> LoadFromJsonFile(string fileName)
        {
            var jsonString = File.ReadAllText($@".\Content\GameMetadata\{fileName}.json");
            var deserialized = JsonSerializer.Deserialize<List<ActionTypeForDeserialization>>(jsonString);
            var list = new List<ActionType>();
            foreach (var item in deserialized)
            {
                list.Add(ActionType.Create(item.Id, item.Name, item.ButtonName, item.AppliesToAll));
            }

            return list;
        }
    }
}