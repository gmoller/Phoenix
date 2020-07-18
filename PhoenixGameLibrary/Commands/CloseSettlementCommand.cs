namespace PhoenixGameLibrary.Commands
{
    public class CloseSettlementCommand : Command
    {
        public override void Execute()
        {
            var settlement = (Settlement)Payload;
            settlement.IsSelected = false;
        }
    }
}