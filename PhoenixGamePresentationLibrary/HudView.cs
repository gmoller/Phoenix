using System;
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

        private SpriteFont _font;
        private readonly Rectangle _area;

        private Frame _hudViewFrame;

        private Dictionary<string, Image> _movementTypeImages;

        private Label _test;

        private readonly UnitsStackView _selectedUnitsStackView;

        internal HudView(WorldView worldView, UnitsStackViews unitsStackViews)
        {
            var width = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.1305f); // 13.05% of screen width
            var height = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Height * 0.945f); // 94.5% of screen height
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 250x1020

            _worldView = worldView;
            _selectedUnitsStackView = unitsStackViews.Selected;
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

            string GetTextFuncForMoves() => _selectedUnitsStackView == null ? string.Empty : $"Moves: {_selectedUnitsStackView.MovementPoints}";
            var lblMoves = new LabelAutoSized(unitFrame.BottomLeft.ToVector2() + new Vector2(10.0f, -15.0f), Alignment.BottomLeft, GetTextFuncForMoves, "CrimsonText-Regular-12", Color.White); // , _unitFrame
            lblMoves.LoadContent(content);

            var button1 = new Button(unitFrame.BottomLeft.ToVector2(), Alignment.TopLeft, new Vector2(unitFrame.Width * 0.5f, 30.0f), "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_n", "simpleb_h");
            button1.LoadContent(content);
            //button1.Click += BtnDoneClick;
            var label1 = new LabelSized(button1.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button1.Size.ToVector2(), Alignment.MiddleCenter, "DONE", "Carolingia-Regular-12", Color.Black, null, button1);
            label1.LoadContent(content);
            button1.AddControl(label1);

            var button2 = new Button(button1.TopRight.ToVector2(), Alignment.TopLeft, new Vector2(unitFrame.Width * 0.5f, 30.0f), "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_n", "simpleb_h");
            button2.LoadContent(content);
            //button2.Click += BtnPatrolClick;
            var label2 = new LabelSized(button2.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button2.Size.ToVector2(), Alignment.MiddleCenter, "PATROL", "Carolingia-Regular-12", Color.Black, null, button2);
            label2.LoadContent(content);
            button2.AddControl(label2);

            var button3 = new Button(button1.BottomLeft.ToVector2(), Alignment.TopLeft, new Vector2(unitFrame.Width * 0.5f, 30.0f), "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_n", "simpleb_h");
            button3.LoadContent(content);
            //button3.Click += BtnWaitClick;
            var label3 = new LabelSized(button3.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button3.Size.ToVector2(), Alignment.MiddleCenter, "WAIT", "Carolingia-Regular-12", Color.Black, null, button3);
            label3.LoadContent(content);
            button3.AddControl(label3);

            var button4 = new Button(button3.TopRight.ToVector2(), Alignment.TopLeft, new Vector2(unitFrame.Width * 0.5f, 30.0f), "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_n", "simpleb_h");
            button4.LoadContent(content);
            //button4.Click += BtnBuildClick;
            var label4 = new LabelSized(button4.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button4.Size.ToVector2(), Alignment.MiddleCenter, "BUILD", "Carolingia-Regular-12", Color.Black, null, button4);
            label4.LoadContent(content);
            button4.AddControl(label4);

            unitFrame.AddControls(lblMoves, button1, button2, button3, button4);
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

            _movementTypeImages = LoadMovementTypeImages(unitFrame, content);

            _test = new LabelSized(new Vector2(0.0f, 1080.0f), Alignment.BottomLeft, new Vector2(50.0f, 50.0f), Alignment.TopRight, "Test", "CrimsonText-Regular-12", Color.Red, null, null, Color.Blue);
            _test.Click += delegate { _test.MoveTopLeftPosition(10, -10); };
            _test.LoadContent(content);
        }

        private Dictionary<string, Image> LoadMovementTypeImages(Frame unitFrame, ContentManager content)
        {
            var movementTypes = Globals.Instance.MovementTypes;

            var movementTypeImages = new Dictionary<string, Image>();
            foreach (var movementType in movementTypes)
            {
                var image = new Image(unitFrame.BottomRight.ToVector2() + new Vector2(-12.0f, -20.0f), Alignment.BottomRight, new Vector2(18.0f, 12.0f), "MovementTypes", movementType.Name);
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
            if (_selectedUnitsStackView == null) return;

            DrawSelectedUnits(spriteBatch);
            DrawUnselectedUnits(spriteBatch);

            var imgMovementTypes = _selectedUnitsStackView.GetMovementTypeImages(_movementTypeImages);
            int i = 0;
            foreach (var imgMovementType in imgMovementTypes)
            {
                imgMovementType.MoveTopLeftPosition(-19 * i++, 0);
                imgMovementType.Draw(spriteBatch);
            }
        }

        private void DrawSelectedUnits(SpriteBatch spriteBatch)
        {
            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * 0.5f + 10.0f;

            _selectedUnitsStackView.DrawBadges(spriteBatch, new Vector2(x, y));
        }

        private void DrawUnselectedUnits(SpriteBatch spriteBatch)
        {
            var i = _selectedUnitsStackView.Count;
            var x = _area.X + 20.0f;
            var y = _area.Y + _area.Height * 0.5f + 10.0f;

            // find other stacks on this location:_unitsStacksView.Selected.Location
            var otherUnitStacks = _selectedUnitsStackView.GetUnitStacksSharingSameLocation();
            foreach (var unitStack in otherUnitStacks)
            {
                unitStack.DrawBadges(spriteBatch, new Vector2(x, y), i, false);
                i += unitStack.Count;
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