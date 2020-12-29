using System.Collections.Generic;
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
        public int NumberOfSlotsX { get; }
        public int NumberOfSlotsY { get; }
        public float SlotPadding { get; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="textureName"></param>
        /// <param name="size"></param>
        /// <param name="numberOfSlotsX"></param>
        /// <param name="numberOfSlotsY"></param>
        /// <param name="slotPadding"></param>
        public DynamicSlots(string name, string textureName, PointI size, int numberOfSlotsX, int numberOfSlotsY, float slotPadding) : base(name)
        {
            NumberOfSlotsX = numberOfSlotsX;
            NumberOfSlotsY = numberOfSlotsY;
            SlotPadding = slotPadding;

            CreateSlots(textureName, TopLeft, size, slotPadding, numberOfSlotsX, numberOfSlotsY);
        }

        private DynamicSlots(DynamicSlots other) : base(other)
        {
            NumberOfSlotsX = other.NumberOfSlotsX;
            NumberOfSlotsY = other.NumberOfSlotsY;
            SlotPadding = other.SlotPadding;

            CreateSlots(other["TextureNormal"].Name, TopLeft, Size, SlotPadding, NumberOfSlotsX, NumberOfSlotsY);
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

        private void CreateSlots(string textureName, PointI topLeft, PointI size, float slotPadding, int numberOfSlotsX, int numberOfSlotsY)
        {
            var startPositionX = topLeft.X + slotPadding;
            var startPositionY = topLeft.Y + slotPadding;
            var slotWidth = (size.X - slotPadding * 2.0f) / numberOfSlotsX;
            var slotHeight = (size.Y - slotPadding * 2.0f) / numberOfSlotsY;

            var slots = InstantiateSlots(textureName, startPositionX, startPositionY, slotWidth, slotHeight, numberOfSlotsX, numberOfSlotsY);

            foreach (var slot in slots)
            {
                AddControl(slot, Alignment.TopLeft, Alignment.TopLeft, slot.Position);
            }
        }

        private List<Slot> InstantiateSlots(string textureName, float startPositionX, float startPositionY, float slotWidth, float slotHeight, int numberOfSlotsX, int numberOfSlotsY)
        {
            var x = startPositionX;
            var y = startPositionY;

            var slots = new List<Slot>();
            for (var j = 0; j < numberOfSlotsY; j++)
            {
                for (var i = 0; i < numberOfSlotsX; i++)
                {
                    var slot = new Slot($"slot[{i}.{j}]")
                    {
                        TextureNormal = textureName,
                        Size = new PointI((int)slotWidth, (int)slotHeight),
                        Position = new PointI((int)x, (int)y)
                    };
                    slots.Add(slot);
                    x += slotWidth;
                }

                x = startPositionX;
                y += slotHeight;
            }

            return slots;
        }
    }
}