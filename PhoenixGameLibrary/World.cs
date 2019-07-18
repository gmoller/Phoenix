using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLogic;
using HexLibrary;
using Utilities;
using PhoenixGameLibrary.Views;

namespace PhoenixGameLibrary
{
    public class World
    {
        private readonly Settlements _settlements;
        private HudView _hud;
        private int _turnNumber;

        public OverlandMap OverlandMap { get; }
        public Faction PlayerFaction { get; }
        public string CurrentDate
        {
            get
            {
                var year = 1400 + _turnNumber / 12;
                var month = _turnNumber % 12 + 1;
                var monthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);

                return $"{monthString} {year}";
            }
        }

        public Camera Camera { get; }
        public NotificationList NotificationList { get; }

        public bool IsInSettlementView { get; set; }
        public bool FixedCamera => IsInSettlementView; // || _btnEndTurn.MouseOver;

        public World()
        {
            Globals.Instance.World = this;

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LookAt(new Vector2(800.0f, 400.0f));
            PlayerFaction = new Faction();
            OverlandMap = new OverlandMap(this);
            _settlements = new Settlements();
            _turnNumber = 0;
            NotificationList = new NotificationList();
        }

        public void LoadContent(ContentManager content)
        {
            OverlandMap.LoadContent(content);
            _settlements.AddSettlement("Fairhaven", "Barbarians", new Point(12, 9), OverlandMap.CellGrid, content);

            _hud = new HudView();
            _hud.LoadContent(content);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            if (!FixedCamera)
            {
                var zoom = input.MouseWheelUp ? 0.05f : 0.0f;
                zoom = input.MouseWheelDown ? -0.05f : zoom;
                Camera.AdjustZoom(zoom);
                var panCameraDistance = input.IsLeftMouseButtonDown && input.HasMouseMoved ? input.MouseMovement.ToVector2() : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);

                panCameraDistance = input.MouseIsAtTopOfScreen ? new Vector2(0.0f, -2.0f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
                panCameraDistance = input.MouseIsAtBottomOfScreen ? new Vector2(0.0f, 2.0f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
                panCameraDistance = input.MouseIsAtLeftOfScreen ? new Vector2(-2.0f, 0.0f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
                panCameraDistance = input.MouseIsAtRightOfScreen ? new Vector2(2.0f, 0.0f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds : Vector2.Zero;
                Camera.MoveCamera(panCameraDistance);
            }
            Camera.Update(gameTime, input);

            OverlandMap.Update(gameTime, input);
            _settlements.Update(gameTime, input);
            _hud.Update(gameTime);

            PlayerFaction.FoodPerTurn = _settlements.FoodProducedThisTurn;

            var worldPos = Camera.ScreenToWorld(new Vector2(input.MousePostion.X, input.MousePostion.Y));
            DeviceManager.Instance.WorldPosition = new Point((int)worldPos.X, (int)worldPos.Y);
            var worldHex = HexOffsetCoordinates.OffsetCoordinatesFromPixel((int)worldPos.X, (int)worldPos.Y);
            DeviceManager.Instance.WorldHex = new Point(worldHex.Col, worldHex.Row);
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, Camera.Transform);

            OverlandMap.Draw();
            _settlements.DrawOverland();

            spriteBatch.End();

            _settlements.DrawSettlement(spriteBatch);
            _hud.Draw(spriteBatch);
        }

        public void EndTurn()
        {
            NotificationList.Clear();
            _settlements.EndTurn();
            _turnNumber++;
        }
    }
}