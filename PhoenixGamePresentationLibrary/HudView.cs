﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AssetsLibrary;
using GuiControls;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    internal class HudView
    {
        private readonly WorldView _worldView;

        private SpriteFont _font;
        private readonly Rectangle _area;

        private Frame _hudViewFrame;

        private Label _test;

        private readonly UnitsStackViews _unitsStackViews;
        private UnitsStackView SelectedUnitsStackView => _unitsStackViews.Selected;

        internal HudView(WorldView worldView, UnitsStackViews unitsStackViews)
        {
            var width = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.1305f); // 13.05% of screen width
            var height = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Height * 0.945f); // 94.5% of screen height
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 250x1020

            _worldView = worldView;
            _unitsStackViews = unitsStackViews;
        }

        internal void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            var topLeftPosition = new Vector2(_area.X, _area.Y);

            #region HudViewFrame
            var size = new Vector2(_area.Width, _area.Height);
            _hudViewFrame = new Frame(topLeftPosition, Alignment.TopLeft, size, "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);
            _hudViewFrame.LoadContent(content);

            string GetTextFuncForDate() => Globals.Instance.World.CurrentDate;
            var lblCurrentDate = new LabelAutoSized(new Vector2(_hudViewFrame.Width * 0.5f, 20.0f), Alignment.MiddleCenter, GetTextFuncForDate, "Maleficio-Regular-18", Color.Aquamarine, _hudViewFrame);
            lblCurrentDate.LoadContent(content);

            #region ResourceFrame
            var resourceFrame = new Frame(new Vector2(10.0f, 50.0f), Alignment.TopLeft, new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole", _hudViewFrame);
            resourceFrame.LoadContent(content);

            var imgGold = new Image(new Vector2(10.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R", resourceFrame);
            imgGold.LoadContent(content);

            var imgMana = new Image(imgGold.RelativeTopLeft.ToVector2() + new Vector2(0.0f, imgGold.Height) + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R", resourceFrame);
            imgMana.LoadContent(content);

            var imgFood = new Image(imgMana.RelativeTopLeft.ToVector2() + new Vector2(0.0f, imgMana.Height) + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R", resourceFrame);
            imgFood.LoadContent(content);

            string GetTextFuncForGold() => $"{Globals.Instance.World.PlayerFaction.GoldInTreasury} GP (+{Globals.Instance.World.PlayerFaction.GoldPerTurn})";
            var lblGold = new LabelAutoSized(imgGold.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, GetTextFuncForGold, "CrimsonText-Regular-12", Color.Yellow, resourceFrame);
            lblGold.LoadContent(content);

            string GetTextFuncForMana() => "5 MP (+1)";
            var lblMana = new LabelAutoSized(imgMana.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, GetTextFuncForMana, "CrimsonText-Regular-12", Color.Yellow, resourceFrame);
            lblMana.LoadContent(content);

            string GetTextFuncForFood() => $"{Globals.Instance.World.PlayerFaction.FoodPerTurn} Food";
            var lblFood = new LabelAutoSized(imgFood.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, GetTextFuncForFood, "CrimsonText-Regular-12", Color.Yellow, resourceFrame);
            lblFood.LoadContent(content);

            resourceFrame.AddControls(imgGold, lblGold, imgMana, lblMana, imgFood, lblFood);
            #endregion

            #region UnitFrame
            var unitFrame = new Frame(new Vector2(10.0f, 500.0f), Alignment.TopLeft, new Vector2(_area.Width - 20.0f, _area.Height * 0.30f /* 30% of parent */), "GUI_Textures_1", "frame1_whole", _hudViewFrame);
            unitFrame.LoadContent(content);

            string GetTextFuncForMoves() => SelectedUnitsStackView == null ? string.Empty : $"Moves: {SelectedUnitsStackView.MovementPoints}";
            var lblMoves = new LabelAutoSized(unitFrame.BottomLeft.ToVector2() + new Vector2(10.0f, -15.0f), Alignment.BottomLeft, GetTextFuncForMoves, "CrimsonText-Regular-12", Color.White); // , _unitFrame
            lblMoves.LoadContent(content);

            unitFrame.AddControl(lblMoves);
            #endregion

            //var json = _hudViewFrame.Serialize();
            //_hudViewFrame.Deserialize(json);
            //var newFrame = new Frame(json);

            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            var btnEndTurn = new Button(pos, Alignment.BottomRight, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_a", "reg_button_h");
            btnEndTurn.LoadContent(content);
            btnEndTurn.Click += BtnEndTurnClick;

            var label = new LabelSized(btnEndTurn.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, btnEndTurn.Size.ToVector2(), Alignment.MiddleCenter, "Next Turn", "CrimsonText-Regular-12", Color.White, Color.Blue, btnEndTurn);
            label.LoadContent(content);
            btnEndTurn.AddControl(label);

            _hudViewFrame.AddControls(lblCurrentDate, resourceFrame, unitFrame, btnEndTurn);

            #endregion

            _test = new LabelSized(new Vector2(0.0f, 1080.0f), Alignment.BottomLeft, new Vector2(50.0f, 50.0f), Alignment.TopRight, "Test", "CrimsonText-Regular-12", Color.Red, null, null, Color.Blue);
            _test.Click += delegate { _test.MoveTopLeftPosition(10, -10); };
            _test.LoadContent(content);
        }

        public void Update(InputHandler input, float deltaTime)
        {
            var mouseOver = _area.Contains(input.MousePosition);
            input.Eaten = mouseOver;

            _hudViewFrame.Update(input, deltaTime);

            _test.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _hudViewFrame.Draw(spriteBatch);

            DrawUnits(spriteBatch);
            DrawNotifications(spriteBatch);
            DrawTileInfo(spriteBatch);

            _test.Draw(spriteBatch);
        }

        private void DrawUnits(SpriteBatch spriteBatch)
        {
            if (SelectedUnitsStackView == null) return;

            DrawSelectedUnits(spriteBatch);
            DrawUnselectedUnits(spriteBatch);
            DrawMovementTypeImages(spriteBatch);
            DrawActionButtons(spriteBatch);
        }

        private void DrawSelectedUnits(SpriteBatch spriteBatch)
        {
            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * 0.5f + 10.0f;

            SelectedUnitsStackView.DrawBadges(spriteBatch, new Vector2(x, y));
        }

        private void DrawUnselectedUnits(SpriteBatch spriteBatch)
        {
            var i = SelectedUnitsStackView.Count;
            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * 0.5f + 10.0f;

            // find other stacks on this location:_unitsStacksView.Selected.Location
            var otherUnitStacks = SelectedUnitsStackView.GetUnitStacksSharingSameLocation();
            foreach (var unitStack in otherUnitStacks)
            {
                unitStack.DrawBadges(spriteBatch, new Vector2(x, y), i, false);
                i += unitStack.Count;
            }
        }

        private void DrawMovementTypeImages(SpriteBatch spriteBatch)
        {
            var imgMovementTypes = SelectedUnitsStackView.GetMovementTypeImages();
            var i = 0;
            var x = 1910 - 18 - 12; // position of unitFrame BottomRight: (1910;806) : size: (18;12)
            var y = 806 - 12 - 20;
            foreach (var imgMovementType in imgMovementTypes)
            {
                imgMovementType.SetTopLeftPosition(x - 19 * i, y);
                imgMovementType.Draw(spriteBatch);
                i++;
            }
        }

        private void DrawActionButtons(SpriteBatch spriteBatch)
        {
            var actionButtons = SelectedUnitsStackView.ActionButtons;
            var i = 0;
            var x = 1680; // position of unitFrame BottomRight: (1680;806)
            var y = 806;
            foreach (var actionButton in actionButtons)
            {
                var xOffset = actionButton.Width * (i % 2);
                var yOffset = actionButton.Height * (i / 2); // math.Floor
                actionButton.SetTopLeftPosition(x + xOffset, y + yOffset);
                actionButton.Draw(spriteBatch);
                i++;
            }
        }

        private void DrawNotifications(SpriteBatch spriteBatch)
        {
            var x = _area.X + 10.0f;
            var y = _area.Y + 400.0f;
            foreach (var item in Globals.Instance.World.NotificationList)
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
            var cellGrid = Globals.Instance.World.OverlandMap.CellGrid;
            var hexPoint = DeviceManager.Instance.WorldHexPointedAtByMouseCursor;
            if (hexPoint.X >= 0 && hexPoint.Y >= 0 && hexPoint.X < PhoenixGameLibrary.Constants.WORLD_MAP_COLUMNS && hexPoint.Y < PhoenixGameLibrary.Constants.WORLD_MAP_ROWS)
            {
                var cell = cellGrid.GetCell(hexPoint.X, hexPoint.Y);
                if (cell.SeenState != SeenState.Never)
                {
                    var terrainType = Globals.Instance.TerrainTypes[cell.TerrainTypeId];
                    var text1 = $"{terrainType.Name} - {terrainType.FoodOutput} food";
                    spriteBatch.DrawString(_font, text1, new Vector2(x, y), Color.White);

                    if (terrainType.CanSettleOn)
                    {
                        var catchment = cellGrid.GetCatchment(hexPoint.X, hexPoint.Y, 2);
                        var maxPop = PhoenixGameLibrary.Helpers.BaseFoodLevel.DetermineBaseFoodLevel(new Utilities.Point(hexPoint.X, hexPoint.Y), catchment);
                        var text2 = $"Maximum Pop - {maxPop}";
                        spriteBatch.DrawString(_font, text2, new Vector2(x, y + 15.0f), Color.White);
                    }
                }
            }
        }

        private void BtnEndTurnClick(object sender, EventArgs e)
        {
            _worldView.EndTurn();
        }
    }
}