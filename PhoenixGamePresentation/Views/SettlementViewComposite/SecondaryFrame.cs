using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Input;
using PhoenixGameLibrary;
using Utilities;
using Color = Microsoft.Xna.Framework.Color;
using PointI = Utilities.PointI;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class SecondaryFrame : Control
    {
        #region State
        private readonly SettlementView _settlementView;

        private Frame _frmSecondary;
        #endregion State

        internal SecondaryFrame(SettlementView settlementView, Vector2 topLeftPosition, string textureAtlas) :
            base(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), textureAtlas, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "SecondaryFrame")
        {
            _settlementView = settlementView;

            _frmSecondary = new Frame(topLeftPosition, Alignment.TopLeft, new Vector2(556.0f, 741.0f), TextureAtlas, "frame_main", "frmSecondary");

            _frmSecondary.AddControl(new Frame("frmBuildings", new Vector2(515.0f, 450.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 50));
            _frmSecondary["frmBuildings"].AddControl(new LabelSized("lblBuildings", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Buildings", "CrimsonText-Regular-12", Color.Orange, Color.DarkBlue), Alignment.TopLeft, Alignment.TopLeft, new PointI(20, 0));
            _frmSecondary["frmBuildings"].AddControl(new BuildingsView("buildingsView", settlementView, TextureAtlas), Alignment.TopCenter);

            _frmSecondary.AddControl(new Frame("frmUnits", new Vector2(515.0f, 75.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 535));
            _frmSecondary["frmUnits"].AddControl(new LabelSized("lblUnits", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Units", "CrimsonText-Regular-12", Color.Orange, Color.DarkBlue), Alignment.TopLeft, Alignment.TopLeft, new PointI(20, 0));
            _frmSecondary["frmUnits"].AddControl(new DynamicSlots("slots20", new Vector2(515.0f, 65.0f), TextureAtlas, "slot", 10, 2, 10.0f), Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));

            CreateUnitLabels(_frmSecondary["frmUnits.slots20"]);

            _frmSecondary.AddControl(new Frame("frmOther", new Vector2(515.0f, 65.0f), TextureAtlas, "frame2_whole", 50, 50, 50, 50), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 640));
            _frmSecondary["frmOther"].AddControl(new LabelSized("lblOther", new Vector2(100.0f, 15.0f), Alignment.TopLeft, "Other", "CrimsonText-Regular-12", Color.Orange, Color.DarkBlue), Alignment.TopLeft, Alignment.TopLeft, new PointI(20, 0));
            _frmSecondary["frmOther"].AddControl(new DynamicSlots("slots2", new Vector2(515.0f, 55.0f), TextureAtlas, "slot", 2, 1, 10.0f), Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));

            _frmSecondary.AddControl(new Frame("frmFooter", new Vector2(563.0f, 71.0f), TextureAtlas, "frame_bottom"), Alignment.BottomCenter, Alignment.BottomCenter, new PointI(0, 5));
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            _frmSecondary.LoadContent(content);
            _frmSecondary["frmUnits"].LoadContent(content);
            _frmSecondary["frmBuildings"].LoadContent(content);
            _frmSecondary["frmBuildings.lblBuildings"].LoadContent(content);
            _frmSecondary["frmBuildings.buildingsView"].LoadContent(content);
            _frmSecondary["frmUnits.lblUnits"].LoadContent(content);
            _frmSecondary["frmUnits.slots20"].LoadContent(content, true);
            _frmSecondary["frmOther"].LoadContent(content);
            _frmSecondary["frmOther.lblOther"].LoadContent(content);
            _frmSecondary["frmOther.slots2"].LoadContent(content);
            _frmSecondary["frmFooter"].LoadContent(content);
        }

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            _frmSecondary.Update(input, deltaTime, viewport);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _frmSecondary.Draw(spriteBatch);
         }

        private void CreateUnitLabels(IControl slots)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;
            var i = 0;
            foreach (var unit in unitTypes)
            {
                if (_settlementView.Settlement.UnitCanBeBuilt(unit.Name))
                {
                    slots[i].AddControl(new LabelSized(unit.Name, new Vector2(42.0f, 20.0f), Alignment.MiddleCenter, unit.ShortName, "CrimsonText-Regular-6", Color.Red, null, Color.PowderBlue), Alignment.TopLeft, Alignment.TopLeft);
                    slots[i][unit.Name].Click += UnitClick;

                    i++;
                }
            }
        }

        private void UnitClick(object sender, EventArgs e)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;

            var unit = (LabelSized)sender;
            var unitType = unitTypes[unit.Name];
            _settlementView.Settlement.AddToProductionQueue(unitType);
        }
    }
}