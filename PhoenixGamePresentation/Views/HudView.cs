using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using PhoenixGamePresentation.ExtensionMethods;
using PhoenixGamePresentation.Handlers;
using Zen.Assets;
using Zen.GuiControls;
using Zen.GuiControls.TheControls;
using Zen.Hexagons;
using Zen.Input;
using Zen.MonoGameUtilities;
using Zen.Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class HudView : ViewBase, IDisposable
    {
        #region State
        private Rectangle Area { get; }
        private Controls Controls { get; }
        private StackViews StackViews { get; }
        #endregion

        private StackView.StackView SelectedStackView => StackViews.Current;

        internal HudView(WorldView worldView, StackViews stackViews, InputHandler input)
        {
            var x = worldView.Camera.GetViewport.Right;
            var y = 0;
            var width = 1920 - x;
            var height = 1020;
            Area = new Rectangle(x, y, width, height); // 1680,0,240,1020

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("position1", "0;0"),
                new KeyValuePair<string, string>("size1", $"{width};{height}"),
                new KeyValuePair<string, string>("size2", $"{width - 20};{Convert.ToInt32(height * 0.15f)}"), // 15% of parent
                new KeyValuePair<string, string>("size3", $"{width - 20};{Convert.ToInt32(height * 0.30f)}")  // 30% of parent
            };

            var spec = ResourceReader.ReadResource("PhoenixGamePresentation.Views.HudViewControls.txt", Assembly.GetExecutingAssembly());
            Controls = ControlCreator.CreateFromSpecification(spec, pairs);
            Controls.SetOwner(this);

            #region ResourceFrame

            //HudViewFrame.AddControl(new Frame("resourceFrame", new Vector2(Area.Width - 20.0f, Area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1.frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 250));
            //HudViewFrame["resourceFrame"].AddControl(new Image("imgGold", new Vector2(50.0f, 50.0f), "Icons_1.Coin_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 10));
            //HudViewFrame["resourceFrame"].AddControl(new Image("imgMana", new Vector2(50.0f, 50.0f), "Icons_1.Potion_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 70));
            //HudViewFrame["resourceFrame"].AddControl(new Image("imgFood", new Vector2(50.0f, 50.0f), "Icons_1.Bread_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 130));

            //string GetTextFuncForGold() => $"{WorldView.PlayerFaction.GoldInTreasury} GP (+{WorldView.PlayerFaction.GoldPerTurn})";
            //HudViewFrame["resourceFrame.imgGold"].AddControl(new Label("lblGold", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForGold, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));
            //string GetTextFuncForMana() => "5 MP (+1)";
            //HudViewFrame["resourceFrame.imgMana"].AddControl(new Label("lblMana", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForMana, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));
            //string GetTextFuncForFood() => $"{WorldView.PlayerFaction.FoodPerTurn} Food";
            //HudViewFrame["resourceFrame.imgFood"].AddControl(new Label("lblFood", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForFood, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));

            #endregion

            WorldView = worldView;
            StackViews = stackViews;

            worldView.CellGrid.NewCellSeen += NewCellSeen;

            SetupViewport(x, y, width, height + Controls["frmHudView.btnEndTurn"].Height); // 1680,0,240,1020 + 56 : btnEndTurn.Height = 56

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "HudView");
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "HudView");

            WorldView.SubscribeToStatusChanges("HudView", worldView.HandleStatusChange);
        }

        public static string GetTextFuncForDate(object sender)
        {
            var lbl = (Label)sender;
            var owner = (HudView)lbl.Owner;
            var currentDate = owner.WorldView.CurrentDate;

            return currentDate;
        }

        public static string GetTextFuncForMoves(object sender)
        {
            var lbl = (Label)sender;
            var owner = (HudView)lbl.Owner;
            var moves = owner.SelectedStackView == null ? string.Empty : $"Moves: {owner.SelectedStackView.MovementPoints}";

            return moves;
        }

        internal void LoadContent(ContentManager content)
        {
            Controls.LoadContent(content, true);

            var createdImage = MinimapHandler.Create(WorldView.CellGrid);
            AssetsManager.Instance.AddTexture("MapImage", createdImage);
            var mapImage = (Image)Controls["frmHudView.frmMinimap.imgMinimap"];
            mapImage.TextureNormal = "MapImage";
        }

        public void Update(GameTime gameTime)
        {
            if (WorldView.GameStatus == GameStatus.CityView) return;

            Controls["frmHudView.btnEndTurn"].Enabled = WorldView.AllStacksHaveBeenGivenOrders;

            Controls.Update(Input, gameTime, Viewport);
            WorldView.GetActionButtons.Update(Input, gameTime, Viewport);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            var originalViewport = spriteBatch.GraphicsDevice.Viewport;
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: ViewportAdapter.GetScaleMatrix());

            Controls.Draw(spriteBatch);

            var imgMinimap = Controls["frmHudView.frmMinimap.imgMinimap"];
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(WorldView, imgMinimap.Size);

            minimapViewedRectangle.X += imgMinimap.Left;
            minimapViewedRectangle.Y += imgMinimap.Top;

            spriteBatch.DrawRectangle(minimapViewedRectangle, Color.White);
            spriteBatch.DrawPoint(minimapViewedRectangle.Center.ToVector2(), Color.White);

            var font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            var frmUnits = Controls["frmHudView.frmUnits"];
            var frmUnitsBottomRight = frmUnits.BottomRight;
            var hexPoint = WorldView.Camera.ScreenPixelToWorldHex(Input.MousePosition); // get tile mouse is over

            DrawUnits(spriteBatch, StackViews, SelectedStackView, Area.Height, frmUnitsBottomRight, WorldView.GetActionButtons);
            DrawNotifications(spriteBatch, font, WorldView.NotificationList);
            DrawTileInfo(spriteBatch, font, 10.0f, Area.Height * 0.96f, WorldView.CellGrid, hexPoint);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = originalViewport;
        }

        private static void DrawUnits(SpriteBatch spriteBatch, StackViews stackViews, StackView.StackView selectedStackView, int height, PointI frmUnitsBottomRight, EnumerableDictionary<IControl> actionButtons)
        {
            if (selectedStackView == null) return;

            DrawUnitBadges(spriteBatch, stackViews, selectedStackView, 20.0f, height * Constants.ONE_HALF + 10.0f);
            DrawMovementTypeImages(spriteBatch, frmUnitsBottomRight, selectedStackView.GetMovementTypeImages());
            DrawActionButtons(spriteBatch, selectedStackView.Actions, actionButtons);
        }

        private static void DrawUnitBadges(SpriteBatch spriteBatch, StackViews stackViews, StackView.StackView selectedStackView, float x, float y)
        {
            var stackViews2 = GetStackViewsSharingSameLocation(stackViews, selectedStackView);

            var i = 0;
            foreach (var stackView in stackViews2)
            {
                DrawUnitBadges(spriteBatch, stackViews, new Vector2(x, y), i, stackView);
                i += stackView.Count;
            }
        }

        private static List<StackView.StackView> GetStackViewsSharingSameLocation(StackViews stackViews, StackView.StackView selectedStackView)
        {
            var stackViews2 = new List<StackView.StackView>();
            foreach (var stackView in stackViews)
            {
                if (stackView.LocationHex == selectedStackView.LocationHex) // same location
                {
                    stackViews2.Add(stackView);
                }
            }

            return stackViews2;
        }

        private static void DrawUnitBadges(SpriteBatch spriteBatch, StackViews stackViews, Vector2 topLeftPosition, int index, StackView.StackView stackView)
        {
            var x = topLeftPosition.X + 30;
            var y = topLeftPosition.Y + 30;
            foreach (var unit in stackView.Stack)
            {
                var indexMod3 = index % 3;
                var indexDividedBy3 = index / 3; // Floor
                var xOffset = (stackView.ScreenFrame.Width + 10.0f) * indexMod3;
                var yOffset = (stackView.ScreenFrame.Height + 10.0f) * indexDividedBy3;
                DrawUnitBadge(spriteBatch, stackViews, new Vector2(x + xOffset, y + yOffset), unit, stackView);
                index++;
            }
        }

        private static void DrawUnitBadge(SpriteBatch spriteBatch, StackViews stackViews, Vector2 centerPosition, Unit unit, StackView.StackView stackView)
        {
            // draw background
            var sourceRectangle = stackView.IsSelected ? stackViews.SquareGreenFrame.ToRectangle() : stackViews.SquareGrayFrame.ToRectangle();
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, stackView.ScreenFrame.Width, stackView.ScreenFrame.Height);
            spriteBatch.Draw(stackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.FlipVertically, 0.0f);

            // draw unit icon
            var frame = stackViews.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, sourceRectangle.Width, sourceRectangle.Height);
            spriteBatch.Draw(stackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.None, 0.0f);
        }

        private static void DrawMovementTypeImages(SpriteBatch spriteBatch, PointI frmUnitsBottomRight, List<IControl> imgMovementTypes)
        {
            var i = 0;
            //size: (18;12)
            var x = frmUnitsBottomRight.X - 18 - 12;
            var y = frmUnitsBottomRight.Y - 12 - 20;
            foreach (var imgMovementType in imgMovementTypes)
            {
                imgMovementType.Position = new PointI(x - 19 * i, y);
                imgMovementType.Draw(spriteBatch);
                i++;
            }
        }

        private static void DrawActionButtons(SpriteBatch spriteBatch, EnumerableList<string> actions, EnumerableDictionary<IControl> actionButtons)
        {
            var selectedStackViewActions = actions;
            foreach (var actionButton in actionButtons)
            {
                actionButton.Enabled = selectedStackViewActions.Contains(actionButton.Name);
                actionButton.Draw(spriteBatch);
            }
        }

        private static void DrawNotifications(SpriteBatch spriteBatch, SpriteFont font, NotificationList notificationList)
        {
            var x = 10.0f;
            var y = 460.0f;
            foreach (var item in notificationList)
            {
                var lines = TextWrapper.WrapText(item, 150.0f, font);
                foreach (var line in lines)
                {
                    spriteBatch.DrawString(font, line, new Vector2(x, y), Color.Pink);
                    y += 20.0f;
                }
            }
        }

        private static void DrawTileInfo(SpriteBatch spriteBatch, SpriteFont font, float x, float y, CellGrid cellGrid, HexOffsetCoordinates hexPoint)
        {
            var cell = cellGrid.GetCell(hexPoint);
            if (cell.SeenState == SeenState.NeverSeen) return;

            var terrainType = GetTerrainType(cell);
            var terrainTypeText = $"{terrainType.Name} - {terrainType.FoodOutput} food";
            var maximumPopulationText = GetMaximumPopulationText(terrainType, cellGrid, hexPoint);

            spriteBatch.DrawString(font, terrainTypeText, new Vector2(x, y), Color.White);
            spriteBatch.DrawString(font, maximumPopulationText, new Vector2(x, y + 15.0f), Color.White);
        }

        private static TerrainType GetTerrainType(Cell cell)
        {
            var gameMetadata = CallContext<GameMetadata>.GetData("GameMetadata");
            var terrainTypes = gameMetadata.TerrainTypes;
            var terrainType = terrainTypes[cell.TerrainTypeId];

            return terrainType;
        }

        private static string GetMaximumPopulationText(TerrainType terrainType, CellGrid cellGrid, HexOffsetCoordinates hexPoint)
        {
            if (!terrainType.CanSettleOn) return string.Empty;

            var catchment = cellGrid.GetCatchment(hexPoint.Col, hexPoint.Row, 2);
            var maxPop = PhoenixGameLibrary.Helpers.BaseFoodLevel.DetermineBaseFoodLevel(new PointI(hexPoint.Col, hexPoint.Row), catchment);
            var text = $"Maximum Pop - {maxPop}";

            return text;
        }

        private void NewCellSeen(object sender, EventArgs e)
        {
            var cellGrid = (CellGrid)sender;

            var createdImage = MinimapHandler.Create(cellGrid);
            AssetsManager.Instance.AddTexture("MapImage", createdImage);
            var mapImage = (Image)Controls["frmHudView.frmMinimap.imgMinimap"];
            mapImage.TextureNormal = "MapImage";
        }

        #region Event Handlers

        public static void MinimapClick(object sender, EventArgs e)
        {
            // Where on the minimap is clicked?
            // Convert that to world pixel
            if (!(e is MouseEventArgs mouseEventArgs)) return;

            var imgMinimap = (Image)sender;
            var hudView = (HudView)imgMinimap.Owner;
            var minimapPosition = mouseEventArgs.Mouse.Location - new Point(hudView.Viewport.X + imgMinimap.Left, hudView.Viewport.Y + imgMinimap.Top);
            var normalizedX = minimapPosition.X / (float)imgMinimap.Size.X;
            var normalizedY = minimapPosition.Y / (float)imgMinimap.Size.Y;

            var worldView = hudView.WorldView;
            var x = (int) (worldView.WorldWidthInPixels * normalizedX);
            var y = (int) (worldView.WorldHeightInPixels * normalizedY);

            worldView.Camera.LookAtPixel(new PointI(x, y));
        }

        public static void EndTurn(object sender, EventArgs e)
        {
            var btnEndTurn = (Button)sender;
            var hudView = (HudView)btnEndTurn.Owner;
            var worldView = hudView.WorldView;
            worldView.EndTurn();
        }

        #endregion

        public void Dispose()
        {
            if (IsDisposed) return;

            // dispose managed state (managed objects)
            Input.UnsubscribeAllFromEventHandler("HudView");

            // set large fields to null
            ViewportAdapter = null;

            IsDisposed = true;
        }
    }
}