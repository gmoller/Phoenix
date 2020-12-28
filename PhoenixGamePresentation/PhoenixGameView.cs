using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Views;
using Zen.Assets;
using Zen.Input;
using Zen.Utilities;

namespace PhoenixGamePresentation
{
    public class PhoenixGameView
    {
        #region State
        private WorldView WorldView { get; }
        private CursorView CursorView { get; }
        private InputHandler Input { get; }
        #endregion

        public PhoenixGameView(PhoenixGame phoenixGame)
        {
            Input = new InputHandler();
            WorldView = new WorldView(phoenixGame.World, CameraClampMode.NoClamp, Input);
            CursorView = new CursorView(WorldView, Input);
        } 

        public void LoadContent(ContentManager content)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            var graphicsDevice = context.GraphicsDevice;
            var pixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { new Color(Color.Black, 180) });

            ContentLoader.LoadContent($@"{Directory.GetCurrentDirectory()}\Content\", "Fonts", "Textures", "TextureAtlases", graphicsDevice);
            var foo = AssetsManager.Instance.GetSpriteFont("DarkXShadowX21s Skyrim Font");

            AssetsManager.Instance.AddTexture("TransparentBackground", pixel);

            //http://www.iconian.com/index.html
            AssetsManager.Instance.ContentManager = content;
            AssetsManager.Instance.AddSpriteFont("DarkXShadowX21s Skyrim Font-12", "Fonts\\DarkXShadowX21s Skyrim Font-12");
            AssetsManager.Instance.AddSpriteFont("DarkXShadowX21s Skyrim Font-18", "Fonts\\DarkXShadowX21s Skyrim Font-18");
            AssetsManager.Instance.AddSpriteFont("DarkXShadowX21s Skyrim Font-24", "Fonts\\DarkXShadowX21s Skyrim Font-24");
            AssetsManager.Instance.AddSpriteFont("DarkXShadowX21s Skyrim Font-36", "Fonts\\DarkXShadowX21s Skyrim Font-36");
            AssetsManager.Instance.AddSpriteFont("Arial-12", "Fonts\\Arial-12");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-6", "Fonts\\Maleficio-Regular-6");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-8", "Fonts\\Maleficio-Regular-8");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-12", "Fonts\\Maleficio-Regular-12");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-18", "Fonts\\Maleficio-Regular-18");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-24", "Fonts\\Maleficio-Regular-24");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-36", "Fonts\\Maleficio-Regular-36");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-48", "Fonts\\Maleficio-Regular-48");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-60", "Fonts\\Maleficio-Regular-60");
            AssetsManager.Instance.AddSpriteFont("Maleficio-Regular-72", "Fonts\\Maleficio-Regular-72");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-6", "Fonts\\Carolingia-Regular-6");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-8", "Fonts\\Carolingia-Regular-8");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-12", "Fonts\\Carolingia-Regular-12");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-18", "Fonts\\Carolingia-Regular-18");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-24", "Fonts\\Carolingia-Regular-24");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-36", "Fonts\\Carolingia-Regular-36");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-48", "Fonts\\Carolingia-Regular-48");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-60", "Fonts\\Carolingia-Regular-60");
            AssetsManager.Instance.AddSpriteFont("Carolingia-Regular-72", "Fonts\\Carolingia-Regular-72");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-6", "Fonts\\CrimsonText-Regular-6");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-8", "Fonts\\CrimsonText-Regular-8");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-12", "Fonts\\CrimsonText-Regular-12");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-18", "Fonts\\CrimsonText-Regular-18");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-24", "Fonts\\CrimsonText-Regular-24");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-36", "Fonts\\CrimsonText-Regular-36");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-48", "Fonts\\CrimsonText-Regular-48");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-60", "Fonts\\CrimsonText-Regular-60");
            AssetsManager.Instance.AddSpriteFont("CrimsonText-Regular-72", "Fonts\\CrimsonText-Regular-72");

            AssetsManager.Instance.AddTexture("Cursor", "Textures\\cursor");
            AssetsManager.Instance.AddTexture("VillageSmall00", "Textures\\villageSmall00");
            AssetsManager.Instance.AddTexture("brutal-helm", "Textures\\brutal-helm");
            AssetsManager.Instance.AddTexture("NoiseTexture", "Textures\\noiseTexture");

            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_1", "TextureAtlases\\terrain_hextiles_basic_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_basic_2", "TextureAtlases\\terrain_hextiles_basic_2");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_1", "TextureAtlases\\terrain_hextiles_cold_1");
            AssetsManager.Instance.AddTexture("terrain_hextiles_cold_2", "TextureAtlases\\terrain_hextiles_cold_2");
            AssetsManager.Instance.AddTexture("NewTerrain", "TextureAtlases\\NewTerrain");

            AssetsManager.Instance.AddTexture("GUI_Textures_1", "TextureAtlases\\GUI_Textures_1");
            AssetsManager.Instance.AddTexture("Icons_1", "TextureAtlases\\Icons_1");
            AssetsManager.Instance.AddTexture("Buildings", "TextureAtlases\\Buildings");
            AssetsManager.Instance.AddTexture("Citizens", "TextureAtlases\\Citizens");
            AssetsManager.Instance.AddTexture("Units", "TextureAtlases\\Units");
            AssetsManager.Instance.AddTexture("MovementTypes", "TextureAtlases\\MovementTypes");
            AssetsManager.Instance.AddTexture("Squares", "TextureAtlases\\Squares");
            AssetsManager.Instance.AddTexture("Squares_Transparent", "TextureAtlases\\Squares_Transparent");

            WorldView.LoadContent(content);
            CursorView.LoadContent(content);

            WorldView.BeginTurn();
        }

        public void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            Input.Update(WorldView, deltaTime);
            WorldView.Update(gameTime);
            CursorView.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            WorldView.Draw(spriteBatch);
            CursorView.Draw(spriteBatch);
        }
    }
}