namespace PhoenixGameLibrary.Commands
{
    public class EndTurnCommand : Command
    {
        public override void Execute()
        {
            var payload = (World)Payload;
            payload.EndTurn();
        }
    }
}