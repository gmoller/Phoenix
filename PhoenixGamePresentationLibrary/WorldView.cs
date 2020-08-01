using HexLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
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
            _unitsView = new UnitsView(this);
            _settlementsView = new SettlementsView(_world.Settlements);
            _hudView = new HudView(this, _unitsView);

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LoadContent(content);

            _overlandSettlementsView.LoadContent(content);
            _unitsView.LoadContent(content);
            _settlementsView.LoadContent(content);
            _hudView.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            Camera.Update(input, deltaTime);

            var worldPosPointedAtByMouseCursor = Camera.ScreenToWorld(new Vector2(input.MousePosition.X, input.MousePosition.Y));
            DeviceManager.Instance.WorldPositionPointedAtByMouseCursor = new Utilities.Point((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
            var worldHex = HexOffsetCoordinates.FromPixel((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
            DeviceManager.Instance.WorldHexPointedAtByMouseCursor = new Utilities.Point(worldHex.Col, worldHex.Row);

            _overlandMapView.Update(input, deltaTime);
            _overlandSettlementsView.Update(input, deltaTime);
            _unitsView.Update(input, deltaTime);
            _settlementsView.Update(input, deltaTime);
            _hudView.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            // FrontToBack
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);
            _overlandMapView.Draw(spriteBatch);
            _overlandSettlementsView.Draw(spriteBatch);

            _unitsView.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            _hudView.Draw(spriteBatch);
            _settlementsView.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void BeginTurn()
        {
            Command beginTurnCommand = new BeginTurnCommand();
            beginTurnCommand.Payload = _world;
            beginTurnCommand.Execute();

            _unitsView.Refresh(_world.Units);
            foreach (var unitView in _unitsView)
            {
                unitView.SelectUnit();
            }
        }

        public void EndTurn()
        {
            Command endTurnCommand = new EndTurnCommand();
            endTurnCommand.Payload = _world;
            endTurnCommand.Execute();

            BeginTurn();
        }
    }
}