using GameLogic;

namespace PhoenixGameLibrary.Commands
{
    public class CloseSettlementCommand : Command
    {
        internal override void Execute()
        {
            Globals.Instance.World.IsInSettlementView = false;
        }
    }
}