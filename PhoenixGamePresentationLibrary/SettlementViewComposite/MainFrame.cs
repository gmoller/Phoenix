using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class MainFrame
    {
        #region State
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;
        private readonly string _textureAtlas;

        private Frame _mainFrame;
        #endregion

        internal MainFrame(SettlementView parent, Vector2 topLeftPosition, string textureAtlas)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
            _textureAtlas = textureAtlas;
        }

        internal void LoadContent(ContentManager content)
        {
            _mainFrame = new Frame(_topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), _textureAtlas, "frame_main", "mainFrame");
            _mainFrame.LoadContent(content);

            var headerFrame = new Frame(new Vector2(-2.0f, -100.0f), Alignment.TopLeft, new Vector2(560.0f, 146.0f), _textureAtlas, "frame_big_heading", "headerFrame", _mainFrame);
            headerFrame.LoadContent(content);

            string GetTextFuncForSettlementType() => $"{_parent.Settlement.SettlementType} of";
            var lblSettlementName1 = new LabelAutoSized(new Vector2(278.0f, 49.0f), Alignment.MiddleCenter, GetTextFuncForSettlementType, "Carolingia-Regular-24", Color.Purple, "lblSettlementName1", Color.DarkBlue, headerFrame);
            lblSettlementName1.LoadContent(content);
            string GetTextFuncForSettlementName() => $"{_parent.Settlement.Name}";
            var lblSettlementName2 = new LabelAutoSized(new Vector2(278.0f, 80.0f), Alignment.MiddleCenter, GetTextFuncForSettlementName, "Carolingia-Regular-24", Color.Purple, "lblSettlementName2", Color.DarkBlue, headerFrame);
            lblSettlementName2.LoadContent(content);

            var footerFrame = new Frame(new Vector2(-2.0f, 680.0f), Alignment.TopLeft, new Vector2(563.0f, 71.0f), _textureAtlas, "frame_bottom", "footerFrame", _mainFrame);
            footerFrame.LoadContent(content);

            var btnClose = new Button(new Vector2(508.0f, 9.0f), Alignment.TopLeft, new Vector2(43.0f, 44.0f), _textureAtlas, "close_button_n", "close_button_a", "close_button_a", "close_button_h", "btnClose", 1.0f, _mainFrame.ChildControls["headerFrame"]);
            btnClose.LoadContent(content);
            btnClose.Click += CloseButtonClick;
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _mainFrame.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _mainFrame.Draw(spriteBatch);
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            _parent.CloseButtonClick(sender, e);
        }
    }
}