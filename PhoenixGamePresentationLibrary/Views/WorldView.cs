using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using GuiControls;
using HexLibrary;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentationLibrary.ExtensionMethods;
using Utilities;
using Color = Microsoft.Xna.Framework.Color;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary.Views
{
    public class WorldView
    {
        #region State
        public World World { get; }

        private readonly OverlandMapView _overlandMapView;
        private readonly OverlandSettlementViews _overlandSettlementsView;
        private readonly StackViews _stackViews;
        private readonly SettlementView _settlementView;
        private readonly HudView _hudView;
        private readonly Dictionary<string, IControl> _movementTypeImages;
        private readonly Dictionary<string, IControl> _actionButtons;

        public Camera Camera { get; }

        public GameStatus GameStatus { get; set; }
        #endregion 

        public EnumerableDictionary<IControl> MovementTypeImages => new EnumerableDictionary<IControl>(_movementTypeImages);
        public EnumerableDictionary<IControl> ActionButtons => new EnumerableDictionary<IControl>(_actionButtons);

        internal WorldView(World world)
        {
            World = world;

            _overlandMapView = new OverlandMapView(this, World.OverlandMap);
            _overlandSettlementsView = new OverlandSettlementViews(this, World.Settlements);
            _stackViews = new StackViews(this, World.Stacks);
            _settlementView = new SettlementView(this, World.Settlements[0]);
            _hudView = new HudView(this, _stackViews);

            _movementTypeImages = InitializeMovementTypeImages();
            _actionButtons = InitializeActionButtons();

            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            Camera = new Camera(this, new Rectangle(0, 0, context.ActualResolution.X, context.ActualResolution.Y));
        }

        internal void LoadContent(ContentManager content)
        {
            Camera.LoadContent(content);
            _overlandMapView.LoadContent(content);
            _overlandSettlementsView.LoadContent(content);
            _stackViews.LoadContent(content);
            _settlementView.LoadContent(content);
            _hudView.LoadContent(content);

            _movementTypeImages.LoadContent(content);
            _actionButtons.LoadContent(content, true);
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

        private Point GetWorldPositionPointedAtByMouseCursor(Camera camera, Microsoft.Xna.Framework.Point mousePosition)
        {
            var worldPosPointedAtByMouseCursor = camera.ScreenToWorld(new Vector2(mousePosition.X, mousePosition.Y));

            return new Point((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
        }

        private Point GetWorldHexPointedAtByMouseCursor(Point worldPositionPointedAtByMouseCursor)
        {
            var worldHex = HexOffsetCoordinates.FromPixel(worldPositionPointedAtByMouseCursor.X, worldPositionPointedAtByMouseCursor.Y);

            return new Point(worldHex.Col, worldHex.Row);
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

        private Dictionary<string, IControl> InitializeMovementTypeImages()
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var movementTypes = context.GameMetadata.MovementTypes;

            var movementTypeImages = new Dictionary<string, IControl>();
            foreach (var movementType in movementTypes)
            {
                var image = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(18.0f, 12.0f), "MovementTypes", movementType.Name, "image");
                movementTypeImages.Add(movementType.Name, image);
            }

            return movementTypeImages;
        }

        private Dictionary<string, IControl> InitializeActionButtons()
        {
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var actionTypes = context.GameMetadata.ActionTypes;

            var actionButtons = new Dictionary<string, IControl>();
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

                var button = new Button(position, Alignment.TopLeft, buttonSize, "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_h", "simpleb_a", actionType.Name);
                button.Click += (o, args) => BtnClick(o, new ButtonClickEventArgs(actionType.Name));
                var label = new LabelSized(button.Size.ToVector2() * Constants.ONE_HALF, Alignment.MiddleCenter, button.Size.ToVector2(), Alignment.MiddleCenter, actionType.ButtonName, "Maleficio-Regular-12", Color.Black, $"label{i}", null);
                button.AddControl(label);

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