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

        private LabelOld _lblUnits;
        private Frame _smallFrameUnits;
        private List<Label> _unitLabels;

        internal UnitsFrame(SettlementView parent, Vector2 topLeftPosition)
        {
            _parent = parent;
            _topLeftPosition = topLeftPosition;
        }

        internal void LoadContent(ContentManager content)
        {
            _lblUnits = new LabelOld("lblUnits", "CrimsonText-Regular-12", _topLeftPosition + new Vector2(0.0f, -10.0f), HorizontalAlignment.Left, VerticalAlignment.Top, new Vector2(10, 10), string.Empty, HorizontalAlignment.Left, Color.Orange, Color.Red);

            _unitLabels = CreateUnitLabels(content);

            var slots2 = new DynamicSlots("slots2", _topLeftPosition + new Vector2(0.0f, 0.0f), ContentAlignment.TopLeft, new Vector2(515, 75), "GUI_Textures_1", "slot", 10, 2, 10.0f, _unitLabels);
            slots2.LoadContent(content);
            _smallFrameUnits = new Frame("SmallFrameUnits", _topLeftPosition + new Vector2(0.0f, 0.0f), ContentAlignment.TopLeft, new Vector2(515, 75), "GUI_Textures_1", "frame2_whole", 50, 50, 50, 50, slots2);
            _smallFrameUnits.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            _lblUnits.Text = "Units";

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

        private List<Label> CreateUnitLabels(ContentManager content)
        {
            var units = new List<Label>();

            int baseTopLeftX = (int)(_topLeftPosition.X + 15.0f);
            int baseTopLeftY = (int)(_topLeftPosition.Y + 15.0f);
            int x = baseTopLeftX;
            int y = baseTopLeftY;

            foreach (var unit in Globals.Instance.UnitTypes)
            {
                if (_parent.Settlement.UnitCanBeBuilt(unit.Name))
                {
                    var lbl = new Label(unit.Name, new Vector2(x, y), ContentAlignment.TopLeft, new Vector2(42.0f, 20.0f), unit.ShortName, "CrimsonText-Regular-6", Color.Red, null, Color.PowderBlue);
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
            var unit = (Label)sender;
            var unit2 = Globals.Instance.UnitTypes[unit.Name];
            _parent.Settlement.AddToProductionQueue(unit2);
        }
    }
}