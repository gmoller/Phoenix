﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;

namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private HexGrid _hexGrid;
        private Label _btnEndTurn;

        public OverlandMap(Camera camera)
        {
            _hexGrid = new HexGrid(Constants.WORLD_MAP_WIDTH_IN_HEXES, Constants.WORLD_MAP_HEIGHT_IN_HEXES, camera);
        }

        public void LoadContent(ContentManager content)
        {
            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");
            AssetsManager.Instance.AddTexture("reg_button_n", "Textures\\reg_button_n");

            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            var texture = AssetsManager.Instance.GetTexture("reg_button_n");
            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            _btnEndTurn = new Label(font, pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue, null, null, texture);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _hexGrid.Update(gameTime, input);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _hexGrid.Draw(spriteBatch);
            _btnEndTurn.Draw(spriteBatch);
        }
    }
}