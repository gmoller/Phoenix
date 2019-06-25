using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using AssetsLibrary;
using GuiControls;
using Utilities;

namespace PhoenixGameLibrary
{
    public class OverlandMap
    {
        private CellGrid _cellGrid;
        private Button _btnEndTurn;

        public OverlandMap(Camera camera)
        {
            _cellGrid = new CellGrid(Constants.WORLD_MAP_COLUMNS, Constants.WORLD_MAP_ROWS, camera);
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
            AssetsManager.Instance.AddTexture("reg_button_a", "Textures\\reg_button_a");
            AssetsManager.Instance.AddTexture("reg_button_h", "Textures\\reg_button_h");

            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            var textureNormal = AssetsManager.Instance.GetTexture("reg_button_n");
            var textureActive = AssetsManager.Instance.GetTexture("reg_button_a");
            var textureHover = AssetsManager.Instance.GetTexture("reg_button_h");
            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var label = new Label(font, pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), "Next Turn", HorizontalAlignment.Center, Color.White, Color.Blue);
            _btnEndTurn = new Button(pos, HorizontalAlignment.Right, VerticalAlignment.Bottom, new Vector2(245.0f, 56.0f), textureNormal, textureActive, textureHover, label);
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            _cellGrid.Update(gameTime, input);
            _btnEndTurn.Update(gameTime);
        }

        public void Draw()
        {
            _cellGrid.Draw();
            _btnEndTurn.Draw();
        }
    }
}