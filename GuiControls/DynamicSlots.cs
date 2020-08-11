using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DynamicSlots : Control
    {
        #region State
        private readonly int _numberOfSlotsX;
        private readonly int _numberOfSlotsY;
        private List<IControl> _slots;
        private readonly float _slotPadding;
        private readonly List<LabelSized> _labels;
        #endregion

        public DynamicSlots(
            Vector2 position, 
            Alignment positionAlignment, 
            Vector2 size, 
            string textureAtlas, 
            string textureName, 
            int numberOfSlotsX, 
            int numberOfSlotsY, 
            float slotPadding,
            string name,
            List<LabelSized> labels = null,
            float layerDepth = 0.0f,
            IControl parent = null) :
            base(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureName,
                null,
                null,
                null,
                null,
                name,
                layerDepth,
                parent)
        {
            _numberOfSlotsX = numberOfSlotsX;
            _numberOfSlotsY = numberOfSlotsY;
            _slotPadding = slotPadding;

            _labels = labels;
        }

        protected DynamicSlots(DynamicSlots copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new DynamicSlots(this); }

        public override void LoadContent(ContentManager content)
        {
            var startX = TopLeft.X + _slotPadding;
            var startY = TopLeft.Y + _slotPadding;
            var slotWidth = (Size.X - _slotPadding * 2.0f) / _numberOfSlotsX;
            var slotHeight = (Size.Y - _slotPadding * 2.0f) / _numberOfSlotsY;
            _slots = CreateSlots(startX, startY, slotWidth, slotHeight, _numberOfSlotsX, _numberOfSlotsY, content);
        }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            var i = 0;
            foreach (var slot in _slots)
            {
                slot.Draw(spriteBatch);
                if (_labels != null && i < _labels.Count)
                {
                    _labels[i].Draw(spriteBatch);
                    i++;
                }
            }
        }

        protected override void InDraw(Matrix? transform = null)
        {
            var i = 0;
            foreach (var slot in _slots)
            {
                slot.Draw(transform);
                if (_labels != null && i < _labels.Count)
                {
                    _labels[i].Draw();
                    i++;
                }
            }
        }

        private List<IControl> CreateSlots(float startX, float startY, float slotWidth, float slotHeight, int numberOfSlotsX, int numberOfSlotsY, ContentManager content)
        {
            var slots = new List<IControl>();

            var x = startX;
            var y = startY;

            for (var j = 0; j < numberOfSlotsY; ++j)
            {
                for (var i = 0; i < numberOfSlotsX; ++i)
                {
                    var slot = new Slot((int)x, (int)y, (int)slotWidth, (int)slotHeight, TextureAtlas, TextureName, "slot");
                    slot.LoadContent(content);
                    slots.Add(slot);
                    x += slotWidth;
                }

                x = startX;
                y += slotHeight;
            }

            return slots;
        }
    }

    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Slot : Control
    {
        public Slot(
            int x,
            int y,
            int width,
            int height,
            string textureAtlas,
            string textureName,
            string name) : 
            base(
                new Vector2(x, y), 
                Alignment.TopLeft, 
                new Vector2(width, height), 
                textureAtlas, 
                textureName, 
                "", 
                "", 
                "", 
                "",
                name)
        {
        }

        protected Slot(Slot copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new Slot(this); }

        protected override void InDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, ActualDestinationRectangle, SourceRectangle, Color, 0.0f, Vector2.Zero, SpriteEffects.None, LayerDepth);
        }
    }
}