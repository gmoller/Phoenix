using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Hex;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    public class OverlandSettlementView : IDisposable
    {
        #region State
        private readonly WorldView _worldView;

        private Texture2D _texture;
        //private Texture2D _textures;
        //private AtlasSpec _atlas;

        public Settlement Settlement { get; set; }

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        public OverlandSettlementView(WorldView worldView, InputHandler input)
        {
            _worldView = worldView;

            input.AddCommandHandler("OverlandSettlementView", 0, new MouseInputAction(MouseButtons.RightButton, MouseInputActionType.ButtonPressed, OpenSettlement));
            _input = input;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            //_textures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            //_atlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");
        }

        public void Update(InputHandler input, float deltaTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cellGrid = _worldView.World.OverlandMap.CellGrid;
            var cell = cellGrid.GetCell(Settlement.Location.X, Settlement.Location.Y);
            DrawSettlement(spriteBatch, cell);
        }

        private bool CursorIsOnThisSettlement(Settlement settlement)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            var hexToMoveTo = context.WorldHexPointedAtByMouseCursor;
            var cursorIsOnThisSettlement = settlement.Location == hexToMoveTo;

            return cursorIsOnThisSettlement;
        }

        private void DrawSettlement(SpriteBatch spriteBatch, Cell cell)
        {
            var position = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Hex.Constants.HexActualWidth * Constants.ONE_HALF), (int)(Hex.Constants.HexActualHeight * 0.75f));
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Microsoft.Xna.Framework.Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        #region Event Handlers

        private void OpenSettlement(object sender, EventArgs e)
        {
            if (_worldView.GameStatus == GameStatus.OverlandMap && CursorIsOnThisSettlement(Settlement))
            {
                Command openSettlementCommand = new OpenSettlementCommand { Payload = (Settlement, _worldView.World.Settlements) };
                openSettlementCommand.Execute();

                _worldView.Camera.LookAtCell(Settlement.Location);

                _worldView.GameStatus = GameStatus.CityView;
            }
        }

        #endregion

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)
                _input.RemoveCommandHandler("OverlandSettlementView", 0, new MouseInputAction(MouseButtons.RightButton, MouseInputActionType.ButtonPressed, OpenSettlement));

                // TODO: set large fields to null
                _texture = null;
                Settlement = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}