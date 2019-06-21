using System.Diagnostics;

namespace PhoenixGameLibrary.GameData
{
    /// <summary>
    /// This struct is immutable.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Texture
    {
        public string TexturePalette { get; }
        public byte TextureId { get; }

        public Texture(string texturePalette, byte textureId)
        {
            TexturePalette = texturePalette;
            TextureId = textureId;
        }

        private string DebuggerDisplay => $"{{TexturePalette={TexturePalette},TextureId={TextureId}}}";
    }
}