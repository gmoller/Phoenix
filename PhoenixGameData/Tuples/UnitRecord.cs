using System.Linq;
using Zen.Utilities;

namespace PhoenixGameData.Tuples
{
    public class UnitRecord : IIdentifiedById
    {
        public int Id { get; } // Primary key
        public int UnitTypeId { get; } // Foreign key -> GameMetadata.UnitType
        public int StackId { get; set; } // foreign key -> GameData.Stack
        public float MovementPoints { get; set; }

        public UnitRecord(int unitTypeId, int stackId)
        {
            Id = GameDataRepository.GetNextSequence("Unit");
            UnitTypeId = unitTypeId;
            StackId = stackId;
            MovementPoints = 0.0f;
        }
    }

    internal class UnitsCollection : DataList<UnitRecord>
    {
        internal DataList<UnitRecord> GetByStackId(int stackId)
        {
            var list = Create();
            foreach (var item in Items.Where(item => item.StackId == stackId))
            {
                list.Add(item);
            }

            return list;
        }
    }
}