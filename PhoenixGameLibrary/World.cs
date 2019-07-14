using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using GameLogic;
using GuiControls;
using HexLibrary;
using Utilities;
using PhoenixGameLibrary.Views;
using Microsoft.Xna.Framework.Graphics;

namespace PhoenixGameLibrary
{
    public class World
    {
        private readonly Settlements _settlements;
        private HudView _hud;
        private Button _btnEndTurn;

        public OverlandMap OverlandMap { get; }
        public Faction PlayerFaction { get; }

        public Camera Camera { get; }

        public bool CanScrollMap { get; set; }

        public World()
        {
            Globals.Instance.World = this;

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LookAt(new Vector2(800.0f, 400.0f));
            PlayerFaction = new Faction();
            OverlandMap = new OverlandMap(this);
            _settlements = new Settlements();
        }

        public void LoadContent(ContentManager content)
        {
            OverlandMap.LoadContent(content);
            _settlements.AddSettlement("Fairhaven", "Nomads", new Point(12, 9), OverlandMap.CellGrid, content);

            _hud = new HudView();
            _hud.LoadContent(content);

            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var label = new Label("lblNextTurn", "CrimsonText-Regular-12", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue);
            _btnEndTurn = new Button("btnEndTurn", pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_h", label);
            _btnEndTurn.Click += btnEndTurnClick;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            if (CanScrollMap)
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

            _btnEndTurn.Update(gameTime);
            Globals.Instance.World.CanScrollMap = !_btnEndTurn.MouseOver;

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

            _hud.Draw();
            _settlements.DrawSettlement();
            _btnEndTurn.Draw();
        }

        public void EndTurn()
        {
            _settlements.EndTurn();
        }

        private void btnEndTurnClick(object sender, EventArgs e)
        {
            OverlandMap.EndTurn();
        }
    }
}