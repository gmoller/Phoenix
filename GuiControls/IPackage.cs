using Input;

namespace GuiControls
{
    public interface IPackage
    {
        void Reset();
        ControlStatus Process(IControl control, InputHandler input, float deltaTime);
    }
}