using AssetsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Utilities;

namespace GuiControls
{
    [JsonObject(MemberSerialization.OptIn)]
    public class LabelUIElement : UIElement
    {
        [JsonProperty] public Color TextColor { get; set; }

        private LabelUIElement(string name, string fontName, Point position, Point size, Color textColor, params UIElement[] children) : base(name, fontName, position, size, children)
        {
            TextColor = textColor;
            Text = string.Empty;
        }

        public static LabelUIElement Create(string name, string fontName, Point position, Point size, Color textColor, params UIElement[] children)
        {
            return new LabelUIElement(name, fontName, position, size, textColor, children);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            var font = AssetsManager.Instance.GetSpriteFont(FontName);
            spriteBatch.DrawString(font, Text, Position.ToVector2(), TextColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.End();

            foreach (UIElement item in Children)
            {
                item.Draw();
            }
        }
    }
}