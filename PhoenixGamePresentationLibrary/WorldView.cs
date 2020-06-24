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
        private readonly SettlementView.SettlementView _settlementView;
        private readonly UnitsView _unitsView;
        private readonly HudView _hudView;

        public Camera Camera { get; }

        internal WorldView(World world)
        {
            _world = world;
            _overlandMapView = new OverlandMapView(this, world.OverlandMap);
            _settlementView = new SettlementView.SettlementView();
            _unitsView = new UnitsView(this, world.Units);
            _hudView = new HudView();

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LookAt(new Vector2(800.0f, 400.0f));
        }

        internal void LoadContent(ContentManager content)
        {
            _settlementView.LoadContent(content);
            _unitsView.LoadContent(content);
            _hudView.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            if (!_world.IsInSettlementView)
            {
                var zoom = input.MouseWheelUp ? 0.05f : 0.0f;
                zoom = input.MouseWheelDown ? -0.05f : zoom;
                Camera.AdjustZoom(zoom);
                var panCameraDistance = input.IsLeftMouseButtonDown && input.HasMouseMoved ? input.MouseMovement.ToVector2() : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);

                panCameraDistance = input.MouseIsAtTopOfScreen ? new Vector2(0.0f, -2.0f) * deltaTime : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
                panCameraDistance = input.MouseIsAtBottomOfScreen ? new Vector2(0.0f, 2.0f) * deltaTime : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
                panCameraDistance = input.MouseIsAtLeftOfScreen ? new Vector2(-2.0f, 0.0f) * deltaTime : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
                panCameraDistance = input.MouseIsAtRightOfScreen ? new Vector2(2.0f, 0.0f) * deltaTime : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
            }
            Camera.Update(input, deltaTime);

            _overlandMapView.Update(input);
            _unitsView.Update(input, deltaTime);
            _hudView.Update(deltaTime);

            if (_world.IsInSettlementView)
            {
                _settlementView.Settlement = _world.Settlement;
                _settlementView.Update(deltaTime, input);
            }

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

            if (_world.IsInSettlementView)
            {
                _settlementView.Draw(spriteBatch);
            }
        }
    }
}