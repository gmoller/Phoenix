namespace PhoenixGameLibrary.Commands
{
    public class CloseSettlementCommand : Command
    {
        internal override void Execute()
        {
            var settlement = (Settlement)Payload;
            settlement.IsSelected = false;
        }
    }
}