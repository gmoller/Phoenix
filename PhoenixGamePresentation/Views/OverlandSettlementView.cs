using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Hex;
using Input;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Events;

namespace PhoenixGamePresentation.Views
{
    public class OverlandSettlementView : IDisposable
    {
        #region State
        internal WorldView WorldView { get; }

        private Texture2D _texture;
        //private Texture2D _textures;
        //private AtlasSpec _atlas;

        public Settlement Settlement { get; set; }

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        public OverlandSettlementView(WorldView worldView, InputHandler input)
        {
            WorldView = worldView;

            input.SubscribeToEventHandler("OverlandSettlementView", 0, this, MouseInputActionType.RightButtonPressed, OpenSettlementEvent.HandleEvent);
            _input = input;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = AssetsManager.Instance.GetTexture("VillageSmall00");
            //_textures = AssetsManager.Instance.GetTexture("GUI_Textures_1");
            //_atlas = AssetsManager.Instance.GetAtlas("GUI_Textures_1");
        }

        public void Update(float deltaTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cellGrid = WorldView.World.OverlandMap.CellGrid;
            var cell = cellGrid.GetCell(Settlement.Location.X, Settlement.Location.Y);
            DrawSettlement(spriteBatch, cell);
        }

        private void DrawSettlement(SpriteBatch spriteBatch, Cell cell)
        {
            var position = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Hex.Constants.HexActualWidth * Constants.ONE_HALF), (int)(Hex.Constants.HexActualHeight * 0.75f));
            var sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // dispose managed state (managed objects)
                _input.UnsubscribeAllFromEventHandler("OverlandSettlementView");

                // set large fields to null
                _texture = null;
                Settlement = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}