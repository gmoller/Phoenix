using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLogic;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        private readonly OverlandMap _overlandMap;
        private readonly Settlements _settlements;

        public Camera Camera { get; }

        public World()
        {
            Globals.Instance.World = this;

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LookAt(new Vector2(800.0f, 400.0f));
            _overlandMap = new OverlandMap(this, Camera);
            _settlements = new Settlements(Camera);
        }

        public void LoadContent(ContentManager content)
        {
            _overlandMap.LoadContent(content);
            _settlements.AddSettlement("Fairhaven", Globals.Instance.RaceTypes[0], new Point(12, 9), _overlandMap.CellGrid, content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            Camera.UpdateCamera(gameTime, input);
            _overlandMap.Update(gameTime, input);
            _settlements.Update(gameTime, input);

            var worldPos = Camera.ScreenToWorld(new Vector2(input.MousePostion.X, input.MousePostion.Y));
            DeviceManager.Instance.WorldPosition = new Point((int)worldPos.X, (int)worldPos.Y);
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel((int)worldPos.X, (int)worldPos.Y);
            DeviceManager.Instance.WorldHex = new Point(worldHex.Col, worldHex.Row);
        }

        public void Draw()
        {
            _overlandMap.Draw();
            _settlements.Draw();
        }

        public void EndTurn()
        {
            _settlements.EndTurn();
        }
    }
}