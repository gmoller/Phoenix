﻿using System.Collections.Generic;
using GuiControls;
using HexLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Input;
using PhoenixGameLibrary;
using PhoenixGameLibrary.Commands;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGamePresentationLibrary
{
    public class WorldView
    {
        private OverlandMapView _overlandMapView;
        private OverlandSettlementViews _overlandSettlementsView;
        private UnitsStackViews _unitsStacksView;
        private SettlementViews _settlementsView;
        private HudView _hudView;

        public Camera Camera { get; private set; }
        public World World { get; }
        public Dictionary<string, Image> MovementTypeImages { get; private set; }
        public Dictionary<string, Button> ActionButtons { get; private set; }

        internal WorldView(World world)
        {
            World = world;
        }

        internal void LoadContent(ContentManager content)
        {
            _overlandMapView = new OverlandMapView(this, World.OverlandMap);
            _overlandSettlementsView = new OverlandSettlementViews(this, World.Settlements);
            _unitsStacksView = new UnitsStackViews(this, World.UnitsStacks);
            _settlementsView = new SettlementViews(this, World.Settlements);
            _hudView = new HudView(this, _unitsStacksView);

            Camera = new Camera(new Rectangle(0, 0, DeviceManager.Instance.GraphicsDevice.Viewport.Width, DeviceManager.Instance.GraphicsDevice.Viewport.Height));
            Camera.LoadContent(content);

            _overlandSettlementsView.LoadContent(content);
            _unitsStacksView.LoadContent(content);
            _settlementsView.LoadContent(content);
            _hudView.LoadContent(content);

            MovementTypeImages = LoadMovementTypeImages(content);
            ActionButtons = LoadActionButtons(content);
        }

        internal void Update(InputHandler input, float deltaTime)
        {
            Camera.Update(input, deltaTime);

            var worldPosPointedAtByMouseCursor = Camera.ScreenToWorld(new Vector2(input.MousePosition.X, input.MousePosition.Y));
            DeviceManager.Instance.WorldPositionPointedAtByMouseCursor = new Utilities.Point((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
            var worldHex = HexOffsetCoordinates.FromPixel((int)worldPosPointedAtByMouseCursor.X, (int)worldPosPointedAtByMouseCursor.Y);
            DeviceManager.Instance.WorldHexPointedAtByMouseCursor = new Utilities.Point(worldHex.Col, worldHex.Row);

            _overlandMapView.Update(input, deltaTime);
            _overlandSettlementsView.Update(input, deltaTime);
            _unitsStacksView.Update(input, deltaTime);
            _settlementsView.Update(input, deltaTime);
            _hudView.Update(input, deltaTime);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Camera.Transform); // FrontToBack
            _overlandMapView.Draw(spriteBatch);
            _overlandSettlementsView.Draw(spriteBatch);

            _unitsStacksView.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            _hudView.Draw(spriteBatch);
            _settlementsView.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void BeginTurn()
        {
            Command beginTurnCommand = new BeginTurnCommand { Payload = World };
            beginTurnCommand.Execute();

            _unitsStacksView.SelectNext();
            //_unitsStacksView[0].SelectStack();
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

        private Dictionary<string, Image> LoadMovementTypeImages(ContentManager content)
        {
            var movementTypes = Globals.Instance.MovementTypes;

            var movementTypeImages = new Dictionary<string, Image>();
            foreach (var movementType in movementTypes)
            {
                var image = new Image(Vector2.Zero, Alignment.TopLeft, new Vector2(18.0f, 12.0f), "MovementTypes", movementType.Name);
                image.LoadContent(content);
                movementTypeImages.Add(movementType.Name, image);
            }

            return movementTypeImages;
        }

        private Dictionary<string, Button> LoadActionButtons(ContentManager content)
        {
            var actionTypes = Globals.Instance.ActionTypes;

            var actionButtons = new Dictionary<string, Button>();
            foreach (var actionType in actionTypes)
            {
                var button = new Button(Vector2.Zero, Alignment.TopLeft, new Vector2(115.0f, 30.0f), "GUI_Textures_1", "simpleb_n", "simpleb_a", "simpleb_n", "simpleb_h");
                button.LoadContent(content);
                var label = new LabelSized(button.Size.ToVector2() * 0.5f, Alignment.MiddleCenter, button.Size.ToVector2(), Alignment.MiddleCenter, actionType.ButtonName, "Maleficio-Regular-12", Color.Black, null, button);
                label.LoadContent(content);
                button.AddControl(label);
                //button.ChildControls[label.Name];

                actionButtons.Add(actionType.Name, button);
            }

            return actionButtons;
        }
    }
}