namespace PhoenixGameLibrary.Commands
{
    public class OpenSettlementCommand : Command
    {
        public override void Execute()
        {
            var payload = ((Settlement settlement, Settlements settlements))Payload;

            var settlements = payload.settlements;
            settlements.Selected = payload.settlement;
        }
    }
}