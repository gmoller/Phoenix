using PhoenixGameConfig;
using Zen.Utilities;

namespace PhoenixGameData.GameData
{
    public class MovementConfig
    {
        private readonly GameConfigRepository _gameConfigRepository;

        private readonly dynamic _movement;

        public int Id => (int)_movement.Id;
        public string Name => _movement.Name;
        public float IncrementSightBy { get; }

        public MovementConfig(int movementId)
        {
            _gameConfigRepository = CallContext<GameConfigRepository>.GetData("GameConfigRepository");

            var movements = _gameConfigRepository.GetEntities("Movement");

            _movement = movements.GetById(movementId);

            IncrementSightBy = DetermineIncrementSightBy(movementId);
        }

        private float DetermineIncrementSightBy(int movementId)
        {
            var movementIncrementSightBys = _gameConfigRepository.GetEntities("MovementIncrementSightBy");
            var filtered = movementIncrementSightBys.Filter("MovementId", movementId);

            var amt = 0;
            foreach (var item in filtered)
            {
                var val = (int)item.Value;
                if (val > amt)
                {
                    amt = val;
                }
            }

            return amt;
        }
    }
}