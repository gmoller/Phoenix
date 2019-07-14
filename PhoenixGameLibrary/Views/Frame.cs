using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using AssetsLibrary;
using Utilities;

namespace PhoenixGameLibrary.Views.SettlementView
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Frame
    {
        private readonly Texture2D _texture;

        [JsonProperty] private Vector2 TopLeftPosition { get; }
        [JsonProperty] private Vector2 Size { get; }
        [JsonProperty] private string TextureString { get; }

        private readonly Rectangle _topLeft;
        private readonly Rectangle _topMiddle;
        private readonly Rectangle _topRight;
        private readonly Rectangle _leftMiddle;
        private readonly Rectangle _rightMiddle;
        private readonly Rectangle _bottomMiddle;
        private readonly Rectangle _bottomLeft;
        private readonly Rectangle _bottomRight;

        [JsonConstructor]
        private Frame(Vector2 topLeftPosition, Vector2 size, string textureString)
        {
            TopLeftPosition = topLeftPosition;
            Size = size;
            TextureString = textureString;
            _texture = AssetsManager.Instance.GetTexture(textureString);
            var atlas = AssetsManager.Instance.GetAtlas(textureString);

            var frame = atlas.Frames["frame2_top_left"];
            _topLeft = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_top_middle"];
            _topMiddle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_top_right"];
            _topRight = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_left_middle"];
            _leftMiddle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_right_middle"];
            _rightMiddle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_bottom_left"];
            _bottomLeft = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_bottom_middle"];
            _bottomMiddle = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame2_bottom_right"];
            _bottomRight = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
        }

        public static Frame Create(Vector2 topLeftPosition, Vector2 size, string textureString)
        {
            var smallFrame = new Frame(topLeftPosition, size, textureString);

            return smallFrame;
        }

        public static Frame Deserialize(string json)
        {
            var frame = JsonConvert.DeserializeObject<Frame>(json);

            return frame;
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
        }

        public void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            var rectTopLeft = new Rectangle((int)TopLeftPosition.X, (int)TopLeftPosition.Y, _topLeft.Width, _topLeft.Height);
            spriteBatch.Draw(_texture, rectTopLeft, _topLeft, Color.White);

            var middleWidth = (int)Size.X - _topLeft.Width - _topRight.Width;
            var rectTop = new Rectangle((int)TopLeftPosition.X + _topLeft.Width, (int)TopLeftPosition.Y, middleWidth, _topMiddle.Height);
            spriteBatch.Draw(_texture, rectTop, _topMiddle, Color.White);

            var rectTopRight = new Rectangle((int)(TopLeftPosition.X + _topLeft.Width + middleWidth), (int)TopLeftPosition.Y, _topRight.Width, _topRight.Height);
            spriteBatch.Draw(_texture, rectTopRight, _topRight, Color.White);

            var middleHeight = (int)Size.Y - _topLeft.Height - _bottomLeft.Height;
            var rectLeft = new Rectangle((int)TopLeftPosition.X, (int)TopLeftPosition.Y + _topLeft.Height, _leftMiddle.Width, middleHeight);
            spriteBatch.Draw(_texture, rectLeft, _leftMiddle, Color.White);
            
            var rectRight = new Rectangle((int)(TopLeftPosition.X + _topLeft.Width + middleWidth), (int)TopLeftPosition.Y + _topLeft.Height, _topRight.Width, middleHeight);
            spriteBatch.Draw(_texture, rectRight, _rightMiddle, Color.White);

            var rectBottomLeft = new Rectangle((int)TopLeftPosition.X, (int)TopLeftPosition.Y + _topLeft.Height + rectLeft.Height, _bottomLeft.Width, _bottomLeft.Height);
            spriteBatch.Draw(_texture, rectBottomLeft, _bottomLeft, Color.White);

            var rectBottom = new Rectangle((int)TopLeftPosition.X + _bottomLeft.Width, (int)TopLeftPosition.Y + _topLeft.Height + middleHeight, middleWidth, _bottomLeft.Height);
            spriteBatch.Draw(_texture, rectBottom, _bottomMiddle, Color.White);

            var rectBottomRight = new Rectangle((int)TopLeftPosition.X + _bottomLeft.Width + rectBottom.Width, (int)TopLeftPosition.Y + _topRight.Height + middleHeight, _bottomRight.Width, _bottomRight.Height);
            spriteBatch.Draw(_texture, rectBottomRight, _bottomRight, Color.White);

            var rectMiddle = new Rectangle((int)TopLeftPosition.X + _leftMiddle.Width, (int)TopLeftPosition.Y + rectTop.Height, middleWidth, middleHeight);
            spriteBatch.FillRectangle(rectMiddle, Color.Black * 0.75f, 0.0f); // 0.75

            spriteBatch.End();
        }

        public string Serialize()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);

            return json;
        }
    }
}