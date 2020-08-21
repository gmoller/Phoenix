using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GuiControls
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DynamicSlots : Control
    {
        #region State
        private readonly int _numberOfSlotsX;
        private readonly int _numberOfSlotsY;
        private readonly float _slotPadding;
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
            IControl parent = null) :
            this(
                position,
                positionAlignment,
                size,
                textureAtlas,
                textureName,
                numberOfSlotsX,
                numberOfSlotsY,
                slotPadding,
                name,
                0.0f,
                parent)
        {
        }

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

            var startX = TopLeft.X + _slotPadding;
            var startY = TopLeft.Y + _slotPadding;
            var slotWidth = (Size.X - _slotPadding * 2.0f) / _numberOfSlotsX;
            var slotHeight = (Size.Y - _slotPadding * 2.0f) / _numberOfSlotsY;
            CreateSlots(startX, startY, slotWidth, slotHeight, _numberOfSlotsX, _numberOfSlotsY);
        }

        protected DynamicSlots(DynamicSlots copyThis) : base(copyThis)
        {
        }

        public override IControl Clone() { return new DynamicSlots(this); }

        public override void LoadContent(ContentManager content)
        {
            foreach (var child in ChildControls)
            {
                child.LoadContent(content);
            }
        }

        private void CreateSlots(float startX, float startY, float slotWidth, float slotHeight, int numberOfSlotsX, int numberOfSlotsY)
        {
            var x = startX;
            var y = startY;

            for (var j = 0; j < numberOfSlotsY; ++j)
            {
                for (var i = 0; i < numberOfSlotsX; ++i)
                {
                    var slot = new Slot((int)x, (int)y, (int)slotWidth, (int)slotHeight, TextureAtlas, TextureName, $"slot[{i}.{j}]", this);
                    x += slotWidth;
                }

                x = startX;
                y += slotHeight;
            }
        }
    }
}