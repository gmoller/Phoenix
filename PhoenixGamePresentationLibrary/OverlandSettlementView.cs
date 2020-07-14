using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary
{
    public class OverlandSettlementView
    {
        private readonly WorldView _worldView;

        private Texture2D _texture;
        //private Texture2D _textures;
        //private AtlasSpec2 _atlas;

        public Settlement Settlement { get; set; }

        public OverlandSettlementView(WorldView worldView)
        {
            _worldView = worldView;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            //_textures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            //_atlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");
        }

        public void Update(InputHandler input, float deltaTime)
        {
            if (input.IsRightMouseButtonReleased && CursorIsOnThisSettlement(Settlement))
            {
                Command openSettlementCommand = new OpenSettlementCommand();
                openSettlementCommand.Payload = Settlement;
                Globals.Instance.MessageQueue.Enqueue(openSettlementCommand);

                var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(Settlement.Location.X, Settlement.Location.Y);
                _worldView.Camera.LookAt(worldPixelLocation);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            var cell = cellGrid.GetCell(Settlement.Location.X, Settlement.Location.Y);
            DrawSettlement(spriteBatch, cell);
        }

        private bool CursorIsOnThisSettlement(Settlement settlement)
        {
            var hexPoint = GetHexPoint();

            return settlement.Location == hexPoint;
        }

        private Point GetHexPoint()
        {
            var hex = DeviceManager.Instance.WorldHex;
            var hexPoint = new Point(hex.X, hex.Y);

            return hexPoint;
        }

        private void DrawSettlement(SpriteBatch spriteBatch, Cell cell)
        {
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(HexLibrary.Constants.HEX_ACTUAL_WIDTH * 0.5f), (int)(HexLibrary.Constants.HEX_ACTUAL_HEIGHT * 0.75f));
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);

            //var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(cell.Column, cell.Row);
            //var layerDepth = cell.Index / 10000.0f + 0.00001f;
            //var size = new Vector2((float)(HexLibrary.Constants.HEX_ACTUAL_WIDTH * 0.5f), HexLibrary.Constants.HEX_ACTUAL_HEIGHT * 0.75f);
            //var imgSettlement = new Image("imgSettlement", position - PhoenixGameLibrary.Constants.HEX_ORIGIN / 2 + new Vector2(10.0f, 0.0f), size, "VillageSmall00", layerDepth);
            //imgSettlement.Draw();
        }
    }
}