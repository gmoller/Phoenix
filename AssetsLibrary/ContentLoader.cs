using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SpriteFontPlus;

namespace AssetsLibrary
{
    public static class ContentLoader
    {
        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            var ttfFiles = GetAnyFilesFromContentDirectory("*.ttf");
            var fonts = BakeTtfFiles(ttfFiles, graphicsDevice);
            AddToAssetsManager(ttfFiles, fonts);

            var pngFiles = GetAnyFilesFromContentDirectory("*.png");
            var textures = BakeTextures(pngFiles, graphicsDevice);
            AddToAssetsManager(pngFiles, textures);

            var atlasSpecFiles = GetAnyFilesFromContentDirectory("*.atlasspec");
            var atlases = CreateAtlases(atlasSpecFiles);
            AddToAssetsManager(atlasSpecFiles, atlases);
        }

        private static string[] GetAnyFilesFromContentDirectory(string searchPattern)
        {
            string path = $@"{Directory.GetCurrentDirectory()}\Content\";
            var directoryInfo = new DirectoryInfo(path);

            FileInfo[] ttfFiles = { };
            if (directoryInfo.Exists)
            {
                // get any ttf files
                ttfFiles = directoryInfo.GetFiles(searchPattern);
            }

            return ttfFiles.Select(item => item.FullName).ToArray();
        }

        private static List<SpriteFont> BakeTtfFiles(string[] files, GraphicsDevice graphicsDevice)
        {
            var fonts = new List<SpriteFont>();

            foreach (string file in files)
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
                SpriteFont item = bakeResult.CreateSpriteFont(graphicsDevice);
                fonts.Add(item);
            }

            return fonts;
        }

        private static List<Texture2D> BakeTextures(string[] files, GraphicsDevice graphicsDevice)
        {
            var textures = new List<Texture2D>();

            foreach (string file in files)
            {
                using (FileStream fileStream = new FileStream(file, FileMode.Open))
                {
                    Texture2D item = Texture2D.FromStream(graphicsDevice, fileStream);
                    textures.Add(item);
                }
            }

            return textures;
        }

        private static List<AtlasSpec> CreateAtlases(string[] files)
        {
            var atlases = new List<AtlasSpec>();

            foreach (string file in files)
            {
                var json = File.ReadAllText(file);
                var item = JsonConvert.DeserializeObject<AtlasSpec>(json);
                atlases.Add(item);
            }

            return atlases;
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

        private static void AddToAssetsManager(string[] files, List<AnimationSpec> animations)
        {
            int i = 0;
            foreach (AnimationSpec animationSpec in animations)
            {
                AssetsManager.Instance.AddAnimation(Path.GetFileNameWithoutExtension(files[i++]), animationSpec);
            }
        }

        private static void AddToAssetsManager(string[] files, List<AtlasSpec> atlases)
        {
            int i = 0;
            foreach (AtlasSpec atlasSpec in atlases)
            {
                AssetsManager.Instance.AddAtlas(Path.GetFileNameWithoutExtension(files[i++]), atlasSpec);
            }
        }
    }
}