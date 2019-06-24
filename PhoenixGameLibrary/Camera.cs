﻿using Microsoft.Xna.Framework;

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

        public Camera(Rectangle viewport)
        {
            _bounds = viewport;
            _zoom = 1.0f;
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

        public void UpdateCamera(GameTime gameTime, InputHandler input)
        {
            MoveCamera(new Vector2(-input.PanCameraDistance.X * 5.0f, -input.PanCameraDistance.Y * 5.0f));
            ZoomCamera(input);
            ClampCamera();

            UpdateMatrix();
        }

        private void ZoomCamera(InputHandler input)
        {
            if (input.CameraZoomIn)
            {
                AdjustZoom(0.05f);
            }
            if (input.CameraZoomOut)
            {
                AdjustZoom(-0.05f);
            }
        }

        private void AdjustZoom(float zoomAmount)
        {
            _zoom += zoomAmount;
            if (_zoom < 0.1f) // 0.35f
            {
                _zoom = 0.1f;
            }
            if (_zoom > 5.0f) // 2.0f
            {
                _zoom = 5.0f;
            }
        }

        private void ClampCamera()
        {
            // TODO: scale not taken into account!
            _position.X = MathHelper.Clamp(_position.X, _bounds.Center.X, Constants.WORLD_MAP_WIDTH_IN_PIXELS - _bounds.Center.X);
            _position.Y = MathHelper.Clamp(_position.Y, _bounds.Center.Y, Constants.WORLD_MAP_HEIGHT_IN_PIXELS - _bounds.Center.Y);
        }

        private void MoveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = _position + movePosition;

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

            VisibleArea = new Rectangle((int)min.X - (int)HexLibrary.Constants.HEX_WIDTH, (int)min.Y - 96, (int)(max.X - min.X) + (int)HexLibrary.Constants.HEX_WIDTH * 2, (int)(max.Y - min.Y) + 192); // 60, 92
        }
    }
}