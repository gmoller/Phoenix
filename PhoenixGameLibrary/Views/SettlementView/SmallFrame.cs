using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using AssetsLibrary;
using Utilities;

namespace PhoenixGameLibrary.Views.SettlementView
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SmallFrame
    {
        private readonly Texture2D _texture;

        [JsonProperty] private Vector2 TopLeftPosition { get; }
        [JsonProperty] private Vector2 Size { get; }
        [JsonProperty] private string TextureString { get; }
        private readonly Rectangle _top;
        private readonly Rectangle _left;
        private readonly Rectangle _right;
        private readonly Rectangle _bottom;
        private readonly Rectangle _cornerTopLeft;
        private readonly Rectangle _cornerTopRight;
        private readonly Rectangle _cornerBottomLeft;
        private readonly Rectangle _cornerBottomRight;

        [JsonConstructor]
        private SmallFrame(Vector2 topLeftPosition, Vector2 size, string textureString)
        {
            TopLeftPosition = topLeftPosition;
            Size = size;
            TextureString = textureString;
            _texture = AssetsManager.Instance.GetTexture(textureString);
            var atlas = AssetsManager.Instance.GetAtlas(textureString);

            var frame = atlas.Frames["frame1_top"];
            _top = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_left"];
            _left = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_right"];
            _right = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_bottom"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_corner"];
            _cornerTopLeft = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_corner"];
            _cornerTopRight = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_corner"];
            _cornerBottomLeft = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame1_corner"];
            _cornerBottomRight = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
        }

        public static SmallFrame Create(Vector2 topLeftPosition, Vector2 size, string textureString)
        {
            var smallFrame = new SmallFrame(topLeftPosition, size, textureString);

            return smallFrame;
        }

        public static SmallFrame Deserialize(string json)
        {
            var smallFrame = JsonConvert.DeserializeObject<SmallFrame>(json);

            return smallFrame;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            // frame
            var rectLeft = new Rectangle((int)TopLeftPosition.X, (int)TopLeftPosition.Y + 5, _left.Width, (int)Size.Y);
            spriteBatch.Draw(_texture, rectLeft, _left, Color.White);
            var rectRight = new Rectangle((int)(TopLeftPosition.X + Size.X + 0), (int)TopLeftPosition.Y + 5, _right.Width, (int)Size.Y);
            spriteBatch.Draw(_texture, rectRight, _right, Color.White);

            var rectTop = new Rectangle((int)TopLeftPosition.X + 6, (int)TopLeftPosition.Y, (int)Size.X, _top.Height);
            spriteBatch.Draw(_texture, rectTop, _top, Color.White);
            var rectBottom = new Rectangle((int)TopLeftPosition.X + 6, (int)(TopLeftPosition.Y + Size.Y), (int)Size.X, _bottom.Height);
            spriteBatch.Draw(_texture, rectBottom, _bottom, Color.White);

            // corners
            var rectTopLeft = new Rectangle((int)TopLeftPosition.X + 1, (int)TopLeftPosition.Y + 1, _cornerTopLeft.Width, _cornerTopLeft.Height);
            spriteBatch.Draw(_texture, rectTopLeft, _cornerTopLeft, Color.White);
            var rectTopRight = new Rectangle((int)(TopLeftPosition.X + Size.X - 2), (int)TopLeftPosition.Y + 1, _cornerTopRight.Width, _cornerTopRight.Height);
            spriteBatch.Draw(_texture, rectTopRight, _cornerTopRight, Color.White);
            var rectBottomLeft = new Rectangle((int)(TopLeftPosition.X + Size.X - 2), (int)(TopLeftPosition.Y + Size.Y - 1), _cornerBottomLeft.Width, _cornerBottomLeft.Height);
            spriteBatch.Draw(_texture, rectBottomLeft, _cornerBottomLeft, Color.White);
            var rectBottomRight = new Rectangle((int)TopLeftPosition.X + 1, (int)(TopLeftPosition.Y + Size.Y - 1), _cornerBottomRight.Width, _cornerBottomRight.Height);
            spriteBatch.Draw(_texture, rectBottomRight, _cornerBottomRight, Color.White);

            spriteBatch.End();
        }

        public string Serialize()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            return json;
        }
    }
}