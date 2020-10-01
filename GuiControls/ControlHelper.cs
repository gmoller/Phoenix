using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Input;
using Microsoft.Xna.Framework.Content;
using Utilities;
using Utilities.ExtensionMethods;

namespace GuiControls
{
    internal static class ControlHelper
    {
        internal static bool IsMouseOverControl(Rectangle actualDestinationRectangle, InputHandler input, Viewport? viewport)
        {
            var mousePosition = GetMousePosition(input, viewport);
            var mouseIsOverControl = actualDestinationRectangle.Contains(mousePosition.X, mousePosition.Y);

            return mouseIsOverControl;
        }

        internal static PointI GetMousePosition(InputHandler input, Viewport? viewport)
        {
            var mousePosition = viewport.HasValue ? new PointI(input.MousePosition.X - viewport.Value.X, input.MousePosition.Y - viewport.Value.Y) : new PointI(input.MousePosition.X, input.MousePosition.Y);

            return mousePosition;
        }

        internal static void LoadChildControls(Dictionary<string, IControl> childControls, ContentManager content)
        {
            foreach (var childControl in childControls.Values)
            {
                childControl.LoadContent(content, true);
            }
        }

        internal static void UpdateChildControls(Dictionary<string, IControl> childControls, InputHandler input, float deltaTime, Viewport? viewport)
        {
            foreach (var childControl in childControls.Values)
            {
                childControl.Update(input, deltaTime, viewport);
            }
        }

        internal static void DrawChildControls(Dictionary<string, IControl> childControls, SpriteBatch spriteBatch)
        {
            foreach (var childControl in childControls.Values)
            {
                childControl.Draw(spriteBatch);
            }
        }

        internal static void DrawChildControls(Dictionary<string, IControl> childControls, Matrix? transform = null)
        {
            foreach (var childControl in childControls.Values)
            {
                childControl.Draw(transform);
            }
        }

        internal static IControl FindControl(string key, Dictionary<string, IControl> childControls)
        {
            var split = key.Split('.');
            var childControl = childControls[split[0]];
            for (var i = 1; i < split.Length; i++)
            {
                childControl = childControl[split[i]];
            }

            return childControl;
        }

        internal static PointI DetermineTopLeft(IControl childControl, Alignment parentAlignment, Alignment childAlignment, PointI offset, Vector2 position, Alignment alignment, PointI size)
        {
            var actualRectangle = DetermineArea(position, alignment, size);

            PointI topLeft;
            switch (parentAlignment)
            {
                case Alignment.TopLeft when childAlignment == Alignment.TopLeft:
                    topLeft = new PointI(actualRectangle.Left, actualRectangle.Top);
                    break;
                case Alignment.TopCenter when childAlignment == Alignment.TopCenter:
                    topLeft = new PointI(actualRectangle.Left + (int)((actualRectangle.Size.X - childControl.Size.X) * 0.5f), actualRectangle.Top);
                    break;
                case Alignment.TopRight when childAlignment == Alignment.TopRight:
                    topLeft = new PointI(actualRectangle.Right - childControl.Size.X, actualRectangle.Top);
                    break;

                case Alignment.MiddleLeft when childAlignment == Alignment.MiddleLeft:
                    topLeft = new PointI(actualRectangle.Left, actualRectangle.Top + (int)((actualRectangle.Size.Y - childControl.Size.Y) * 0.5f));
                    break;
                case Alignment.MiddleCenter when childAlignment == Alignment.MiddleCenter:
                    topLeft = new PointI(actualRectangle.Left + (int)((actualRectangle.Size.X - childControl.Size.X) * 0.5f), actualRectangle.Top + (int)((actualRectangle.Size.Y - childControl.Size.Y) * 0.5f));
                    break;
                case Alignment.MiddleRight when childAlignment == Alignment.MiddleRight:
                    topLeft = new PointI(actualRectangle.Right, actualRectangle.Top + (int)((actualRectangle.Size.Y - childControl.Size.Y) * 0.5f));
                    break;
                case Alignment.MiddleRight when childAlignment == Alignment.MiddleLeft:
                    topLeft = new PointI(actualRectangle.Right, actualRectangle.Top + (int)((actualRectangle.Size.Y - childControl.Size.Y) * 0.5f));
                    break;

                case Alignment.BottomLeft when childAlignment == Alignment.BottomLeft:
                    topLeft = new PointI(actualRectangle.Left, actualRectangle.Bottom - childControl.Size.Y);
                    break;
                case Alignment.BottomLeft when childAlignment == Alignment.TopLeft:
                    topLeft = new PointI(actualRectangle.Left, actualRectangle.Bottom);
                    break;
                case Alignment.BottomCenter when childAlignment == Alignment.BottomCenter:
                    topLeft = new PointI(actualRectangle.Left + (int)((actualRectangle.Size.X - childControl.Size.X) * 0.5f), actualRectangle.Bottom - childControl.Size.Y);
                    break;
                case Alignment.BottomCenter when childAlignment == Alignment.TopCenter:
                    topLeft = new PointI(actualRectangle.Left + (int)((actualRectangle.Size.X - childControl.Size.X) * 0.5f), actualRectangle.Bottom);
                    break;
                case Alignment.BottomRight when childAlignment == Alignment.BottomRight:
                    topLeft = new PointI(actualRectangle.Right - childControl.Size.X, actualRectangle.Bottom - childControl.Size.Y);
                    break;
                default:
                    throw new Exception($"ParentAlignment [{parentAlignment}] with ChildAlignment [{childAlignment}] not implemented.");
            }
            topLeft += offset;

            return topLeft;
        }

        internal static Rectangle DetermineArea(Vector2 position, Alignment alignment, PointI size)
        {
            var topLeft = DetermineTopLeft(position, alignment, size);
            var actualDestinationRectangle = new Rectangle((int)topLeft.X, (int)topLeft.Y, size.X, size.Y);

            return actualDestinationRectangle;
        }

        internal static Vector2 DetermineTopLeft(Vector2 position, Alignment alignment, PointI size)
        {
            Vector2 topLeft;
            switch (alignment)
            {
                case Alignment.TopLeft:
                    topLeft = position;
                    break;
                case Alignment.TopCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y);
                    break;
                case Alignment.TopRight:
                    topLeft = new Vector2(position.X - size.X, position.Y);
                    break;
                case Alignment.MiddleLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y * 0.5f);
                    break;
                case Alignment.MiddleCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y * 0.5f);
                    break;
                case Alignment.MiddleRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y * 0.5f);
                    break;
                case Alignment.BottomLeft:
                    topLeft = new Vector2(position.X, position.Y - size.Y);
                    break;
                case Alignment.BottomCenter:
                    topLeft = new Vector2(position.X - size.X * 0.5f, position.Y - size.Y);
                    break;
                case Alignment.BottomRight:
                    topLeft = new Vector2(position.X - size.X, position.Y - size.Y);
                    break;
                default:
                    throw new Exception($"Alignment [{alignment}] not implemented.");
            }

            return topLeft;
        }

        internal static string GetTextureAtlas(string textureName)
        {
            string returnTextureAtlas = null;

            if (textureName.HasValue())
            {
                var split = textureName.Split('.');
                switch (split.Length)
                {
                    case 1:
                        break;
                    case 2:
                        returnTextureAtlas = split[0];
                        break;
                    default:
                        throw new Exception($"TextureName [{textureName}] may not have more than one period.");
                }
            }

            return returnTextureAtlas;
        }

        internal static string GetTextureName(string textureName)
        {
            string returnTextureName = null;

            if (textureName.HasValue())
            {
                var split = textureName.Split('.');
                switch (split.Length)
                {
                    case 1:
                        returnTextureName = split[0];
                        break;
                    case 2:
                        returnTextureName = split[1];
                        break;
                    default:
                        throw new Exception($"TextureName [{textureName}] may not have more than one period.");
                }
            }

            return returnTextureName;
        }
    }
}