using HexLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class WorldView
    {
        private readonly World _world;

        private readonly OverlandMapView _overlandMapView;
        private readonly UnitsView _unitsView;
        private readonly SettlementsView _settlementsView;
        private readonly HudView _hudView;

        public Camera Camera { get; }

        internal WorldView(World world)
        {
            _world = world;
            _overlandMapView = new OverlandMapView(this, world.OverlandMap);
            _unitsView = new UnitsView(this, world.Units);
            _settlementsView = new SettlementsView(this, world.Settlements);
            _hudView = new HudView();

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LookAt(new Vector2(800.0f, 400.0f));
        }

        internal void LoadContent(ContentManager content)
        {
            _unitsView.LoadContent(content);
            _settlementsView.LoadContent(content);
            _hudView.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            Camera.Update(input, deltaTime);

            _overlandMapView.Update(input, deltaTime);
            _unitsView.Update(input, deltaTime);
            _settlementsView.Update(input, deltaTime);
            _hudView.Update(input, deltaTime);

            var worldPos = Camera.ScreenToWorld(new Vector2(input.MousePostion.X, input.MousePostion.Y));
            DeviceManager.Instance.WorldPosition = new Utilities.Point((int)worldPos.X, (int)worldPos.Y);
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel((int)worldPos.X, (int)worldPos.Y);
            DeviceManager.Instance.WorldHex = new Utilities.Point(worldHex.Col, worldHex.Row);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            _overlandMapView.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            _unitsView.Draw(spriteBatch);
            spriteBatch.End();

            _hudView.Draw(spriteBatch);

            _settlementsView.Draw(spriteBatch);
        }
    }
}