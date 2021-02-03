using System.Diagnostics;

namespace PhoenixGameConfig
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public struct Texture
    {
        public string TexturePalette { get; set; }
        public byte TextureId { get; set; }

        public Texture(string texturePalette, byte textureId)
        {
            TexturePalette = texturePalette;
            TextureId = textureId;
        }

        public override bool Equals(object obj)
        {
            return obj is Texture texture && this == texture;
        }

        public override int GetHashCode()
        {
            return TexturePalette.GetHashCode() ^ TextureId.GetHashCode();
        }

        public static bool operator == (Texture a, Texture b)
        {
            return a.TexturePalette == b.TexturePalette && a.TextureId == b.TextureId;
        }

        public static bool operator != (Texture a, Texture b)
        {
            return !(a == b);
        }

        public override string ToString() => DebuggerDisplay;

        private string DebuggerDisplay => $"{{TexturePalette={TexturePalette},TextureId={TextureId}}}";
    }
}