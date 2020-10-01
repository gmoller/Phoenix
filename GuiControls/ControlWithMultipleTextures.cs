using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Assets.ExtensionMethods;
using Input;
using Utilities.ExtensionMethods;

namespace GuiControls
{
    public abstract class ControlWithMultipleTextures : ControlWithSingleTexture
    {
        #region State
        private readonly string _textureNormal;
        private readonly string _textureActive;
        private readonly string _textureHover;
        private readonly string _textureDisabled;
        #endregion

        protected ControlWithMultipleTextures(Vector2 position, Alignment positionAlignment, Vector2 size, string textureName, string textureNormal, string textureActive, string textureHover, string textureDisabled, string name, float layerDepth = 0.0f)
            : base(position, positionAlignment, size, textureName, name, layerDepth)
        {
            _textureNormal = textureNormal.HasValue() ? textureNormal.KeepOnlyAfterCharacter('.') : TextureName;
            _textureActive = textureActive.HasValue() ? textureActive.KeepOnlyAfterCharacter('.') : TextureName;
            _textureHover = textureHover.HasValue() ? textureHover.KeepOnlyAfterCharacter('.') : TextureName;
            _textureDisabled = textureDisabled.HasValue() ? textureDisabled.KeepOnlyAfterCharacter('.') : TextureName;
        }

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            base.Update(input, deltaTime, viewport);

            if (Status == ControlStatus.Active)
            {
                SetTexture(_textureActive);
            }

            string texture;
            if (Enabled)
            {
                if (Status == ControlStatus.MouseOver)
                {
                    if (Atlas.IsNull() && string.IsNullOrEmpty(_textureHover)) throw new Exception("_textureHover is empty!");
                    texture = Atlas.HasValue() ? (_textureHover.HasValue() ? _textureHover : TextureName) : _textureHover;
                }
                else
                {
                    if (Atlas.IsNull() && string.IsNullOrEmpty(_textureNormal)) throw new Exception("_textureNormal is empty!");
                    texture = Atlas.HasValue() ? (_textureNormal.HasValue() ? _textureNormal : TextureName) : _textureNormal;
                }
            }
            else
            {
                if (Atlas.IsNull() && string.IsNullOrEmpty(_textureDisabled)) throw new Exception("_textureDisabled is empty!");
                texture = Atlas.HasValue() ? (_textureDisabled.HasValue() ? _textureDisabled : TextureName) : _textureDisabled;
            }
            SetTexture(texture);
        }
    }
}