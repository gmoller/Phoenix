using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using Hex;
using Input;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGamePresentation.ExtensionMethods;
using Utilities;
using Color = Microsoft.Xna.Framework.Color;

namespace PhoenixGamePresentation.Views
{
    public class WorldView : IDisposable
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

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        public int WorldWidthInPixels => Constants.WORLD_MAP_WIDTH_IN_PIXELS;
        public int WorldHeightInPixels => Constants.WORLD_MAP_HEIGHT_IN_PIXELS;
        public EnumerableDictionary<IControl> MovementTypeImages => new EnumerableDictionary<IControl>(_movementTypeImages);
        public EnumerableDictionary<IControl> ActionButtons => new EnumerableDictionary<IControl>(_actionButtons);

        public WorldView(World world, InputHandler input)
        {
            World = world;

            _overlandMapView = new OverlandMapView(this, World.OverlandMap, input);
            _overlandSettlementsView = new OverlandSettlementViews(this, World.Settlements, input);
            _stackViews = new StackViews(this, World.Stacks, input);
            _settlementView = new SettlementView(this, World.Settlements.Count > 0 ? World.Settlements[0] : new Settlement(World, "Test", "Barbarians", PointI.Zero, 1, World.OverlandMap.CellGrid), input);
            _hudView = new HudView(this, _stackViews, input);

            _movementTypeImages = InitializeMovementTypeImages();
            _actionButtons = InitializeActionButtons();

            Camera = new Camera(this, new Rectangle(0, 0, 1670, 1080), CameraClampMode.AutoClamp, input);

            _input = input;
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

        internal void Update(float deltaTime)
        {
            Camera.Update(deltaTime);

            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");

            //TODO: change this
            context.WorldPositionPointedAtByMouseCursor = GetWorldPositionPointedAtByMouseCursor(Camera, _input.MousePosition);
            context.WorldHexPointedAtByMouseCursor = GetWorldHexPointedAtByMouseCursor(context.WorldPositionPointedAtByMouseCursor);

            _overlandMapView.Update(deltaTime);
            _overlandSettlementsView.Update(deltaTime);
            _stackViews.Update(deltaTime);

            _settlementView.Settlement = World.Settlements.Selected;
            if (_settlementView.Settlement != null)
            {
                _settlementView.Update(deltaTime, null);
            }

            _hudView.Update(deltaTime);
            _stackViews.RemoveDeadUnits();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _overlandMapView.Draw(spriteBatch, Camera);
            _overlandSettlementsView.Draw(spriteBatch, Camera);
            _stackViews.Draw(spriteBatch, Camera);

            _hudView.Draw(spriteBatch);

            _settlementView.Draw(spriteBatch);
        }

        private PointI GetWorldPositionPointedAtByMouseCursor(Camera camera, Point mousePosition)
        {
            var worldPosPointedAtByMouseCursor = camera.ScreenToWorld(new Vector2(mousePosition.X, mousePosition.Y));

            return new PointI((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
        }

        private PointI GetWorldHexPointedAtByMouseCursor(PointI worldPositionPointedAtByMouseCursor)
        {
            var worldHex = HexOffsetCoordinates.FromPixel(worldPositionPointedAtByMouseCursor.X, worldPositionPointedAtByMouseCursor.Y);

            return new PointI(worldHex.Col, worldHex.Row);
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
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var movementTypes = gameMetadata.MovementTypes;

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
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var actionTypes = gameMetadata.ActionTypes;

            var actionButtons = new Dictionary<string, IControl>();
            var i = 0;
            //var x = 1680; // position of unitFrame BottomRight: (1680;806)
            var x = 10;
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

        #region Event Handlers

        private void BtnClick(object sender, ButtonClickEventArgs e)
        {
            //TODO: ToDictionary
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

        #endregion

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // TODO: dispose managed state (managed objects)

                // TODO: set large fields to null

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}