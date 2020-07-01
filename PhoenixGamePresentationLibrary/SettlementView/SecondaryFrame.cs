using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using Input;
using Utilities;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class SecondaryFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;
        private readonly Texture2D _texture;
        private readonly AtlasSpec2 _atlas;

        private Rectangle _main;
        private Rectangle _bottom;

        internal SecondaryFrame(SettlementView parent, Vector2 topLeftPosition, Texture2D texture, AtlasSpec2 atlas)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
            _texture = texture;
            _atlas = atlas;
        }

        internal void LoadContent(ContentManager content)
        {
            var frame = _atlas.Frames["frame_main"];
            _main = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = _atlas.Frames["frame_bottom"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
        }

        internal void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X, _topLeftPosition.Y), _main, Color.White);
            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X - 2.0f, _topLeftPosition.Y + 680.0f), _bottom, Color.White);

            spriteBatch.End();
        }
    }
}
