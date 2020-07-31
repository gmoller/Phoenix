using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using PhoenixGameLibrary.GameData;

namespace PhoenixGamePresentationLibrary.SettlementView
{
    internal class UnitsFrame
    {
        private readonly SettlementView _parent;
        private readonly Vector2 _topLeftPosition;

        private LabelAutoSized _lblUnits;
        private Frame _smallFrameUnits;
        private List<LabelSized> _unitLabels;

        internal UnitsFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblUnits = new LabelAutoSized(_topLeftPosition + new Vector2(0.0f, -15.0f), Alignment.TopLeft, "Units", "CrimsonText-Regular-12", Color.Orange, Color.Red);
            _lblUnits.LoadContent(content);

            _unitLabels = CreateUnitLabels(content);

            var slots2 = new DynamicSlots(_topLeftPosition + new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(515, 75), "GUI_Textures_1", "slot", 10, 2, 10.0f, _unitLabels);
            slots2.LoadContent(content);
            _smallFrameUnits = new Frame(_topLeftPosition + new Vector2(0.0f, 0.0f), Alignment.TopLeft, new Vector2(515, 75), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, slots2);
            _smallFrameUnits.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            foreach (var lbl in _unitLabels)
            {
                lbl.Update(input, deltaTime);
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _lblUnits.Draw();
            _smallFrameUnits.Draw();
            spriteBatch.End();
        }

        private List<LabelSized> CreateUnitLabels(ContentManager content)
        {
            var units = new List<LabelSized>();

            var baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            var baseTopLeftY = (int)(_topLeftPosition.Y + 15.0f);
            var x = baseTopLeftX;
            var y = baseTopLeftY;

            foreach (var unit in Globals.Instance.UnitTypes)
            {
                if (_parent.Settlement.UnitCanBeBuilt(unit.Name))
                {
                    var lbl = new LabelSized(new Vector2(x, y), Alignment.TopLeft, new Vector2(42.0f, 20.0f), Alignment.MiddleCenter, unit.ShortName, "CrimsonText-Regular-6", Color.Red, null, Color.PowderBlue, null, 0.0f, null, unit.Name);
                    lbl.LoadContent(content);
                    lbl.Click += UnitClick;
                    units.Add(lbl);

                    x += 49;
                }
            }

            return units;
        }

        private void UnitClick(object sender, EventArgs e)
        { 
            var unit = (LabelSized)sender;
            var unit2 = Globals.Instance.UnitTypes[unit.Name];
            _parent.Settlement.AddToProductionQueue(unit2);
        }
    }
}