using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using GuiControls;
using Input;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using MonoGameUtilities.ViewportAdapters;
using PhoenixGameLibrary;
using PhoenixGamePresentation.ExtensionMethods;
using PhoenixGamePresentation.Handlers;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class HudView : IDisposable
    {
        #region State
        private readonly WorldView _worldView;

        private SpriteFont _font;
        private readonly Rectangle _area;

        private readonly Frame _hudViewFrame;
        private EnumerableDictionary<IControl> _actionButtons;

        //TODO: these should be label controls
        private string _text1;
        private string _text2;

        private readonly StackViews _stackViews;

        private Viewport _viewport;
        private ViewportAdapter _viewportAdapter;

        private readonly InputHandler _input;
        private bool _disposedValue;
        #endregion End State

        private StackView SelectedStackView => _stackViews.Current;

        internal HudView(WorldView worldView, StackViews stackViews, InputHandler input)
        {
            var width = (int)(1920 * 0.1305f); // 13.05% of screen width
            var height = (int)(1080 * 0.945f); // 94.5% of screen height
            var x = 1920 - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 1670,0,250,1020

            #region HudViewFrame

            _hudViewFrame = new Frame(Vector2.Zero, Alignment.TopLeft, new Vector2(_area.Width, _area.Height), "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47, "hudViewFrame");

            //_hudViewFrame.AddControl(new Image("imgBackground", new Vector2(250, 1080), "NoiseTexture"));

            string GetTextFuncForDate() => _worldView.World.CurrentDate;
            _hudViewFrame.AddControl(new LabelSized("lblCurrentDate", new Vector2(150.0f, 15.0f), Alignment.MiddleCenter, GetTextFuncForDate, "Maleficio-Regular-18", Color.Aquamarine), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 20));

            #region MiniMapFrame

            _hudViewFrame.AddControl(new Frame("miniMapFrame", new Vector2(_area.Width - 20.0f, _area.Height * 0.15f /* 15% of parent */), "GUI_Textures_1", "frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 50));
            var image = new Image("mapImage", new Vector2(200.0f, 116.0f), null);
            image.Click += MiniMapClick;
            _hudViewFrame["miniMapFrame"].AddControl(image, Alignment.MiddleCenter, Alignment.MiddleCenter);

            #endregion

            #region ResourceFrame

            _hudViewFrame.AddControl(new Frame("resourceFrame", new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 250));
            _hudViewFrame["resourceFrame"].AddControl(new Image("imgGold", new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 10));
            _hudViewFrame["resourceFrame"].AddControl(new Image("imgMana", new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 70));
            _hudViewFrame["resourceFrame"].AddControl(new Image("imgFood", new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 130));

            string GetTextFuncForGold() => $"{_worldView.World.PlayerFaction.GoldInTreasury} GP (+{_worldView.World.PlayerFaction.GoldPerTurn})";
            _hudViewFrame["resourceFrame.imgGold"].AddControl(new LabelSized("lblGold", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForGold, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));
            string GetTextFuncForMana() => "5 MP (+1)";
            _hudViewFrame["resourceFrame.imgMana"].AddControl(new LabelSized("lblMana", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForMana, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));
            string GetTextFuncForFood() => $"{_worldView.World.PlayerFaction.FoodPerTurn} Food";
            _hudViewFrame["resourceFrame.imgFood"].AddControl(new LabelSized("lblFood", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForFood, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));

            #endregion

            #region UnitFrame

            _hudViewFrame.AddControl(new Frame("unitFrame", new Vector2(_area.Width - 20.0f, _area.Height * 0.30f /* 30% of parent */), "GUI_Textures_1", "frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 500));

            string GetTextFuncForMoves() => SelectedStackView == null ? string.Empty : $"Moves: {SelectedStackView.MovementPoints}";
            _hudViewFrame["unitFrame"].AddControl(new LabelSized("lblMoves", new Vector2(130.0f, 15.0f), Alignment.MiddleLeft, GetTextFuncForMoves, "CrimsonText-Regular-12", Color.White), Alignment.BottomLeft, Alignment.BottomLeft, new PointI(10, -13));

            #endregion

            var btnEndTurn = new Button("btnEndTurn", new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_h", "reg_button_a");
            btnEndTurn.Click += EndTurn;

            btnEndTurn.AddControl(new LabelSized("lblEndTurn", btnEndTurn.Size.ToVector2(), Alignment.MiddleCenter, "Next Turn", "CrimsonText-Regular-12", Color.White, Color.Blue), Alignment.MiddleCenter, Alignment.MiddleCenter);

            _hudViewFrame.AddControl(btnEndTurn, Alignment.BottomCenter, Alignment.TopCenter, PointI.Zero);

            #endregion

            _worldView = worldView;
            _stackViews = stackViews;

            worldView.World.OverlandMap.CellGrid.NewCellSeen += NewCellSeen;

            //var json = _hudViewFrame.Serialize();
            //_hudViewFrame.Deserialize(json);
            //var newFrame = new Frame(json);

            SetupViewport(_area.X, _area.Y, _area.Width, _area.Height + btnEndTurn.Height);

            input.AddCommandHandler("HudView", 0, new MouseInputAction(MouseInputActionType.Moved, CheckIfMouseIsOverHudView));
            _input = input;
        }

        private void SetupViewport(int x, int y, int width, int height)
        {
            var context = CallContext<GlobalContextPresentation>.GetData("GlobalContextPresentation");
            _viewport = new Viewport(x, y, width, height, 0.0f, 1.0f);
            _viewportAdapter = new ScalingViewportAdapter(context.GraphicsDevice, width, height);
        }

        internal void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            _hudViewFrame.LoadContent(content);
            //_hudViewFrame["imgBackground"].LoadContent(content);
            _hudViewFrame["lblCurrentDate"].LoadContent(content);

            _hudViewFrame["resourceFrame"].LoadContent(content);
            _hudViewFrame["resourceFrame.imgGold"].LoadContent(content);
            _hudViewFrame["resourceFrame.imgMana"].LoadContent(content);
            _hudViewFrame["resourceFrame.imgFood"].LoadContent(content);
            _hudViewFrame["resourceFrame.imgGold.lblGold"].LoadContent(content);
            _hudViewFrame["resourceFrame.imgMana.lblMana"].LoadContent(content);
            _hudViewFrame["resourceFrame.imgFood.lblFood"].LoadContent(content);

            _hudViewFrame["miniMapFrame"].LoadContent(content);

            _hudViewFrame["unitFrame"].LoadContent(content);
            _hudViewFrame["unitFrame.lblMoves"].LoadContent(content);

            _hudViewFrame["btnEndTurn"].LoadContent(content);
            _hudViewFrame["btnEndTurn.lblEndTurn"].LoadContent(content);

            var createdImage = MinimapHandler.Create(_worldView.World);
            var mapImage = (Image)_hudViewFrame["miniMapFrame.mapImage"];
            mapImage.SetTexture(createdImage);

            _actionButtons = _worldView.ActionButtons;
        }

        public void Update(float deltaTime)
        {
            _text1 = string.Empty;
            _text2 = string.Empty;
            if (_worldView.GameStatus == GameStatus.CityView) return;

            _hudViewFrame.Update(_input, deltaTime, _viewport);
            _actionButtons.Update(_input, deltaTime, _viewport);

            // get tile mouse is over
            var cellGrid = _worldView.World.OverlandMap.CellGrid;
            var hexPoint = _worldView.Camera.ScreenPixelToWorldHex(_input.MousePosition);

            // TODO: check if cell is on screen
            var cell = cellGrid.GetCell(hexPoint);
            if (cell.SeenState == SeenState.NeverSeen) return;

            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;
            var terrainType = terrainTypes[cell.TerrainTypeId];
            _text1 = $"{terrainType.Name} - {terrainType.FoodOutput} food";

            if (!terrainType.CanSettleOn) return;

            var catchment = cellGrid.GetCatchment(hexPoint.Col, hexPoint.Row, 2);
            var maxPop = PhoenixGameLibrary.Helpers.BaseFoodLevel.DetermineBaseFoodLevel(new PointI(hexPoint.Col, hexPoint.Row), catchment);
            _text2 = $"Maximum Pop - {maxPop}";
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = _viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: _viewportAdapter.GetScaleMatrix());

            _hudViewFrame.Draw(spriteBatch);

            var minimapImage = _hudViewFrame["miniMapFrame.mapImage"];
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(_worldView, minimapImage.Size);

            minimapViewedRectangle.X += minimapImage.Left;
            minimapViewedRectangle.Y += minimapImage.Top;

            spriteBatch.DrawRectangle(minimapViewedRectangle, Color.White);
            spriteBatch.DrawPoint(minimapViewedRectangle.Center.ToVector2(), Color.White);

            DrawUnits(spriteBatch);
            DrawNotifications(spriteBatch);
            DrawTileInfo(spriteBatch);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        private void DrawUnits(SpriteBatch spriteBatch)
        {
            if (SelectedStackView == null) return;

            DrawUnitBadges(spriteBatch);
            DrawMovementTypeImages(spriteBatch);
            DrawActionButtons(spriteBatch);
        }

        private void DrawUnitBadges(SpriteBatch spriteBatch)
        {
            var stackViews = SelectedStackView.GetStackViewsSharingSameLocation();

            var x = 20.0f;
            var y = _area.Height * Constants.ONE_HALF + 10.0f;
            int i = 0;
            foreach (var stackView in stackViews)
            {
                stackView.DrawBadges(spriteBatch, new Vector2(x, y), i, stackView.IsSelected);
                i += stackView.Count;
            }
        }

        private void DrawMovementTypeImages(SpriteBatch spriteBatch)
        {
            var imgMovementTypes = SelectedStackView.GetMovementTypeImages();
            var i = 0;
            //size: (18;12)
            var x = _hudViewFrame["unitFrame"].BottomRight.X - 18 - 12;
            var y = _hudViewFrame["unitFrame"].BottomRight.Y - 12 - 20;
            foreach (var imgMovementType in imgMovementTypes)
            {
                imgMovementType.SetTopLeftPosition(new PointI(x - 19 * i, y));
                imgMovementType.Draw(spriteBatch);
                i++;
            }
        }

        private void DrawActionButtons(SpriteBatch spriteBatch)
        {
            var selectedStackViewActions = SelectedStackView.Actions;
            foreach (var actionButton in _actionButtons)
            {
                actionButton.Enabled = selectedStackViewActions.Contains(actionButton.Name);
                actionButton.Draw(spriteBatch);
            }
        }

        private void DrawNotifications(SpriteBatch spriteBatch)
        {
            var x = 10.0f;
            var y = 460.0f;
            foreach (var item in _worldView.World.NotificationList)
            {
                var lines = TextWrapper.WrapText(item, 150.0f, _font);
                foreach (var line in lines)
                {
                    spriteBatch.DrawString(_font, line, new Vector2(x, y), Color.Pink);
                    y += 20.0f;
                }
            }
        }

        private void DrawTileInfo(SpriteBatch spriteBatch)
        {
            const float x = 10.0f;
            var y = _area.Height * 0.96f;

            spriteBatch.DrawString(_font, _text1, new Vector2(x, y), Color.White);
            spriteBatch.DrawString(_font, _text2, new Vector2(x, y + 15.0f), Color.White);
        }

        private void NewCellSeen(object sender, EventArgs e)
        {
            var createdImage = MinimapHandler.Create(_worldView.World);
            var mapImage = (Image)_hudViewFrame["miniMapFrame.mapImage"];
            mapImage.SetTexture(createdImage);
        }

        #region Event Handlers

        private void CheckIfMouseIsOverHudView(object sender, MouseEventArgs e)
        {
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
            var mouseOverHudView = _area.Contains(e.Mouse.Location) || _hudViewFrame.ChildControls["btnEndTurn"].MouseOver;
            _hudViewFrame.Enabled = mouseOverHudView;

            if (_worldView.GameStatus != GameStatus.CityView)
            {
                _worldView.GameStatus = mouseOverHudView ? GameStatus.InHudView : GameStatus.OverlandMap;
            }
        }

        private void MiniMapClick(object sender, EventArgs e)
        {
            // Where on the minimap is clicked?
            // Convert that to world pixel
            if (!(e is MouseEventArgs mouseEventArgs)) return;

            var minimapImage = _hudViewFrame["miniMapFrame.mapImage"];
            var minimapPosition = mouseEventArgs.Mouse.Location - new Point(_viewport.X + minimapImage.Left, _viewport.Y + minimapImage.Top);
            var normalizedX = minimapPosition.X / (float) minimapImage.Size.X;
            var normalizedY = minimapPosition.Y / (float) minimapImage.Size.Y;

            var x = (int) (_worldView.WorldWidthInPixels * normalizedX);
            var y = (int) (_worldView.WorldHeightInPixels * normalizedY);

            _worldView.Camera.LookAtPixel(new PointI(x, y));
        }

        private void EndTurn(object sender, EventArgs e)
        {
            _worldView.EndTurn();
        }

        #endregion

        public void Dispose()
        {
            if (!_disposedValue)
            {
                // dispose managed state (managed objects)
                _input.RemoveCommandHandler("HudView", 0, new MouseInputAction(MouseInputActionType.Moved, CheckIfMouseIsOverHudView));

                // set large fields to null
                _viewportAdapter = null;

                _disposedValue = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}