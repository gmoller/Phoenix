using System.Linq;
using PhoenixGameData.StrongTypes;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public readonly struct UnitRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public UnitTypeId UnitTypeId { get; } // Foreign key -> GameMetadata.UnitType
        public StackId StackId { get; } // foreign key -> GameData.Stack
        public MovementPoints MovementPoints { get; }

        public UnitRecord(int unitId, int stackId)
        {
            Id = GameDataSequences.GetNextSequence("Unit");
            UnitTypeId = new UnitTypeId(unitId);
            StackId = new StackId(stackId);

            var gameConfigCache = CallContext<GameConfigCache>.GetData("GameConfigCache");
            var unit = gameConfigCache.GetUnitConfigById(unitId);
            MovementPoints = new MovementPoints((float)unit.MovementPoints);
        }

        public UnitRecord(UnitRecord unitRecord, StackId stackId, MovementPoints movementPoints)
        {
            Id = unitRecord.Id;
            UnitTypeId = unitRecord.UnitTypeId;
            StackId = stackId;
            MovementPoints = movementPoints;
        }

        public UnitRecord(UnitRecord unitRecord, StackId stackId)
        {
            Id = unitRecord.Id;
            UnitTypeId = unitRecord.UnitTypeId;
            StackId = stackId;
            MovementPoints = unitRecord.MovementPoints;
        }

        public UnitRecord(UnitRecord unitRecord, MovementPoints movementPoints)
        {
            Id = unitRecord.Id;
            UnitTypeId = unitRecord.UnitTypeId;
            StackId = unitRecord.StackId;
            MovementPoints = movementPoints;
        }
    }

    public class UnitRecords : DataList<UnitRecord>
    {
        internal UnitRecords GetByStackId(int stackId)
        {
            var list = new UnitRecords();
            foreach (var item in Items.Where(item => item.StackId.Value == stackId))
            {
                list.Add(item);
            }

            return list;
        }
    }
}