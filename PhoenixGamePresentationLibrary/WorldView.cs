using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using HexLibrary;
using Input;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class WorldView
    {
        #region State
        public World World { get; }

        private OverlandMapView _overlandMapView;
        private OverlandSettlementViews _overlandSettlementsView;
        private StackViews _stackViews;
        private SettlementView _settlementView;
        private HudView _hudView;
        private Dictionary<string, Image> _movementTypeImages;
        private Dictionary<string, Button> _actionButtons;

        public Camera Camera { get; private set; }
        #endregion 

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
            _settlementView = new SettlementView(this, World.Settlements[0]);
            _settlementView.LoadContent(content);
            _hudView = new HudView(this, _stackViews);

            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            Camera = new Camera(new Rectangle(0, 0, context.ActualResolution.X, context.ActualResolution.Y));
            Camera.LoadContent(content);

            _movementTypeImages = LoadMovementTypeImages(content);
            _actionButtons = LoadActionButtons(content);

            _overlandSettlementsView.LoadContent(content);
            _stackViews.LoadContent(content);

            _hudView.LoadContent(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            Camera.Update(input, deltaTime);

            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");

            var worldPositionPointedAtByMouseCursor = GetWorldPositionPointedAtByMouseCursor(Camera, input.MousePosition);
            context.WorldPositionPointedAtByMouseCursor = worldPositionPointedAtByMouseCursor;
            context.WorldHexPointedAtByMouseCursor = GetWorldHexPointedAtByMouseCursor(worldPositionPointedAtByMouseCursor);

            _overlandMapView.Update(input, deltaTime);
            _overlandSettlementsView.Update(input, deltaTime);
            _stackViews.Update(input, deltaTime);

            _settlementView.Settlement = World.Settlements.Selected;
            if (_settlementView.Settlement != null)
            {
                _settlementView.Update(input, deltaTime);
            }

            _hudView.Update(input, deltaTime);
            _stackViews.RemoveDeadUnits();
        }

        internal void Draw(SpriteBatch spriteBatch, ViewportAdapter viewportAdapter)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform * viewportAdapter.GetScaleMatrix()); // FrontToBack
            _overlandMapView.Draw(spriteBatch);
            _overlandSettlementsView.Draw(spriteBatch);

            _stackViews.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: viewportAdapter.GetScaleMatrix());
            _hudView.Draw(spriteBatch);
            if (_settlementView.Settlement != null)
            {
                _settlementView.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private Utilities.Point GetWorldPositionPointedAtByMouseCursor(Camera camera, Microsoft.Xna.Framework.Point mousePosition)
        {
            var worldPosPointedAtByMouseCursor = Camera.ScreenToWorld(new Vector2(mousePosition.X, mousePosition.Y));

            return new Utilities.Point((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
        }

        private Utilities.Point GetWorldHexPointedAtByMouseCursor(Utilities.Point worldPositionPointedAtByMouseCursor)
        {
            var worldHex = HexOffsetCoordinates.FromPixel(worldPositionPointedAtByMouseCursor.X, worldPositionPointedAtByMouseCursor.Y);

            return new Utilities.Point(worldHex.Col, worldHex.Row);
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
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var movementTypes = context.GameMetadata.MovementTypes;

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
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var actionTypes = context.GameMetadata.ActionTypes;

            var actionButtons = new Dictionary<string, Button>();
            var i = 0;
            var x = 1680; // position of unitFrame BottomRight: (1680;806)
            var y = 806;
            var buttonSize = new Vector2(115.0f, 30.0f);
            foreach (var actionType in actionTypes)
            {
                var xOffset = buttonSize.X * (i % 2);
                var yOffset = buttonSize.Y * (i / 2); // math.Floor
                var position = new Vector2(x + xOffset, y + yOffset);
                i++;

                var button = new Button(position, Alignment.TopLeft, buttonSize, "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_a", "simpleb_h", actionType.Name);
                button.LoadContent(content);
                button.Click += (o, args) => BtnClick(o, new ButtonClickEventArgs(actionType.Name));
                var label = new LabelSized(button.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button.Size.ToVector2(), Alignment.MiddleCenter, actionType.ButtonName, "Maleficio-Regular-12", Color.Black, $"label{i}", null);
                button.AddControl(label);
                label.LoadContent(content);

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