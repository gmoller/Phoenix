using GuiControls;
using Input;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public static class EnumerableDictionaryExtensions
    {
        public static void Update(this EnumerableDictionary<IControl> dictionary, InputHandler input, float deltaTime)
        {
            foreach (var item in dictionary)
            {
                item.Update(input, deltaTime);
            }
        }
    }
}