using Microsoft.Xna.Framework;

namespace AssetsLibrary
{
    public class AnimationSpec
    {
        public string SpriteSheetName { get; set; }
        public int FrameDuration { get; set; } // in milliseconds
        public int NumberOfFrames { get; set; }
        public bool IsRepeating { get; set; }

        public Rectangle[] Frames { get; set; }
    }
}