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
using Point = Utilities.Point;

namespace PhoenixGamePresentation
{
    public enum CameraClampMode
    {
        NoClamp,
        AutoClamp,
        ClampOnUpdate
    }

    public class Camera
    {
        #region State
        private readonly WorldView _worldView;
        private readonly Rectangle _viewport;

        private readonly CameraClampMode _clampMode;
        //private float _rotation;

        private Vector2 _cameraFocusPointInWorld;
        public Vector2 CameraFocusPointInWorld
        {
            get => _cameraFocusPointInWorld;
            private set
            {
                _cameraFocusPointInWorld = value;
                if (_clampMode == CameraClampMode.AutoClamp)
                {
                    _cameraFocusPointInWorld = ClampCamera(Zoom);
                }
            }
        }

        private float _zoom;
        public float Zoom
        {
            get => _zoom; 
            set
            {
                _zoom = value;
                _zoom = ClampZoom(Zoom);
                CalculateNumberOfHexesFromCenter(_viewport, _zoom);
            }
        }

        public int NumberOfHexesToLeft { get; private set; }
        public int NumberOfHexesToRight { get; private set; }
        public int NumberOfHexesAbove { get; private set; }
        public int NumberOfHexesBelow { get; private set; }
        #endregion End State

        public Matrix Transform => GetTransform();

        public Point CameraFocusCellInWorld
        {
            get
            {
                var hexOffsetCoordinates = HexOffsetCoordinates.FromPixel((int)CameraFocusPointInWorld.X, (int)CameraFocusPointInWorld.Y);

                return new Point(hexOffsetCoordinates.Col, hexOffsetCoordinates.Row);
            }
        }

        public Rectangle CameraRectangleInWorld
        {
            get
            {
                var frustum = GetBoundingFrustum();
                var rectangle = new Rectangle(frustum.Left.D.Round(), frustum.Top.D.Round(), _viewport.Width, _viewport.Height);

                return rectangle;
            }
        }

        public Camera(WorldView worldView, Rectangle viewport, CameraClampMode clampMode)
        {
            _worldView = worldView;
            _viewport = viewport;
            _clampMode = clampMode;

            Zoom = 1.0f;
            CameraFocusPointInWorld = Vector2.Zero;
            //_rotation = 0.0f;
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
            CameraFocusPointInWorld = newPosition.ToVector2();
        }

        /// <summary>
        /// Center camera on cell.
        /// </summary>
        /// <param name="hexPoint"></param>
        public void LookAtCell(Point hexPoint)
        {
            var newPosition = HexOffsetCoordinates.ToPixel(hexPoint.X, hexPoint.Y); // in world
            CameraFocusPointInWorld = newPosition.ToVector2();
        }

        /// <summary>
        /// Center camera on pixel.
        /// </summary>
        /// <param name="newPosition"></param>
        public void LookAtPixel(Point newPosition)
        {
            CameraFocusPointInWorld = newPosition.ToVector2();
        }

        private void CalculateNumberOfHexesFromCenter(Rectangle viewport, float zoom)
        {
            NumberOfHexesToLeft = (int)(Math.Ceiling(viewport.Width / Hex.Constants.HexWidth * (1 / zoom) * Constants.ONE_HALF)) + 1;
            NumberOfHexesToRight = NumberOfHexesToLeft;
            NumberOfHexesAbove = (int)(Math.Ceiling(viewport.Height / Hex.Constants.HexThreeQuarterHeight * (1 / zoom) * Constants.ONE_HALF)) + 1;
            NumberOfHexesBelow = NumberOfHexesAbove;
        }

        public void Update(InputHandler input, float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap)
            {
                CameraFocusPointInWorld = ClampCamera(Zoom);
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

            CameraFocusPointInWorld = ClampCamera(Zoom);
        }

        private void ResetZoom(bool resetZoom)
        {
            if (!resetZoom) return;

            Zoom = 1.0f;
            CalculateNumberOfHexesFromCenter(_viewport, Zoom);
        }

        private void AdjustZoom(float zoomAmount)
        {
            if (zoomAmount.AboutEquals(0.0f)) return;

            Zoom += zoomAmount;
            Zoom = ClampZoom(Zoom);

            CalculateNumberOfHexesFromCenter(_viewport, Zoom);
        }

        private void MoveCamera(Vector2 movePosition)
        {
            var newPosition = CameraFocusPointInWorld + movePosition;

            CameraFocusPointInWorld = newPosition;
        }

        private Vector2 ClampCamera(float zoom)
        {
            if (_clampMode == CameraClampMode.ClampOnUpdate || _clampMode == CameraClampMode.AutoClamp)
            {
                var x = MathHelper.Clamp(CameraFocusPointInWorld.X, _viewport.Center.X * (1 / zoom),
                    Constants.WORLD_MAP_WIDTH_IN_PIXELS - _viewport.Center.X * (1 / zoom));
                var y = MathHelper.Clamp(CameraFocusPointInWorld.Y, _viewport.Center.Y * (1 / zoom),
                    Constants.WORLD_MAP_HEIGHT_IN_PIXELS - _viewport.Center.Y * (1 / zoom));

                return new Vector2(x, y);
            }

            return CameraFocusPointInWorld;
        }

        private float ClampZoom(float zoom)
        {
            return MathHelper.Clamp(zoom, 0.5f, 2.0f);
        }

        private Matrix GetTransform()
        {
            var transform = Matrix.CreateTranslation(new Vector3(-CameraFocusPointInWorld.X, -CameraFocusPointInWorld.Y, 0.0f)) *
                        //Matrix.CreateRotationZ(_rotation) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateTranslation(new Vector3(_viewport.Width * Constants.ONE_HALF, _viewport.Height * Constants.ONE_HALF, 0.0f));

            return transform;
        }

        private BoundingFrustum GetBoundingFrustum()
        {
            var projectionMatrix = GetProjectionMatrix(Transform);
            var boundingFrustum = new BoundingFrustum(projectionMatrix);

            return boundingFrustum;
        }

        private Matrix GetProjectionMatrix(Matrix viewMatrix)
        {
            var projection = Matrix.CreateOrthographicOffCenter(0, _viewport.Width, _viewport.Height, 0, -1, 0);
            Matrix.Multiply(ref viewMatrix, ref projection, out projection);

            return projection;
        }
    }
}