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
        private OverlandMapView _overlandMapView;
        private OverlandSettlementsView _overlandSettlementsView;
        private UnitsStacksView _unitsStacksView;
        private SettlementsView _settlementsView;
        private HudView _hudView;

        public Camera Camera { get; private set; }
        public World World { get; }

        internal WorldView(World world)
        {
            World = world;
        }

        internal void LoadContent(ContentManager content)
        {
            _overlandMapView = new OverlandMapView(this, World.OverlandMap);
            _overlandSettlementsView = new OverlandSettlementsView(this, World.Settlements);
            _unitsStacksView = new UnitsStacksView(this, World.UnitsStacks);
            _settlementsView = new SettlementsView(World.Settlements);
            _hudView = new HudView(this, _unitsStacksView);

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LoadContent(content);

            _overlandSettlementsView.LoadContent(content);
            _unitsStacksView.LoadContent(content);
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
            _unitsStacksView.Update(input, deltaTime);
            _settlementsView.Update(input, deltaTime);
            _hudView.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform); // FrontToBack
            _overlandMapView.Draw(spriteBatch);
            _overlandSettlementsView.Draw(spriteBatch);

            _unitsStacksView.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            _hudView.Draw(spriteBatch);
            _settlementsView.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void BeginTurn()
        {
            Command beginTurnCommand = new BeginTurnCommand { Payload = World };
            beginTurnCommand.Execute();

            _unitsStacksView[0].SelectStack();
        }

        public void EndTurn()
        {
            Command endTurnCommand = new EndTurnCommand
            {
                Payload = World
            };
            endTurnCommand.Execute();
            if (World.UnitsStacks.Count != _unitsStacksView.Count)
            {
                //_unitsStacksView.Realign();
            }

            BeginTurn();
        }
    }
}