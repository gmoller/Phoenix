using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Zen.GuiControls;

namespace PhoenixGamePresentation.ExtensionMethods
{
    public static class DictionaryExtensions
    {
        public static void LoadContent(this Dictionary<string, IControl> dictionary, ContentManager content, bool loadChildrenContent = false)
        {
            foreach (var item in dictionary.Values)
            {
                item.LoadContent(content, loadChildrenContent);
            }
        }
    }
}