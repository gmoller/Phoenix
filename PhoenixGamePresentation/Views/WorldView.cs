using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GuiControls;
using GuiControls.PackagesClasses;
using Hex;
using Input;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using PhoenixGamePresentation.ExtensionMethods;
using PhoenixGamePresentation.Handlers;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    public class WorldView : IDisposable
    {
        #region State
        private World World { get; }

        private OverlandMapView OverlandMapView { get; }
        private OverlandSettlementViews OverlandSettlementViews { get; }
        private StackViews StackViews { get; }
        private SettlementView SettlementView { get; }
        private HudView HudView { get; }
        private Tooltip Tooltip { get; }
        private Dictionary<string, IControl> MovementTypeImages { get; }
        private Dictionary<string, IControl> ActionButtons { get; }

        public Camera Camera { get; }

        private GameStatusHandler GameStatusHandler { get; }

        private InputHandler Input { get; }
        private Viewport Viewport { get; }
        private ViewportAdapter ViewportAdapter { get; }
        private bool IsDisposed { get; set; }
        #endregion

        public WorldView(World world, CameraClampMode cameraClampMode, InputHandler input)
        {
            World = world;
            GameStatusHandler = new GameStatusHandler(GameStatus.OverlandMap);

            Input = input;

            Camera = new Camera(this, new Rectangle(0, 0, 1680, 1080), cameraClampMode, Input);
            var globalContextPresentation = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            globalContextPresentation.Camera = Camera;

            OverlandMapView = new OverlandMapView(this, World.OverlandMap, Input);
            OverlandSettlementViews = new OverlandSettlementViews(this, World.Settlements, Input);
            StackViews = new StackViews(this, World.Stacks, Input);
            SettlementView = new SettlementView(this, World.Settlements.Count > 0 ? World.Settlements[0] : new Settlement(World, "Test", "Barbarians", PointI.Zero, 1, World.OverlandMap.CellGrid), Input);
            HudView = new HudView(this, StackViews, Input);
            Tooltip = new Tooltip(Vector2.Zero, Alignment.TopLeft, new Vector2(200.0f, 300.0f), "GUI_Textures_1.sp_frame", 25, 25, 25, 25, "tooltip") { Enabled = false };

            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            Viewport = new Viewport(0, 0, Camera.GetViewport.Width, Camera.GetViewport.Height, 0.0f, 1.0f);
            ViewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, Camera.GetViewport.Width, Camera.GetViewport.Height);

            MovementTypeImages = InitializeMovementTypeImages();
            ActionButtons = InitializeActionButtons();
        }

        #region Accessors
        internal HexLibrary HexLibrary => World.HexLibrary;
        internal StackView.StackView CurrentlySelectedStackView => StackViews.Current?.IsSelected == true ? StackViews.Current : null;
        internal CellGrid CellGrid => World.OverlandMap.CellGrid;
        internal Settlements Settlements => World.Settlements;
        internal Stacks Stacks => World.Stacks;
        internal GameStatus GameStatus => GameStatusHandler.GameStatus;
        internal int WorldWidthInPixels => Constants.WORLD_MAP_WIDTH_IN_PIXELS;
        internal int WorldHeightInPixels => Constants.WORLD_MAP_HEIGHT_IN_PIXELS;
        internal EnumerableDictionary<IControl> GetMovementTypeImages => new EnumerableDictionary<IControl>(MovementTypeImages);
        internal EnumerableDictionary<IControl> GetActionButtons => new EnumerableDictionary<IControl>(ActionButtons);
        internal string CurrentDate => World.CurrentDate;
        internal NotificationList NotificationList => World.NotificationList;
        internal Faction PlayerFaction => World.PlayerFaction;
        internal bool AllStacksHaveBeenGivenOrders => StackViews.AllStacksHaveBeenGivenOrders;
        #endregion

        internal void LoadContent(ContentManager content)
        {
            Camera.LoadContent(content);
            OverlandMapView.LoadContent(content);
            OverlandSettlementViews.LoadContent(content);
            StackViews.LoadContent(content);
            SettlementView.LoadContent(content);
            HudView.LoadContent(content);
            Tooltip.LoadContent(content);

            MovementTypeImages.LoadContent(content);
            ActionButtons.LoadContent(content, true);
        }

        internal void Update(float deltaTime)
        {
            Camera.Update(deltaTime);

            OverlandMapView.Update(deltaTime);
            OverlandSettlementViews.Update(deltaTime);
            StackViews.Update(deltaTime);

            SettlementView.Settlement = World.Settlements.Selected;
            if (SettlementView.Settlement != null)
            {
                SettlementView.Update(deltaTime, null);
            }

            HudView.Update(deltaTime);
            StackViews.RemoveDeadUnits();
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            OverlandMapView.Draw(spriteBatch, Camera);
            OverlandSettlementViews.Draw(spriteBatch, Camera);
            StackViews.Draw(spriteBatch, Camera);

            HudView.Draw(spriteBatch);

            if (Tooltip.Enabled)
            {
                var originalViewport = spriteBatch.GraphicsDevice.Viewport;
                spriteBatch.GraphicsDevice.Viewport = Viewport;
                spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: ViewportAdapter.GetScaleMatrix());
                Tooltip.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.GraphicsDevice.Viewport = originalViewport;
            }

            SettlementView.Draw(spriteBatch);
        }

        public void BeginTurn()
        {
            World.BeginTurn();
            StackViews.BeginTurn();
        }

        public void EndTurn()
        {
            if (StackViews.AllStacksHaveBeenGivenOrders)
            {
                World.EndTurn();
                BeginTurn();
            }
        }

        private Dictionary<string, IControl> InitializeMovementTypeImages()
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var movementTypes = gameMetadata.MovementTypes;

            var movementTypeImages = new Dictionary<string, IControl>();
            foreach (var movementType in movementTypes)
            {
                var image = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(18.0f, 12.0f), $"MovementTypes.{movementType.Name}", "image");
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
            var x = 10;
            var y = 806;
            var buttonSize = new Vector2(115.0f, 30.0f);
            foreach (var actionType in actionTypes)
            {
                var xOffset = buttonSize.X * (i % 2);
                var yOffset = buttonSize.Y * (i / 2); // math.Floor
                var position = new Vector2(x + xOffset, y + yOffset);
                i++;

                var textureString = "GUI_Textures_1.simpleb_";
                var button = new Button(position, Alignment.TopLeft, buttonSize, $"{textureString}n", $"{textureString}a", $"{textureString}h", $"{textureString}a", actionType.Name);
                button.AddPackage(new ControlClick((o, args) => BtnClick(o, new ButtonClickEventArgs(actionType.Name))));
                var label = new LabelSized($"label{i}", button.Size.ToVector2(), Alignment.MiddleCenter, actionType.ButtonName, "Maleficio-Regular-12", Color.Black);
                button.AddControl(label, Alignment.MiddleCenter, Alignment.MiddleCenter);

                actionButtons.Add(actionType.Name, button);
            }

            return actionButtons;
        }

        private void HoveringOverTooltip(StackView.StackView stackView)
        {
            var enable = Tooltip.StartHover();

            if (enable)
            {
                EnableTooltip(stackView);
            }
        }

        private void EnableTooltip(StackView.StackView stackView)
        {
            Tooltip.Enabled = true;
            var position = Camera.WorldHexToScreenPixel(stackView.LocationHex).ToPointI() + new PointI(25, 25);
            Tooltip.SetTopLeftPosition(position);
            Tooltip.SetText();
        }

        private void DisableTooltip()
        {
            Tooltip.Enabled = false;
            Tooltip.StopHover();
            Tooltip.SetTopLeftPosition(PointI.Zero);
        }

        internal void CheckForSelectionOfStack(Point mouseLocation)
        {
            StackViews.CheckForSelectionOfStack(mouseLocation);
        }

        internal void CheckIfMouseIsHoveringOverStack(Point mouseLocation)
        {
            var stackViewHoveredOver = StackViews.GetStackViewFromLocation(mouseLocation);
            if (stackViewHoveredOver == null)
            {
                DisableTooltip();
            }
            else
            {
                HoveringOverTooltip(stackViewHoveredOver);
            }
        }

        internal void ChangeState(GameStatus from, GameStatus to)
        {
            GameStatusHandler.ChangeStatus(from, to);
        }

        internal void SubscribeToStatusChanges(string owner, Action<GameStatus, GameStatus, string> action)
        {
            GameStatusHandler.SubscribeToStatusChanges(owner, action);
        }

        internal void UnsubscribeFromStatusChanges(string owner)
        {
            GameStatusHandler.UnsubscribeFromStatusChanges(owner);
        }

        internal void HandleStatusChange(GameStatus from, GameStatus to, string owner)
        {
            Input.Unsubscribe(from.ToString(), owner);
            Input.Subscribe(to.ToString(), owner);
        }

        #region Event Handlers

        private void BtnClick(object sender, ButtonClickEventArgs e)
        {
            StackViews.DoAction(e.Action);
        }

        #endregion

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)

                // set large fields to null

                IsDisposed = true;
            }
        }
    }
}