using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using Hex;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentation.Views
{
    public class OverlandSettlementView : IDisposable
    {
        #region State
        private WorldView WorldView { get; }

        private Texture2D Texture { get; set; }

        public Settlement Settlement { get; set; }

        private InputHandler Input { get; }
        private bool IsDisposed { get; set; }
        #endregion End State

        public OverlandSettlementView(WorldView worldView, InputHandler input)
        {
            WorldView = worldView;

            Input = input;
        }

        public void LoadContent(ContentManager content)
        {
            Texture = AssetsManager.Instance.GetTexture("VillageSmall00");
        }

        public void Update(float deltaTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var cellGrid = WorldView.CellGrid;
            var cell = cellGrid.GetCell(Settlement.Location.X, Settlement.Location.Y);
            DrawSettlement(spriteBatch, cell);
        }

        private void DrawSettlement(SpriteBatch spriteBatch, Cell cell)
        {
            var position = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)(Hex.Constants.HexActualWidth * Constants.ONE_HALF), (int)(Hex.Constants.HexActualHeight * 0.75f));
            var sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("OverlandSettlementView");

                // set large fields to null
                Texture = null;
                Settlement = null;

                IsDisposed = true;
            }
        }
    }
}