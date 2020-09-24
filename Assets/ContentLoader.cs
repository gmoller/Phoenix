using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SpriteFontPlus;

namespace Assets
{
    public static class ContentLoader
    {
        public static void LoadContent(string contentFolder, string fonts, string textures, string atlases, GraphicsDevice graphicsDevice)
        {
            var ttfFiles = GetFilesFromDirectory($"{contentFolder}{fonts}", "*.ttf");
            var fontsList = BakeTtfFiles(ttfFiles, graphicsDevice);
            AddToAssetsManager(fontsList);

            var pngFiles = GetFilesFromDirectory($"{contentFolder}{textures}", "*.png");
            var texturesList = BakeTextures(pngFiles, graphicsDevice);
            AddToAssetsManager(texturesList);

            var atlasSpecFiles = GetFilesFromDirectory($"{contentFolder}{atlases}", "*.atlasspec");
            var atlasesList = CreateAtlases(atlasSpecFiles);
            AddToAssetsManager(atlasesList);
        }

        private static string[] GetFilesFromDirectory(string path, string searchPattern)
        {
            var directoryInfo = new DirectoryInfo(path);

            FileInfo[] files = { };
            if (directoryInfo.Exists)
            {
                // get any files that match search pattern
                files = directoryInfo.GetFiles(searchPattern);
            }
            var fileNames = files.Select(item => item.FullName).ToArray();

            return fileNames;
        }

        private static List<(string fileName, SpriteFont font)> BakeTtfFiles(string[] files, GraphicsDevice graphicsDevice)
        {
            var fonts = new List<(string fileName, SpriteFont font)>();

            foreach (var file in files)
            {
                var bakeResult = TtfFontBaker.Bake(
                    File.ReadAllBytes(file),
                    25,
                    512,
                    512,
                    new[] { CharacterRange.BasicLatin });
                var font = bakeResult.CreateSpriteFont(graphicsDevice);
                var item = (file, font);
                fonts.Add(item);
            }

            return fonts;
        }

        private static List<(string fileName, Texture2D texture)> BakeTextures(string[] files, GraphicsDevice graphicsDevice)
        {
            var textures = new List<(string fileName, Texture2D texture)>();

            foreach (var file in files)
            {
                using var fileStream = new FileStream(file, FileMode.Open);
                var texture = Texture2D.FromStream(graphicsDevice, fileStream);
                var item = (file, texture);

                textures.Add(item);
            }

            return textures;
        }

        private static List<(string fileName, AtlasSpec atlasspec)> CreateAtlases(string[] files)
        {
            var atlases = new List<(string fileName, AtlasSpec atlasspec)>();

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var atlasspec = JsonConvert.DeserializeObject<AtlasSpec>(json);
                var item = (file, atlasspec);
                atlases.Add(item);
            }

            return atlases;
        }

        private static void AddToAssetsManager(List<(string fileName, SpriteFont font)> list)
        {
            foreach (var item in list)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.fileName);
                AssetsManager.Instance.AddSpriteFont(fileNameWithoutExtension, item.font);
            }
        }

        private static void AddToAssetsManager(List<(string fileName, Texture2D texture)> list)
        {
            foreach (var item in list)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.fileName);
                AssetsManager.Instance.AddTexture(fileNameWithoutExtension, item.texture);
            }
        }

        private static void AddToAssetsManager(List<(string fileName, AtlasSpec atlasspec)> list)
        {
            foreach (var item in list)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.fileName);
                AssetsManager.Instance.AddAtlas(fileNameWithoutExtension, item.atlasspec);
            }
        }
    }
}