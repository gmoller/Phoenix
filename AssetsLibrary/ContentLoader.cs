using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using SpriteFontPlus;

namespace AssetsLibrary
{
    public static class ContentLoader
    {
        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            string[] ttfFiles = GetAnyFilesFromContentDirectory("*.ttf");
            List<SpriteFont> fonts = BakeTtfFiles(ttfFiles, graphicsDevice);
            AddToAssetsManager(ttfFiles, fonts);

            string[] pngFiles = GetAnyFilesFromContentDirectory("*.png");
            List<Texture2D> textures = BakeTextures(pngFiles, graphicsDevice);
            AddToAssetsManager(pngFiles, textures);
        }

        private static string[] GetAnyFilesFromContentDirectory(string searchPattern)
        {
            string path = $@"{Directory.GetCurrentDirectory()}\..\..\..\..\Content\";
            var directoryInfo = new DirectoryInfo(path);

            FileInfo[] ttfFiles = { };
            if (directoryInfo.Exists)
            {
                // get any ttf files
                ttfFiles = directoryInfo.GetFiles(searchPattern);
            }

            return ttfFiles.Select(item => item.FullName).ToArray();
        }

        private static List<SpriteFont> BakeTtfFiles(string[] ttfFiles, GraphicsDevice graphicsDevice)
        {
            var fonts = new List<SpriteFont>();

            foreach (string file in ttfFiles)
            {
                TtfFontBakerResult bakeResult = TtfFontBaker.Bake(File.ReadAllBytes(file), 50, 256, 256, new[]
                {
                    CharacterRange.BasicLatin,
                    //CharacterRange.Latin1Supplement,
                    //CharacterRange.LatinExtendedA,
                    //CharacterRange.LatinExtendedB,
                    //CharacterRange.Cyrillic,
                    //CharacterRange.CyrillicSupplement,
                    //CharacterRange.Hiragana,
                    //CharacterRange.Katakana
                });
                SpriteFont font = bakeResult.CreateSpriteFont(graphicsDevice);
                fonts.Add(font);
            }

            return fonts;
        }

        private static List<Texture2D> BakeTextures(string[] pngFiles, GraphicsDevice graphicsDevice)
        {
            var textures = new List<Texture2D>();

            foreach (string file in pngFiles)
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {
                    Texture2D texture = Texture2D.FromStream(graphicsDevice, fileStream);
                    textures.Add(texture);
                }
            }

            return textures;
        }

        private static void AddToAssetsManager(string[] files, List<SpriteFont> fonts)
        {
            int i = 0;
            foreach (SpriteFont font in fonts)
            {
                AssetsManager.Instance.AddSpriteFont(Path.GetFileNameWithoutExtension(files[i++]), font);
            }
        }

        private static void AddToAssetsManager(string[] files, List<Texture2D> textures)
        {
            int i = 0;
            foreach (Texture2D texture in textures)
            {
                AssetsManager.Instance.AddTexture(Path.GetFileNameWithoutExtension(files[i++]), texture);
            }
        }
    }
}