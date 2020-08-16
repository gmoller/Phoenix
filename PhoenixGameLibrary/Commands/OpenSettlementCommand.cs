namespace PhoenixGameLibrary.Commands
{
    public class OpenSettlementCommand : Command
    {
        public override void Execute()
        {
            var settlement = (Settlement)Payload;
            settlement.IsSelected = true;
            // TODO: look at settlement
        }
    }
}