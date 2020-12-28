using System.Diagnostics;
using Zen.GuiControls;

namespace PhoenixGamePresentation.ControlsX
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Slot : Control
    {
        public string TextureNormal
        {
            get => Textures.ContainsKey("TextureNormal") ? Textures["TextureNormal"].TextureString : string.Empty;
            set => AddTexture("TextureNormal", new Texture("TextureNormal", value, ControlHelper.TextureNormalIsValid, control => Bounds));
        }

        /// <summary>
        /// A satisfying little slot.
        /// </summary>
        /// <param name="name">Name of control.</param>
        public Slot(string name) : base(name)
        {
        }

        private Slot(Slot other) : base(other)
        {
        }

        public override IControl Clone()
        {
            return new Slot(this);
        }
    }
}