using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Assets;
using GuiControls;
using Hex;
using Input;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using Utilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGamePresentation.Events;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class OverlandMapView : IDisposable
    {
        #region State
        private WorldView WorldView { get; } // readonly
        private OverlandMap OverlandMap { get; } // readonly

        private Label Test { get; } // readonly

        private Viewport Viewport { get; set; }
        private ViewportAdapter ViewportAdapter { get; set; }

        private InputHandler Input { get; } // readonly
        private bool IsDisposed { get; set; }
        #endregion End State

        internal OverlandMapView(WorldView worldView, OverlandMap overlandMap, InputHandler input)
        {
            WorldView = worldView;
            OverlandMap = overlandMap;

            Test = new LabelSized(new Vector2(0.0f, 1080.0f), Alignment.BottomLeft, new Vector2(50.0f, 50.0f), Alignment.TopRight, "Test", "CrimsonText-Regular-12", Color.Red, "test", null, Color.Blue);
            Test.Click += delegate { Test.MoveTopLeftPosition(new PointI(10, -10)); };

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "OverlandMapView");
            Input.Register(0, this, Keys.Enter, KeyboardInputActionType.Released, EndTurnEvent.HandleEvent);
            Input.Register(1, this, Keys.D1, KeyboardInputActionType.Released, (sender, e) => { WorldView.Camera.LookAtPixel(new PointI(840, 540)); }); // for testing
            Input.Register(2, this, MouseInputActionType.RightButtonPressed, OpenSettlementEvent.HandleEvent);
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "OverlandMapView");

            WorldView.SubscribeToStatusChanges("OverlandMapView", worldView.HandleStatusChange);
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        internal void LoadContent(ContentManager content)
        {
            Test.LoadContent(content);
        }

        internal void Update(float deltaTime)
        {
            if (WorldView.GameStatus != GameStatus.OverlandMap) return;

            Test.Update(Input, deltaTime, Viewport);
        }

        internal void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.Transform * ViewportAdapter.GetScaleMatrix()); // FrontToBack

            DrawCellGrid(spriteBatch, OverlandMap.CellGrid, WorldView.Camera);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;

            //Test.Draw(spriteBatch);
        }

        private void DrawCellGrid(SpriteBatch spriteBatch, CellGrid cellGrid, Camera camera)
        {
            var fromRow = camera.CameraTopLeftHex.Y - 1;
            var toRow = camera.CameraBottomRightHex.Y + 1;

            var fromColumn = camera.CameraTopLeftHex.X - 1;
            var toColumn = camera.CameraBottomRightHex.X + 1;

            for (var r = fromRow; r <= toRow; r++)
            {
                for (var q = fromColumn; q <= toColumn; q++)
                {
                    var cell = cellGrid.GetCell(q, r);

                    if (cell.SeenState == SeenState.NeverSeen)
                    {
                        DrawCell(spriteBatch, cell, Color.White);
                    }
                    else if (cell.IsSeenByPlayer(WorldView.World))
                    {
                        DrawCell(spriteBatch, cell, Color.White);
                    }  
                    else
                    {
                        DrawCell(spriteBatch, cell, Color.DarkGray);
                    }

                    //DrawHexBorder(spriteBatch, cell);
                    DrawBorders(spriteBatch, cell);
                }
            }
        }

        private void DrawBorders(SpriteBatch spriteBatch, Cell cell)
        {
            if (cell.Borders.IsBitSet((byte)Direction.East)) DrawBorder(spriteBatch, cell, HexVertexDirection.NorthEast,  HexVertexDirection.SouthEast);
            if (cell.Borders.IsBitSet((byte)Direction.SouthEast)) DrawBorder(spriteBatch, cell, HexVertexDirection.SouthEast,  HexVertexDirection.South);
            if (cell.Borders.IsBitSet((byte)Direction.SouthWest)) DrawBorder(spriteBatch, cell, HexVertexDirection.South,  HexVertexDirection.SouthWest);
            if (cell.Borders.IsBitSet((byte)Direction.West)) DrawBorder(spriteBatch, cell, HexVertexDirection.SouthWest,  HexVertexDirection.NorthWest);
            if (cell.Borders.IsBitSet((byte)Direction.NorthWest)) DrawBorder(spriteBatch, cell, HexVertexDirection.NorthWest, HexVertexDirection.North);
            if (cell.Borders.IsBitSet((byte)Direction.NorthEast)) DrawBorder(spriteBatch, cell, HexVertexDirection.North, HexVertexDirection.NorthEast);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Cell cell, HexVertexDirection vertexDirection1, HexVertexDirection vertexDirection2)
        {
            var centerPosition = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row).ToVector2();
            var point1 = Hex.Hex.GetCorner(vertexDirection1).ToVector2();
            var point2 = Hex.Hex.GetCorner(vertexDirection2).ToVector2();

            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, cell.ControlledByFaction == 1 ? Color.DarkGreen : Color.Red, 5.0f, 0.5f);
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Color color)
        {
            bool neverSeen = cell.SeenState == SeenState.NeverSeen;
            var texture = AssetsManager.Instance.GetTexture(neverSeen ? cell.TextureFogOfWar.TexturePalette  : cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(neverSeen ? cell.TextureFogOfWar.TexturePalette : cell.Texture.TexturePalette);
            var frame = spec.Frames[neverSeen ? cell.TextureFogOfWar.TextureId : cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var centerPosition = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row);
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            var layerDepth = cell.Index / 10000.0f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        private void DrawHexBorder(SpriteBatch spriteBatch, Cell cell)
        {
            var centerPosition = HexOffsetCoordinates.ToPixel(cell.Column, cell.Row).ToVector2();

            var color = Color.PeachPuff;
            var point0 = Hex.Hex.GetCorner(HexVertexDirection.North).ToVector2();
            var point1 = Hex.Hex.GetCorner(HexVertexDirection.NorthEast).ToVector2();
            var point2 = Hex.Hex.GetCorner(HexVertexDirection.SouthEast).ToVector2();
            var point3 = Hex.Hex.GetCorner(HexVertexDirection.South).ToVector2();
            var point4 = Hex.Hex.GetCorner(HexVertexDirection.SouthWest).ToVector2();
            var point5 = Hex.Hex.GetCorner(HexVertexDirection.NorthWest).ToVector2();

            spriteBatch.DrawLine(centerPosition + point0, centerPosition + point1, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point2, centerPosition + point3, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point3, centerPosition + point4, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point4, centerPosition + point5, color, 1.0f);
            spriteBatch.DrawLine(centerPosition + point5, centerPosition + point0, color, 1.0f);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("OverlandMapView");

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}