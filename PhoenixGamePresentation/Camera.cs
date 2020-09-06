using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Input;
using Hex;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGamePresentation.Events;
using PhoenixGamePresentation.Views;
using Utilities;
using Utilities.ExtensionMethods;

namespace PhoenixGamePresentation
{
    public class Camera : IDisposable
    {
        #region State
        internal WorldView WorldView { get; }
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
                _zoom = MathHelper.Clamp(value, 0.5f, 2.0f);
                CalculateNumberOfHexesFromCenter(_viewport, _zoom);
            }
        }

        public int NumberOfHexesToLeft { get; private set; }
        public int NumberOfHexesToRight { get; private set; }
        public int NumberOfHexesAbove { get; private set; }
        public int NumberOfHexesBelow { get; private set; }

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        public Matrix Transform => GetTransform();

        public HexOffsetCoordinates CameraFocusHexInWorld
        {
            get
            {
                var hexOffsetCoordinates = HexOffsetCoordinates.FromPixel(CameraFocusPointInWorld.X, CameraFocusPointInWorld.Y);

                return hexOffsetCoordinates;
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

        public PointI CameraTopLeftHex
        {
            get
            {
                //var centerHex = CameraFocusHexInWorld;

                //var fromColumn = centerHex.Col - NumberOfHexesToLeft;
                //fromColumn = Math.Max(0, fromColumn);

                //var fromRow = centerHex.Row - NumberOfHexesAbove;
                //fromRow = Math.Max(0, fromRow);

                //return new PointI(fromColumn, fromRow);

                var cameraRectangle = CameraRectangleInWorld;
                var hexTopLeft = WorldPixelToWorldHex(new PointI(cameraRectangle.Left, cameraRectangle.Top));

                return hexTopLeft.ToPointI();
            }
        }

        public PointI CameraBottomRightHex
        {
            get
            {
                //var centerHex = CameraFocusHexInWorld;

                //var toColumn = centerHex.Col + NumberOfHexesToRight;
                //toColumn = Math.Min(PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS, toColumn);

                //var toRow = centerHex.Row + NumberOfHexesBelow;
                //toRow = Math.Min(PhoenixGameLibrary.Constants.WORLD_MAP_ROWS, toRow);

                //return new PointI(toColumn, toRow);

                var cameraRectangle = CameraRectangleInWorld;
                var hexBottomRight = WorldPixelToWorldHex(new PointI(cameraRectangle.Right, cameraRectangle.Bottom));

                return hexBottomRight.ToPointI();
            }
        }

        public Camera(WorldView worldView, Rectangle viewport, CameraClampMode clampMode, InputHandler input)
        {
            WorldView = worldView;
            _viewport = viewport;
            _clampMode = clampMode;

            Zoom = 1.0f;
            CameraFocusPointInWorld = Vector2.Zero;
            //_rotation = 0.0f;

            input.SubscribeToEventHandler("Camera", 0, this, Keys.OemTilde, KeyboardInputActionType.Released, ResetZoomEvent.HandleEvent);
            input.SubscribeToEventHandler("Camera", 0, this, MouseInputActionType.WheelUp, IncreaseZoomEvent.HandleEvent);
            input.SubscribeToEventHandler("Camera", 0, this, MouseInputActionType.WheelDown, DecreaseZoomEvent.HandleEvent);
            input.SubscribeToEventHandler("Camera", 0, this, MouseInputActionType.RightButtonDrag, DragCameraEvent.HandleEvent);
            input.SubscribeToEventHandler("Camera", 0, this, MouseInputActionType.Moved, MoveCameraEvent.HandleEvent);
            _input = input;
        }

        internal void LoadContent(ContentManager content)
        {
        }

        #region ToScreenPixel

        /// <summary>
        /// Translates the center position of a hex in the world to a position on the screen.
        /// </summary>
        /// <param name="worldHex"></param>
        /// <returns></returns>
        public Vector2 WorldHexToScreenPixel(HexOffsetCoordinates worldHex)
        {
            var worldPosition = HexOffsetCoordinates.ToPixel(worldHex);
            var screenPosition = WorldPixelToScreenPixel(worldPosition.ToVector2());

            return screenPosition;
        }

        /// <summary>
        /// Translates the position in the world to a position on the screen.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldPixelToScreenPixel(Point worldPosition)
        {
            return WorldPixelToScreenPixel(worldPosition.ToVector2());
        }

        /// <summary>
        /// Translates the position in the world to a position on the screen.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldPixelToScreenPixel(PointI worldPosition)
        {
            return WorldPixelToScreenPixel(worldPosition.ToVector2());
        }

        /// <summary>
        /// Translates the position in the world to a position on the screen.
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector2 WorldPixelToScreenPixel(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Transform);
        }

        #endregion

        #region ToWorldPixel

        /// <summary>
        /// Translates the position on the screen to it's position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenPixelToWorldPixel(Point screenPosition)
        {
            return ScreenPixelToWorldPixel(screenPosition.ToVector2());
        }

        /// <summary>
        /// Translates the position on the screen to it's position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenPixelToWorldPixel(PointI screenPosition)
        {
            return ScreenPixelToWorldPixel(screenPosition.ToVector2());
        }

        /// <summary>
        /// Translates the position on the screen to it's position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public Vector2 ScreenPixelToWorldPixel(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
        }

        #endregion

        #region ToWorldHex

        /// <summary>
        /// Translates the position on the screen to it's hex position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public HexOffsetCoordinates ScreenPixelToWorldHex(Point screenPosition)
        {
            return ScreenPixelToWorldHex(screenPosition.ToVector2());
        }

        /// <summary>
        /// Translates the position on the screen to it's hex position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public HexOffsetCoordinates ScreenPixelToWorldHex(PointI screenPosition)
        {
            return ScreenPixelToWorldHex(screenPosition.ToVector2());
        }

        /// <summary>
        /// Translates the position on the screen to it's hex position in the world.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <returns></returns>
        public HexOffsetCoordinates ScreenPixelToWorldHex(Vector2 screenPosition)
        {
            var worldPosition = ScreenPixelToWorldPixel(screenPosition);
            var worldHex = HexOffsetCoordinates.FromPixel(worldPosition.X, worldPosition.Y);

            return worldHex;
        }

        public HexOffsetCoordinates WorldPixelToWorldHex(PointI worldPosition)
        {
            var screenPixel = WorldPixelToScreenPixel(worldPosition);
            var worldHex = ScreenPixelToWorldHex(screenPixel);

            return worldHex;
        }

        #endregion

        #region LookAtCell

        /// <summary>
        /// Center camera on cell.
        /// </summary>
        /// <param name="hexPoint"></param>
        public void LookAtCell(HexOffsetCoordinates hexPoint)
        {
            LookAtCell(hexPoint.ToPointI());
        }

        /// <summary>
        /// Center camera on cell.
        /// </summary>
        /// <param name="hexPoint"></param>
        public void LookAtCell(Point hexPoint)
        {
            LookAtCell(hexPoint.ToPointI());
        }

        /// <summary>
        /// Center camera on cell.
        /// </summary>
        /// <param name="hexPoint"></param>
        public void LookAtCell(PointI hexPoint)
        {
            var newPosition = HexOffsetCoordinates.ToPixel(hexPoint);
            CameraFocusPointInWorld = newPosition.ToVector2();
        }

        #endregion

        #region LookAtPixel

        /// <summary>
        /// Center camera on position in the world.
        /// </summary>
        /// <param name="newPosition"></param>
        public void LookAtPixel(Point newPosition)
        {
            LookAtPixel(newPosition.ToVector2());
        }

        /// <summary>
        /// Center camera on position in the world.
        /// </summary>
        /// <param name="newPosition"></param>
        public void LookAtPixel(PointI newPosition)
        {
            LookAtPixel(newPosition.ToVector2());
        }

        /// <summary>
        /// Center camera on position in the world.
        /// </summary>
        /// <param name="newPosition"></param>
        public void LookAtPixel(Vector2 newPosition)
        {
            CameraFocusPointInWorld = newPosition;
        }

        #endregion

        #region LookAtCellPointedAtByMouse

        /// <summary>
        /// Center camera on mouse location.
        /// </summary>
        /// <param name="mouseLocation"></param>
        public void LookAtCellPointedAtByMouse(Point mouseLocation)
        {
            LookAtCell(mouseLocation.ToPointI());
        }

        #endregion

        private void CalculateNumberOfHexesFromCenter(Rectangle viewport, float zoom)
        {
            NumberOfHexesToLeft = (int)(Math.Ceiling(viewport.Width / Hex.Constants.HexWidth * (1 / zoom) * Constants.ONE_HALF)) + 1;
            NumberOfHexesToRight = NumberOfHexesToLeft;
            NumberOfHexesAbove = (int)(Math.Ceiling(viewport.Height / Hex.Constants.HexThreeQuarterHeight * (1 / zoom) * Constants.ONE_HALF)) + 1;
            NumberOfHexesBelow = NumberOfHexesAbove;
        }

        public void Update(float deltaTime)
        {
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

        internal void MoveCamera(Vector2 movePosition)
        {
            var newPosition = CameraFocusPointInWorld + movePosition;

            CameraFocusPointInWorld = newPosition;
        }

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)
                _input.UnsubscribeAllFromEventHandler("Camera");

                // TODO: set large fields to null

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}