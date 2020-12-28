using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using Zen.GuiControls;
using Zen.Utilities;

namespace PhoenixGamePresentation.ControlsX
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class DynamicSlots : Control
    {
        #region State
        public int NumberOfSlotsX { get; set; }
        public int NumberOfSlotsY { get; set; }
        public float SlotPadding { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="textureName"></param>
        /// <param name="numberOfSlotsX"></param>
        /// <param name="numberOfSlotsY"></param>
        /// <param name="slotPadding"></param>
        public DynamicSlots(string name, string textureName, int numberOfSlotsX, int numberOfSlotsY, float slotPadding) : base(name)
        {
            NumberOfSlotsX = numberOfSlotsX;
            NumberOfSlotsY = numberOfSlotsY;
            SlotPadding = slotPadding;

            CreateSlots(textureName, slotPadding, numberOfSlotsX, numberOfSlotsY);
        }

        private DynamicSlots(DynamicSlots other) : base(other)
        {
            NumberOfSlotsX = other.NumberOfSlotsX;
            NumberOfSlotsY = other.NumberOfSlotsY;
            SlotPadding = other.SlotPadding;

            CreateSlots(other["TextureNormal"].Name, SlotPadding, NumberOfSlotsX, NumberOfSlotsY);
        }

        public override IControl Clone()
        {
            return new DynamicSlots(this);
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            base.LoadContent(content, loadChildrenContent);

            ChildControls.LoadContent(content, loadChildrenContent);
        }

        private void CreateSlots(string textureName, float slotPadding, int numberOfSlotsX, int numberOfSlotsY)
        {
            var startPositionX = TopLeft.X + slotPadding;
            var startPositionY = TopLeft.Y + slotPadding;
            var slotWidth = (Size.X - slotPadding * 2.0f) / numberOfSlotsX;
            var slotHeight = (Size.Y - slotPadding * 2.0f) / numberOfSlotsY;

            var x = startPositionX;
            var y = startPositionY;

            for (var j = 0; j < numberOfSlotsY; j++)
            {
                for (var i = 0; i < numberOfSlotsX; i++)
                {
                    var slot = new Slot($"slot[{i}.{j}]")
                    {
                        TextureNormal = textureName,
                        Size = new PointI((int) slotWidth, (int) slotHeight)
                    };
                    AddControl(slot, Alignment.TopLeft, Alignment.TopLeft, new PointI((int)x, (int)y));
                    x += slotWidth;
                }

                x = startPositionX;
                y += slotHeight;
            }
        }
    }
}