﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary
{
    public class PhoenixGameView
    {
        private readonly WorldView _worldView;
        private readonly CursorView _cursorView;
        private readonly Cursor _cursor;
        private readonly InputHandler _input;

        public PhoenixGameView(PhoenixGame phoenixGame)
        {
            _worldView = new WorldView(phoenixGame.World);
            _cursor = new Cursor();
            _cursorView = new CursorView(_cursor);

            _input = new InputHandler();
            _input.Initialize();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            ContentLoader.LoadContent(graphicsDevice);
            AssetsManager.Instance.ContentManager = content;
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-12", "Fonts\\Maleficio-Regular-12");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-18", "Fonts\\Maleficio-Regular-18");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-24", "Fonts\\Maleficio-Regular-24");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-36", "Fonts\\Maleficio-Regular-36");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-48", "Fonts\\Maleficio-Regular-48");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-60", "Fonts\\Maleficio-Regular-60");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-72", "Fonts\\Maleficio-Regular-72");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-12", "Fonts\\Carolingia-Regular-12");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-18", "Fonts\\Carolingia-Regular-18");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-24", "Fonts\\Carolingia-Regular-24");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-36", "Fonts\\Carolingia-Regular-36");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-48", "Fonts\\Carolingia-Regular-48");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-60", "Fonts\\Carolingia-Regular-60");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-72", "Fonts\\Carolingia-Regular-72");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-12", "Fonts\\CrimsonText-Regular-12");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-18", "Fonts\\CrimsonText-Regular-18");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-24", "Fonts\\CrimsonText-Regular-24");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-36", "Fonts\\CrimsonText-Regular-36");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-48", "Fonts\\CrimsonText-Regular-48");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-60", "Fonts\\CrimsonText-Regular-60");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-72", "Fonts\\CrimsonText-Regular-72");

            AssetsManager.Instance.AddTexture("Cursor", "Textures\\cursor");
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");

            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");
            AssetsManager.Instance.AddAtlas("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");

            AssetsManager.Instance.AddTexture("GUI_Textures_1", "TextureAtlases\\GUI_Textures_1");
            AssetsManager.Instance.AddAtlas("GUI_Textures_1", "TextureAtlases\\GUI_Textures_1");

            AssetsManager.Instance.AddTexture("Icons_1", "TextureAtlases\\Icons_1");
            AssetsManager.Instance.AddAtlas("Icons_1", "TextureAtlases\\Icons_1");

            AssetsManager.Instance.AddTexture("Buildings", "TextureAtlases\\Buildings");
            AssetsManager.Instance.AddAtlas("Buildings", "TextureAtlases\\Buildings");

            AssetsManager.Instance.AddTexture("Citizens", "TextureAtlases\\Citizens");
            AssetsManager.Instance.AddAtlas("Citizens", "TextureAtlases\\Citizens");

            _worldView.LoadContent(content);
            _cursorView.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            _input.Update(gameTime);
            _worldView.Update(gameTime, _input);
            _cursor.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _worldView.Draw(spriteBatch);
            _cursorView.Draw(spriteBatch);
        }
    }
}