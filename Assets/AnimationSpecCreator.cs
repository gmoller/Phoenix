using Microsoft.Xna.Framework;

namespace Assets
{
    public class AnimationSpecCreator
    {
        public static AnimationSpec Create(string spriteSheetTextureName, int spriteSheetTextureWidth, int spriteSheetTextureHeight, int frameWidth, int frameHeight, int duration, bool isRepeating)
        {
            var spec = new AnimationSpec { SpriteSheetName = spriteSheetTextureName, FrameDuration = duration, IsRepeating = isRepeating };

            int cols = spriteSheetTextureWidth / frameWidth;
            int rows = spriteSheetTextureHeight / frameHeight;
            spec.NumberOfFrames = cols * rows;
            spec.Frames = new Rectangle[spec.NumberOfFrames];

            int x = 0;
            int y = 0;
            for (int i = 0; i < spec.NumberOfFrames; i++)
            {
                spec.Frames[i] = new Rectangle(x, y, frameWidth, frameHeight);
                x += frameWidth;
                if (x >= spriteSheetTextureWidth)
                {
                    x = 0;
                    y += frameHeight;
                }
            }

            return spec;
        }
    }
}