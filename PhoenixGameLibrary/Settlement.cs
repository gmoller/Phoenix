using System;
using Microsoft.Xna.Framework;

namespace PhoenixGameLibrary
{
    /// <summary>
    /// A settlement is an immovable game entity that can be controlled by the
    /// player/AI to do things such as build new units and add buildings to
    /// improve the settlement.
    /// </summary>
    public class Settlement
    {
        public string Name { get; }
        public Point Location { get; }
        public int Population { get; private set; } // every 1 population is 1,000 residents

        public SettlementType SettlementType
        {
            get
            {
                if (Population <= 4) return SettlementType.Hamlet;
                if (Population >= 5 && Population <= 8) return SettlementType.Village;
                if (Population >= 9 && Population <= 12) return SettlementType.Town;
                if (Population >= 13 && Population <= 16) return SettlementType.City;
                if (Population >= 17)  return SettlementType.Capital;

                throw new Exception("Unknown settlement type.");
            }
        }

        public Settlement(string name, Point location, int settlementSize)
        {
            Name = name;
            Location = location;
            Population = settlementSize;
        }
    }

    public enum SettlementType
    {
        Outpost,
        Hamlet,
        Village,
        Town,
        City,
        Capital
    }
}