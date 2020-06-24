using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameLogic;
using HexLibrary;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class UnitView
    {
        private readonly WorldView _worldView;

        internal Unit Unit { get; set; }

        internal UnitView(WorldView worldView, Unit unit)
        {
            _worldView = worldView;
            Unit = unit;
        }

        internal void LoadContent(ContentManager content)
        {
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            if (input.IsLeftMouseButtonReleased && Unit.IsSelected)
            {
                var currentHex = Unit.Location;
                var hexToMoveTo = GetHexPoint();

                if (IsWithinOneHexOf(currentHex, hexToMoveTo))
                {
                    var cellToMoveTo = Globals.Instance.World.OverlandMap.CellGrid.GetCell(hexToMoveTo.X, hexToMoveTo.Y);
                    int movementCost = Globals.Instance.TerrainTypes[cellToMoveTo.TerrainTypeId].MovementCostWalking;

                    if (Unit.MovementPoints >= movementCost)
                    {
                        // move unit
                        Unit.Location = hexToMoveTo;
                        Unit.MovementPoints -= movementCost;
                    }
                } 
            }

            if (input.IsRightMouseButtonReleased)
            {
                if (CursorIsOnThisUnit())
                {
                    // TODO: show in hudview
                    Globals.Instance.MessageQueue.Enqueue(new SelectUnitCommand());

                    var hexPoint = GetHexPoint();
                    var worldPixelLocation = HexOffsetCoordinates.OffsetCoordinatesToPixel(hexPoint.X, hexPoint.Y);
                    _worldView.Camera.LookAt(worldPixelLocation);
                }
            }
        }

        private bool IsWithinOneHexOf(Utilities.Point currentHex, Utilities.Point hexToMoveTo)
        {
            var neighbors = HexOffsetCoordinates.GetAllNeighbors(currentHex.X, currentHex.Y);

            foreach (var neighbor in neighbors)
            {
                if (neighbor.Col == hexToMoveTo.X && neighbor.Row == hexToMoveTo.Y)
                {
                    return true;
                }
            }

            return false;
            //return neighbors.Includes(hexToMoveTo);
        }

        private bool CursorIsOnThisUnit()
        {
            var hexPoint = GetHexPoint();

            return Unit.Location == hexPoint;
        }

        private Utilities.Point GetHexPoint()
        {
            var hex = DeviceManager.Instance.WorldHex;
            var hexPoint = new Utilities.Point(hex.X, hex.Y);

            return hexPoint;
        }

        internal void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (!Unit.IsSelected)
            {
                DrawUnit(spriteBatch, texture);
            }
            else if (!Unit.Blink)
            {
                DrawUnit(spriteBatch, texture);
            }
        }

        private void DrawUnit(SpriteBatch spriteBatch, Texture2D texture)
        {
            var position = HexOffsetCoordinates.OffsetCoordinatesToPixel(Unit.Location.X, Unit.Location.Y);
            var destinationRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
            var sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.Navy, 0.0f, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0.0f);
        }
    }
}