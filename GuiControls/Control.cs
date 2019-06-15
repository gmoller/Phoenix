using System;
using Microsoft.Xna.Framework;

namespace GuiControls
{
    public abstract class Control
    {
        protected readonly VerticalAlignment VerticalAlignment;
        protected readonly HorizontalAlignment HorizontalAlignment;
        protected readonly Vector2 Position; // Position of the control relative to it's alignment
        protected readonly Vector2 Size;
        protected readonly float Scale;

        protected int ScaledWidth => (int)(Size.X * Scale);
        protected int ScaledHeight => (int)(Size.Y * Scale);

        public Rectangle Area => DetermineArea(VerticalAlignment, HorizontalAlignment, Position, ScaledWidth, ScaledHeight);
        public Vector2 TopLeft => new Vector2(Area.Left, Area.Top);
        public Vector2 TopRight => new Vector2(Area.Right, Area.Top);
        public Vector2 BottomLeft => new Vector2(Area.Left, Area.Bottom);
        public Vector2 BottomRight => new Vector2(Area.Right, Area.Bottom);
        public Vector2 Center => new Vector2(Area.Center.X, Area.Center.Y);

        protected Control(Vector2 position, Vector2 size, VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, float scale)
        {
            Position = position;
            Size = size;
            VerticalAlignment = verticalAlignment;
            HorizontalAlignment = horizontalAlignment;
            Scale = scale;
        }

        private Rectangle DetermineArea(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, Vector2 position, int scaledWidth, int scaledHeight)
        {
            Vector2 topLeftPosition = DetermineTopLeftPosition(verticalAlignment, horizontalAlignment, position, scaledWidth, scaledHeight);

            Rectangle area = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, scaledWidth, scaledHeight);

            return area;
        }

        private Vector2 DetermineTopLeftPosition(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, Vector2 position, int scaledWidth, int scaledHeight)
        {
            Vector2 offset;

            if (verticalAlignment == VerticalAlignment.Top && horizontalAlignment == HorizontalAlignment.Left)
            {
                offset = new Vector2(0.0f, 0.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Top && horizontalAlignment == HorizontalAlignment.Center)
            {
                offset = new Vector2(scaledWidth / 2.0f, 0.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Top && horizontalAlignment == HorizontalAlignment.Right)
            {
                offset = new Vector2(scaledWidth, 0.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Middle && horizontalAlignment == HorizontalAlignment.Left)
            {
                offset = new Vector2(0.0f, scaledHeight / 2.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Middle && horizontalAlignment == HorizontalAlignment.Center)
            {
                offset = new Vector2(scaledWidth / 2.0f, scaledHeight / 2.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Middle && horizontalAlignment == HorizontalAlignment.Right)
            {
                offset = new Vector2(scaledWidth, scaledHeight / 2.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Bottom && horizontalAlignment == HorizontalAlignment.Left)
            {
                offset = new Vector2(0.0f, scaledHeight);
            }
            else if (verticalAlignment == VerticalAlignment.Bottom && horizontalAlignment == HorizontalAlignment.Center)
            {
                offset = new Vector2(scaledWidth / 2.0f, scaledHeight);
            }
            else if (verticalAlignment == VerticalAlignment.Bottom && horizontalAlignment == HorizontalAlignment.Right)
            {
                offset = new Vector2(scaledWidth, scaledHeight);
            }
            else
            {
                throw new NotImplementedException($"{verticalAlignment}{horizontalAlignment} alignment has not been implemented.");
            }

            Vector2 topLeftPosition = position - offset;

            return topLeftPosition;
        }
    }
}
