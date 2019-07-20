using Microsoft.Xna.Framework;
using GameLogic;
using HexLibrary;

namespace PhoenixGameLibrary
{
    public class PhoenixGame
    {
        public World World { get; }
        public Cursor Cursor { get; }

        public PhoenixGame()
        {
            World = new World();
            Cursor = new Cursor();

            World.AddStartingSettlement();
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            World.Update(gameTime, input);
            Cursor.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

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