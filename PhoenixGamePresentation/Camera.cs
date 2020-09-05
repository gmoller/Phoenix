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
    public class Camera : IDisposable
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

        public PointI CameraFocusCellInWorld
        {
            get
            {
                var hexOffsetCoordinates = HexOffsetCoordinates.FromPixel((int)CameraFocusPointInWorld.X, (int)CameraFocusPointInWorld.Y);

                return new PointI(hexOffsetCoordinates.Col, hexOffsetCoordinates.Row);
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

        public Camera(WorldView worldView, Rectangle viewport, CameraClampMode clampMode, InputHandler input)
        {
            _worldView = worldView;
            _viewport = viewport;
            _clampMode = clampMode;

            Zoom = 1.0f;
            CameraFocusPointInWorld = Vector2.Zero;
            //_rotation = 0.0f;

            input.AddCommandHandler("Camera", 0, new KeyboardInputAction(Keys.OemTilde, KeyboardInputActionType.Released, ResetZoom));
            input.AddCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.WheelUp, IncreaseZoom));
            input.AddCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.WheelDown, DecreaseZoom));
            input.AddCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.RightButtonDrag, DragCamera));
            input.AddCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.Moved, MoveCamera));
            _input = input;
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
        public void LookAtCell(PointI hexPoint)
        {
            var newPosition = HexOffsetCoordinates.ToPixel(hexPoint.X, hexPoint.Y); // in world
            CameraFocusPointInWorld = newPosition.ToVector2();
        }

        /// <summary>
        /// Center camera on pixel.
        /// </summary>
        /// <param name="newPosition"></param>
        public void LookAtPixel(PointI newPosition)
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

        public void Update(float deltaTime)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap)
            {
                CameraFocusPointInWorld = ClampCamera(Zoom);
                return;
            }

            CameraFocusPointInWorld = ClampCamera(Zoom);
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

        #region Event Handlers

        private void ResetZoom(object sender, EventArgs e)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            Zoom = 1.0f;
        }

        private void IncreaseZoom(object sender, EventArgs e)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            Zoom += 0.05f;
        }

        private void DecreaseZoom(object sender, EventArgs e)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            Zoom -= 0.05f;
        }

        private void DragCamera(object sender, EventArgs e)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            var mouseEventArgs = (MouseEventArgs)e;

            var panCameraDistance = mouseEventArgs.MouseMovement.ToVector2();

            MoveCamera(panCameraDistance);

            // TODO: adjust speed depending on zoom level
        }

        private void MoveCamera(object sender, EventArgs e)
        {
            if (_worldView.GameStatus != GameStatus.OverlandMap) return;

            var mouseEventArgs = (MouseEventArgs)e;

            var panCameraDistance = IsMouseIsAtTopOfScreen(mouseEventArgs) ? new Vector2(0.0f, -1.0f) * mouseEventArgs.DeltaTime : Vector2.Zero;
            panCameraDistance += MouseIsAtBottomOfScreen(mouseEventArgs) ? new Vector2(0.0f, 1.0f) * mouseEventArgs.DeltaTime : Vector2.Zero;
            panCameraDistance += MouseIsAtLeftOfScreen(mouseEventArgs) ? new Vector2(-1.0f, 0.0f) * mouseEventArgs.DeltaTime : Vector2.Zero;
            panCameraDistance += MouseIsAtRightOfScreen(mouseEventArgs) ? new Vector2(1.0f, 0.0f) * mouseEventArgs.DeltaTime : Vector2.Zero;

            MoveCamera(panCameraDistance);

            // TODO: adjust speed depending on zoom level
        }

        private bool IsMouseIsAtTopOfScreen(MouseEventArgs mouseEventArgs)
        {
            return mouseEventArgs.Y < 20.0f && mouseEventArgs.Y >= 0.0f;
        }

        public bool MouseIsAtBottomOfScreen(MouseEventArgs mouseEventArgs)
        {
            return mouseEventArgs.Y > 1080 - 20.0f && mouseEventArgs.Y <= 1080.0f;
        }

        public bool MouseIsAtLeftOfScreen(MouseEventArgs mouseEventArgs)
        {
            return mouseEventArgs.X < 20.0f && mouseEventArgs.X >= 0.0f;;
        }

        public bool MouseIsAtRightOfScreen(MouseEventArgs mouseEventArgs)
        {
            return mouseEventArgs.X > 1670.0f - 20.0f && mouseEventArgs.X <= 1670.0f;
        }

        private void MoveCamera(Vector2 movePosition)
        {
            var newPosition = CameraFocusPointInWorld + movePosition;

            CameraFocusPointInWorld = newPosition;
        }

        #endregion

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)
                _input.RemoveCommandHandler("Camera", 0, new KeyboardInputAction(Keys.OemTilde, KeyboardInputActionType.Released, ResetZoom));
                _input.RemoveCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.WheelUp, IncreaseZoom));
                _input.RemoveCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.WheelDown, DecreaseZoom));
                _input.RemoveCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.RightButtonDrag, DragCamera));
                _input.RemoveCommandHandler("Camera", 0, new MouseInputAction(MouseInputActionType.Moved, MoveCamera));

                // TODO: set large fields to null

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}