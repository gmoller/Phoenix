using GuiControls;
using Input;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public static class EnumerableListExtensions
    {
        public static void Update(this EnumerableList<IControl> list, InputHandler input, float deltaTime)
        {
            foreach (var item in list)
            {
                item.Update(input, deltaTime);
            }
        }
    }
}