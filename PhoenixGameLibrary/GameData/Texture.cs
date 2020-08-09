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

        #region Overrides and Overloads

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

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        private string DebuggerDisplay => $"{{TexturePalette={TexturePalette},TextureId={TextureId}}}";

        #endregion
    }
}