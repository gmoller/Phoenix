using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using Zen.GuiControls;
using Zen.GuiControls.PackagesClasses;
using Zen.Input;
using Zen.MonoGameUtilities.ExtensionMethods;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views.SettlementViewComposite
{
    internal class SecondaryFrame : Control
    {
        #region State
        private readonly SettlementView _settlementView;
        private Controls Controls { get; }
        #endregion State

        internal SecondaryFrame(SettlementView settlementView, Vector2 topLeftPosition, string textureAtlas) :
            base("SecondaryFrame")
        {
            Size = new PointI(556, 741);
            SetPosition(topLeftPosition.ToPointI());

            _settlementView = settlementView;

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("textureName1", $"{textureAtlas}.frame_main"),
                new KeyValuePair<string, string>("textureName2", $"{textureAtlas}.frame2_whole"),
                new KeyValuePair<string, string>("textureName3", $"{textureAtlas}.frame_bottom"),
                new KeyValuePair<string, string>("position1", $"{Convert.ToInt32(topLeftPosition.X)};{Convert.ToInt32(topLeftPosition.Y)}"),
                new KeyValuePair<string, string>("size1", $"{Size.X};{Size.Y}"),
            };

            var spec = ResourceReader.ReadResource("PhoenixGamePresentation.Views.SettlementViewComposite.SecondaryFrameControls.txt", Assembly.GetExecutingAssembly());
            Controls = ControlCreator.CreateFromSpecification(spec, pairs);

            Controls["frmSecondary.frmBuildings"].AddControl(new BuildingsView("buildingsView", settlementView, textureAtlas), Alignment.TopCenter);
            var slots20 = new DynamicSlots("slots20", $"{textureAtlas}.slot", 10, 2, 10.0f)
            {
                Size = new PointI(515, 65)
            };
            Controls["frmSecondary.frmUnits"].AddControl(slots20, Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));
            CreateUnitLabels(Controls["frmSecondary.frmUnits.slots20"]);
            var slots2 = new DynamicSlots("slots2", $"{textureAtlas}.slot", 2, 1, 10.0f)
            {
                Size = new PointI(515, 65)
            };
            Controls["frmSecondary.frmOther"].AddControl(slots2, Alignment.TopLeft, Alignment.TopLeft, new PointI(0, 5));
        }

        public override void LoadContent(ContentManager content, bool loadChildrenContent = false)
        {
            Controls.LoadContent(content, true);
        }

        public override void Update(InputHandler input, float deltaTime, Viewport? viewport)
        {
            Controls.Update(input, deltaTime, viewport);
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
                if (_settlementView.Settlement.UnitCanBeBuilt(unit.Name))
                {
                    var lbl = new Label(unit.Name, "CrimsonText-Regular-6")
                    {
                        Size = new PointI(42, 20),
                        ContentAlignment = Alignment.MiddleCenter,
                        Text = unit.ShortName,
                        TextColor = Color.Red,
                        BackgroundColor = Color.PowderBlue
                    };
                    slots[i].AddControl(lbl, Alignment.TopLeft, Alignment.TopLeft);
                    slots[i][unit.Name].AddPackage(new ControlClick((o, args) => UnitClick(o, new EventArgs())));

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
            _settlementView.Settlement.AddToProductionQueue(unitType);
        }
    }
}