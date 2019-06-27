using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using HexLibrary;
using Utilities;

namespace PhoenixGameLibrary
{
    public class World
    {
        private readonly Camera _camera;
        private readonly OverlandMap _overlandMap;
        private readonly Settlements _settlements;

        public World()
        {
            _camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            _camera.LookAt(new Vector2(800.0f, 400.0f));
            _overlandMap = new OverlandMap(this, _camera);
            _settlements = new Settlements(_camera);
        }

        public void LoadContent(ContentManager content)
        {
            _overlandMap.LoadContent(content);
            _settlements.AddSettlement("Fairhaven", new Point(12, 9), content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _camera.UpdateCamera(gameTime, input);
            _overlandMap.Update(gameTime, input);
            _settlements.Update(gameTime, input);

            var worldPos = _camera.ScreenToWorld(new Vector2(input.MousePostion.X, input.MousePostion.Y));
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