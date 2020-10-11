using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Assets;
using GuiControls;
using GuiControls.PackagesClasses;
using Hex;
using Input;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGamePresentation.Events;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation.Views
{
    internal class OverlandMapView : ViewBase, IDisposable
    {
        private static readonly HexLibrary HexLibrary = new HexLibrary(HexType.PointyTopped, OffsetCoordinatesType.Odd);

        #region State
        private OverlandMap OverlandMap { get; }

        private Label Test { get; }
        #endregion

        internal OverlandMapView(WorldView worldView, OverlandMap overlandMap, InputHandler input)
        {
            WorldView = worldView;
            OverlandMap = overlandMap;

            Test = new LabelSized(new Vector2(0.0f, 1080.0f), Alignment.BottomLeft, new Vector2(50.0f, 50.0f), Alignment.TopRight, "Test", "CrimsonText-Regular-12", Color.Red, "test", null, Color.Blue);
            Test.AddPackage(new ControlClick((o, args) => Test.MoveTopLeftPosition(new PointI(10, -10))));

            SetupViewport(0, 0, WorldView.Camera.GetViewport.Width, WorldView.Camera.GetViewport.Height);

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "OverlandMapView");
            Input.Register(0, this, Keys.Enter, KeyboardInputActionType.Released, (sender, e) => { worldView.EndTurn(); });
            Input.Register(1, this, Keys.D1, KeyboardInputActionType.Released, (sender, e) => { WorldView.Camera.LookAtPixel(new PointI(840, 540)); }); // for testing
            Input.Register(2, this, Keys.C, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.FocusCameraOn(); });
            Input.Register(3, this, MouseInputActionType.RightButtonPressed, OpenSettlementEvent.HandleEvent);
            Input.Register(4, this, MouseInputActionType.RightButtonPressed, (sender, e) => { WorldView.CurrentlySelectedStackView?.SetPotentialMovement(); });
            Input.Register(5, this, MouseInputActionType.RightButtonReleased, (sender, e) => { WorldView.CurrentlySelectedStackView?.ResetPotentialMovement(); });
            Input.Register(6, this, MouseInputActionType.LeftButtonReleased, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromMouseInitiation(e.Mouse.Location); });
            Input.Register(7, this, Keys.NumPad1, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromKeyboardInitiation(e.Key); });
            Input.Register(8, this, Keys.NumPad3, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromKeyboardInitiation(e.Key); });
            Input.Register(9, this, Keys.NumPad4, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromKeyboardInitiation(e.Key); });
            Input.Register(10, this, Keys.NumPad6, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromKeyboardInitiation(e.Key); });
            Input.Register(11, this, Keys.NumPad7, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromKeyboardInitiation(e.Key); });
            Input.Register(12, this, Keys.NumPad9, KeyboardInputActionType.Released, (sender, e) => { WorldView.CurrentlySelectedStackView?.CheckForUnitMovementFromKeyboardInitiation(e.Key); });
            Input.Register(13, this, MouseInputActionType.RightButtonReleased, (sender, e) => { WorldView.CheckForSelectionOfStack(e.Mouse.Location); });
            Input.Register(14, this, MouseInputActionType.CheckForHoverOver, (sender, e) => { WorldView.CheckIfMouseIsHoveringOverStack(e.Mouse.Location); });
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "OverlandMapView");

            WorldView.SubscribeToStatusChanges("OverlandMapView", worldView.HandleStatusChange);
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
                    else if (cell.IsSeenByPlayer(WorldView.Settlements, WorldView.Stacks))
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
            if (cell.Borders.IsBitSet((byte)Direction.North)) DrawBorder(spriteBatch, cell, Direction.NorthEast, Direction.SouthEast);
            if (cell.Borders.IsBitSet((byte)Direction.NorthEast)) DrawBorder(spriteBatch, cell, Direction.North, Direction.NorthEast);
            if (cell.Borders.IsBitSet((byte)Direction.East)) DrawBorder(spriteBatch, cell, Direction.NorthEast, Direction.SouthEast);
            if (cell.Borders.IsBitSet((byte)Direction.SouthEast)) DrawBorder(spriteBatch, cell, Direction.SouthEast, Direction.South);
            if (cell.Borders.IsBitSet((byte)Direction.South)) DrawBorder(spriteBatch, cell, Direction.NorthEast, Direction.SouthEast);
            if (cell.Borders.IsBitSet((byte)Direction.SouthWest)) DrawBorder(spriteBatch, cell, Direction.South, Direction.SouthWest);
            if (cell.Borders.IsBitSet((byte)Direction.West)) DrawBorder(spriteBatch, cell, Direction.SouthWest, Direction.NorthWest);
            if (cell.Borders.IsBitSet((byte)Direction.NorthWest)) DrawBorder(spriteBatch, cell, Direction.NorthWest, Direction.North);
        }

        private void DrawBorder(SpriteBatch spriteBatch, Cell cell, Direction vertexDirection1, Direction vertexDirection2)
        {
            var centerPosition = HexLibrary.ToPixel(new HexOffsetCoordinates(cell.Column, cell.Row)).ToVector2();
            var point1 = HexLibrary.GetCorner(vertexDirection1).ToVector2();
            var point2 = HexLibrary.GetCorner(vertexDirection2).ToVector2();

            spriteBatch.DrawLine(centerPosition + point1, centerPosition + point2, cell.ControlledByFaction == 1 ? Color.Yellow : Color.Red, 5.0f, 0.5f); // DarkGreen
        }

        private void DrawCell(SpriteBatch spriteBatch, Cell cell, Color color)
        {
            bool neverSeen = cell.SeenState == SeenState.NeverSeen;
            var texture = AssetsManager.Instance.GetTexture(neverSeen ? cell.TextureFogOfWar.TexturePalette  : cell.Texture.TexturePalette);
            var spec = AssetsManager.Instance.GetAtlas(neverSeen ? cell.TextureFogOfWar.TexturePalette : cell.Texture.TexturePalette);
            var frame = spec.Frames[neverSeen ? cell.TextureFogOfWar.TextureId : cell.Texture.TextureId];
            var sourceRectangle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            var centerPosition = HexLibrary.ToPixel(new HexOffsetCoordinates(cell.Column, cell.Row));
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, 111, 192);
            var layerDepth = cell.Index / 10000.0f;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color, 0.0f, Constants.HEX_ORIGIN, SpriteEffects.None, layerDepth);
        }

        private void DrawHexBorder(SpriteBatch spriteBatch, Cell cell)
        {
            var centerPosition = HexLibrary.ToPixel(new HexOffsetCoordinates(cell.Column, cell.Row)).ToVector2();

            var color = Color.PeachPuff;
            var point0 = HexLibrary.GetCorner(Direction.North).ToVector2();
            var point1 = HexLibrary.GetCorner(Direction.NorthEast).ToVector2();
            var point2 = HexLibrary.GetCorner(Direction.SouthEast).ToVector2();
            var point3 = HexLibrary.GetCorner(Direction.South).ToVector2();
            var point4 = HexLibrary.GetCorner(Direction.SouthWest).ToVector2();
            var point5 = HexLibrary.GetCorner(Direction.NorthWest).ToVector2();

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