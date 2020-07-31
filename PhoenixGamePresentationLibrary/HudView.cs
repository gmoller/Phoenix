using System;
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
        private Image _imgGold;
        private Label _lblMana;
        private Image _imgMana;
        private Label _lblFood;
        private Image _imgFood;

        private Frame _unitFrame;
        private Label _lblMoves;
        private Image _imgMovementType; // UnitStackMovementType

        private SpriteFont _font;

        private Button _btnEndTurn;
        private Label _test;

        private readonly UnitsView _unitsView;

        internal HudView(WorldView worldView, UnitsView unitsView)
        {
            var width = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Width * 0.1305f); // 13.05% of screen width
            var height = (int)(DeviceManager.Instance.GraphicsDevice.Viewport.Height * 0.945f); // 94.5% of screen height
            var x = DeviceManager.Instance.GraphicsDevice.Viewport.Width - width;
            var y = 0;
            _area = new Rectangle(x, y, width, height); // 250x1020

            _worldView = worldView;
            _unitsView = unitsView;
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

            _imgGold = new Image(new Vector2(10.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Coin_R", _resourceFrame);
            _imgGold.LoadContent(content);
            _imgMana = new Image(_imgGold.RelativeTopLeft.ToVector2() + new Vector2(0.0f, _imgGold.Height) + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Potion_R", _resourceFrame);
            _imgMana.LoadContent(content);
            _imgFood = new Image(_imgMana.RelativeTopLeft.ToVector2() + new Vector2(0.0f, _imgMana.Height) + new Vector2(0.0f, 10.0f), Alignment.TopLeft, new Vector2(50.0f, 50.0f), "Icons_1", "Bread_R", _resourceFrame);
            _imgFood.LoadContent(content);
            _lblGold = new LabelAutoSized(_imgGold.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Yellow, _resourceFrame);
            _lblGold.LoadContent(content);
            _lblMana = new LabelAutoSized(_imgMana.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Yellow, _resourceFrame);
            _lblMana.LoadContent(content);
            _lblFood = new LabelAutoSized(_imgFood.RelativeMiddleRight.ToVector2() + new Vector2(20.0f, 0.0f), Alignment.MiddleLeft, string.Empty, "CrimsonText-Regular-12", Color.Yellow, _resourceFrame);
            _lblFood.LoadContent(content);

            _unitFrame = new Frame(new Vector2(10.0f, 500.0f), Alignment.TopLeft, new Vector2(_area.Width - 20.0f, _area.Height * 0.30f /* 30% of parent */), "GUI_Textures_1", "frame1_whole", _hudViewFrame);
            _unitFrame.LoadContent(content);

            _lblMoves = new LabelAutoSized(_unitFrame.BottomLeft.ToVector2() + new Vector2(10.0f, -15.0f), Alignment.BottomLeft, string.Empty, "CrimsonText-Regular-12", Color.White); // , _unitFrame
            _lblMoves.LoadContent(content);

            _imgMovementType = new Image(_unitFrame.BottomRight.ToVector2() + new Vector2(-12.0f, -20.0f), Alignment.BottomRight, new Vector2(18.0f, 12.0f), "MovementTypes", "Move_Boot");
            _imgMovementType.LoadContent(content);

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

        public void Update(InputHandler input, float deltaTime)
        {
            var mouseOver = _area.Contains(input.MousePosition);
            input.Eaten = mouseOver;

            _hudViewFrame.Update(input, deltaTime);
            _lblCurrentDate.Update(input, deltaTime);
            _lblCurrentDate.Text = Globals.Instance.World.CurrentDate;

            _resourceFrame.Update(input, deltaTime);
            _imgGold.Update(input, deltaTime);
            _lblGold.Update(input, deltaTime);
            _lblGold.Text = $"{Globals.Instance.World.PlayerFaction.GoldInTreasury} GP (+{Globals.Instance.World.PlayerFaction.GoldPerTurn})";

            _imgMana.Update(input, deltaTime);
            _lblMana.Update(input, deltaTime);
            _lblMana.Text = "5 MP (+1)";

            _imgFood.Update(input, deltaTime);
            _lblFood.Update(input, deltaTime);
            _lblFood.Text = $"{Globals.Instance.World.PlayerFaction.FoodPerTurn} Food";

            _unitFrame.Update(input, deltaTime);
            _lblMoves.Update(input, deltaTime);
            _lblMoves.Text = "Moves:";
            _imgMovementType.Update(input, deltaTime);

            _btnEndTurn.Update(input, deltaTime);

            _test.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            _hudViewFrame.Draw();

            _lblCurrentDate.Draw();

            _resourceFrame.Draw();

            _imgGold.Draw();
            _lblGold.Draw();

            _imgMana.Draw();
            _lblMana.Draw();

            _imgFood.Draw();
            _lblFood.Draw();

            _unitFrame.Draw();
            _lblMoves.Draw();
            _imgMovementType.Draw();

            DrawNotifications(spriteBatch);
            DrawSelectedUnits(spriteBatch);
            DrawTileInfo(spriteBatch);

            _btnEndTurn.Draw();

            _test.Draw();

            spriteBatch.End();
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
            var startX = _area.X + 10.0f;
            var startY = _area.Y + _area.Height / 2.0f;
            var x = startX;
            var y = startY;

            foreach (var unitView in _unitsView)
            {
                if (unitView.IsSelected)
                {
                    unitView.DrawBadge(spriteBatch, new Vector2(x, y));
                    x += 75.0f;
                    if (x > startX + 150.0f)
                    {
                        x = startX;
                        y += 75.0f;
                    }
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