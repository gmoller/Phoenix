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

            var buildingsView = new BuildingsView("buildingsView", settlementView, textureAtlas);
            var frmBuildings = Controls["frmSecondary.frmBuildings"];
            frmBuildings.AddControl(buildingsView, Alignment.TopCenter);

            var slots10 = new DynamicSlots("slots20", $"{textureAtlas}.slot", new PointI(515, 65), 5, 2, 10.0f);
            var frmUnits = Controls["frmSecondary.frmUnits"];
            frmUnits.AddControl(slots10, Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));
            CreateUnitLabels(slots10);

            var slots5 = new DynamicSlots("slots2", $"{textureAtlas}.slot", new PointI(515, 65), 5, 1, 10.0f);
            var frmOther = Controls["frmSecondary.frmOther"];
            frmOther.AddControl(slots5, Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));
            CreateOtherLabels(slots5);
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
                    var lbl = InstantiateLabel("CrimsonText-Regular-8", new PointI(84, 20), unit.Name, unit.ShortName);
                    var slot = slots[i];
                    slot.AddControl(lbl, Alignment.MiddleCenter, Alignment.MiddleCenter);
                    lbl.AddPackage(new ControlClick((o, args) => UnitClick(o, new EventArgs())));

                    i++;
                }
            }
        }

        private void CreateOtherLabels(IControl slots)
        {
            var lbl1 = InstantiateLabel("CrimsonText-Regular-10", new PointI(84, 40), "Housing", "Housing");
            var lbl2 = InstantiateLabel("CrimsonText-Regular-10", new PointI(84, 40), "TradeGoods", $"Trade{Environment.NewLine}Goods");

            var slot0 = slots[0];
            slot0.AddControl(lbl1, Alignment.MiddleCenter, Alignment.MiddleCenter);
            lbl1.AddPackage(new ControlClick((o, args) => OtherClick(o, new EventArgs())));

            var slot1 = slots[1];
            slot1.AddControl(lbl2, Alignment.MiddleCenter, Alignment.MiddleCenter);
            lbl2.AddPackage(new ControlClick((o, args) => OtherClick(o, new EventArgs())));
        }

        private Label InstantiateLabel(string font, PointI size, string name, string text)
        {
            var lbl = new Label(name)
            {
                FontName = font,
                Size = size,
                ContentAlignment = Alignment.MiddleCenter,
                Text = text,
                Color = Color.Red,
                BackgroundColor = Color.PowderBlue
            };

            return lbl;
        }

        private void UnitClick(object sender, EventArgs e)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var unitTypes = gameMetadata.UnitTypes;

            var unit = (Label)sender;
            var unitType = unitTypes[unit.Name];
            SettlementView.Settlement.AddToProductionQueue(unitType);
        }

        private void OtherClick(object sender, EventArgs e)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            //var otherTypes = gameMetadata.OtherTypes;

            var other = (Label)sender;
            //var otherType = otherTypes[other.Name];
            //SettlementView.Settlement.AddToProductionQueue(otherType);
        }
    }
}