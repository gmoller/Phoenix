using System;
using System.Runtime.Remoting.Messaging;
using AssetsLibrary;
using GuiControls;
using Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGamePresentationLibrary.ExtensionMethods;
using PhoenixGamePresentationLibrary.Handlers;
using Utilities;
using Point = Utilities.Point;

namespace PhoenixGamePresentationLibrary.Views
{
    internal class HudView
    {
        #region State
        private readonly WorldView _worldView;

        private SpriteFont _font;
        private readonly Rectangle _area;

        private readonly Frame _hudViewFrame;
        private EnumerableDictionary<IControl> _actionButtons;

        private readonly StackViews _stackViews;
        #endregion

        private StackView SelectedStackView => _stackViews.Current;

        internal HudView(WorldView worldView, StackViews stackViews)
        {
            var width = (int)(1920 * 0.1305f); // 13.05% of screen width
            var height = (int)(1080 * 0.945f); // 94.5% of screen height
            var x = 1920 - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 250x1020

            #region HudViewFrame

            var topLeftPosition = new Vector2(_area.X, _area.Y);
            _hudViewFrame = new Frame(topLeftPosition, Alignment.TopLeft, new Vector2(_area.Width, _area.Height), "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47, "hudViewFrame");

            string GetTextFuncForDate() => _worldView.World.CurrentDate;
            _hudViewFrame.AddControl(new LabelSized("lblCurrentDate", new Vector2(150.0f, 15.0f), Alignment.MiddleCenter, GetTextFuncForDate, "Maleficio-Regular-18", Color.Aquamarine), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 20));

            #region MiniMapFrame

            _hudViewFrame.AddControl(new Frame("miniMapFrame", new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 50));
            _hudViewFrame["miniMapFrame"].AddControl(new Image("mapImage", new Vector2(200.0f, 170.0f)), Alignment.MiddleCenter, Alignment.MiddleCenter);

            #endregion

            #region ResourceFrame

            _hudViewFrame.AddControl(new Frame("resourceFrame", new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 250));
            _hudViewFrame["resourceFrame"].AddControl(new Image("imgGold", new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R"), Alignment.TopLeft, Alignment.TopLeft, new Point(10, 10));
            _hudViewFrame["resourceFrame"].AddControl(new Image("imgMana", new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R"), Alignment.TopLeft, Alignment.TopLeft, new Point(10, 70));
            _hudViewFrame["resourceFrame"].AddControl(new Image("imgFood", new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R"), Alignment.TopLeft, Alignment.TopLeft, new Point(10, 130));

            string GetTextFuncForGold() => $"{_worldView.World.PlayerFaction.GoldInTreasury} GP (+{_worldView.World.PlayerFaction.GoldPerTurn})";
            _hudViewFrame["resourceFrame.imgGold"].AddControl(new LabelSized("lblGold", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForGold, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new Point(20, 0));
            string GetTextFuncForMana() => "5 MP (+1)";
            _hudViewFrame["resourceFrame.imgMana"].AddControl(new LabelSized("lblMana", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForMana, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new Point(20, 0));
            string GetTextFuncForFood() => $"{_worldView.World.PlayerFaction.FoodPerTurn} Food";
            _hudViewFrame["resourceFrame.imgFood"].AddControl(new LabelSized("lblFood", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForFood, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new Point(20, 0));

            #endregion

            #region UnitFrame

            _hudViewFrame.AddControl(new Frame("unitFrame", new Vector2(_area.Width - 20.0f, _area.Height * 0.30f /* 30% of parent */), "GUI_Textures_1", "frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new Point(0, 500));

            string GetTextFuncForMoves() => SelectedStackView == null ? string.Empty : $"Moves: {SelectedStackView.MovementPoints}";
            _hudViewFrame["unitFrame"].AddControl(new LabelSized("lblMoves", new Vector2(130.0f, 15.0f), Alignment.MiddleLeft, GetTextFuncForMoves, "CrimsonText-Regular-12", Color.White), Alignment.BottomLeft, Alignment.BottomLeft, new Point(10, -13));

            #endregion

            var btnEndTurn = new Button("btnEndTurn", new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_h", "reg_button_a");
            btnEndTurn.Click += EndTurnButtonClick;

            btnEndTurn.AddControl(new LabelSized("lblEndTurn", btnEndTurn.Size.ToVector2(), Alignment.MiddleCenter, "Next Turn", "CrimsonText-Regular-12", Color.White, Color.Blue), Alignment.MiddleCenter, Alignment.MiddleCenter);

            _hudViewFrame.AddControl(btnEndTurn, Alignment.BottomCenter, Alignment.TopCenter, Point.Zero);

            #endregion

            _worldView = worldView;
            _stackViews = stackViews;

            //var json = _hudViewFrame.Serialize();
            //_hudViewFrame.Deserialize(json);
            //var newFrame = new Frame(json);
        }

        internal void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            _hudViewFrame.LoadContent(content);
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

            _actionButtons = _worldView.ActionButtons;
        }

        public void Update(InputHandler input, float deltaTime)
        {
            if (_worldView.GameStatus == GameStatus.CityView) return;

            // Causes
            var mouseOverHudView = _area.Contains(input.MousePosition) || _hudViewFrame.ChildControls["btnEndTurn"].Area.Contains(input.MousePosition);
            var redrawMiniMap = true;

            // Actions
            _hudViewFrame.Enabled = mouseOverHudView;

            if (redrawMiniMap)
            {
                // call minimap creator
                var createdImage = MinimapHandler.Create(_worldView.World);
                var mapImage = (Image)_hudViewFrame["miniMapFrame.mapImage"];
                mapImage.SetTexture(createdImage);
            }

            _hudViewFrame.Update(input, deltaTime);
            _actionButtons.Update(input, deltaTime);

            // Status change?
            if (_worldView.GameStatus != GameStatus.CityView)
            {
                _worldView.GameStatus = mouseOverHudView ? GameStatus.InHudView : GameStatus.OverlandMap;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _hudViewFrame.Draw(spriteBatch);

            DrawUnits(spriteBatch);
            DrawNotifications(spriteBatch);
            DrawTileInfo(spriteBatch);
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

            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * Constants.ONE_HALF + 10.0f;
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
            var x = 1910 - 18 - 12; // position of unitFrame BottomRight: (1910;806) : size: (18;12)
            var y = 806 - 12 - 20;
            foreach (var imgMovementType in imgMovementTypes)
            {
                imgMovementType.SetTopLeftPosition(new Point(x - 19 * i, y));
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
            var x = _area.X + 10.0f;
            var y = _area.Y + 400.0f;
            foreach (var item in _worldView.World.NotificationList)
            {
                var lines = TextWrapper.WrapText(item, 150, _font);
                foreach (var line in lines)
                {
                    spriteBatch.DrawString(_font, line, new Vector2(x, y), Color.Pink);
                    y += 20.0f;
                }
            }
        }

        private void DrawTileInfo(SpriteBatch spriteBatch)
        {
            var x = _area.X + 10.0f;
            var y = _area.Y + _area.Height * 0.96f;

            // get tile mouse is over
            var cellGrid = _worldView.World.OverlandMap.CellGrid;
            var context = (GlobalContext)CallContext.LogicalGetData("AmbientGlobalContext");
            var hexPoint = context.WorldHexPointedAtByMouseCursor;
            if (hexPoint.X >= 0 && hexPoint.Y >= 0 && hexPoint.X < PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS && hexPoint.Y < PhoenixGameLibrary.Constants.WORLD_MAP_ROWS)
            {
                var cell = cellGrid.GetCell(hexPoint.X, hexPoint.Y);
                if (cell.SeenState != SeenState.NeverSeen)
                {
                    var terrainTypes = context.GameMetadata.TerrainTypes;
                    var terrainType = terrainTypes[cell.TerrainTypeId];
                    var text1 = $"{terrainType.Name} - {terrainType.FoodOutput} food";
                    spriteBatch.DrawString(_font, text1, new Vector2(x, y), Color.White);

                    if (terrainType.CanSettleOn)
                    {
                        var catchment = cellGrid.GetCatchment(hexPoint.X, hexPoint.Y, 2);
                        var maxPop = PhoenixGameLibrary.Helpers.BaseFoodLevel.DetermineBaseFoodLevel(new Point(hexPoint.X, hexPoint.Y), catchment);
                        var text2 = $"Maximum Pop - {maxPop}";
                        spriteBatch.DrawString(_font, text2, new Vector2(x, y + 15.0f), Color.White);
                    }
                }
            }
        }

        private void EndTurnButtonClick(object sender, EventArgs e)
        {
            _worldView.EndTurn();
        }
    }
}