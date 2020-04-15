using GameLogic;

namespace PhoenixGameLibrary.Commands
{
    public class OpenSettlementCommand : Command
    {
        internal override void Execute()
        {
            Globals.Instance.World.IsInSettlementView = true;

            var settlement = Globals.Instance.World.Settlements[0];
            Globals.Instance.World.Settlement = settlement; // TODO: get by settlementId
        }
    }
}