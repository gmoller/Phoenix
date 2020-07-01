using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PhoenixGamePresentationLibrary
{
    public class Camera
    {
        private readonly Rectangle _viewport;

        private float _zoom;
        private float _rotation;
        private Vector2 _position;

        public Rectangle VisibleArea { get; private set; }
        public Matrix Transform { get; private set; }
        public int Width => _viewport.Width;
        public int Height => _viewport.Height;

        public Camera(Rectangle viewport)
        {
            _viewport = viewport;
        }

        internal void LoadContent(ContentManager content)
        {
            _zoom = 1.0f;
            _rotation = 0.0f;
            _position = Vector2.Zero;
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Transform);
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
        }

        public void LookAt(Vector2 newPosition)
        {
            _position = newPosition;
        }

        public void MoveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = _position + movePosition;

            _position = newPosition;
        }

        public void AdjustZoom(float zoomAmount)
        {
            _zoom += zoomAmount;
            _zoom = MathHelper.Clamp(_zoom, 0.35f, 2.0f); // 0.1 - 5.0f
        }

        public void Update(InputHandler input, float deltaTime)
        {
            var zoom = input.MouseWheelUp ? 0.05f : 0.0f;
            zoom = input.MouseWheelDown ? -0.05f : zoom;
            AdjustZoom(zoom);

            var panCameraDistance = input.IsLeftMouseButtonDown && input.HasMouseMoved ? input.MouseMovement.ToVector2() : Vector2.Zero;
            MoveCamera(panCameraDistance);

            panCameraDistance = input.MouseIsAtTopOfScreen ? new Vector2(0.0f, -2.0f) * deltaTime : Vector2.Zero;
            MoveCamera(panCameraDistance);
            panCameraDistance = input.MouseIsAtBottomOfScreen ? new Vector2(0.0f, 2.0f) * deltaTime : Vector2.Zero;
            MoveCamera(panCameraDistance);
            panCameraDistance = input.MouseIsAtLeftOfScreen ? new Vector2(-2.0f, 0.0f) * deltaTime : Vector2.Zero;
            MoveCamera(panCameraDistance);
            panCameraDistance = input.MouseIsAtRightOfScreen ? new Vector2(2.0f, 0.0f) * deltaTime : Vector2.Zero;
            MoveCamera(panCameraDistance);

            ClampCamera();
            UpdateMatrix();
        }

        private void ClampCamera()
        {
            // TODO: scale not taken into account!
            _position.X = MathHelper.Clamp(_position.X, _viewport.Center.X, Constants.WORLD_MAP_WIDTH_IN_PIXELS - _viewport.Center.X);
            _position.Y = MathHelper.Clamp(_position.Y, _viewport.Center.Y, Constants.WORLD_MAP_HEIGHT_IN_PIXELS - _viewport.Center.Y);
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
                        Matrix.CreateRotationZ(_rotation) *
                        Matrix.CreateScale(_zoom) *
                        Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0));
            UpdateVisibleArea();
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

            VisibleArea = new Rectangle((int)min.X - (int)HexLibrary.Constants.HEX_WIDTH, (int)min.Y - 96, (int)(max.X - min.X) + (int)HexLibrary.Constants.HEX_WIDTH * 2, (int)(max.Y - min.Y) + 192); // 60, 92
        }
    }
}