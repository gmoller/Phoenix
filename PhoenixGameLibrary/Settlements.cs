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

        public int FoodProducedThisTurn { get; private set; }

        public Settlements()
        {
            _settlements = new List<Settlement>();
        }

        public void Update(GameTime gameTime, InputHandler input)
        {
            int foodProducedThisTurn = 0;
            foreach (var settlement in _settlements)
            {
                settlement.Update(gameTime, input);
                foodProducedThisTurn += settlement.FoodSurplus;
            }

            FoodProducedThisTurn = foodProducedThisTurn;
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

        public void AddSettlement(string name, string raceTypeName, Point hexLocation, CellGrid cellGrid, ContentManager content)
        {
            var settlement = new Settlement(name, raceTypeName, hexLocation, 4, cellGrid, "BuildersHall", "Barracks", "Smithy");
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