namespace PhoenixGameLibrary.Commands
{
    public class CloseSettlementCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Settlement settlement, Settlements settlements))Payload;

            var settlements = payload.settlements;

            settlements.Selected = null;
        }
    }
}