namespace PhoenixGameLibrary.Commands
{
    public class BeginTurnCommand : Command
    {
        public override void Execute()
        {
            var payload = (World)Payload;
            payload.BeginTurn();
        }
    }
}