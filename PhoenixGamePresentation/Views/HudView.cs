﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Assets;
using GuiControls;
using GuiControls.PackagesClasses;
using Input;
using MonoGameUtilities;
using MonoGameUtilities.ExtensionMethods;
using PhoenixGameLibrary;
using PhoenixGamePresentation.ExtensionMethods;
using PhoenixGamePresentation.Handlers;
using Utilities;

namespace PhoenixGamePresentation.Views
{
    internal class HudView : ViewBase, IDisposable
    {
        #region State
        private SpriteFont Font { get; set; }
        private Rectangle Area { get; }

        private Frame HudViewFrame { get; }
        private EnumerableDictionary<IControl> ActionButtons { get; set; }

        //TODO: these should be label controls
        private string _text1;
        private string _text2;

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

            #region HudViewFrame

            HudViewFrame = new Frame(Vector2.Zero, Alignment.TopLeft, new Vector2(Area.Width, Area.Height), "GUI_Textures_1.frame3_whole", 47, 47, 47, 47, "hudViewFrame");

            //HudViewFrame.AddControl(new Image("imgBackground", new Vector2(250, 1080), "NoiseTexture"));

            string GetTextFuncForDate() => WorldView.CurrentDate;
            HudViewFrame.AddControl(new LabelSized("lblCurrentDate", new Vector2(150.0f, 15.0f), Alignment.MiddleCenter, GetTextFuncForDate, "DarkXShadowX21s Skyrim Font", Color.Aquamarine), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 20));
             
            #region MiniMapFrame

            HudViewFrame.AddControl(new Frame("miniMapFrame", new Vector2(Area.Width - 20.0f, Area.Height * 0.15f /* 15% of parent */), "GUI_Textures_1.frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 50));
            var image = new Image("mapImage", new Vector2(200.0f, 116.0f));
            image.AddPackage(new ControlClick((o, args) => MiniMapClick(o, new MouseEventArgs(input.Mouse, WorldView, 0.0f))));
            //TODO: minimap drag
            HudViewFrame["miniMapFrame"].AddControl(image, Alignment.MiddleCenter, Alignment.MiddleCenter);

            #endregion

            #region ResourceFrame

            HudViewFrame.AddControl(new Frame("resourceFrame", new Vector2(Area.Width - 20.0f, Area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1.frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 250));
            HudViewFrame["resourceFrame"].AddControl(new Image("imgGold", new Vector2(50.0f, 50.0f), "Icons_1.Coin_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 10));
            HudViewFrame["resourceFrame"].AddControl(new Image("imgMana", new Vector2(50.0f, 50.0f), "Icons_1.Potion_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 70));
            HudViewFrame["resourceFrame"].AddControl(new Image("imgFood", new Vector2(50.0f, 50.0f), "Icons_1.Bread_R"), Alignment.TopLeft, Alignment.TopLeft, new PointI(10, 130));

            string GetTextFuncForGold() => $"{WorldView.PlayerFaction.GoldInTreasury} GP (+{WorldView.PlayerFaction.GoldPerTurn})";
            HudViewFrame["resourceFrame.imgGold"].AddControl(new LabelSized("lblGold", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForGold, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));
            string GetTextFuncForMana() => "5 MP (+1)";
            HudViewFrame["resourceFrame.imgMana"].AddControl(new LabelSized("lblMana", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForMana, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));
            string GetTextFuncForFood() => $"{WorldView.PlayerFaction.FoodPerTurn} Food";
            HudViewFrame["resourceFrame.imgFood"].AddControl(new LabelSized("lblFood", new Vector2(130.0f, 15.0f), Alignment.TopLeft, GetTextFuncForFood, "CrimsonText-Regular-12", Color.Yellow), Alignment.MiddleRight, Alignment.MiddleLeft, new PointI(20, 0));

            #endregion

            #region UnitFrame

            HudViewFrame.AddControl(new Frame("unitFrame", new Vector2(Area.Width - 20.0f, Area.Height * 0.30f /* 30% of parent */), "GUI_Textures_1.frame1_whole"), Alignment.TopCenter, Alignment.TopCenter, new PointI(0, 500));

            string GetTextFuncForMoves() => SelectedStackView == null ? string.Empty : $"Moves: {SelectedStackView.MovementPoints}";
            HudViewFrame["unitFrame"].AddControl(new LabelSized("lblMoves", new Vector2(130.0f, 15.0f), Alignment.MiddleLeft, GetTextFuncForMoves, "CrimsonText-Regular-12", Color.White), Alignment.BottomLeft, Alignment.BottomLeft, new PointI(10, -13));

            #endregion

            var btnEndTurn = new Button("btnEndTurn", new Vector2(245.0f, 56.0f), "GUI_Textures_1.reg_button_n", "GUI_Textures_1.reg_button_a", "GUI_Textures_1.reg_button_h", "GUI_Textures_1.reg_button_a");
            btnEndTurn.AddPackage(new ControlClick((o, args) => EndTurn(o, new EventArgs())));

            btnEndTurn.AddControl(new LabelSized("lblEndTurn", btnEndTurn.Size.ToVector2(), Alignment.MiddleCenter, "Next Turn", "CrimsonText-Regular-12", Color.White, Color.Blue), Alignment.MiddleCenter, Alignment.MiddleCenter);

            HudViewFrame.AddControl(btnEndTurn, Alignment.BottomCenter, Alignment.TopCenter, PointI.Zero);

            #endregion

            WorldView = worldView;
            StackViews = stackViews;

            worldView.CellGrid.NewCellSeen += NewCellSeen;

            SetupViewport(Area.X, Area.Y, Area.Width, Area.Height + btnEndTurn.Height);

            Input = input;
            Input.BeginRegistration(GameStatus.OverlandMap.ToString(), "HudView");
            Input.EndRegistration();

            Input.Subscribe(GameStatus.OverlandMap.ToString(), "HudView");

            WorldView.SubscribeToStatusChanges("HudView", worldView.HandleStatusChange);
        }

        internal void LoadContent(ContentManager content)
        {
            Font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");
            HudViewFrame.LoadContent(content);
            //HudViewFrame["imgBackground"].LoadContent(content);
            HudViewFrame["lblCurrentDate"].LoadContent(content);

            HudViewFrame["resourceFrame"].LoadContent(content);
            HudViewFrame["resourceFrame.imgGold"].LoadContent(content);
            HudViewFrame["resourceFrame.imgMana"].LoadContent(content);
            HudViewFrame["resourceFrame.imgFood"].LoadContent(content);
            HudViewFrame["resourceFrame.imgGold.lblGold"].LoadContent(content);
            HudViewFrame["resourceFrame.imgMana.lblMana"].LoadContent(content);
            HudViewFrame["resourceFrame.imgFood.lblFood"].LoadContent(content);

            HudViewFrame["miniMapFrame"].LoadContent(content);

            HudViewFrame["unitFrame"].LoadContent(content);
            HudViewFrame["unitFrame.lblMoves"].LoadContent(content);

            HudViewFrame["btnEndTurn"].LoadContent(content);
            HudViewFrame["btnEndTurn.lblEndTurn"].LoadContent(content);

            var createdImage = MinimapHandler.Create(WorldView.CellGrid);
            var mapImage = (Image)HudViewFrame["miniMapFrame.mapImage"];
            mapImage.SetTexture(createdImage);

            ActionButtons = WorldView.GetActionButtons;
        }

        public void Update(float deltaTime)
        {
            _text1 = string.Empty;
            _text2 = string.Empty;
            if (WorldView.GameStatus == GameStatus.CityView) return;

            HudViewFrame["btnEndTurn"].Enabled = WorldView.AllStacksHaveBeenGivenOrders;

            HudViewFrame.Update(Input, deltaTime, Viewport);
            ActionButtons.Update(Input, deltaTime, Viewport);

            // get tile mouse is over
            var cellGrid = WorldView.CellGrid;
            var hexPoint = WorldView.Camera.ScreenPixelToWorldHex(Input.MousePosition);

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
            spriteBatch.GraphicsDevice.Viewport = Viewport;
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: ViewportAdapter.GetScaleMatrix());

            HudViewFrame.Draw(spriteBatch);

            var minimapImage = HudViewFrame["miniMapFrame.mapImage"];
            var minimapViewedRectangle = MinimapHandler.GetViewedRectangle(WorldView, minimapImage.Size);

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
            var stackViews = GetStackViewsSharingSameLocation(SelectedStackView);

            var x = 20.0f;
            var y = Area.Height * Constants.ONE_HALF + 10.0f;
            int i = 0;
            foreach (var stackView in stackViews)
            {
                DrawUnitBadges(spriteBatch, new Vector2(x, y), i, stackView);
                i += stackView.Count;
            }
        }

        private List<StackView.StackView> GetStackViewsSharingSameLocation(StackView.StackView selectedStackView)
        {
            var stackViews = new List<StackView.StackView>();
            foreach (var stackView in StackViews)
            {
                if (stackView.LocationHex == selectedStackView.LocationHex) // same location
                {
                    stackViews.Add(stackView);
                }
            }

            return stackViews;
        }

        private void DrawUnitBadges(SpriteBatch spriteBatch, Vector2 topLeftPosition, int index, StackView.StackView stackView)
        {
            var x = topLeftPosition.X + 30;
            var y = topLeftPosition.Y + 30;
            foreach (var unit in stackView.Stack)
            {
                var indexMod3 = index % 3;
                var indexDividedBy3 = index / 3; // Floor
                var xOffset = (stackView.ScreenFrame.Width + 10.0f) * indexMod3;
                var yOffset = (stackView.ScreenFrame.Height + 10.0f) * indexDividedBy3;
                DrawUnitBadge(spriteBatch, new Vector2(x + xOffset, y + yOffset), unit, stackView);
                index++;
            }
        }

        private void DrawUnitBadge(SpriteBatch spriteBatch, Vector2 centerPosition, Unit unit, StackView.StackView stackView)
        {
            // draw background
            var sourceRectangle = stackView.IsSelected ? StackViews.SquareGreenFrame.ToRectangle() : StackViews.SquareGrayFrame.ToRectangle();
            var destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, stackView.ScreenFrame.Width, stackView.ScreenFrame.Height);
            spriteBatch.Draw(StackViews.GuiTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.FlipVertically, 0.0f);

            // draw unit icon
            var frame = StackViews.UnitAtlas.Frames[unit.UnitTypeTextureName];
            sourceRectangle = frame.ToRectangle();
            destinationRectangle = new Rectangle((int)centerPosition.X, (int)centerPosition.Y, sourceRectangle.Width, sourceRectangle.Height);
            spriteBatch.Draw(StackViews.UnitTextures, destinationRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(sourceRectangle.Width * Constants.ONE_HALF, sourceRectangle.Height * Constants.ONE_HALF), SpriteEffects.None, 0.0f);
        }

        private void DrawMovementTypeImages(SpriteBatch spriteBatch)
        {
            var imgMovementTypes = SelectedStackView.GetMovementTypeImages();
            var i = 0;
            //size: (18;12)
            var x = HudViewFrame["unitFrame"].BottomRight.X - 18 - 12;
            var y = HudViewFrame["unitFrame"].BottomRight.Y - 12 - 20;
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
            foreach (var actionButton in ActionButtons)
            {
                actionButton.Enabled = selectedStackViewActions.Contains(actionButton.Name);
                actionButton.Draw(spriteBatch);
            }
        }

        private void DrawNotifications(SpriteBatch spriteBatch)
        {
            var x = 10.0f;
            var y = 460.0f;
            foreach (var item in WorldView.NotificationList)
            {
                var lines = TextWrapper.WrapText(item, 150.0f, Font);
                foreach (var line in lines)
                {
                    spriteBatch.DrawString(Font, line, new Vector2(x, y), Color.Pink);
                    y += 20.0f;
                }
            }
        }

        private void DrawTileInfo(SpriteBatch spriteBatch)
        {
            const float x = 10.0f;
            var y = Area.Height * 0.96f;

            spriteBatch.DrawString(Font, _text1, new Vector2(x, y), Color.White);
            spriteBatch.DrawString(Font, _text2, new Vector2(x, y + 15.0f), Color.White);
        }

        private void NewCellSeen(object sender, EventArgs e)
        {
            var createdImage = MinimapHandler.Create(WorldView.CellGrid);
            var mapImage = (Image)HudViewFrame["miniMapFrame.mapImage"];
            mapImage.SetTexture(createdImage);
        }

        #region Event Handlers

        private void MiniMapClick(object sender, EventArgs e)
        {
            // Where on the minimap is clicked?
            // Convert that to world pixel
            if (!(e is MouseEventArgs mouseEventArgs)) return;

            var minimapImage = HudViewFrame["miniMapFrame.mapImage"];
            var minimapPosition = mouseEventArgs.Mouse.Location - new Point(Viewport.X + minimapImage.Left, Viewport.Y + minimapImage.Top);
            var normalizedX = minimapPosition.X / (float) minimapImage.Size.X;
            var normalizedY = minimapPosition.Y / (float) minimapImage.Size.Y;

            var x = (int) (WorldView.WorldWidthInPixels * normalizedX);
            var y = (int) (WorldView.WorldHeightInPixels * normalizedY);

            WorldView.Camera.LookAtPixel(new PointI(x, y));
        }

        private void EndTurn(object sender, EventArgs e)
        {
            WorldView.EndTurn();
        }

        #endregion

        public void Dispose()
        {
            if (!IsDisposed)
            {
                // dispose managed state (managed objects)
                Input.UnsubscribeAllFromEventHandler("HudView");

                // set large fields to null
                ViewportAdapter = null;

                IsDisposed = true;
            }
        }
    }
}