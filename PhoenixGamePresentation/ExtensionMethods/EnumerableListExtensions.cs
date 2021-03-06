﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;
using Zen.Input;
using Zen.Utilities;

namespace PhoenixGamePresentation.ExtensionMethods
{
    public static class EnumerableListExtensions
    {
        public static void Update(this EnumerableList<IControl> list, InputHandler input, GameTime gameTime, Viewport? viewport)
        {
            foreach (var item in list)
            {
                item.Update(input, gameTime, viewport);
            }
        }
    }
}