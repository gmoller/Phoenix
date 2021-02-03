using PhoenixGameData;
using PhoenixGameData.Tuples;
using Zen.Utilities;

namespace PhoenixTests
{
    public static class Repository
    {
        private static readonly GameDataRepository Repo;

        static Repository()
        {
            Repo = new GameDataRepository();
            Repo.Add(new FactionRecord(0, 0, 0)); // Barbarians
            Repo.Add(new SettlementRecord(0, 1, new PointI(12, 9), "Testville"));
            Repo.Add(new StackRecord(1, new PointI(12, 9)));
            Repo.Add(new UnitRecord(100, 1));
            Repo.Add(new StackRecord(1, new PointI(15, 7)));
            Repo.Add(new UnitRecord(0, 2));
            Repo.Add(new StackRecord(1, new PointI(12, 9)));
            Repo.Add(new UnitRecord(1, 3));
        }
    }
}