using Microsoft.Xna.Framework;
using GameLogic;
using Utilities;
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
                var message = queue.Dequeue();

                if (message == "EndTurn")
                {
                    World.EndTurn();
                }
                else if (message == "OpenSettlement")
                {
                    World.IsInSettlementView = true;

                    var settlement = World.Settlements[0];
                    World.Settlement = settlement; // TODO: get by settlementId
                    var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(settlement.Location.X, settlement.Location.Y);
                    World.Camera.LookAt(worldPixelLocation);
                }
                else if (message == "CloseSettlement")
                {
                    World.IsInSettlementView = false;
                }
            }
        }
    }
}