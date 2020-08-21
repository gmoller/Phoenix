using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using PhoenixGameLibrary;

namespace PhoenixGamePresentationLibrary.SettlementViewComposite
{
    internal class SecondaryFrame : Control
    {
        #region State
        private readonly SettlementView _settlementView;

        private Frame _frmSecondary;
        #endregion

        internal SecondaryFrame(SettlementView settlementView, Vector2 topLeftPosition, string textureAtlas) :
            base(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), textureAtlas, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "SecondaryFrame")
        {
            _settlementView = settlementView;

            _frmSecondary = new Frame(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), TextureAtlas, "frame_main", "frmSecondary");

            var frmUnits = new Frame(new Vector2(20.0f, 535.0f), Alignment.TopLeft, new Vector2(515.0f, 75.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmUnits", _frmSecondary);
            var lblUnits = new LabelAutoSized(new Vector2(20.0f, 0.0f), Alignment.TopLeft, "Units", "CrimsonText-Regular-12", Color.Orange, "lblUnits", Color.DarkBlue, frmUnits);
            var slots20 = new DynamicSlots(new Vector2(0.0f, 5.0f), Alignment.TopLeft, new Vector2(515.0f, 65.0f), TextureAtlas, "slot", 10, 2, 10.0f, "slots20", frmUnits);
            CreateUnitLabels(slots20, frmUnits.Area);

            var frmOther = new Frame(new Vector2(20.0f, 640.0f), Alignment.TopLeft, new Vector2(515.0f, 65.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50, "frmOther", _frmSecondary);
            var lblOther = new LabelAutoSized(new Vector2(20.0f, 0.0f), Alignment.TopLeft, "Other", "CrimsonText-Regular-12", Color.Orange, "lblOther", Color.DarkBlue, frmOther);
            var slots2 = new DynamicSlots(new Vector2(0.0f, 5.0f), Alignment.TopLeft, new Vector2(515.0f, 55.0f), TextureAtlas, "slot", 2, 1, 10.0f, "slots2", frmOther);

            var frmFooter = new Frame(new Vector2(-2.0f, 680.0f), Alignment.TopLeft, new Vector2(563.0f, 71.0f), TextureAtlas, "frame_bottom", "frmFooter", _frmSecondary);
        }

        public override void LoadContent(ContentManager content)
        {
            _frmSecondary.LoadContent(content);
            _frmSecondary["frmUnits"].LoadContent(content);
            _frmSecondary["frmUnits.lblUnits"].LoadContent(content);
            _frmSecondary["frmUnits.slots20"].LoadContent(content);
            _frmSecondary["frmOther"].LoadContent(content);
            _frmSecondary["frmOther.lblOther"].LoadContent(content);
            _frmSecondary["frmOther.slots2"].LoadContent(content);
            _frmSecondary["frmFooter"].LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Matrix? transform = null)
        {
            _frmSecondary.Update(input, deltaTime, transform);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _frmSecondary.Draw(spriteBatch);
        }

        private void CreateUnitLabels(IControl slots, Rectangle area)
        {
            var baseTopLeftX = (int)(area.X + 15.0f);
            var baseTopLeftY = (int)(area.Y + 15.0f);
            var x = baseTopLeftX;
            var y = baseTopLeftY;

            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var unitTypes = context.GameMetadata.UnitTypes;
            foreach (var unit in unitTypes)
            {
                if (_settlementView.Settlement.UnitCanBeBuilt(unit.Name))
                {
                    var lbl = new LabelSized(new Vector2(x, y), Alignment.TopLeft, new Vector2(42.0f, 20.0f), Alignment.MiddleCenter, unit.ShortName, "CrimsonText-Regular-6", Color.Red, unit.Name, null, Color.PowderBlue);
                    lbl.Click += UnitClick;
                    slots.AddControl(lbl);

                    x += 49;
                }
            }
        }

        private void UnitClick(object sender, EventArgs e)
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var unitTypes = context.GameMetadata.UnitTypes;

            var unit = (LabelSized)sender;
            var unitType = unitTypes[unit.Name];
            _settlementView.Settlement.AddToProductionQueue(unitType);
        }
    }
}