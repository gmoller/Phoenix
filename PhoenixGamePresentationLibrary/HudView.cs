﻿using System;
using System.Collections.Generic;
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

        private readonly Rectangle _area;

        private Frame _hudViewFrame;
        private Label _lblCurrentDate;

        private Frame _resourceFrame;
        private Label _lblGold;
        //private Label _lblMana;
        private Label _lblFood;

        private Frame _unitFrame;
        private Label _lblMoves;
        private List<IControl> _imgMovementTypes;
        private Dictionary<string, Image> _movementTypeImages;

        private SpriteFont _font;

        private Button _btnEndTurn;
        private Label _test;

        private readonly UnitsStackViews _unitsStacksView;

        internal HudView(WorldView worldView, UnitsStackViews unitsStacksView)
        {
            var width = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.1305f); // 13.05% of screen width
            var height = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Height * 0.945f); // 94.5% of screen height
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 250x1020

            _worldView = worldView;
            _unitsStacksView = unitsStacksView;

            _movementTypeImages = new Dictionary<string, Image>();
        }

        internal void LoadContent(ContentManager content)
        {
            _font = AssetsManager.Instance.GetSpriteFont("CrimsonText-Regular-12");

            var topLeftPosition = new Vector2(_area.X, _area.Y);

            var size = new Vector2(_area.Width, _area.Height);
            _hudViewFrame = new Frame(topLeftPosition, Alignment.TopLeft, size, "GUI_Textures_1", "frame3_whole", 47, 47, 47, 47);
            _hudViewFrame.LoadContent(content);

            _lblCurrentDate = new LabelAutoSized(new Vector2(_hudViewFrame.Width * 0.5f, 20.0f), Alignment.MiddleCenter, "Date:", "Maleficio-Regular-18", Color.Aquamarine, _hudViewFrame);
            _lblCurrentDate.LoadContent(content);

            _resourceFrame = new Frame(new Vector2(10.0f, 50.0f), Alignment.TopLeft, new Vector2(_area.Width - 20.0f, _area.Height * 0.20f /* 20% of parent */), "GUI_Textures_1", "frame1_whole", _hudViewFrame);
            _resourceFrame.LoadContent(content);

            var imgGold = new Image(new Vector2(10.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R", _resourceFrame);
            imgGold.LoadContent(content);

            var imgMana = new Image(imgGold.RelativeTopLeft.ToVector2() + new Vector2(0.0f, imgGold.Height) + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R", _resourceFrame);
            imgMana.LoadContent(content);

            var imgFood = new Image(imgMana.RelativeTopLeft.ToVector2() + new Vector2(0.0f, imgMana.Height) + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R", _resourceFrame);
            imgFood.LoadContent(content);

            _lblGold = new LabelAutoSized(imgGold.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Yellow, _resourceFrame);
            _lblGold.LoadContent(content);

            Func<string> getTextFunc = () => "5 MP (+1)";
            var lblMana = new LabelAutoSized(imgMana.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, getTextFunc, "CrimsonText-Regular-12", Color.Yellow, _resourceFrame);
            lblMana.LoadContent(content);

            _lblFood = new LabelAutoSized(imgFood.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Yellow, _resourceFrame);
            _lblFood.LoadContent(content);

            _resourceFrame.AddControls(imgGold, imgMana, lblMana, imgFood);

            _unitFrame = new Frame(new Vector2(10.0f, 500.0f), Alignment.TopLeft, new Vector2(_area.Width - 20.0f, _area.Height * 0.30f /* 30% of parent */), "GUI_Textures_1", "frame1_whole", _hudViewFrame);
            _unitFrame.LoadContent(content);

            _lblMoves = new LabelAutoSized(_unitFrame.BottomLeft.ToVector2() + new Vector2(10.0f, -15.0f), Alignment.BottomLeft, string.Empty, "CrimsonText-Regular-12", Color.White); // , _unitFrame
            _lblMoves.LoadContent(content);

            _movementTypeImages = LoadMovementTypeImages(content);

            var pos = new Vector2(DeviceManager.Instance.MapViewport.X + DeviceManager.Instance.MapViewport.Width, DeviceManager.Instance.MapViewport.Y + DeviceManager.Instance.MapViewport.Height);
            _btnEndTurn = new Button(pos, Alignment.BottomRight, new Vector2(245.0f, 56.0f), "GUI_Textures_1", "reg_button_n", "reg_button_a", "reg_button_a", "reg_button_h");
            _btnEndTurn.LoadContent(content);
            _btnEndTurn.Click += BtnEndTurnClick;

            var label = new LabelSized(_btnEndTurn.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, new Vector2(245.0f, 56.0f), Alignment.MiddleCenter, "Next Turn", "CrimsonText-Regular-12", Color.White, Color.Blue, _btnEndTurn);
            label.LoadContent(content);
            _btnEndTurn.Label = label;

            _test = new LabelSized(new Vector2(0.0f, 1080.0f), Alignment.BottomLeft, new Vector2(50.0f, 50.0f), Alignment.TopRight, "Test", "CrimsonText-Regular-12", Color.Red, null, null, Color.Blue);
            _test.Click += delegate { _test.MoveTopLeftPosition(10, -10); };
            _test.LoadContent(content);
        }

        private Dictionary<string, Image> LoadMovementTypeImages(ContentManager content)
        {
            var movementTypes = Globals.Instance.MovementTypes;

            var movementTypeImages = new Dictionary<string, Image>();
            foreach (var movementType in movementTypes)
            {
                var image = new Image(_unitFrame.BottomRight.ToVector2() + new Vector2(-12.0f, -20.0f), Alignment.BottomRight, new Vector2(18.0f, 12.0f), "MovementTypes", movementType.Name);
                image.LoadContent(content);
                movementTypeImages.Add(movementType.Name, image);
            }

            return movementTypeImages;
        }

        public void Update(InputHandler input, float deltaTime)
        {
            var mouseOver = _area.Contains(input.MousePosition);
            input.Eaten = mouseOver;

            _hudViewFrame.Update(input, deltaTime);
            _lblCurrentDate.Update(input, deltaTime);
            _lblCurrentDate.Text = Globals.Instance.World.CurrentDate;

            _resourceFrame.Update(input, deltaTime);
            _lblGold.Update(input, deltaTime);
            _lblGold.Text = $"{Globals.Instance.World.PlayerFaction.GoldInTreasury} GP (+{Globals.Instance.World.PlayerFaction.GoldPerTurn})";

            //_resourceFrame["lblMana"].Text = "5 MP (+1)";
            //_lblMana.Text = "5 MP (+1)";

            _lblFood.Update(input, deltaTime);
            _lblFood.Text = $"{Globals.Instance.World.PlayerFaction.FoodPerTurn} Food";

            if (_unitsStacksView.Selected != null)
            {
                _unitFrame.Update(input, deltaTime);
                _lblMoves.Update(input, deltaTime);
                _lblMoves.Text = $"Moves: {_unitsStacksView.Selected.MovementPoints}";
                _imgMovementTypes = new List<IControl>();
                foreach (var movementType in _unitsStacksView.Selected.MovementTypes)
                {
                    var img = _movementTypeImages[movementType];
                    var imgMovementType = img.Clone();
                    imgMovementType.MoveTopLeftPosition(-19 * _imgMovementTypes.Count, 0);
                    _imgMovementTypes.Add(imgMovementType);
                }

                foreach (var imgMovementType in _imgMovementTypes)
                {
                    imgMovementType.Update(input, deltaTime);
                }
            }

            _btnEndTurn.Update(input, deltaTime);

            _test.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            _hudViewFrame.Draw(spriteBatch);

            _lblCurrentDate.Draw(spriteBatch);

            _resourceFrame.Draw(spriteBatch);

            _lblGold.Draw(spriteBatch);
            _lblFood.Draw(spriteBatch);

            if (_unitsStacksView.Selected != null)
            {
                _unitFrame.Draw(spriteBatch);
                DrawSelectedUnits(spriteBatch);
                DrawUnselectedUnits(spriteBatch);
                _lblMoves.Draw(spriteBatch);
                foreach (var imgMovementType in _imgMovementTypes)
                {
                    imgMovementType.Draw(spriteBatch);
                }
            }

            DrawNotifications(spriteBatch);
            DrawTileInfo(spriteBatch);

            _btnEndTurn.Draw(spriteBatch);

            _test.Draw(spriteBatch);
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

        private void DrawSelectedUnits(SpriteBatch spriteBatch)
        {
            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * 0.5f + 10.0f;

            _unitsStacksView.Selected.DrawBadges(spriteBatch, new Vector2(x, y));
        }

        private void DrawUnselectedUnits(SpriteBatch spriteBatch)
        {
            var i = _unitsStacksView.Selected.Count;
            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * 0.5f + 10.0f;

            // find other stacks on this location:_unitsStacksView.Selected.Location
            var otherUnitStacks = _unitsStacksView.Selected.GetUnitStacksSharingSameLocation();
            foreach (var unitStack in otherUnitStacks)
            {
                unitStack.DrawBadges(spriteBatch, new Vector2(x, y), i, false);
                i += unitStack.Count;
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