using PhoenixGameLibrary;
using PhoenixGameLibrary.GameData;

namespace PhoenixGamePresentationLibrary
{
    internal class ExploreHandler
    {
        internal void HandleExplore(StackView stackView)
        {
            if (stackView.Status == UnitStatus.Explore)
            {
                // if no destination, choose one
                if (stackView.MovementPath.Count == 0)
                {
                    // find closest unexplored cell
                    var cell = Globals.Instance.World.OverlandMap.CellGrid.GetClosestUnexploredCell(stackView.Location);

                    // find best path to unexplored cell
                    var path = MovementPathDeterminer.DetermineMovementPath(stackView.FirstUnit, stackView.Location, cell.ToPoint); // TODO: don't use first unit, use stack as a whole

                    // set movement path to this path
                    stackView.SetMovementPath(path);
                }
            }
        }
    }
}