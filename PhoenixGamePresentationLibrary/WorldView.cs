using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using HexLibrary;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class WorldView
    {
        private OverlandMapView _overlandMapView;
        private OverlandSettlementViews _overlandSettlementsView;
        private StackViews _stackViews;
        private SettlementViews _settlementViews;
        private HudView _hudView;
        private Dictionary<string, Image> _movementTypeImages;
        private Dictionary<string, Button> _actionButtons;

        public Camera Camera { get; private set; }
        public World World { get; }
        public EnumerableDictionary<Image> MovementTypeImages => new EnumerableDictionary<Image>(_movementTypeImages);
        public EnumerableDictionary<Button> ActionButtons => new EnumerableDictionary<Button>(_actionButtons);

        internal WorldView(World world)
        {
            World = world;
        }

        internal void LoadContent(ContentManager content)
        {
            _overlandMapView = new OverlandMapView(this, World.OverlandMap);
            _overlandSettlementsView = new OverlandSettlementViews(this, World.Settlements);
            _stackViews = new StackViews(this, World.Stacks);
            _settlementViews = new SettlementViews(this, World.Settlements);
            _hudView = new HudView(this, _stackViews);

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.ScreenResolution.X, DeviceManager.Instance.ScreenResolution.Y));
            Camera.LoadContent(content);

            _overlandSettlementsView.LoadContent(content);
            _stackViews.LoadContent(content);
            _settlementViews.LoadContent(content);
            _hudView.LoadContent(content);

            _movementTypeImages = LoadMovementTypeImages(content);
            _actionButtons = LoadActionButtons(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            Camera.Update(input, deltaTime);

            var worldPosPointedAtByMouseCursor = Camera.ScreenToWorld(new Vector2(input.MousePosition.X, input.MousePosition.Y));
            DeviceManager.Instance.WorldPositionPointedAtByMouseCursor = new Utilities.Point((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
            var worldHex = HexOffsetCoordinates.FromPixel((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
            DeviceManager.Instance.WorldHexPointedAtByMouseCursor = new Utilities.Point(worldHex.Col, worldHex.Row);

            _overlandMapView.Update(input, deltaTime);
            _overlandSettlementsView.Update(input, deltaTime);
            _stackViews.Update(input, deltaTime);
            _settlementViews.Update(input, deltaTime);
            _hudView.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform* DeviceManager.Instance.ViewportAdapter.GetScaleMatrix()); // FrontToBack
            _overlandMapView.Draw(spriteBatch);
            _overlandSettlementsView.Draw(spriteBatch);

            _stackViews.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: DeviceManager.Instance.ViewportAdapter.GetScaleMatrix());
            _hudView.Draw(spriteBatch);
            _settlementViews.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void BeginTurn()
        {
            Command beginTurnCommand = new BeginTurnCommand { Payload = World };
            beginTurnCommand.Execute();

            _stackViews.BeginTurn();
        }

        public void EndTurn()
        {
            Command endTurnCommand = new EndTurnCommand
            {
                Payload = World
            };
            endTurnCommand.Execute();

            BeginTurn();
        }

        private Dictionary<string, Image> LoadMovementTypeImages(ContentManager content)
        {
            var movementTypes = Globals.Instance.MovementTypes;

            var movementTypeImages = new Dictionary<string, Image>();
            foreach (var movementType in movementTypes)
            {
                var image = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(18.0f, 12.0f), "MovementTypes", movementType.Name, "image");
                image.LoadContent(content);
                movementTypeImages.Add(movementType.Name, image);
            }

            return movementTypeImages;
        }

        private Dictionary<string, Button> LoadActionButtons(ContentManager content)
        {
            var actionTypes = Globals.Instance.ActionTypes;

            var actionButtons = new Dictionary<string, Button>();
            var i = 0;
            foreach (var actionType in actionTypes)
            {
                i++;
                var button = new Button(Vector2.Zero, Alignment.TopLeft, new Vector2(115.0f, 30.0f), "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_n", "simpleb_h", "button");
                button.LoadContent(content);
                button.Click += (o, args) => BtnClick(o, new ButtonClickEventArgs(actionType.Name));
                var label = new LabelSized(button.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button.Size.ToVector2(), Alignment.MiddleCenter, actionType.ButtonName, "Maleficio-Regular-12", Color.Black, $"label{i}", null, button);
                label.LoadContent(content);
                //var test = button.ChildControls[label.Name];

                actionButtons.Add(actionType.Name, button);
            }

            return actionButtons;
        }

        private void BtnClick(object sender, ButtonClickEventArgs e)
        {
            switch (e.Action)
            {
                case "Done":
                    _stackViews.DoDoneAction(); 
                    break;
                case "Patrol":
                    _stackViews.DoPatrolAction();
                    break;
                case "Wait":
                    _stackViews.DoWaitAction();
                    break;
                case "BuildOutpost":
                    _stackViews.DoBuildAction();
                    break;
                case "Fortify":
                    _stackViews.DoFortifyAction();
                    break;
                case "Explore":
                    _stackViews.DoExploreAction();
                    break;
                default:
                    throw new Exception($"Action [{e.Action}] is not implemented.");
            }
        }
    }
}