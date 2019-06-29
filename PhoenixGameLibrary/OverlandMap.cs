using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AssetsLibrary;
using GuiControls;
using Utilities;
using System;

namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private readonly World _world;
        private Button _btnEndTurn;

        public CellGrid CellGrid { get; }

        public OverlandMap(World world, Camera camera)
        {
            _world = world;
            CellGrid = new CellGrid(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS, camera);
        }

        public void LoadContent(ContentManager content)
        {
            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var label = new Label(font, pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue);
            _btnEndTurn = new Button(pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_h", label);
            _btnEndTurn.Click += btnEndTurnClick;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            CellGrid.Update(gameTime, input);
            _btnEndTurn.Update(gameTime);
        }

        public void Draw()
        {
            CellGrid.Draw();
            _btnEndTurn.Draw();
        }
        private void btnEndTurnClick(object sender, EventArgs e)
        {
            _world.EndTurn();
        }
    }
}