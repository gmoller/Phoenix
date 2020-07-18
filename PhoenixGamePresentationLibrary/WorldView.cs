using HexLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class WorldView
    {
        private readonly World _world;

        private OverlandMapView _overlandMapView;
        private OverlandSettlementsView _overlandSettlementsView;
        private UnitsView _unitsView;
        private SettlementsView _settlementsView;
        private HudView _hudView;

        public Camera Camera { get; private set; }

        internal WorldView(World world)
        {
            _world = world;
        }

        internal void LoadContent(ContentManager content)
        {
            _overlandMapView = new OverlandMapView(this, _world.OverlandMap);
            _overlandSettlementsView = new OverlandSettlementsView(this, _world.Settlements);
            _unitsView = new UnitsView(this, _world.Units);
            _settlementsView = new SettlementsView(_world.Settlements);
            _hudView = new HudView(_unitsView);

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LoadContent(content);
            Camera.LookAtPixel(new Vector2(800.0f, 400.0f));

            _overlandSettlementsView.LoadContent(content);
            _unitsView.LoadContent(content);
            _settlementsView.LoadContent(content);
            _hudView.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            var worldPos = Camera.ScreenToWorld(new Vector2(input.MousePostion.X, input.MousePostion.Y));
            DeviceManager.Instance.WorldPosition = new Utilities.Point((int)worldPos.X, (int)worldPos.Y);
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel((int)worldPos.X, (int)worldPos.Y);
            DeviceManager.Instance.WorldHexPointedAtByMouseCursor = new Utilities.Point(worldHex.Col, worldHex.Row);

            Camera.Update(input, deltaTime);

            _overlandMapView.Update(input, deltaTime);
            _overlandSettlementsView.Update(input, deltaTime);
            _unitsView.Update(input, deltaTime);
            _settlementsView.Update(input, deltaTime);
            _hudView.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            _overlandMapView.Draw(spriteBatch);
            _overlandSettlementsView.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            _unitsView.Draw(spriteBatch);
            spriteBatch.End();

            _hudView.Draw(spriteBatch);

            _settlementsView.Draw(spriteBatch);
        }
    }
}