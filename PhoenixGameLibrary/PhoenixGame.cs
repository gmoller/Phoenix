using GameLogic;
using Utilities;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }

        public PhoenixGame()
        {
            World = new World();

            World.AddStartingSettlement(new Point(12, 9));
            World.AddStartingUnit(new Point(12, 9));
        }

        public void Update(float deltaTime)
        {
            World.Update(deltaTime);

            ProcessMessages();
        }

        private void ProcessMessages()
        {
            var queue = Globals.Instance.MessageQueue;
            while (queue.Count > 0)
            {
                var command = queue.Dequeue();
                command.Execute();
            }
        }
    }
}