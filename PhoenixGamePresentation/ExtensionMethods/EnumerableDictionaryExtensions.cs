using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zen.GuiControls;
using Zen.Input;
using Zen.Utilities;

namespace PhoenixGamePresentation.ExtensionMethods
{
    public static class EnumerableDictionaryExtensions
    {
        public static void Update(this EnumerableDictionary<IControl> dictionary, InputHandler input, GameTime gameTime, Viewport? viewport)
        {
            foreach (var item in dictionary)
            {
                item.Update(input, gameTime, viewport);
            }
        }
    }
}