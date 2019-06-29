using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using PhoenixGameLibrary.GameData;
using Utilities;

namespace PhoenixGameLibrary
{
    public class Settlements
    {
        private List<Settlement> _settlements;

        public Settlements()
        {
            _settlements = new List<Settlement>();
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            foreach (var settlement in _settlements)
            {
                settlement.Update(gameTime, input);
            }
        }

        public void Draw()
        {
            DeviceManager.Instance.SetViewport(DeviceManager.Instance.MapViewport);

            foreach (var settlement in _settlements)
            {
                settlement.Draw();
            }

            DeviceManager.Instance.ResetViewport();
        }

        public void AddSettlement(string name, RaceType raceType, Point hexLocation, CellGrid cellGrid, ContentManager content)
        {
            var settlement = new Settlement(name, raceType, hexLocation, 4, cellGrid);
            settlement.LoadContent(content);
            _settlements.Add(settlement);
        }

        public void EndTurn()
        {
            foreach (var settlement in _settlements)
            {
                settlement.EndTurn();
            }
        }
    }
}