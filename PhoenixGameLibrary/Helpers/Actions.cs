using PhoenixGameData;
using Zen.Utilities;

namespace PhoenixGameLibrary.Helpers
{
    public static class Actions
    {
        private static DynamicDataList _actionsThatApplyToAll;

        public static DynamicDataList GetActionsThatApplyToAll()
        {
            if (_actionsThatApplyToAll is null)
            {
                var list = DynamicDataList.Create("Actions");

                var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
                var actionIds = gameConfigCache.GetActionConfigIds();

                foreach (var actionId in actionIds)
                {
                    var action = gameConfigCache.GetActionConfigById(actionId);
                    if (action.AppliesToAll)
                    {
                        list.Add(action);
                    }
                }

                _actionsThatApplyToAll = list;
            }

            return _actionsThatApplyToAll;
        }
    }
}