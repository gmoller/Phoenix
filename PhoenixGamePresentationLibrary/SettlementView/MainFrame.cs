﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Utilities;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class MainFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;
        private readonly Texture2D _texture;
        private readonly Rectangle _main;
        private readonly Rectangle _heading;
        private readonly Rectangle _bottom;
        private readonly Button _btnClose;

        internal MainFrame(SettlementView parent, Vector2 topLeftPosition, Texture2D texture, AtlasSpec2 atlas)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
            _texture = texture;

            var frame = atlas.Frames["frame_main"];
            _main = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_big_heading"];
            _heading = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_bottom"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);

            _btnClose = new Button("btnClose", new Vector2(_topLeftPosition.X + 506.0f, _topLeftPosition.Y - 92.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(43.0f, 44.0f), "GUI_Textures_1", "close_button_n", "close_button_a", "close_button_a", "close_button_h");
            _btnClose.Click += CloseButtonClick;
        }

        internal void Update(float deltaTime, InputHandler input)
        {
            _btnClose.Update(deltaTime);
        }

        internal void Draw()
        {
            var spriteBatch = DeviceManager.Instance.GetCurrentSpriteBatch();
            spriteBatch.Begin();

            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X, _topLeftPosition.Y), _main, Color.White);
            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X - 2.0f, _topLeftPosition.Y - 100.0f), _heading, Color.White);
            spriteBatch.Draw(_texture, new Vector2(_topLeftPosition.X - 2.0f, _topLeftPosition.Y + 680.0f), _bottom, Color.White);

            spriteBatch.End();

            _btnClose.Draw();
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            _parent.CloseButtonClick(sender, e);
        }
    }

    internal class SecondaryFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;
        private readonly Texture2D _texture;
        private readonly Rectangle _main;
        private readonly Rectangle _bottom;

        internal SecondaryFrame(SettlementView parent, Vector2 topLeftPosition, Texture2D texture, AtlasSpec2 atlas)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
            _texture = texture;

            var frame = atlas.Frames["frame_main"];
            _main = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
            frame = atlas.Frames["frame_bottom"];
            _bottom = new Rectangle(frame.X, frame.Y, frame.Width, frame.Height);
        }

        internal void Update(float deltaTime, InputHandler input)
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