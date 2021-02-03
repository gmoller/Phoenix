using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixGameData.GameData
{
    public class ActionConfig
    {
        private GameConfigRepository _gameConfigRepository;

        private readonly dynamic _action;

        public int Id => (int)_action.Id;
        public string Name => (string)_action.Name;
        public string ButtonName => (string)_action.ButtonName;
        public bool AppliesToAll => (bool)_action.AppliesToAll;

        public ActionConfig(int actionId)
        {
            _gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");
            var actions = _gameConfigRepository.GetEntities("Action");

            _action = actions.GetById(actionId);
        }
    }
}