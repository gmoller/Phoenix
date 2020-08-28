using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Input;
using Hex;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Views;
using Utilities;
using Utilities.ExtensionMethods;
using MathHelper = Microsoft.Xna.Framework.MathHelper;

namespace PhoenixGamePresentation
{
    public class Camera
    {
        #region State
        private readonly WorldView _worldView;

        private readonly Rectangle _viewport;

        private float _rotation;
        private Vector2 _centerPosition;

        public Matrix Transform { get; private set; }
        public float Zoom { get; private set; }

        public int NumberOfHexesToLeft { get; private set; }
        public int NumberOfHexesToRight { get; private set; }
        public int NumberOfHexesAbove { get; private set; }
        public int NumberOfHexesBelow { get; private set; }
        #endregion

        public Vector2 CameraPostionInWorld => _centerPosition;
        public Vector2 CameraTopLeftPostionInWorld => new Vector2(_centerPosition.X - Width * Constants.ONE_HALF, _centerPosition.Y - Height * Constants.ONE_HALF);
        public Vector2 CameraTopRightPostionInWorld => new Vector2(_centerPosition.X + Width * Constants.ONE_HALF, _centerPosition.Y - Height * Constants.ONE_HALF);
        public Vector2 CameraBottomLeftPostionInWorld => new Vector2(_centerPosition.X - Width * Constants.ONE_HALF, _centerPosition.Y + Height * Constants.ONE_HALF);
        public Vector2 CameraBottomRightPostionInWorld => new Vector2(_centerPosition.X + Width * Constants.ONE_HALF, _centerPosition.Y + Height * Constants.ONE_HALF);
        public Rectangle WorldViewport => new Rectangle((int)CameraTopLeftPostionInWorld.X, (int)CameraTopLeftPostionInWorld.Y, _viewport.Width, _viewport.Height);

        public int Width => _viewport.Width;
        public int Height => _viewport.Height;

        public Camera(WorldView worldView, Rectangle viewport)
        {
            _worldView = worldView;
            _viewport = viewport;

            _centerPosition = Vector2.Zero;
            _rotation = 0.0f;
            Zoom = 1.0f;
            CalculateNumberOfHexesFromCenter(viewport, Zoom);
        }

        internal void LoadContent(ContentManager content)
        {
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Transform);
        }

        /// <summary>
        /// Translates the position of a vector to it's position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
        }

        public void LookAtCellPointedAtByMouse()
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            var hexPoint = context.WorldHexPointedAtByMouseCursor;
            var newPosition = HexOffsetCoordinates.ToPixel(hexPoint.X, hexPoint.Y);
            _centerPosition = newPosition.ToVector2();
        }

        /// <summary>
        /// Center camera on cell.
        /// </summary>
        /// <param name="hexPoint"></param>
        public void LookAtCell(Utilities.Point hexPoint)
        {
            var newPosition = HexOffsetCoordinates.ToPixel(hexPoint.X, hexPoint.Y); // in world
            _centerPosition = newPosition.ToVector2();
        }

        /// <summary>
        /// Center camera on pixel.
        /// </summary>
        /// <param name="newPosition"></param>
        public void LookAtPixel(Vector2 newPosition)
        {
            _centerPosition = newPosition;
        }

        private void CalculateNumberOfHexesFromCenter(Rectangle viewport, float zoom)
        {
            NumberOfHexesToLeft = (int)(Math.Ceiling(viewport.Width / Hex.Constants.HexWidth * (1 / zoom)) * Constants.ONE_HALF) + 1;
            NumberOfHexesToRight = NumberOfHexesToLeft;
            NumberOfHexesAbove = (int)(Math.Ceiling(viewport.Height / Hex.Constants.HexThreeQuarterHeight * (1 / zoom)) * Constants.ONE_HALF) + 1;
            NumberOfHexesBelow = NumberOfHexesAbove;
        }

        public void Update(InputHandler input, float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap)
            {
                ClampCamera(Zoom);
                UpdateMatrix();
                return;
            }

            // Causes
            var zoomAmount = input.MouseWheelUp ? 0.05f : 0.0f;
            zoomAmount += input.MouseWheelDown ? -0.05f : zoomAmount;
            var resetZoom = input.IsKeyReleased(Keys.OemTilde);

            var panCameraDistance = input.IsRightMouseButtonDown && input.HasMouseMoved ? input.MouseMovement.ToVector2() : Vector2.Zero;
            // TODO: adjust speed depending on zoom level
            panCameraDistance += input.MouseIsAtTopOfScreen ? new Vector2(0.0f, -1.0f) * deltaTime : Vector2.Zero;
            panCameraDistance += input.MouseIsAtBottomOfScreen ? new Vector2(0.0f, 1.0f) * deltaTime : Vector2.Zero;
            panCameraDistance += input.MouseIsAtLeftOfScreen ? new Vector2(-1.0f, 0.0f) * deltaTime : Vector2.Zero;
            panCameraDistance += input.MouseIsAtRightOfScreen ? new Vector2(1.0f, 0.0f) * deltaTime : Vector2.Zero;

            // Actions
            ResetZoom(resetZoom);
            AdjustZoom(zoomAmount);
            MoveCamera(panCameraDistance);

            ClampCamera(Zoom);
            UpdateMatrix();

            // Status change?
        }

        private void ResetZoom(bool resetZoom)
        {
            if (resetZoom)
            {
                Zoom = 1.0f;
                CalculateNumberOfHexesFromCenter(_viewport, Zoom);
            }
        }

        private void AdjustZoom(float zoomAmount)
        {
            if (zoomAmount.AboutEquals(0.0f)) return;

            Zoom += zoomAmount;
            Zoom = MathHelper.Clamp(Zoom, 0.35f, 5.0f);

            CalculateNumberOfHexesFromCenter(_viewport, Zoom);
        }

        private void MoveCamera(Vector2 movePosition)
        {
            var newPosition = _centerPosition + movePosition;

            _centerPosition = newPosition;
        }

        private void ClampCamera(float zoom)
        {
            _centerPosition.X = MathHelper.Clamp(_centerPosition.X, _viewport.Center.X * (1 / zoom), Constants.WORLD_MAP_WIDTH_IN_PIXELS - _viewport.Center.X * (1 / zoom));
            _centerPosition.Y = MathHelper.Clamp(_centerPosition.Y, _viewport.Center.Y * (1 / zoom), Constants.WORLD_MAP_HEIGHT_IN_PIXELS - _viewport.Center.Y * (1 / zoom));
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-_centerPosition.X, -_centerPosition.Y, 0.0f)) *
                        Matrix.CreateRotationZ(_rotation) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateTranslation(new Vector3(_viewport.Width * Constants.ONE_HALF, _viewport.Height * Constants.ONE_HALF, 0.0f));
        }

        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(_viewport.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, _viewport.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(_viewport.Width, _viewport.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
        }
    }
}