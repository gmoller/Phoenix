using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using Zen.Assets;
using Zen.Input;
using Zen.Hexagons;

namespace PhoenixGamePresentation.Views
{
    internal class OverlandSettlementView : ViewBase, IDisposable
    {
        #region State
        private Texture2D Texture { get; set; }
        public Settlement Settlement { get; set; }
        #endregion End State

        public OverlandSettlementView(WorldView worldView, InputHandler input)
        {
            WorldView = worldView;

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "OverlandSettlementView");
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "OverlandSettlementView");

            WorldView.SubscribeToStatusChanges("OverlandSettlementView", worldView.HandleStatusChange);
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
            var position = WorldView.HexLibrary.FromOffsetCoordinatesToPixel(new HexOffsetCoordinates(cell.Column, cell.Row));
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, (int)WorldView.HexLibrary.Width, (int)WorldView.HexLibrary.Height + 64);
            var sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            var layerDepth = cell.Index / 10000.0f + 0.00001f;
            var origin = new Vector2(sourceRectangle.Width * 0.5f, (sourceRectangle.Height * 2 / 3.0f + sourceRectangle.Height * 1 / 3.0f + sourceRectangle.Height * 1 / 3.0f) * 0.5f);
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0.0f, origin, SpriteEffects.None, layerDepth);
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