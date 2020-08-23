using System.Runtime.Remoting.Messaging;
using AssetsLibrary;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;

namespace PhoenixGamePresentationLibrary.Views
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
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            // Causes
            var openSettlement = input.IsRightMouseButtonReleased && CursorIsOnThisSettlement(Settlement);

            // Actions
            if (openSettlement)
            {
                Command openSettlementCommand = new OpenSettlementCommand { Payload = (Settlement, _worldView.World.Settlements) };
                openSettlementCommand.Execute();

                _worldView.Camera.LookAtCell(Settlement.Location);
            }

            // Status change?
            if (openSettlement)
            {
                _worldView.GameStatus = GameStatus.CityView;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cellGrid = _worldView.World.OverlandMap.CellGrid;
            var cell = cellGrid.GetCell(Settlement.Location.X, Settlement.Location.Y);
            DrawSettlement(spriteBatch, cell);
        }

        private bool CursorIsOnThisSettlement(Settlement settlement)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var hexToMoveTo = context.WorldHexPointedAtByMouseCursor;
            var cursorIsOnThisSettlement = settlement.Location == hexToMoveTo;

            return cursorIsOnThisSettlement;
        }

        private void DrawSettlement(SpriteBatch spriteBatch, Cell cell)
        {
            var position = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(HexLibrary.Constants.HexActualWidth * 0.5f), (int)(HexLibrary.Constants.HexActualHeight * 0.75f));
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }
    }
}