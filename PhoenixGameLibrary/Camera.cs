using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Input;
using Microsoft.Xna.Framework.Input;

namespace PhoenixGameLibrary
{
    public class Camera
    {
        private float _zoom;
        private readonly float _rotation;
        private readonly Rectangle _bounds;
        private Vector2 _position;

        public Rectangle VisibleArea { get; private set; }
        public Matrix Transform { get; private set; }
        public int Width => _bounds.Width;
        public int Height => _bounds.Height;

        public Camera(Viewport viewport)
        {
            _bounds = viewport.Bounds;
            _zoom = 1.0f;
            _position = Vector2.Zero;
        }

        public void UpdateCamera(GameTime gameTime)
        {
            float moveSpeed = 3.0f - _zoom; // 1.0f
            moveSpeed *= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            //moveSpeed = 0.1f;
            Vector2 cameraMovement = Vector2.Zero;

            if (KeyboardHandler.IsKeyDown(Keys.Up))
            {
                cameraMovement.Y = -moveSpeed;
            }
            if (KeyboardHandler.IsKeyDown(Keys.Down))
            {
                cameraMovement.Y = moveSpeed;
            }
            if (KeyboardHandler.IsKeyDown(Keys.Left))
            {
                cameraMovement.X = -moveSpeed;
            }
            if (KeyboardHandler.IsKeyDown(Keys.Right))
            {
                cameraMovement.X = moveSpeed;
            }
            MoveCamera(cameraMovement);

            if (MouseHandler.MouseWheelUp())
            {
                AdjustZoom(0.05f);
            }
            if (MouseHandler.MouseWheelDown())
            {
                AdjustZoom(-0.05f);
            }

            UpdateMatrix();
        }

        private void AdjustZoom(float zoomAmount)
        {
            _zoom += zoomAmount;
            if (_zoom < 0.35f)
            {
                _zoom = 0.35f;
            }
            if (_zoom > 2.0f)
            {
                _zoom = 2.0f;
            }
        }

        private void MoveCamera(Vector2 movePosition)
        {
            // TODO: scale not taken into account!
            Vector2 newPosition = _position + movePosition;
            newPosition.X = MathHelper.Clamp(newPosition.X, 0.0f, Constants.WORLD_MAP_WIDTH_IN_PIXELS - _bounds.Width);
            newPosition.Y = MathHelper.Clamp(newPosition.Y, 0.0f, Constants.WORLD_MAP_HEIGHT_IN_PIXELS - _bounds.Height);

            _position = newPosition;
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
                        Matrix.CreateRotationZ(_rotation) *
                        Matrix.CreateScale(_zoom) *
                        Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f, _bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(_bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, _bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(_bounds.Width, _bounds.Height), inverseViewMatrix);

            var min = new Vector2(
                MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(
                MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }
    }
}