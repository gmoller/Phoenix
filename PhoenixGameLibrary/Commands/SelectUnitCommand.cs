namespace PhoenixGameLibrary.Commands
{
    public class SelectUnitCommand : Command
    {
        internal override void Execute()
        {
            var unit = (Unit)Payload;
            unit.IsSelected = true;
        }
    }
}