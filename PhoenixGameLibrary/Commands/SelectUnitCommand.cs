using GameLogic;

namespace PhoenixGameLibrary.Commands
{
    public class SelectUnitCommand : Command
    {
        internal override void Execute()
        {
            var unit = Globals.Instance.World.Units[0];
            Globals.Instance.World.Unit = unit;

            unit.IsSelected = true;
        }
    }
}