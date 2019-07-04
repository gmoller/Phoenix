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
        [JsonProperty] private int NumberOfSlotsX { get; }
        [JsonProperty] private int NumberOfSlotsY { get; }
        [JsonProperty] private string TextureString { get; }
        private readonly Rectangle _top;
        private readonly Rectangle _left;
        private readonly Rectangle _right;
        private readonly Rectangle _bottom;
        private readonly Rectangle _corner;
        private readonly Rectangle _slot;

        [JsonConstructor]
        private SmallFrame(Vector2 topLeftPosition, Vector2 size, int numberOfSlotsX, int numberOfSlotsY, string textureString)
        {
            TopLeftPosition = topLeftPosition;
            Size = size;
            NumberOfSlotsX = numberOfSlotsX;
            NumberOfSlotsY = numberOfSlotsY;
            TextureString = textureString;
            _texture = AssetsManager.Instance.GetTexture(textureString);
            var atlas = AssetsManager.Instance.GetAtlas(textureString);

            var frame = atlas.Frames["top_h_border_repeat_x"];
            _top = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["left_v_border_repeat_y"];
            _left = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["right_v_border_repeat_y"];
            _right = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["bottom_h_border_repeat_x"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_corner"];
            _corner = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["slot"];
            _slot = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
        }

        public static SmallFrame Create(Vector2 topLeftPosition, Vector2 size, int numberOfSlotsX, int numberOfSlotsY, string textureString)
        {
            var smallFrame = new SmallFrame(topLeftPosition, size, numberOfSlotsX, numberOfSlotsY, textureString);

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
            var rectTopLeft = new Rectangle((int)TopLeftPosition.X + 1, (int)TopLeftPosition.Y + 1, _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectTopLeft, _corner, Color.White);
            var rectTopRight = new Rectangle((int)(TopLeftPosition.X + Size.X - 2), (int)TopLeftPosition.Y + 1, _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectTopRight, _corner, Color.White);
            var rectBottomLeft = new Rectangle((int)(TopLeftPosition.X + Size.X - 2), (int)(TopLeftPosition.Y + Size.Y - 1), _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectBottomLeft, _corner, Color.White);
            var rectBottomRight = new Rectangle((int)TopLeftPosition.X + 1, (int)(TopLeftPosition.Y + Size.Y - 1), _corner.Width, _corner.Height);
            spriteBatch.Draw(_texture, rectBottomRight, _corner, Color.White);

            // slots
            float x = TopLeftPosition.X + 10.0f;
            float y = TopLeftPosition.Y + 10.0f;

            for (int j = 0; j < NumberOfSlotsY; ++j)
            {
                for (int i = 0; i < NumberOfSlotsX; ++i)
                {
                    var rect = new Rectangle((int)x, (int)y, 49, 49);
                    spriteBatch.Draw(_texture, rect, _slot, Color.White); // new Vector2(x, y)
                    x += 49.0f; // _slot.Width + 0.0f;
                }

                x = TopLeftPosition.X + 10.0f; ;
                y += 49.0f;
            }

            spriteBatch.End();
        }

        public string Serialize()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            return json;
        }
    }
}