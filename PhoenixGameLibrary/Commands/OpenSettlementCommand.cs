namespace PhoenixGameLibrary.Commands
{
    public class OpenSettlementCommand : Command
    {
        internal override void Execute()
        {
            var settlement = (Settlement)Payload;
            settlement.IsSelected = true;
        }
    }
}