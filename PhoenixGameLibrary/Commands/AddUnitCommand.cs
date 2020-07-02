using GameLogic;
using Utilities;

namespace PhoenixGameLibrary.Commands
{
    public class AddUnitCommand : Command
    {
        internal override void Execute()
        {
            var position = (Point)Payload;
            Globals.Instance.World.Units.AddUnit(position);
        }
    }
}
