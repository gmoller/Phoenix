using GameLogic;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }

        public PhoenixGame()
        {
            World = new World();

            World.AddStartingSettlement();
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