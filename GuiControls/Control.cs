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

        protected int Width => (int)Size.X;
        protected int Height => (int)Size.Y;

        public Rectangle Area => DetermineArea(VerticalAlignment, HorizontalAlignment, Position, Width, Height);
        public float Left => Area.Left;
        public float Center => Area.Center.X;
        public float Right => Area.Right;
        public float Top => Area.Top;
        public float Middle => Area.Center.Y;
        public float Bottom => Area.Bottom;
        public Vector2 TopLeft => new Vector2(Area.Left, Area.Top);
        public Vector2 TopRight => new Vector2(Area.Right, Area.Top);
        public Vector2 BottomLeft => new Vector2(Area.Left, Area.Bottom);
        public Vector2 BottomRight => new Vector2(Area.Right, Area.Bottom);
        //public Vector2 Center => new Vector2(Area.Center.X, Area.Center.Y);

        protected Control(Vector2 position, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size)
        {
            Position = position;
            HorizontalAlignment = horizontalAlignment;
            VerticalAlignment = verticalAlignment;
            Size = new Vector2(size.X % 2 == 0 ? size.X : size.X + 1, size.Y % 2 == 0 ? size.Y : size.Y + 1);
        }

        protected Control(Control controlToDockTo, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Vector2 size)
        {
            if (verticalAlignment == VerticalAlignment.Top)
            {
                if (horizontalAlignment == HorizontalAlignment.Left)
                {
                    Position = controlToDockTo.TopLeft;
                    VerticalAlignment = VerticalAlignment.Bottom;
                    HorizontalAlignment = HorizontalAlignment.Right;
                }
                else if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    Position = new Vector2(controlToDockTo.Center, controlToDockTo.Top);
                    VerticalAlignment = VerticalAlignment.Bottom;
                    HorizontalAlignment = HorizontalAlignment.Center;
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    Position = controlToDockTo.TopRight;
                    VerticalAlignment = VerticalAlignment.Bottom;
                    HorizontalAlignment = HorizontalAlignment.Left;
                }
                else
                {
                    throw new Exception($"Docking to {verticalAlignment}{horizontalAlignment} of control has not been implemeneted.");
                }
            }
            else if (verticalAlignment == VerticalAlignment.Middle)
            {
                if (horizontalAlignment == HorizontalAlignment.Left)
                {
                    Position = new Vector2(controlToDockTo.Left, controlToDockTo.Middle);
                    VerticalAlignment = VerticalAlignment.Middle;
                    HorizontalAlignment = HorizontalAlignment.Right;
                }
                else if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    Position = new Vector2(controlToDockTo.Center, controlToDockTo.Middle);
                    VerticalAlignment = VerticalAlignment.Middle;
                    HorizontalAlignment = HorizontalAlignment.Center;
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    Position = new Vector2(controlToDockTo.Right, controlToDockTo.Middle);
                    VerticalAlignment = VerticalAlignment.Middle;
                    HorizontalAlignment = HorizontalAlignment.Left;
                }
                else
                {
                    throw new Exception($"Docking to {verticalAlignment}{horizontalAlignment} of control has not been implemeneted.");
                }
            }
            else if (verticalAlignment == VerticalAlignment.Bottom)
            {
                if (horizontalAlignment == HorizontalAlignment.Left)
                {
                    Position = controlToDockTo.BottomLeft;
                    VerticalAlignment = VerticalAlignment.Top;
                    HorizontalAlignment = HorizontalAlignment.Right;
                }
                else if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    Position = new Vector2(controlToDockTo.Center, controlToDockTo.Bottom);
                    VerticalAlignment = VerticalAlignment.Top;
                    HorizontalAlignment = HorizontalAlignment.Center;
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    Position = controlToDockTo.BottomRight;
                    VerticalAlignment = VerticalAlignment.Top;
                    HorizontalAlignment = HorizontalAlignment.Left;
                }
                else
                {
                    throw new Exception($"Docking to {verticalAlignment}{horizontalAlignment} of control has not been implemeneted.");
                }
            }
            else
            {
                throw new Exception($"Docking to {verticalAlignment}{horizontalAlignment} of control has not been implemeneted.");
            }

            Size = new Vector2(size.X % 2 == 0 ? size.X : size.X + 1, size.Y % 2 == 0 ? size.Y : size.Y + 1);
        }

        private Rectangle DetermineArea(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, Vector2 position, int width, int height)
        {
            Vector2 topLeftPosition = DetermineTopLeftPosition(verticalAlignment, horizontalAlignment, position, width, height);

            Rectangle area = new Rectangle((int)topLeftPosition.X, (int)topLeftPosition.Y, width, height);

            return area;
        }

        private Vector2 DetermineTopLeftPosition(VerticalAlignment verticalAlignment, HorizontalAlignment horizontalAlignment, Vector2 position, int width, int height)
        {
            Vector2 offset;

            if (verticalAlignment == VerticalAlignment.Top && horizontalAlignment == HorizontalAlignment.Left)
            {
                offset = new Vector2(0.0f, 0.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Top && horizontalAlignment == HorizontalAlignment.Center)
            {
                offset = new Vector2(width / 2.0f, 0.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Top && horizontalAlignment == HorizontalAlignment.Right)
            {
                offset = new Vector2(width, 0.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Middle && horizontalAlignment == HorizontalAlignment.Left)
            {
                offset = new Vector2(0.0f, height / 2.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Middle && horizontalAlignment == HorizontalAlignment.Center)
            {
                offset = new Vector2(width / 2.0f, height / 2.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Middle && horizontalAlignment == HorizontalAlignment.Right)
            {
                offset = new Vector2(width, height / 2.0f);
            }
            else if (verticalAlignment == VerticalAlignment.Bottom && horizontalAlignment == HorizontalAlignment.Left)
            {
                offset = new Vector2(0.0f, height);
            }
            else if (verticalAlignment == VerticalAlignment.Bottom && horizontalAlignment == HorizontalAlignment.Center)
            {
                offset = new Vector2(width / 2.0f, height);
            }
            else if (verticalAlignment == VerticalAlignment.Bottom && horizontalAlignment == HorizontalAlignment.Right)
            {
                offset = new Vector2(width, height);
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