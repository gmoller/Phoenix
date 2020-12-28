using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using Zen.GuiControls;
using Zen.GuiControls.PackagesClasses;
using Zen.GuiControls.TheControls;
using Zen.Input;
using Zen.MonoGameUtilities.ExtensionMethods;
using Zen.Utilities;
using DynamicSlots = PhoenixGamePresentation.ControlsX.DynamicSlots;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class SecondaryFrame : Control
    {
        #region State
        private SettlementView SettlementView { get; }
        private Controls Controls { get; }
        #endregion State

        internal SecondaryFrame(SettlementView settlementView, Vector2 topLeftPosition, string textureAtlas) :
            base("SecondaryFrame")
        {
            Size = new PointI(556, 741);
            Position = topLeftPosition.ToPointI();

            SettlementView = settlementView;

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textureName1", $"{textureAtlas}.frame_main"),
                new KeyValuePair<string, string>("textureName2", $"{textureAtlas}.frame2_whole"),
                new KeyValuePair<string, string>("textureName3", $"{textureAtlas}.frame_bottom"),
                new KeyValuePair<string, string>("position1", $"{Convert.ToInt32(topLeftPosition.X)};{Convert.ToInt32(topLeftPosition.Y)}"),
                new KeyValuePair<string, string>("size1", $"{Size.X};{Size.Y}")
            };

            var spec = ResourceReader.ReadResource("PhoenixGamePresentation.Views.SettlementViewComposite.SecondaryFrameControls.txt", Assembly.GetExecutingAssembly());
            Controls = ControlCreator.CreateFromSpecification(spec, pairs);

            var ctrl = new BuildingsView("buildingsView", settlementView, textureAtlas);
            var frmBuildings = Controls["frmSecondary.frmBuildings"];
            frmBuildings.AddControl(ctrl, Alignment.TopCenter);
            var slots20 = new DynamicSlots("slots20", $"{textureAtlas}.slot", 10, 2, 10.0f)
            {
                Size = new PointI(515, 65)
            };

            var frmUnits = Controls["frmSecondary.frmUnits"];
            frmUnits.AddControl(slots20, Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));
            var slots20_ = Controls["frmSecondary.frmUnits.slots20"];
            CreateUnitLabels(slots20_);
            var slots2 = new DynamicSlots("slots2", $"{textureAtlas}.slot", 2, 1, 10.0f)
            {
                Size = new PointI(515, 65)
            };
            var frmOther = Controls["frmSecondary.frmOther"];
            frmOther.AddControl(slots2, Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));
        }

        public override IControl Clone()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            Controls.LoadContent(content, true);
        }

        public override void Update(InputHandler input, GameTime gameTime, Viewport? viewport)
        {
            Controls.Update(input, gameTime, viewport);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Controls.Draw(spriteBatch);
        }

        private void CreateUnitLabels(IControl slots)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;
            var i = 0;
            foreach (var unit in unitTypes)
            {
                if (SettlementView.Settlement.UnitCanBeBuilt(unit.Name))
                {
                    var lbl = new Label(unit.Name)
                    {
                        FontName = "CrimsonText-Regular-6",
                        Size = new PointI(42, 20),
                        ContentAlignment = Alignment.MiddleCenter,
                        Text = unit.ShortName,
                        Color = Color.Red,
                        BackgroundColor = Color.PowderBlue
                    };
                    var slot = slots[i];
                    slot.AddControl(lbl, Alignment.TopLeft, Alignment.TopLeft);
                    var unitLabel = slot[unit.Name];
                    unitLabel.AddPackage(new ControlClick((o, args) => UnitClick(o, new EventArgs())));

                    i++;
                }
            }
        }

        private void UnitClick(object sender, EventArgs e)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;

            var unit = (Label)sender;
            var unitType = unitTypes[unit.Name];
            SettlementView.Settlement.AddToProductionQueue(unitType);
        }
    }
}